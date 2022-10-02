using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAppAS2.Helpers.BziRef
{
    internal class AS2Constants
    {
        internal const string AS2ImplementationVersion = "1.2";
        internal const string MimeImplementationVersion = "1.0";
        internal const string AS2ComponentsVersion = "1.0";
        internal const string BTSSystemNamespace = "http://schemas.microsoft.com/BizTalk/2003/system-properties";
        internal const string TreatEPMSuspendAsSuccess = "TreatEPMSuspendAsSuccess";
        internal const string WasSolicitResponse = "WasSolicitResponse";
        internal const string IgnoreSslCertificateNameMismatchErrorsMessageContextPropertyName = "IgnoreSslCertificateNameMismatchErrors";
        internal const string ExpectHttp100ContinueContextPropertyName = "ExpectHttp100Continue";
        internal const string KeepHttpConnectionAliveContextPropertyName = "KeepAlive";
        internal const string ActualRetryCount = "ActualRetryCount";
        internal const string OutboundSignatureCertificate = "OutboundSignatureCertificate";
        internal const string Disposition = "Disposition";
        internal const string DispositionNotificationOptions = "Disposition-Notification-Options";
        internal const string DispositionNotificationTo = "Disposition-Notification-To";
        internal const string ReceiptDeliveryOption = "Receipt-Delivery-Option";
        internal const string AS2From = "AS2-From";
        internal const string AS2To = "AS2-To";
        internal const string AS2ToAgreementAlias = "AS2To";
        internal const string HostAS2To = "BizTalk";
        internal const string AS2Version = "AS2-Version";
        internal const string AS2Date = "date";
        internal const string ContentDescription = "Content-Description";
        internal const string ContentType = "Content-Type";
        internal const string ContentLength = "Content-Length";
        internal const string ContentTransferEncoding = "Content-Transfer-Encoding";
        internal const string ContentDisposition = "Content-Disposition";
        internal const string ContentDispositionTemplate = "attachment; filename=\"{0}\"";
        internal const string MimeVersion = "Mime-Version";
        internal const string MessageId = "Message-ID";
        internal const string ReceivedContentMic = "Received-Content-MIC";
        internal const string OriginalMessageId = "Original-Message-ID";
        internal const string FinalRecipient = "Final-Recipient";
        internal const string AcceptEncoding = "Accept-Encoding";
        internal const string EDIINTFeatures = "EDIINT-Features";
        internal const string EDIINTDeaturesMA = "multiple-attachments";
        internal const string DefaultContentType = "text/plain";
        internal const string EnvelopedDataContentType = "enveloped-data";
        internal const string MultipartMixedContentType = "multipart/mixed";
        internal const string MultipartRelatedContentType = "multipart/related";
        internal const string MultipartReportContentType = "multipart/report";
        internal const string MultipartSignedContentType = "multipart/signed";
        internal const string SignedDataContentType = "signed-data";
        internal const string CompressedDataContentType = "compressed-data";
        internal const string ContentTransferEncodingBase64 = "base64";
        internal const string ContentTransferEncodingBinary = "binary";
        internal const string ContentTransferEncodingQuotedPrintable = "quoted-printable";
        internal const string Boundary = "boundary";
        internal const string ExistingMIMESignatureType = "x-pkcs7-signature";
        internal const string MIMEVersionHeader = "Mime-Version: 1.0\r\n";
        internal const string EnvelopedDataMIMEHeaders = "Mime-Version: 1.0\r\nContent-type: application/pkcs7-mime; smime-type=enveloped-data; name=\"smime.p7m\"\r\nContent-Transfer-Encoding: binary\r\n\r\n";
        internal const string CompressedOnlyDataMIMEHeaders = "Content-type: application/pkcs7-mime; smime-type=compressed-data; name=smime.p7z\r\nContent-Transfer-Encoding: binary\r\n\r\n";
        internal const string CompressedSignedDataMIMEHeaders = "Content-type: application/pkcs7-mime; smime-type=compressed-data; name=smime.p7m\r\nContent-Transfer-Encoding: binary\r\n\r\n";
        internal const string MimePropertyIsMultipartReport = "IsMultipartReport";
        internal const string SuppressMimeVersionFromMultiPartMessage = "SuppressMimeVersionFromMultiPartMessage";
        internal const string ContentDispositionType = "ContentDispositionType";
        internal const string UseNonEncodedFilename = "UseNonEncodedFilename";
        internal const string ReportTypeSecondaryHeader = "report-type";
        internal const string DispositionNotificationSecondaryHeaderValue = "disposition-notification";
        internal const string MdnContentType = "message/disposition-notification";
        internal const string DispNotOptProto = "signed-receipt-protocol";
        internal const string DispNotOptMicAlg = "signed-receipt-micalg";
        internal const string Filename = "filename";
        internal const string MIMEAttachment = "attachment";
        internal const string AutogeneratedFilenameExtension = "bin";
        internal const string EdiGlobalPropertiesNamespace = "http://schemas.microsoft.com/Edi/PropertySchema";
        internal const string AS2GlobalPropertiesNamespace = "http://schemas.microsoft.com/BizTalk/2006/as2-properties";
        internal const string ReliabilityPropertiesNamespace = "http://schemas.microsoft.com/BizTalk/2006/reliability-properties";
        internal const string ReliabilityPropertyTrackingActivityId = "TrackingActivityId";
        internal const string ReceivedContentMicProp = "ReceivedContentMic";
        internal const string MicHashAlgorithmProp = "MicHashAlgorithm";
        internal const string InterchangeControlNo = "InterchangeControlNo";
        internal const string InterchangeDate = "InterchangeDate";
        internal const string InterchangeTime = "InterchangeTime";
        internal const string ReceiverID = "ReceiverID";
        internal const string ReceiverQualifier = "ReceiverQualifier";
        internal const string SenderID = "SenderID";
        internal const string SenderQualifier = "SenderQualifier";
        internal const string AgreementName = "AgreementName";
        internal const string AS2ErrorInvalidSignature = "invalid-signature";
        internal const string AS2ErrorSigningCertificateExpired = "signing-certificate-has-expired";
        internal const string AS2ErrorDecryptionCertificateExpired = "decryption-certificate-has-expired";
        internal const string AS2ErrorAuthenticationFailed = "authentication-failed";
        internal const string AS2ErrorUnsupportedHashAlgorithm = "hash-algorithm-is-not-supported";
        internal const string AS2ErrorSigningCertificateNotFound = "signing-certificate-was-not-found";
        internal const string AS2ErrorDecryptionCertificateNotFound = "decryption-certificate-was-not-found";
        internal const string AS2ErrorPrivateCertificateKeyNotFound = "private-certificate-key-was-not-found";
        internal const string AS2ErrorSigningCertificateHasInvalidTrustChain = "signing-certificate-has-an-invalid-trust-chain";
        internal const string AS2ErrorSigningCertificateRevocationStatusIsUnknown = "signing-certificate-revocation-status-is-unknown";
        internal const string AS2ErrorSigningCertificateHasBeenRevoked = "signing-certificate-has-been-revoked";
        internal const string AS2ErrorUnableToDecodeMessage = "unable-to-decode-message";
        internal const string AS2ErrorGeneralProcessingErrorEncountered = "general-processing-error-encountered";
        internal const string AS2ErrorEdiIntProcessingErrorEncountered = "EDIINT-processing-error-encountered";
        internal const string AS2DispositionTypeExtentionAuthenticationFailed = "authentication-failed";
        internal const string AS2DispositionTypeExtentionDecompressionFailed = "decompression-failed";
        internal const string AS2DispositionTypeExtentionDecryptionFailed = "decryption-failed";
        internal const string AS2DispositionTypeExtentionInsufficientMessageSecurity = "insufficient-message-security";
        internal const string AS2DispositionTypeExtentionIntegrityCheckFailed = "integrity-check-failed";
        internal const string AS2DispositionTypeExtentionUnexpectedProcessingError = "unexpected-processing-error";
        internal const string AS2DispositionTypeExtentionProcessed = "processed";
        internal const string AS2DispositionTypeError = "error";
        internal const string AS2DispositionTypeWarning = "warning";
        internal const string AS2DispositionTypeProcessed = "processed";
        internal const string AS2MicAlgorithmName = "micalg";
        internal const string AS2DefaultMicAlgorithm = "sha1";
        internal const string AS2MicAlgorithmSha1 = "sha1";
        internal const string AS2MicAlgorithmMd5 = "md5";
        internal const string AS2MicAlgorithmSha2_256 = "sha256";
        internal const string AS2MicAlgorithmSha2_384 = "sha384";
        internal const string AS2MicAlgorithmSha2_512 = "sha512";
        internal const string AS2MicAlgorithmSha2_256_Oid = "2.16.840.1.101.3.4.2.1";
        internal const string AS2MicAlgorithmSha2_384_Oid = "2.16.840.1.101.3.4.2.2";
        internal const string AS2MicAlgorithmSha2_512_Oid = "2.16.840.1.101.3.4.2.3";
        internal static byte[] CRLFCRLF = new byte[4]
        {
       13,
       10,
       13,
       10
        };
        internal static byte[] CRLF = new byte[2]
        {
       13,
       10
        };
        internal static byte StartOfCompressedMessageBoundary = 4;
        internal static byte[] CompressedDataLeadBytes = new byte[2]
        {
       48,
       128
        };
        internal static int[] CompressedDataLeadBytesMatchBoundaries = new int[2]
        {
      0,
      CompressedDataLeadBytes.Length - 1
        };
        internal static byte[] CompressedDataOID = new byte[13]
        {
       6,
       11,
       42,
       134,
       72,
       134,
       247,
       13,
       1,
       9,
       16,
       1,
       9
        };
        internal static int[] CompressedDataOIDMatchBoundaries = new int[2]
        {
      0,
      CompressedDataOID.Length - 1
        };
        internal static byte[] CompressedDataTrailBytes = new byte[4]
        {
       160,
       128,
       48,
       128
        };
        internal static int[] CompressedDataTrailBytesMatchBoundaries = new int[2]
        {
      0,
      CompressedDataTrailBytes.Length - 1
        };
        internal static byte[] ZlibCompressionLeadBytes = new byte[5]
        {
       2,
       1,
       0,
       48,
       15
        };
        internal static int[] ZlibCompressionLeadBytesMatchBoundaries = new int[2]
        {
      0,
      3
        };
        internal static byte[] ZlibCompressionOID = new byte[13]
        {
       6,
       11,
       42,
       134,
       72,
       134,
       247,
       13,
       1,
       9,
       16,
       3,
       8
        };
        internal static int[] ZlibCompressionOIDMatchBoundaries = new int[2]
        {
      0,
      ZlibCompressionOID.Length - 1
        };
        internal static byte[] OptionalHeader = new byte[2]
        {
       5,
       0
        };
        internal static int[] OptionalHeaderMatchBoundaries = new int[2]
        {
      0,
      OptionalHeader.Length - 1
        };
        internal static byte[] DataLeadBytes = new byte[2]
        {
       48,
       128
        };
        internal static int[] DataLeadBytesMatchBoundaries = new int[2]
        {
      0,
      DataLeadBytes.Length - 1
        };
        internal static byte[] DataOID = new byte[11]
        {
       6,
       9,
       42,
       134,
       72,
       134,
       247,
       13,
       1,
       7,
       1
        };
        internal static int[] DataOIDMatchBoundaries = new int[2]
        {
      0,
      DataOID.Length - 1
        };
        internal static byte[] DataTrailBytes = new byte[4]
        {
       160,
       128,
       36,
       128
        };
        internal static int[] DataTrailBytesMatchBoundaries = new int[2]
        {
      0,
      DataTrailBytes.Length - 1
        };
        internal static byte[] DataLengthHeader = new byte[2]
        {
       4,
       130
        };
        internal static byte DataLengthTag = 128;
        internal static int[] DataLengthHeaderMatchBoundaries = new int[2]
        {
      0,
      DataLengthHeader.Length - 1
        };
        internal static byte[] ZippedStreamHeader = new byte[2]
        {
       120,
       218
        };
        internal static int[] ZippedStreamHeaderMatchBoundaries = new int[2]
        {
      0,
      ZippedStreamHeader.Length - 1
        };
        internal static byte[] CmsTrailer = new byte[12];
        internal static int[] CmsTrailerMatchBoundaries = new int[2]
        {
      0,
      CmsTrailer.Length - 1
        };
        internal static byte[] PKCSSignedDataOID = new byte[11]
        {
       6,
       9,
       42,
       134,
       72,
       134,
       247,
       13,
       1,
       7,
       2
        };
        internal static int[] PKCSSignedDataOIDMatchBoundaries = new int[2]
        {
      0,
      PKCSSignedDataOID.Length - 1
        };
        internal static byte[] OIWOID = new byte[7]
        {
       6,
       5,
       43,
       14,
       3,
       2,
       26
        };
        internal static int[] OIWOIDMatchBoundaries = new int[2]
        {
      0,
      OIWOID.Length - 1
        };
        internal static byte[] PKCS7DataOID = new byte[11]
        {
       6,
       9,
       42,
       134,
       72,
       134,
       247,
       13,
       1,
       7,
       1
        };
        internal static int[] PKCS7DataOIDMatchBoundaries = new int[2]
        {
      0,
      PKCS7DataOID.Length - 1
        };

        private AS2Constants()
        {
        }
    }
}


