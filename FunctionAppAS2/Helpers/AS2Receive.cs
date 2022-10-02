using System.Net;
using System.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;

namespace FunctionAppAS2.Helpers
{
  
    public class AS2Receive
    {
        

        public void ProcessRequest(HttpContext context)
        {
            string sTo = context.Request.Headers["AS2-To"];
            string sFrom = context.Request.Headers["AS2-From"];
            string sMessageID = context.Request.Headers["Message-ID"];

            if (context.Request.Method == "POST" || context.Request.Method == "PUT" ||
               (context.Request.Method == "GET" && context.Request.QueryString.HasValue))
            {

                if (sFrom == null || sTo == null)
                {
                    //Invalid AS2 Request.
                    //Section 6.2 The AS2-To and AS2-From header fields MUST be present
                    //    in all AS2 messages
                    if (!(context.Request.Method == "GET" && !context.Request.QueryString.HasValue))
                    {
                        AS2Receive.BadRequest(context.Response, "Invalid or unauthorized AS2 request received.");
                    }
                }
                else
                {
                    ProcessAsync(context.Request, "DropLocation");
                }
            }
            else
            {
                AS2Receive.GetMessage(context.Response);
            }
        }

        public static async Task ProcessAsync(HttpRequest request, string dropLocation)
        {
            string filename = /*ParseFilename*/(request.Headers["Subject"]);

            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();


            byte[] data = Encoding.ASCII.GetBytes(requestBody.ToString().Replace("\r\n",""));
            bool isEncrypted = request.ContentType.Contains("application/pkcs7-mime");
            bool isSigned = request.ContentType.Contains("application/pkcs7-signature");

            string message = string.Empty;

            if (isSigned)
            {
                string messageWithMIMEHeaders = System.Text.ASCIIEncoding.ASCII.GetString(data);
                string contentType = request.Headers["Content-Type"];

                message = AS2MIMEUtilities.ExtractPayload(messageWithMIMEHeaders, contentType);
            }
            else if (isEncrypted) // encrypted and signed inside
            {
    
                string encryAl = string.Empty;

                byte[] decryptedData = AS2Encryption.Decrypt(data);

                string messageWithContentTypeLineAndMIMEHeaders = System.Text.ASCIIEncoding.ASCII.GetString(decryptedData);

                // when encrypted, the Content-Type line is actually stored in the start of the message
                int firstBlankLineInMessage = messageWithContentTypeLineAndMIMEHeaders.IndexOf(Environment.NewLine + Environment.NewLine);
                string contentType = messageWithContentTypeLineAndMIMEHeaders.Substring(0, firstBlankLineInMessage);

                message = AS2MIMEUtilities.ExtractPayload(messageWithContentTypeLineAndMIMEHeaders, contentType);
            }
            else // not signed and not encrypted
            {
                message = System.Text.ASCIIEncoding.ASCII.GetString(data);
            }

            System.IO.File.WriteAllText(dropLocation + filename, message);
        }


        /// <summary>
        /// Decrypt using BouncyCastle
        /// </summary>
        /// <param name="data"></param>
        /// <param name="certbytes"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] data, byte[] certbytes, string password)
        {
            Pkcs12Store pkcs12 = new Pkcs12Store(new MemoryStream(certbytes), password.ToArray());

            string keyAlias = pkcs12.Aliases.Cast<string>().First(x => pkcs12.IsKeyEntry(x));

            AsymmetricKeyParameter key = pkcs12.GetKey(keyAlias).Key;
            Org.BouncyCastle.X509.X509Certificate cert = pkcs12.GetCertificate(keyAlias).Certificate;

            try
            {
                var envelopedData = new CmsEnvelopedData(data);
                var recepientInfos = envelopedData.GetRecipientInfos();
                var recepientId = new RecipientID()
                {
                    Issuer = cert.IssuerDN,
                    SerialNumber = cert.SerialNumber
                };
                var recepient = recepientInfos[recepientId];
                return recepient.GetContent(key);
            }
            catch (Exception ex)
            {

                throw ex;
            }
           

            
        }
        public static void GetMessage(HttpResponse response)
        {
            response.StatusCode = 200;
            //response.StatusDescription = "Okay";

            response.WriteAsync(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 3.2 Final//EN"">"
            + @"<HTML><HEAD><TITLE>Generic AS2 Receiver</TITLE></HEAD>"
            + @"<BODY><H1>200 Okay</H1><HR>This is to inform you that the AS2 interface is working and is "
            + @"accessable from your location.  This is the standard response to all who would send a GET "
            + @"request to this page instead of the POST context.Request defined by the AS2 Draft Specifications.<HR></BODY></HTML>");
        }

        public static void BadRequest(HttpResponse response, string message)
        {
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            //response.StatusDescription = "Bad context.Request";

            response.WriteAsync(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 3.2 Final//EN"">"
            + @"<HTML><HEAD><TITLE>400 Bad context.Request</TITLE></HEAD>"
            + @"<BODY><H1>400 Bad context.Request</H1><HR>There was a error processing this context.Request.  The reason given by the server was:"
            + @"<P><font size=-1>" + message + @"</Font><HR></BODY></HTML>");
        }


    }
}
