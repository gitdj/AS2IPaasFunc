using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAppAS2.Helpers.BziRef
{

    //internal class DecompressedStream : ReadOnlySeekableStream
    //{
    //    private ushort s1 = 1;
    //    private ushort s2;
    //    private uint adler32Checksum;

    //    public DecompressedStream(Stream baseStream)
    //      : base(baseStream)
    //    {
    //    }

    //    public uint Adler32Checksum => this.adler32Checksum;

    //    public override int Read(byte[] buffer, int offset, int count)
    //    {
    //        int num = base.Read(buffer, offset, count);
    //        if (num > 0)
    //        {
    //            for (int index = 0; index < num; ++index)
    //            {
    //                this.s1 = (ushort)(((uint)buffer[index] + (uint)this.s1) % 65521U);
    //                this.s2 = (ushort)(((uint)this.s1 + (uint)this.s2) % 65521U);
    //            }
    //            this.adler32Checksum = ((uint)this.s2 << 16) + (uint)this.s1;
    //        }
    //        return num;
    //    }

    //    public override int ReadByte()
    //    {
    //        int num = base.ReadByte();
    //        if (num > 0)
    //        {
    //            this.s1 = (ushort)(((uint)num + (uint)this.s1) % 65521U);
    //            this.s2 = (ushort)(((uint)this.s1 + (uint)this.s2) % 65521U);
    //            this.adler32Checksum = ((uint)this.s2 << 16) + (uint)this.s1;
    //        }
    //        return num;
    //    }
    //}
    internal class ASN1DecompressionStream : Stream
    {
        private Stream inputStream;
        private int dataLength;
        private bool endOfCompressedStream;
        private byte[] internalBuffer;
        private uint adler32Checksum;
        private byte[] adler32ChecksumBytes = new byte[4];

        public ASN1DecompressionStream(Stream inputMessageStream)
        {
            inputStream = inputMessageStream;
            ReadASN1Headers(inputStream);
            dataLength = ReadASN1DataLengthAndFillBuffer(inputStream, true);
        }

        public uint Adler32Checksum => adler32Checksum;

        public override bool CanSeek => false;

        public override bool CanRead => true;

        public override bool CanWrite => false;

        public override long Length => 0;

        public override long Position
        {
            get => 0;
            set => throw new NotImplementedException();
        }

        internal int ReadASN1DataLengthAndFillBuffer(Stream inStream, bool includeDataLengthTrailer)
        {
            byte[] buffer = new byte[AS2Constants.CmsTrailer.Length];
            int count = 0;
            int num1;
            if (AS2Constants.DataLengthHeader.Length == (num1 = ReadASN1Header(inStream, ref buffer, AS2Constants.DataLengthHeader.Length)))
            {
                if (buffer[0] == AS2Constants.DataLengthHeader[0])
                {
                    count = buffer[1];
                    if ((buffer[1] & 128) == 128)
                    {
                        int num2 = buffer[1] & 15;
                        if (num2 > 4)
                            throw new Exception("UnsupportedOctetStringLengthEncountered");
                        count = inStream.ReadByte();
                        if (-1 == count)
                            return 0;
                        for (int index = 0; index < num2 - 1; ++index)
                            count = count << 8 | inStream.ReadByte();
                    }
                    if (includeDataLengthTrailer)
                    {
                        if (AS2Constants.ZippedStreamHeader.Length != inStream.Read(buffer, 0, AS2Constants.ZippedStreamHeader.Length))
                            throw new Exception("InvalidOrMissingDataHeaderEncountered");
                        count -= 2;
                    }
                }
                else
                {
                    if (buffer[0] != AS2Constants.CmsTrailer[0])
                        throw new Exception("InvalidASN1CompressedStructureEncountered");
                    inStream.Read(buffer, AS2Constants.DataLengthHeader.Length, AS2Constants.CmsTrailer.Length - AS2Constants.DataLengthHeader.Length);
                    if (!HeaderUtils.ASN1HeaderMatches(buffer, AS2Constants.CmsTrailer, AS2Constants.CmsTrailerMatchBoundaries))
                        throw new Exception("InvalidOrMissingASN1TrailingBytes");
                    endOfCompressedStream = true;
                    return 0;
                }
            }
            if (num1 == 0)
                return 0;
            if (count == 0)
                return ReadASN1DataLengthAndFillBuffer(inStream, false);
            internalBuffer = new byte[count];
            int offset = inStream.Read(internalBuffer, 0, count);
            int num3;
            while (offset < count && (num3 = inStream.Read(internalBuffer, offset, count - offset)) != 0)
                offset += num3;
            if (offset >= adler32ChecksumBytes.Length)
            {
                int index1 = offset - adler32ChecksumBytes.Length;
                for (int index2 = 0; index2 < adler32ChecksumBytes.Length; ++index2)
                {
                    adler32ChecksumBytes[index2] = internalBuffer[index1];
                    ++index1;
                }
            }
            else
            {
                int index3 = 0;
                for (int index4 = offset; index4 < adler32ChecksumBytes.Length; ++index4)
                {
                    adler32ChecksumBytes[index3] = adler32ChecksumBytes[index4];
                    ++index3;
                }
                int index5 = adler32ChecksumBytes.Length - offset;
                int index6 = 0;
                while (index5 < adler32ChecksumBytes.Length)
                {
                    adler32ChecksumBytes[index5] = internalBuffer[index6];
                    ++index5;
                    ++index6;
                }
            }
            adler32Checksum = (uint)adler32ChecksumBytes[0] << 24;
            adler32Checksum |= (uint)adler32ChecksumBytes[1] << 16;
            adler32Checksum |= (uint)adler32ChecksumBytes[2] << 8;
            adler32Checksum |= adler32ChecksumBytes[3];
            return count;
        }

        internal int ReadASN1Header(Stream inStream, ref byte[] buffer, int length)
        {
            int index = 0;
            for (; index < length; ++index)
            {
                int num = inStream.ReadByte();
                if (num != -1)
                    buffer[index] = (byte)num;
                else
                    break;
            }
            return index;
        }

        internal void ScanForOIDBoundary(Stream inStream)
        {
            switch (inStream.ReadByte())
            {
                case 48:
                case 160:
                    int num = inStream.ReadByte() & 15;
                    for (int index = 0; index < num; ++index)
                        inStream.ReadByte();
                    break;
            }
        }

        internal void ReadASN1Headers(Stream inStream)
        {
            byte[] numArray1 = new byte[AS2Constants.CompressedDataOID.Length];
            ScanForOIDBoundary(inStream);
            inStream.Read(numArray1, 0, numArray1.Length);
            if (!HeaderUtils.ASN1HeaderMatches(numArray1, AS2Constants.CompressedDataOID, AS2Constants.CompressedDataOIDMatchBoundaries))
                throw new Exception("InvalidOrMissingCompressedDataOIDEncountered");
            byte[] buffer = new byte[AS2Constants.ZlibCompressionLeadBytes.Length];
            ScanForOIDBoundary(inStream);
            ScanForOIDBoundary(inStream);
            inStream.Read(buffer, 0, buffer.Length);
            byte[] numArray2 = new byte[AS2Constants.ZlibCompressionOID.Length];
            inStream.Read(numArray2, 0, numArray2.Length);
            if (!HeaderUtils.ASN1HeaderMatches(numArray2, AS2Constants.ZlibCompressionOID, AS2Constants.ZlibCompressionOIDMatchBoundaries))
                throw new Exception("InvalidOrMissingZLibOIDEncountered");
            if ((byte)inStream.ReadByte() == AS2Constants.OptionalHeader[0])
            {
                if ((byte)inStream.ReadByte() != AS2Constants.OptionalHeader[1])
                    throw new Exception("InvalidOptionalZLibFieldEncountered");
            }
            else
                inStream.Seek(-1L, SeekOrigin.Current);
            byte[] numArray3 = new byte[AS2Constants.DataOID.Length];
            ScanForOIDBoundary(inStream);
            inStream.Read(numArray3, 0, numArray3.Length);
            if (!HeaderUtils.ASN1HeaderMatches(numArray3, AS2Constants.DataOID, AS2Constants.DataOIDMatchBoundaries))
                throw new Exception("InvalidOrMissingDataOIDEncountered");
            ScanForOIDBoundary(inStream);
            int num;
            if (AS2Constants.DataTrailBytes[2] == (num = inStream.ReadByte()))
            {
                if (AS2Constants.DataTrailBytes[3] != inStream.ReadByte())
                    throw new Exception("InvalidOrMissingDataOIDEncountered");
            }
            else
            {
                if (num != AS2Constants.StartOfCompressedMessageBoundary)
                    throw new Exception("InvalidOrMissingDataOIDEncountered");
                inStream.Seek(-1L, SeekOrigin.Current);
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (endOfCompressedStream)
                return inputStream.Read(buffer, 0, count);
            if (dataLength == 0)
            {
                dataLength = ReadASN1DataLengthAndFillBuffer(inputStream, false);
                if (dataLength == 0)
                    return 0;
            }
            int length = Math.Min(count, dataLength);
            Array.Copy(internalBuffer, internalBuffer.Length - dataLength, buffer, offset, length);
            dataLength -= length;
            return length;
        }

        public override void SetLength(long value) => throw new NotSupportedException();

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        public override void Flush() => throw new NotImplementedException();
    }
}


