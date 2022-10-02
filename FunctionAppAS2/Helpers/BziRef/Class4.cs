using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Org.BouncyCastle.OpenSsl;

namespace FunctionAppAS2.Helpers.BziRef
{
    public class EncryptionHelper
    {
        public static byte[] encrypt(byte[] plainTextBytes)
        {

            PemReader pr = new PemReader(
                File.OpenText(@"BZIParty1.pfx")
            );
            RsaKeyParameters keys = (RsaKeyParameters)pr.ReadObject();

            // Pure mathematical RSA implementation
            // RsaEngine eng = new RsaEngine();

            // PKCS1 v1.5 paddings
            // Pkcs1Encoding eng = new Pkcs1Encoding(new RsaEngine());

            // PKCS1 OAEP paddings
            OaepEncoding eng = new OaepEncoding(new RsaEngine());
            eng.Init(true, keys);

            int length = plainTextBytes.Length;
            int blockSize = eng.GetInputBlockSize();
            List<byte> cipherTextBytes = new List<byte>();
            for (int chunkPosition = 0;
                chunkPosition < length;
                chunkPosition += blockSize)
            {
                int chunkSize = Math.Min(blockSize, length - chunkPosition);
                cipherTextBytes.AddRange(eng.ProcessBlock(
                    plainTextBytes, chunkPosition, chunkSize
                ));
            }
            return cipherTextBytes.ToArray();
        }

        static string decrypt(string cipherText)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

            PemReader pr = new PemReader(
                File.OpenText("./pem_private.pem")
            );
            AsymmetricCipherKeyPair keys = (AsymmetricCipherKeyPair)pr.ReadObject();

            // Pure mathematical RSA implementation
            // RsaEngine eng = new RsaEngine();

            // PKCS1 v1.5 paddings
            // Pkcs1Encoding eng = new Pkcs1Encoding(new RsaEngine());

            // PKCS1 OAEP paddings
            OaepEncoding eng = new OaepEncoding(new RsaEngine());
            eng.Init(false, keys.Private);

            int length = cipherTextBytes.Length;
            int blockSize = eng.GetInputBlockSize();
            List<byte> plainTextBytes = new List<byte>();
            for (int chunkPosition = 0;
                chunkPosition < length;
                chunkPosition += blockSize)
            {
                int chunkSize = Math.Min(blockSize, length - chunkPosition);
                plainTextBytes.AddRange(eng.ProcessBlock(
                    cipherTextBytes, chunkPosition, chunkSize
                ));
            }
            return Encoding.UTF8.GetString(plainTextBytes.ToArray());
        }
        public static void EncryptFile(string inputFilePath, Stream outputStream, string publicKeyFilePath,
            SymmetricKeyAlgorithmTag algorithmTag = SymmetricKeyAlgorithmTag.Des, bool withIntegrityPacket = true, bool armor = false)
        {
            try
            {
                using (Stream publicKeyStream = File.OpenRead(publicKeyFilePath))
                {
                    PgpPublicKey encKey = ReadPublicKey(publicKeyStream);

                    using (MemoryStream bOut = new MemoryStream())
                    {
                        PgpCompressedDataGenerator comData = new PgpCompressedDataGenerator(CompressionAlgorithmTag.Zip);
                        //PgpUtilities.WriteFileToLiteralData(comData.Open(bOut), PgpLiteralData.Binary, new FileInfo(inputFilePath));

                        comData.Close();
                        PgpEncryptedDataGenerator cPk = new PgpEncryptedDataGenerator(algorithmTag, withIntegrityPacket, new SecureRandom());

                        cPk.AddMethod(encKey);
                        byte[] bytes = bOut.ToArray();

                        if (armor)
                            outputStream = new ArmoredOutputStream(outputStream);

                        using (Stream cOut = cPk.Open(outputStream, bytes.Length))
                        {
                            cOut.Write(bytes, 0, bytes.Length);
                        }
                    }
                }
            }
            catch (PgpException)
            {
                throw;
            }
        }

        private static PgpPublicKey ReadPublicKey(Stream inputStream)
        {
            inputStream = PgpUtilities.GetDecoderStream(inputStream);
            PgpPublicKeyRingBundle pgpPub = new PgpPublicKeyRingBundle(inputStream);

            foreach (PgpPublicKeyRing keyRing in pgpPub.GetKeyRings())
            {
                foreach (PgpPublicKey key in keyRing.GetPublicKeys())
                {
                    if (key.IsEncryptionKey)
                    {
                        return key;
                    }
                }
            }

            throw new Exception("Can't find encryption key in key ring.");
        }
    }
}