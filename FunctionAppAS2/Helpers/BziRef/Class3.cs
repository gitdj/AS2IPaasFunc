using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAppAS2.Helpers.BziRef
{
    internal class HeaderUtils
    {
        internal static readonly char[] s_ColonOrNL = new char[2]
        {
      ':',
      '\n'
        };

        //internal static Stream PrependAS2Headers(
        //  Stream stm,
        //  IDictionary headers,
        //  string contentType,
        //  bool messageSigned)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    string header1 = HeaderUtils.GetHeader(headers, "Content-Transfer-Encoding");
        //    string header2 = HeaderUtils.GetHeader(headers, "Mime-Version");
        //    string header3 = HeaderUtils.GetHeader(headers, "Content-Disposition");
        //    string header4 = HeaderUtils.GetHeader(headers, "Content-Description");
        //    if (!string.IsNullOrEmpty(header2))
        //        HeaderUtils.WriteHeader(sb, "Mime-Version", header2);
        //    if (!string.IsNullOrEmpty(contentType))
        //    {
        //        HeaderUtils.WriteHeader(sb, "Content-Type", contentType);
        //        if (string.IsNullOrEmpty(header1) && contentType.Contains("enveloped-data"))
        //            HeaderUtils.WriteHeader(sb, "Content-Transfer-Encoding", "binary");
        //    }
        //    if (!string.IsNullOrEmpty(header1))
        //        HeaderUtils.WriteHeader(sb, "Content-Transfer-Encoding", header1);
        //    if (messageSigned && !string.IsNullOrEmpty(header4))
        //        HeaderUtils.WriteHeader(sb, "Content-Description", header4);
        //    if (!string.IsNullOrEmpty(header3))
        //        HeaderUtils.WriteHeader(sb, "Content-Disposition", header3);
        //    sb.Append("\r\n");
        //    MemoryStream memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(sb.ToString()));
        //    memoryStream.Seek(0L, SeekOrigin.Begin);
        //    PrependingStream prependingStream = new PrependingStream(stm);
        //    prependingStream.Prefix = (Stream)memoryStream;
        //    Trace.Tracer.TraceMessage(64U, "HeaderUtils:PrependHeaders exiting.");
        //    return (Stream)prependingStream;
        //}

        //        internal static Stream PrependAS2Headers(Stream stm, string headers)
        //        {
        //            Trace.Tracer.TraceMessage(64U, "HeaderUtils:PrependHeaders entered.");
        //            MemoryStream memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(headers));
        //            memoryStream.Seek(0L, SeekOrigin.Begin);
        //            PrependingStream prependingStream = new PrependingStream(stm);
        //            prependingStream.Prefix = (Stream)memoryStream;
        //            Trace.Tracer.TraceMessage(64U, "HeaderUtils:PrependHeaders exiting.");
        //            return (Stream)prependingStream;
        //        }

        //        internal static Stream PrependHTTPHeaders(Stream stm, string httpHeaders)
        //        {
        //            Trace.Tracer.TraceMessage(64U, "HeaderUtils:PrependHeaders entered.");
        //            StringBuilder stringBuilder = new StringBuilder();
        //            stringBuilder.Append(httpHeaders);
        //            stringBuilder.Append("\r\n");
        //            MemoryStream memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(stringBuilder.ToString()));
        //            memoryStream.Seek(0L, SeekOrigin.Begin);
        //            PrependingStream prependingStream = new PrependingStream(stm);
        //            prependingStream.Prefix = (Stream)memoryStream;
        //            Trace.Tracer.TraceMessage(64U, "HeaderUtils:PrependHeaders exiting.");
        //            return (Stream)prependingStream;
        //        }

        //        internal static void WriteHeader(StringBuilder sb, string headerName, string value)
        //        {
        //            Trace.Tracer.TraceMessage(64U, "HeaderUtils:WriteHeader entered.");
        //            sb.Append(headerName);
        //            sb.Append(": ");
        //            sb.Append(value);
        //            sb.Append("\r\n");
        //            Trace.Tracer.TraceMessage(64U, "HeaderUtils:WriteHeader exiting.");
        //        }

        //        internal static string GetHeader(IDictionary headers, string headerName)
        //        {
        //            Trace.Tracer.TraceMessage(64U, "HeaderUtils:GetHeader called.");
        //            foreach (DictionaryEntry header in headers)
        //            {
        //                if (string.Compare(header.Key as string, headerName, StringComparison.OrdinalIgnoreCase) == 0)
        //                    return HeaderUtils.NormalizeHeaderValue(header.Value as string);
        //            }
        //            return (string)null;
        //        }

        //        internal static string GetHeaderKey(IDictionary headers, string headerName)
        //        {
        //            Trace.Tracer.TraceMessage(64U, "HeaderUtils:GetHeaderKey called.");
        //            foreach (DictionaryEntry header in headers)
        //            {
        //                if (string.Compare(header.Key as string, headerName, StringComparison.OrdinalIgnoreCase) == 0)
        //                    return header.Key as string;
        //            }
        //            return (string)null;
        //        }

        //        internal static IDictionary LoadHeaders(IBaseMessageContext messageContext)
        //        {
        //            Trace.Tracer.TraceMessage(64U, "HeaderUtils:LoadHeaders entered.");
        //            string rawHeaders = messageContext.Read(EdiIntProperties.inboundHttpHeaders.Name.Name, EdiIntProperties.inboundHttpHeaders.Name.Namespace) as string;
        //            Trace.Tracer.TraceMessage(64U, "AS2 Decoder:LoadHeaders Inbound HTTP Headers: {0}", (object)rawHeaders);
        //            Trace.Tracer.TraceMessage(64U, "HeaderUtils:LoadHeaders exiting.");
        //            return HeaderUtils.ParseHeaders(rawHeaders);
        //        }

        //        internal static string ParseMimeHeaders(Stream message) => HeaderUtils.ParseMimeHeaders(message, true);

        //        internal static string ParseMimeHeaders(Stream message, bool validateContentType)
        //        {
        //            bool flag1 = false;
        //            long position = message.Position;
        //            byte[] numArray = new byte[8192];
        //            int count = 0;
        //            int offset = 0;
        //            int num;
        //            while (offset < numArray.Length && (num = message.Read(numArray, offset, numArray.Length - offset)) != 0)
        //                offset += num;
        //            while (count <= offset - 1)
        //            {
        //                ++count;
        //                if (count > 4 && numArray[count - 4] == (byte)13 && numArray[count - 3] == (byte)10 && numArray[count - 2] == (byte)13 && numArray[count - 1] == (byte)10)
        //                {
        //                    flag1 = true;
        //                    break;
        //                }
        //            }
        //            if (flag1)
        //            {
        //                string mimeHeaders = Encoding.ASCII.GetString(numArray, 0, count);
        //                if (validateContentType)
        //                {
        //                    bool flag2 = false;
        //                    string str1 = mimeHeaders;
        //                    string[] separator = new string[1] { "\r\n" };
        //                    foreach (string str2 in str1.Split(separator, StringSplitOptions.RemoveEmptyEntries))
        //                    {
        //                        char[] chArray = new char[1] { ':' };
        //                        if (string.Compare("Content-Type", str2.Split(chArray)[0], StringComparison.OrdinalIgnoreCase) == 0)
        //                            flag2 = true;
        //                    }
        //                    if (flag2)
        //                    {
        //                        message.Seek(position + (long)count, SeekOrigin.Begin);
        //                        return mimeHeaders;
        //                    }
        //                }
        //                else
        //                {
        //                    message.Seek(position + (long)count, SeekOrigin.Begin);
        //                    return mimeHeaders;
        //                }
        //            }
        //            message.Seek(position, SeekOrigin.Begin);
        //            return (string)null;
        //        }

        //        internal static IDictionary ParseHeaders(string rawHeaders)
        //        {
        //            IDictionary headers = (IDictionary)new Hashtable();
        //            string str1 = rawHeaders;
        //            int num1 = !string.IsNullOrEmpty(str1) ? str1.Length : 0;
        //            int startIndex = 0;
        //            while (startIndex < num1)
        //            {
        //                int index = str1.IndexOfAny(HeaderUtils.s_ColonOrNL, startIndex);
        //                if (index >= 0)
        //                {
        //                    if (str1[index] == '\n')
        //                        startIndex = index + 1;
        //                    else if (index == startIndex)
        //                    {
        //                        ++startIndex;
        //                    }
        //                    else
        //                    {
        //                        string name = str1.Substring(startIndex, index - startIndex).Trim();
        //                        int num2 = str1.IndexOf('\n', index + 1);
        //                        if (num2 < 0)
        //                            num2 = num1;
        //                        while (num2 < num1 - 1 && (str1[num2 + 1] == ' ' || str1[num2 + 1] == '\t'))
        //                        {
        //                            num2 = str1.IndexOf('\n', num2 + 1);
        //                            if (num2 < 0)
        //                                num2 = num1;
        //                        }
        //                        string str2 = str1.Substring(index + 1, num2 - index - 1).Trim();
        //                        if (!headers.Contains((object)HeaderUtils.NormalizeHeaderName(name)))
        //                            headers[(object)HeaderUtils.NormalizeHeaderName(name)] = (object)str2;
        //                        else if (headers[(object)HeaderUtils.NormalizeHeaderName(name)] as string != str2)
        //                            throw new EdiIntException(EdiIntException.IntegrityCheckFailedError);
        //                        startIndex = num2 + 1;
        //                    }
        //                }
        //                else
        //                    break;
        //            }
        //            if (headers[(object)"AS2-From"] != null)
        //            {
        //                bool isQuoted = HeaderUtils.IsQuoted(headers[(object)"AS2-From"] as string);
        //                if (isQuoted)
        //                    headers[(object)"AS2-From"] = (object)HeaderUtils.StripQuotes(headers[(object)"AS2-From"] as string);
        //                if (!AS2Utils.IsValidAS2Name(headers[(object)"AS2-From"] as string, isQuoted))
        //                    throw new EdiIntException(EdiIntException.InvalidAS2FromNameEncounteredError, new object[1]
        //                    {
        //            (object) (headers[(object) "AS2-From"] as string)
        //                    });
        //            }
        //            if (headers[(object)"AS2-To"] != null)
        //            {
        //                bool isQuoted = HeaderUtils.IsQuoted(headers[(object)"AS2-To"] as string);
        //                if (isQuoted)
        //                    headers[(object)"AS2-To"] = (object)HeaderUtils.StripQuotes(headers[(object)"AS2-To"].ToString());
        //                if (!AS2Utils.IsValidAS2Name(headers[(object)"AS2-To"] as string, isQuoted))
        //                    throw new EdiIntException(EdiIntException.InvalidAS2ToNameEncounteredError, new object[1]
        //                    {
        //            (object) (headers[(object) "AS2-To"] as string)
        //                    });
        //            }
        //            return headers;
        //        }

        //        internal static bool IsQuoted(string str) => !string.IsNullOrEmpty(str) && str.Length > 1 && str.StartsWith("\"", StringComparison.OrdinalIgnoreCase) && str.EndsWith("\"", StringComparison.OrdinalIgnoreCase);

        //        internal static string StripQuotes(string str) => !HeaderUtils.IsQuoted(str) ? str : str.Substring(1, str.Length - 2);

        //        internal static string UnfoldHttpHeaderValue(string str) => str.Replace("\r\n", string.Empty).Replace('\t', ' ');

        //        internal static string GetFilenameFromContentDispositionHeader(
        //          string contentDispositionHeader,
        //          IPipelineContext context)
        //        {
        //            MIME_SMIME_Decoder mimeSmimeDecoder = new MIME_SMIME_Decoder();
        //            IBaseMessageFactory messageFactory = context.GetMessageFactory();
        //            IBaseMessage message = messageFactory.CreateMessage();
        //            IBaseMessagePart messagePart = messageFactory.CreateMessagePart();
        //            contentDispositionHeader = string.Format((IFormatProvider)CultureInfo.InvariantCulture, "Content-Disposition: {0}\r\nContent-Type: text/plain\r\nContent-Transfer-Encoding: binary\r\nMime-Version: 1.0\r\n\r\nMessage", (object)contentDispositionHeader);
        //            MemoryStream memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(contentDispositionHeader));
        //            messagePart.Data = (Stream)memoryStream;
        //            message.AddPart("body", messagePart, true);
        //            return mimeSmimeDecoder.Execute(context, message).BodyPart.PartProperties.Read(EdiIntProperties.mimeFileName.Name.Name, EdiIntProperties.mimeFileName.Name.Namespace) as string;
        //        }

        //        internal static string NormalizeHeaderName(string name)
        //        {
        //            string str = name;
        //            if (string.Compare(name, "Disposition", StringComparison.OrdinalIgnoreCase) == 0)
        //                return "Disposition";
        //            if (string.Compare(name, "Disposition-Notification-Options", StringComparison.OrdinalIgnoreCase) == 0)
        //                return "Disposition-Notification-Options";
        //            if (string.Compare(name, "Disposition-Notification-To", StringComparison.OrdinalIgnoreCase) == 0)
        //                return "Disposition-Notification-To";
        //            if (string.Compare(name, "Receipt-Delivery-Option", StringComparison.OrdinalIgnoreCase) == 0)
        //                return "Receipt-Delivery-Option";
        //            if (string.Compare(name, "AS2-From", StringComparison.OrdinalIgnoreCase) == 0)
        //                return "AS2-From";
        //            if (string.Compare(name, "AS2-To", StringComparison.OrdinalIgnoreCase) == 0)
        //                return "AS2-To";
        //            if (string.Compare(name, "date", StringComparison.OrdinalIgnoreCase) == 0)
        //                return "date";
        //            if (string.Compare(name, "AS2-Version", StringComparison.OrdinalIgnoreCase) == 0)
        //                return "AS2-Version";
        //            if (string.Compare(name, "Content-Type", StringComparison.OrdinalIgnoreCase) == 0)
        //                return "Content-Type";
        //            if (string.Compare(name, "Content-Transfer-Encoding", StringComparison.OrdinalIgnoreCase) == 0)
        //                return "Content-Transfer-Encoding";
        //            if (string.Compare(name, "Content-Disposition", StringComparison.OrdinalIgnoreCase) == 0)
        //                return "Content-Disposition";
        //            if (string.Compare(name, "text/plain", StringComparison.OrdinalIgnoreCase) == 0)
        //                return "text/plain";
        //            if (string.Compare(name, "multipart/report", StringComparison.OrdinalIgnoreCase) == 0)
        //                return "multipart/report";
        //            if (string.Compare(name, "Mime-Version", StringComparison.OrdinalIgnoreCase) == 0)
        //                return "Mime-Version";
        //            if (string.Compare(name, "Message-ID", StringComparison.OrdinalIgnoreCase) == 0)
        //                return "Message-ID";
        //            if (string.Compare(name, "Received-Content-MIC", StringComparison.OrdinalIgnoreCase) == 0)
        //                return "Received-Content-MIC";
        //            if (string.Compare(name, "Original-Message-ID", StringComparison.OrdinalIgnoreCase) == 0)
        //                return "Original-Message-ID";
        //            return string.Compare(name, "Accept-Encoding", StringComparison.OrdinalIgnoreCase) == 0 ? "Accept-Encoding" : str;
        //        }

        //        internal static string SerializeHeaders(IDictionary dict)
        //        {
        //            StringBuilder stringBuilder = new StringBuilder(100);
        //            foreach (DictionaryEntry dictionaryEntry in dict)
        //            {
        //                stringBuilder.Append(HeaderUtils.SerializeOneHeader(dictionaryEntry.Key as string, (string)dictionaryEntry.Value));
        //                stringBuilder.Append("\r\n");
        //            }
        //            return stringBuilder.ToString();
        //        }

        //        internal static string SerializeOneHeader(string header, string value) => header + ": " + value;

        //        internal static string NormalizeHeaderValue(string headerValue) => string.IsNullOrEmpty(headerValue) ? string.Empty : headerValue.Trim();

        //        internal static string GetMultiAttributeFromHeader(string headerValue, string attrName)
        //        {
        //            if (string.IsNullOrEmpty(headerValue))
        //                return (string)null;
        //            int length1 = headerValue.Length;
        //            int num1;
        //            if (string.IsNullOrEmpty(attrName))
        //            {
        //                num1 = 0;
        //            }
        //            else
        //            {
        //                int length2 = attrName.Length;
        //                int startIndex;
        //                for (startIndex = 0; startIndex < length1; startIndex = startIndex + length2 + length2)
        //                {
        //                    startIndex = CultureInfo.InvariantCulture.CompareInfo.IndexOf(headerValue, attrName, startIndex, CompareOptions.IgnoreCase);
        //                    if (startIndex >= 0 && startIndex + length2 < length1)
        //                    {
        //                        char c1 = headerValue[startIndex - 1];
        //                        char c2 = headerValue[startIndex + length2];
        //                        if ((c1 == ';' || char.IsWhiteSpace(c1)) && (c2 == '=' || char.IsWhiteSpace(c2)))
        //                            break;
        //                    }
        //                    else
        //                        break;
        //                }
        //                if (startIndex < 0 || startIndex >= length1)
        //                    return (string)null;
        //                int index = startIndex + length2;
        //                while (index < length1 && char.IsWhiteSpace(headerValue[index]))
        //                    ++index;
        //                if (index >= length1 || headerValue[index] != '=')
        //                    return (string)null;
        //                num1 = index + 1;
        //                while (num1 < length1 && char.IsWhiteSpace(headerValue[num1]))
        //                    ++num1;
        //                if (num1 >= length1)
        //                    return (string)null;
        //            }
        //            int num2 = num1;
        //            int num3;
        //            while (true)
        //            {
        //                num3 = headerValue.IndexOf(';', num2 + 1);
        //                if (num3 != -1)
        //                {
        //                    if (num3 > 0 && headerValue[num3 - 1] == '\\')
        //                        num2 = num3;
        //                    else
        //                        goto label_26;
        //                }
        //                else
        //                    break;
        //            }
        //            num3 = headerValue.Length;
        //label_26:
        //            return headerValue.Substring(num1, num3 - num1).Trim();
        //        }

        //        internal static string GetAttributeFromHeader(string headerValue, string attrName)
        //        {
        //            if (string.IsNullOrEmpty(headerValue))
        //                return (string)null;
        //            int length1 = headerValue.Length;
        //            int length2 = attrName.Length;
        //            int startIndex;
        //            for (startIndex = 1; startIndex < length1; startIndex = startIndex + length2 + length2)
        //            {
        //                startIndex = CultureInfo.InvariantCulture.CompareInfo.IndexOf(headerValue, attrName, startIndex, CompareOptions.IgnoreCase);
        //                if (startIndex >= 0 && startIndex + length2 < length1)
        //                {
        //                    char c1 = startIndex == 0 ? ';' : headerValue[startIndex - 1];
        //                    char c2 = headerValue[startIndex + length2];
        //                    if ((c1 == ';' || char.IsWhiteSpace(c1)) && (c2 == '=' || char.IsWhiteSpace(c2)))
        //                        break;
        //                }
        //                else
        //                    break;
        //            }
        //            if (startIndex < 0 || startIndex >= length1)
        //                return (string)null;
        //            int index1 = startIndex + length2;
        //            while (index1 < length1 && char.IsWhiteSpace(headerValue[index1]))
        //                ++index1;
        //            if (index1 >= length1 || headerValue[index1] != '=')
        //                return (string)null;
        //            int num1 = index1 + 1;
        //            while (num1 < length1 && char.IsWhiteSpace(headerValue[num1]))
        //                ++num1;
        //            if (num1 >= length1)
        //                return (string)null;
        //            string attributeFromHeader;
        //            if (num1 < length1 && headerValue[num1] == '"')
        //            {
        //                if (num1 == length1 - 1)
        //                    return (string)null;
        //                int num2 = headerValue.IndexOf('"', num1 + 1);
        //                if (num2 < 0 || num2 == num1 + 1)
        //                    return (string)null;
        //                attributeFromHeader = headerValue.Substring(num1 + 1, num2 - num1 - 1).Trim();
        //            }
        //            else
        //            {
        //                int index2 = num1;
        //                while (index2 < length1 && headerValue[index2] != ' ')
        //                    ++index2;
        //                if (index2 == num1)
        //                    return (string)null;
        //                attributeFromHeader = headerValue.Substring(num1, index2 - num1).Trim();
        //            }
        //            return attributeFromHeader;
        //        }

        //        internal static void ValidateAS2HttpHeaders(IDictionary headers)
        //        {
        //            if (string.IsNullOrEmpty(headers[(object)"AS2-From"] as string))
        //                throw new EdiIntException(EdiIntException.MissingAS2FromHeaderError);
        //            if (string.IsNullOrEmpty(headers[(object)"AS2-To"] as string))
        //                throw new EdiIntException(EdiIntException.MissingAS2ToHeaderError);
        //            if (string.Compare(headers[(object)"AS2-From"] as string, headers[(object)"AS2-To"] as string, StringComparison.Ordinal) == 0)
        //                throw new EdiIntException(EdiIntException.AS2FromMatchesAS2ToError);
        //            if (!string.IsNullOrEmpty(headers[(object)"Receipt-Delivery-Option"] as string) && !AS2Utils.IsValidHttpUrl(headers[(object)"Receipt-Delivery-Option"] as string) && !AS2Utils.IsValidSmtpUrl(headers[(object)"Receipt-Delivery-Option"] as string))
        //                throw new EdiIntException(EdiIntException.InvalidReceiptDeliveryOptionError, new object[2]
        //                {
        //          (object) (headers[(object) "Receipt-Delivery-Option"] as string),
        //          (object) string.Format((IFormatProvider) CultureInfo.InvariantCulture, StringResources.GetString("AS2HeaderInformation"), (object) (headers[(object) "AS2-From"] as string), (object) (headers[(object) "AS2-To"] as string))
        //                });
        //        }

        //        internal static DispositionModifierEXTypeDescription GetDispositionModifierEXTypeDescription(
        //          string dispositionType)
        //        {
        //            if (-1 != dispositionType.IndexOf("authentication-failed", StringComparison.OrdinalIgnoreCase))
        //                return DispositionModifierEXTypeDescription.AuthenticationFailed;
        //            if (-1 != dispositionType.IndexOf("decompression-failed", StringComparison.OrdinalIgnoreCase))
        //                return DispositionModifierEXTypeDescription.UnexpectedProcessingError;
        //            if (-1 != dispositionType.IndexOf("decryption-failed", StringComparison.OrdinalIgnoreCase))
        //                return DispositionModifierEXTypeDescription.DecryptionFailed;
        //            if (-1 != dispositionType.IndexOf("insufficient-message-security", StringComparison.OrdinalIgnoreCase))
        //                return DispositionModifierEXTypeDescription.InsufficientMessageSecurity;
        //            if (-1 != dispositionType.IndexOf("integrity-check-failed", StringComparison.OrdinalIgnoreCase))
        //                return DispositionModifierEXTypeDescription.IntegrityCheckFailed;
        //            return -1 != dispositionType.IndexOf("unexpected-processing-error", StringComparison.OrdinalIgnoreCase) || !dispositionType.Equals("processed", StringComparison.OrdinalIgnoreCase) ? DispositionModifierEXTypeDescription.UnexpectedProcessingError : DispositionModifierEXTypeDescription.NotValued;
        //        }

        //        internal static DispositionModifierEXType GetDispositionModifierType(
        //          string dispositionType)
        //        {
        //            if (-1 != dispositionType.IndexOf("error", StringComparison.OrdinalIgnoreCase))
        //                return DispositionModifierEXType.Error;
        //            return -1 != dispositionType.IndexOf("warning", StringComparison.OrdinalIgnoreCase) ? DispositionModifierEXType.Warning : DispositionModifierEXType.NotValued;
        //        }

        internal static bool ASN1HeaderMatches(
          byte[] messageBytes,
          byte[] bytesToCompare,
          int[] boundaries)
        {
            bool flag = true;
            for (int index = 0; index < boundaries.Length / 2 && flag; index += 2)
            {
                for (int boundary = boundaries[index]; boundary <= boundaries[index + 1] && boundary < messageBytes.Length; ++boundary)
                {
                    if (messageBytes[boundary] != bytesToCompare[boundary])
                    {
                        flag = false;
                        break;
                    }
                }
            }
            return flag;
        }
    }
}
