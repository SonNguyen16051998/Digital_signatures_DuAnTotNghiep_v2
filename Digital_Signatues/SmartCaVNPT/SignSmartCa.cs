using SignService.Common.HashSignature.Interface;
using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using log4net;
using Newtonsoft.Json;
using System.Threading;
using System.Collections.Generic;
using SignService.Common.HashSignature.Pdf;
using System.Net;
using System.util;
using Digital_Signatues.Services;
using System.Threading.Tasks;

namespace Digital_Signatues.SmartCaVNPT
{
    public class SignSmartCa
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static bool _signSmartCA_PDF(String client_ID, String client_Secret,string UID,string Pass,
            string inputFile, String outputFile, string img_sign, 
            string text_sign, int page, string reason,bool displayValid,RectangleJ rectJ)
        {
            String client_id = client_ID;
            String client_secret = client_Secret;
            var customerEmail = UID;// "03090010105"; 
            var customerPass = Pass;// "123456aA@";

            var access_token = _getAccessToken("https://gwsca.vnpt.vn/auth/token", customerEmail, customerPass, client_id, client_secret);
            if (String.IsNullOrEmpty(access_token))
            {
                Console.WriteLine("Can get access token");
                return false;
            }

            String credential = _getCredentialSmartCA(access_token, "https://gwsca.vnpt.vn/csc/credentials/list");
            String certBase64 = _getAccoutSmartCACert(access_token, "https://gwsca.vnpt.vn/csc/credentials/info", credential);
      
            string _pdfInput = inputFile;
            string _pdfSignedPath = outputFile;

            byte[] unsignData = null;
            try
            {
                unsignData = File.ReadAllBytes(_pdfInput);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            CryptoConfig.AddAlgorithm(typeof(RSAPKCS1SHA256SignatureDescription),
               "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256");
            IHashSigner signer = HashSignerFactory.GenerateSigner(unsignData, certBase64, HashSignerFactory.PDF);
            signer.SetHashAlgorithm(SignService.Common.HashSignature.Common.MessageDigestAlgorithm.SHA256);


            #region Optional -----------------------------------
            // Property: Lý do ký số
            ((PdfHashSigner)signer).SetReason(reason);
            if (displayValid)
            {
                // Kiểu hiển thị chữ ký (OPTIONAL/DEFAULT=TEXT_WITH_BACKGROUND)
                ((PdfHashSigner)signer).SetRenderingMode(PdfHashSigner.RenderMode.TEXT_WITH_BACKGROUND);
            }
            else
            {
                // Kiểu hiển thị chữ ký (OPTIONAL/DEFAULT=TEXT_WITH_BACKGROUND)
                ((PdfHashSigner)signer).SetRenderingMode(PdfHashSigner.RenderMode.LOGO_ONLY);
            }

            ((PdfHashSigner)signer).SetFontSize(10);

            ((PdfHashSigner)signer).SetFontColor("0000ff");
            // Kiểu chữ trên chữ ký
            ((PdfHashSigner)signer).SetFontStyle(PdfHashSigner.FontStyle.Normal);
            // Font chữ trên chữ ký
            ((PdfHashSigner)signer).SetFontName(PdfHashSigner.FontName.Times_New_Roman);
            if (!string.IsNullOrEmpty(text_sign))
            {
                // Thêm comment vào dữ liệu
                ((PdfHashSigner)signer).AddSignatureComment(new PdfSignatureComment
                {
                    Page = page,
                    Rectangle = string.Format("{0},{1},{2},{3}", (float)rectJ.X, (float)rectJ.Y, (float)rectJ.X + (float)rectJ.Width, (float)rectJ.Y + (float)rectJ.Height),
                    Text = text_sign,
                    FontName = "Times_New_Roman",
                    FontSize = 13,
                    FontColor = "0000FF",
                    FontStyle = 2,
                    Type = (int)PdfSignatureComment.Types.TEXT,
                });
            } 
            else
            {
                string img = Path.Combine("wwwroot",img_sign);
                // Hình ảnh hiển thị trên chữ ký (mặc định là logo VNPT)
                var imgBytes = File.ReadAllBytes(img);
                var x = Convert.ToBase64String(imgBytes);
                ((PdfHashSigner)signer).SetCustomImage(imgBytes);
                // Hiển thị ảnh chữ ký tại nhiều vị trí trên tài liệu
                ((PdfHashSigner)signer).AddSignatureView(new PdfSignatureView
                {
                    Rectangle = string.Format("{0},{1},{2},{3}", (float)rectJ.X, (float)rectJ.Y, (float)rectJ.X + (float)rectJ.Width, (float)rectJ.Y + (float)rectJ.Height),
                    Page = page
                });
            }
             
            // Signature widget border type (OPTIONAL)
            ((PdfHashSigner)signer).SetSignatureBorderType(PdfHashSigner.VisibleSigBorder.DASHED);
            #endregion -----------------------------------------
            var hashValue = ((PdfHashSigner)signer).GetSecondHashAsBase64();

            var tranId = _signHash(access_token, "https://gwsca.vnpt.vn/csc/signature/signhash", hashValue, credential);

            if (tranId == "")
            {
                return false;
            }

            var count = 0;
            var isConfirm = false;
            var datasigned = "";
            while (count < 24 && !isConfirm)
            {
                var tranInfo = _getTranInfo(access_token, "https://gwsca.vnpt.vn/csc/credentials/gettraninfo", tranId);
                if (tranInfo != null)
                {
                    if (tranInfo.tranStatus != 1)
                    {
                        count = count + 1;
                        Thread.Sleep(10000);
                    }
                    else
                    {
                        isConfirm = true;
                        datasigned = tranInfo.documents[0].sig;
                    }
                }
                else
                {
                    /*Console.WriteLine("Error from content");*/
                    return false;
                }
            }
            if (!isConfirm)
            {
                /*Console.WriteLine("Signer not confirm from App");*/
                return false;
            }

            if (string.IsNullOrEmpty(datasigned))
            {
                /*Console.WriteLine("Sign error");*/
                return false;
            }


            if (!signer.CheckHashSignature(datasigned))
            {
                /*Console.WriteLine("Signature not match");*/
                return false;
            }
            // ------------------------------------------------------------------------------------------

            // 3. Package external signature to signed file
            byte[] signed = signer.Sign(datasigned);
            File.WriteAllBytes(_pdfSignedPath, signed);


            //File.WriteAllBytes(_pdfSignedPath, Convert.FromBase64String(datasigned));
            /*Console.WriteLine("SignHash PDF: Successfull. signed file at '" + _pdfSignedPath + "'");*/
            //_log.Info("SignHash PDF: Successfull. signed file at '" + _pdfSignedPath + "'");
            return true;
        }
        private static void _signSmartCA_OFFICE()
        {
            var customerEmail = "162952530";// "03090010105"; 
            var customerPass = "871097";// "123456aA@";
            //var access_token = CoreServiceClient.GetAccessToken(customerEmail, customerPass, out string refresh_token);

            var access_token = _getAccessToken("https://rmgateway.vnptit.vn/auth/token", customerEmail, customerPass, "4185-637127995547330633.apps.signserviceapi.com", "NGNhMzdmOGE-OGM2Mi00MTg0");
            if (String.IsNullOrEmpty(access_token))
            {
                Console.WriteLine("Can get access token");
                return;
            }

            String credential = _getCredentialSmartCA(access_token, "https://rmgateway.vnptit.vn/csc/credentials/list");
            String certBase64 = _getAccoutSmartCACert(access_token, "https://rmgateway.vnptit.vn/csc/credentials/info", credential);

            string _pdfInput = @"C:\Users\Hung Vu\Desktop\test.docx";
            string _pdfSignedPath = @"C:\Users\Hung Vu\Desktop\test_signed_1.docx";

            byte[] unsignData = null;
            try
            {
                unsignData = File.ReadAllBytes(_pdfInput);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return;
            }
            CryptoConfig.AddAlgorithm(typeof(RSAPKCS1SHA256SignatureDescription),
               "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256"
            );
            IHashSigner signer = HashSignerFactory.GenerateSigner(unsignData, certBase64, null, HashSignerFactory.OFFICE);
            signer.SetHashAlgorithm(SignService.Common.HashSignature.Common.MessageDigestAlgorithm.SHA256);

            var hashValue = signer.GetSecondHashAsBase64();

            var tranId = _signHash(access_token, "https://rmgateway.vnptit.vn/csc/signature/signhash", hashValue, credential);
            //SignHash End

            //Sign Begin
            //var tranId = _sign(access_token, "https://rmgateway.vnptit.vn/csc/signature/sign", Convert.ToBase64String(unsignData), credential);
            //Sign End

            if (tranId == "")
            {
                Console.WriteLine("Ky so that bai");
            }

            var count = 0;
            var isConfirm = false;
            var datasigned = "";
            while (count < 24 && !isConfirm)
            {
                Console.WriteLine("Get TranInfo PDF lan " + count + " : ");
                //_log.Info("Get TranInfo PDF lan " + count + " : ");
                var tranInfo = _getTranInfo(access_token, "https://rmgateway.vnptit.vn/csc/credentials/gettraninfo", tranId);
                if (tranInfo != null)
                {
                    if (tranInfo.tranStatus != 1)
                    {
                        count = count + 1;
                        Thread.Sleep(10000);
                    }
                    else
                    {
                        isConfirm = true;
                        datasigned = tranInfo.documents[0].sig;
                    }
                }
                else
                {
                    Console.WriteLine("Error from content");
                    return;
                }
            }
            if (!isConfirm)
            {
                Console.WriteLine("Signer not confirm from App");
                return;
            }

            if (string.IsNullOrEmpty(datasigned))
            {
                Console.WriteLine("Sign error");
                return;
            }

            /*
            if (!signer.CheckHashSignature(datasigned))
            {
                Console.WriteLine("Signature not match");
                return;
            }
            */
            // ------------------------------------------------------------------------------------------

            // 3. Package external signature to signed file
            byte[] signed = signer.Sign(datasigned);
            File.WriteAllBytes(_pdfSignedPath, signed);


            //File.WriteAllBytes(_pdfSignedPath, Convert.FromBase64String(datasigned));
            Console.WriteLine("SignHash OFFICE: Successfull. signed file at '" + _pdfSignedPath + "'");
            //_log.Info("SignHash PDF: Successfull. signed file at '" + _pdfSignedPath + "'");
        }

        private static void _signSmartCA_XML()
        {
            var customerEmail = "162952530";// "03090010105"; 
            var customerPass = "871097";// "123456aA@";
            //var access_token = CoreServiceClient.GetAccessToken(customerEmail, customerPass, out string refresh_token);

            var access_token = _getAccessToken("https://rmgateway.vnptit.vn/auth/token", customerEmail, customerPass, "4185-637127995547330633.apps.signserviceapi.com", "NGNhMzdmOGE-OGM2Mi00MTg0");
            if (String.IsNullOrEmpty(access_token))
            {
                Console.WriteLine("Can get access token");
                return;
            }

            String credential = _getCredentialSmartCA(access_token, "https://rmgateway.vnptit.vn/csc/credentials/list");
            String certBase64 = _getAccoutSmartCACert(access_token, "https://rmgateway.vnptit.vn/csc/credentials/info", credential);

            string _pdfInput = @"C:\Users\Hung Vu\Desktop\test.xml";
            string _pdfSignedPath = @"C:\Users\Hung Vu\Desktop\test_signed.xml";

            byte[] unsignData = null;
            try
            {
                unsignData = File.ReadAllBytes(_pdfInput);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return;
            }
            CryptoConfig.AddAlgorithm(typeof(RSAPKCS1SHA256SignatureDescription),
               "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256"
            );
            IHashSigner signer = HashSignerFactory.GenerateSigner(unsignData, certBase64, null, HashSignerFactory.XML);
            signer.SetHashAlgorithm(SignService.Common.HashSignature.Common.MessageDigestAlgorithm.SHA256);

            var hashValue = signer.GetSecondHashAsBase64();

            var tranId = _signHash(access_token, "https://rmgateway.vnptit.vn/csc/signature/signhash", hashValue, credential);
            //SignHash End

            //Sign Begin
            //var tranId = _sign(access_token, "https://rmgateway.vnptit.vn/csc/signature/sign", Convert.ToBase64String(unsignData), credential);
            //Sign End

            if (tranId == "")
            {
                Console.WriteLine("Ky so that bai");
            }

            var count = 0;
            var isConfirm = false;
            var datasigned = "";
            while (count < 24 && !isConfirm)
            {
                Console.WriteLine("Get TranInfo PDF lan " + count + " : ");
                //_log.Info("Get TranInfo PDF lan " + count + " : ");
                var tranInfo = _getTranInfo(access_token, "https://rmgateway.vnptit.vn/csc/credentials/gettraninfo", tranId);
                if (tranInfo != null)
                {
                    if (tranInfo.tranStatus != 1)
                    {
                        count = count + 1;
                        Thread.Sleep(10000);
                    }
                    else
                    {
                        isConfirm = true;
                        datasigned = tranInfo.documents[0].sig;
                    }
                }
                else
                {
                    Console.WriteLine("Error from content");
                    return;
                }
            }
            if (!isConfirm)
            {
                Console.WriteLine("Signer not confirm from App");
                return;
            }

            if (string.IsNullOrEmpty(datasigned))
            {
                Console.WriteLine("Sign error");
                return;
            }


            if (!signer.CheckHashSignature(datasigned))
            {
                Console.WriteLine("Signature not match");
                return;
            }

            // ------------------------------------------------------------------------------------------

            // 3. Package external signature to signed file
            byte[] signed = signer.Sign(datasigned);
            File.WriteAllBytes(_pdfSignedPath, signed);


            //File.WriteAllBytes(_pdfSignedPath, Convert.FromBase64String(datasigned));
            Console.WriteLine("SignHash OFFICE: Successfull. signed file at '" + _pdfSignedPath + "'");
            //_log.Info("SignHash PDF: Successfull. signed file at '" + _pdfSignedPath + "'");
        }

        private static string _getCredentialSmartCA(String accessToken, String serviceUri)
        {
            using (WebClient wc = new WebClient())
            {
                try
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                    wc.Headers[HttpRequestHeader.Authorization] = "Bearer " + accessToken;
                    string HtmlResult = wc.UploadString(serviceUri, "POST", "{}");

                    CredentialSmartCAResponse credentials = JsonConvert.DeserializeObject<CredentialSmartCAResponse>(HtmlResult);
                    if (credentials != null)
                    {
                        return credentials.content[0];
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return null;
        }
        private static String _getAccoutSmartCACert(String accessToken, String serviceUri, string credentialId)
        {
            var req = new ReqCertificateSmartCA
            {
                credentialId = credentialId,
                certificates = "chain",
                certInfo = true,
                authInfo = true
            };
            var body = JsonConvert.SerializeObject(req);

            using (WebClient wc = new WebClient())
            {
                try
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                    wc.Headers[HttpRequestHeader.Authorization] = "Bearer " + accessToken;
                    string HtmlResult = wc.UploadString(serviceUri, "POST", body);

                    CertificateSmartCAResponse res = JsonConvert.DeserializeObject<CertificateSmartCAResponse>(HtmlResult);
                    String certBase64 = res.cert.certificates[0];
                    return certBase64.Replace("\r\n", "");

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return null;
        }

        private static TranInfoSmartCARespContent _getTranInfo(string accessToken, String serviceUri, String tranId)
        {

            var req = new ContenSignHash
            {
                tranId = tranId
            };
            var body = JsonConvert.SerializeObject(req);

            using (WebClient wc = new WebClient())
            {
                try
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                    wc.Headers[HttpRequestHeader.Authorization] = "Bearer " + accessToken;
                    string HtmlResult = wc.UploadString(serviceUri, "POST", body);

                    TranInfoSmartCAResp resp = JsonConvert.DeserializeObject<TranInfoSmartCAResp>(HtmlResult);
                    if (resp.code == 0)
                    {
                        return resp.content;
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return null;
        }

        private static string _signHash(string accessToken, String serviceUri, string data, string credentialId)
        {

            var req = new SignHashSmartCAReq
            {
                credentialId = credentialId,
                refTranId = "1234-5678-90000",
                notifyUrl = "http://10.169.0.221/api/v1/smart_ca_callback",
                description = "Ký xác thực file pdf",
                datas = new List<DataSignHash>()
            };
            var test = new DataSignHash
            {
                name = "test.pdf",
                hash = data
            };
            req.datas.Add(test);
            var body = JsonConvert.SerializeObject(req);

            using (WebClient wc = new WebClient())
            {
                try
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                    wc.Headers[HttpRequestHeader.Authorization] = "Bearer " + accessToken;
                    string HtmlResult = wc.UploadString(serviceUri, "POST", body);

                    SignHashSmartCAResponse resp = JsonConvert.DeserializeObject<SignHashSmartCAResponse>(HtmlResult);
                    if (resp.code == 0)
                    {
                        return resp.content.tranId;
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return null;
        }

        private static string _getAccessToken(String uri, String user, String pass, string client_id, string client_secret)
        {
            string URI = uri;
            String body = $"username={user}&password={pass}&client_id={client_id}&client_secret={client_secret}&grant_type=password";
            using (WebClient wc = new WebClient())
            {
                try
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    string HtmlResult = wc.UploadString(URI, "POST", body);
                    var content = JsonConvert.DeserializeObject<GetTokenResponse>(HtmlResult);
                    return content.access_token;
                }
                catch (Exception ex)
                {
                    throw ex;

                }

            }
            return null;
        }

        public static subAndSerial _getSubjectandSerial(string client_ID,string client_Secret,string UID,string Pass)
        {
            String client_id = client_ID;
            String client_secret = client_Secret;
            var customerEmail = UID;// "03090010105"; 
            var customerPass = Pass;// "123456aA@";

            var access_token = _getAccessToken("https://gwsca.vnpt.vn/auth/token", customerEmail, customerPass, client_id, client_secret);
            String credential = _getCredentialSmartCA(access_token, "https://gwsca.vnpt.vn/csc/credentials/list");
            string serviceUri = "https://gwsca.vnpt.vn/csc/credentials/info";
            var req = new ReqCertificateSmartCA
            {
                credentialId = credential,
                certificates = "chain",
                certInfo = true,
                authInfo = true
            };
            var body = JsonConvert.SerializeObject(req);

            using (WebClient wc = new WebClient())
            {
                try
                {
                    
                    wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                    wc.Headers[HttpRequestHeader.Authorization] = "Bearer " + access_token;
                    string HtmlResult = wc.UploadString(serviceUri, "POST", body);

                    CertificateSmartCAResponse res = JsonConvert.DeserializeObject<CertificateSmartCAResponse>(HtmlResult);
                    var subject_serial = new subAndSerial()
                    {
                        subject = res.cert.subjectDN,
                        serial = res.cert.serialNumber
                    };
                    return subject_serial;

                }
                catch 
                {
                    
                }

            }
            return null;
        }

        public class subAndSerial
        {
            public string subject { get; set; }
            public string serial { get; set; }
        }

        class GetTokenResponse
        {
            // access_token value
            public string access_token { get; set; }
            // refresh_token to get new access_token (see RefreshToken method)
            public string refresh_token { get; set; }
            public string token_type { get; set; }
            // access_token valid time. when expired, using refresh_token to get new or require user re-authorize
            public int expires_in { get; set; }
        }



    }
}
