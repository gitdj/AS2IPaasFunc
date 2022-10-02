using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Org.BouncyCastle.OpenSsl;

namespace FunctionAppAS2.Helpers
{
    public struct ProxySettings
    {
        public string Name;
        public string Username;
        public string Password;
        public string Domain;
    }

    public class AS2Send
    {

        public void PostJson(object jsonReq, string partner)
        {
            
        }

        public static HttpStatusCode SendFile(Uri uri, string filename, byte[] fileData, string from, string to, ProxySettings proxySettings, int timeoutMs, string signingCertFilename, string signingCertPassword, string recipientCertFilename)
        {
            if (String.IsNullOrEmpty(filename)) throw new ArgumentNullException("filename");

            if (fileData.Length == 0) throw new ArgumentException("filedata");

            byte[] content = fileData;




            HttpClientHandler httpClientHandler = new HttpClientHandler();

            if (!String.IsNullOrEmpty(proxySettings.Name))
            {
                WebProxy proxy = new WebProxy(proxySettings.Name);

                NetworkCredential proxyCredential = new NetworkCredential();
                proxyCredential.Domain = proxySettings.Domain;
                proxyCredential.UserName = proxySettings.Username;
                proxyCredential.Password = proxySettings.Password;

                httpClientHandler = new HttpClientHandler()
                {
                    PreAuthenticate = false,
                    Credentials = proxyCredential,
                    AllowAutoRedirect = true,
                    Proxy = proxy
                };
            }

            HttpClient _httpClient = new HttpClient(httpClientHandler);
            _httpClient.BaseAddress = uri;
            _httpClient.Timeout = new TimeSpan(0, 2, 0);




            //These Headers are common to all transactions
            _httpClient.DefaultRequestHeaders.Add("Mime-Version", "1.0");
            _httpClient.DefaultRequestHeaders.Add("AS2-Version", "1.2");

            //_httpClient.DefaultRequestHeaders.UserAgent.Add("MY SENDING AGENT");

            _httpClient.DefaultRequestHeaders.Add("AS2-From", from);
            _httpClient.DefaultRequestHeaders.Add("AS2-To", to);
            _httpClient.DefaultRequestHeaders.Add("Subject", filename + " transmission.");
            _httpClient.DefaultRequestHeaders.Add("Message-Id", "<AS2_" + DateTime.Now.ToString("hhmmssddd") + ">");

            string contentType = (Path.GetExtension(filename) == ".xml") ? "application/xml" : "application/EDIFACT";

            bool encrypt = !string.IsNullOrEmpty(recipientCertFilename);
            bool sign = !string.IsNullOrEmpty(signingCertFilename);

            if (!sign && !encrypt)
            {
                _httpClient.DefaultRequestHeaders.Add("Content-Transfer-Encoding", "binary");
                _httpClient.DefaultRequestHeaders.Add("Content-Disposition", "inline; filename=\"" + filename + "\"");
            }
            if (sign)
            {
                // Wrap the file data with a mime header
                content = AS2MIMEUtilities.CreateMessage(contentType, "binary", "", content);

                content = AS2MIMEUtilities.Sign(content, signingCertFilename, signingCertPassword, out contentType);

                _httpClient.DefaultRequestHeaders.Add("EDIINT-Features", "multiple-attachments");

            }
            if (encrypt)
            {
                if (string.IsNullOrEmpty(recipientCertFilename))
                {
                    throw new ArgumentNullException(recipientCertFilename, "if encrytionAlgorithm is specified then recipientCertFilename must be specified");
                }

                byte[] signedContentTypeHeader = ASCIIEncoding.ASCII.GetBytes("Content-Type: " + contentType + Environment.NewLine);
                byte[] contentWithContentTypeHeaderAdded = AS2MIMEUtilities.ConcatBytes(signedContentTypeHeader, content);

                content = AS2Encryption.Encrypt(contentWithContentTypeHeaderAdded, recipientCertFilename, EncryptionAlgorithm.DES3);


                contentType = "application/pkcs7-mime";
                //contentType = "application/pkcs7-mime; smime-type=enveloped-data; name=\"smime.p7m\"";

            }

            //_httpClient.DefaultRequestHeaders.Add("Content-Type", contentType);
            //_httpClient.DefaultRequestHeaders.Add("Content-Length", content.Length.ToString());

            var stringConent = new StringContent(Convert.ToBase64String
                (content));

            //var byteArrayContent = new ByteArrayContent(content);

            stringConent.Headers.Add("Content-Type", contentType);
            stringConent.Headers.ContentLength = content.Length;

            HttpResponseMessage response = _httpClient.PostAsync("", stringConent).Result;  // Blocking call!


            if (response.IsSuccessStatusCode)
            {
                var apiresp = response.Content.ReadAsStringAsync().Result;

            }
            else
            {
                var apiresp = response.Content.ReadAsStringAsync().Result;
            }


            return response.StatusCode;
        }


        internal static byte[] Encrypt(byte[] message, string recipientCert, string encryptionAlgorithm)
        {
            if (!string.Equals(encryptionAlgorithm, EncryptionAlgorithm.DES3) && !string.Equals(encryptionAlgorithm, EncryptionAlgorithm.RC2))
                throw new ArgumentException("encryptionAlgorithm argument must be 3DES or RC2 - value specified was:" + encryptionAlgorithm);

            X509Certificate2 cert = new X509Certificate2(recipientCert);

            var encryptEngine = new Pkcs1Encoding(new RsaEngine());

            using (var txtreader = new StringReader(cert.GetRawCertDataString()))
            {
                var keyParameter = (AsymmetricKeyParameter)new PemReader(txtreader).ReadObject();

                encryptEngine.Init(true, keyParameter);
            }

            return encryptEngine.ProcessBlock(message, 0, message.Length);

        }

        private static HttpStatusCode HandleWebResponse(HttpWebRequest http)
        {
            HttpWebResponse response = (HttpWebResponse)http.GetResponse();

            response.Close();
            return response.StatusCode;
        }

        private static void SendWebRequest(HttpWebRequest http, byte[] fileData)
        {
            Stream oRequestStream = http.GetRequestStream();
            oRequestStream.Write(fileData, 0, fileData.Length);
            oRequestStream.Flush();
            oRequestStream.Close();
        }
    }
}