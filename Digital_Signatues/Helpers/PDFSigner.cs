using Digital_Signatues.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Digital_Signatues.Helpers
{
    public class PDFSigner
    {
        private string InputPDF = "";
        private string OutputPDF = "";
        private Certicate Cert;
        private string FontPath = "";
        public PDFSigner()
        {

        }
        public PDFSigner(string input, string output)
        {
            this.InputPDF = input;
            this.OutputPDF = output;
        }

        public PDFSigner(string input, string output, Certicate cert)
        {
            this.InputPDF = input;
            this.OutputPDF = output;
            this.Cert = cert;
        }
        public PDFSigner(string input, string output, Certicate cert, string fontPath)
        {
            this.InputPDF = input;
            this.OutputPDF = output;
            this.Cert = cert;
            this.FontPath = fontPath;
        }

       /* public void Verify()
        {
        }*/
        public void SignImage(string sigReason, string sigContact, string sigLocation, string imageFilePath,
         Rectangle rectangle, int page, string fieldName, bool flagKyHethong)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            IExternalSignature externalSignature = new PrivateKeySignature(this.Cert.Akp, DigestAlgorithms.SHA256);
            PdfReader.unethicalreading = true;
            PdfReader reader = new PdfReader(this.InputPDF);
            var output = new FileStream(this.OutputPDF, FileMode.Create, FileAccess.Write);
            PdfStamper Stamper = PdfStamper.CreateSignature(reader, output, '\0', null, true);

            //PdfStamper stamper = PdfStamper.CreateSignature(reader, fout, '\0', true);
            PdfSignatureAppearance appearance = Stamper.SignatureAppearance;
            appearance.Reason = sigReason;
            appearance.Location = sigLocation;
            appearance.Contact = sigContact;
            appearance.SignDate = DateTime.Now;

            appearance.Acro6Layers = true; // không hiển thị dấu valid

            //appearance.SetVisibleSignature(rectangle, page, "Signature");
            appearance.SetVisibleSignature(rectangle, page, fieldName);


            Font NormalFont = FontFactory.GetFont("Arial", 11, Font.NORMAL, BaseColor.BLUE);
            BaseFont bf = BaseFont.CreateFont(this.FontPath, BaseFont.IDENTITY_H, true);
            appearance.Layer2Font = new iTextSharp.text.Font(bf, (float)11.5, Font.NORMAL, BaseColor.BLUE);

            if (flagKyHethong)
            {
                //imageFilePath= Server.Map ""
                appearance.SignatureGraphic = Image.GetInstance(imageFilePath);
                appearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.DESCRIPTION;
                //appearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.NAME_AND_DESCRIPTION;
                //appearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.GRAPHIC_AND_DESCRIPTION;
            }
            else
            {
                appearance.SignatureGraphic = Image.GetInstance(imageFilePath);
                appearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.GRAPHIC;
            }
            //appearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.GRAPHIC;
            //appearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.GRAPHIC_AND_DESCRIPTION;

            //signatureAppearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.DESCRIPTION;


            appearance.CertificationLevel = PdfSignatureAppearance.NOT_CERTIFIED; // cho phép ký nhiều chữ ký
                                                                                  //appearance.CertificationLevel = PdfSignatureAppearance.CERTIFIED_FORM_FILLING_AND_ANNOTATIONS; // ký và khóa nội dung, sẽ bể những chữ ký có trước
                                                                                  //appearance.CertificationLevel
            /*
            appearance.setCertificationLevel(PdfSignatureAppearance.CERTIFIED_NO_CHANGES_ALLOWED);

            field.put(PdfName.LOCK, stamper.getWriter().addToBody(new PdfSigLockDictionary(LockPermissions.NO_CHANGES_ALLOWED)).getIndirectReference());
            field.setFlags(PdfAnnotation.FLAGS_PRINT);
            field.setPage(1);
            field.setWidget(new Rectangle(150, 250, 300, 401), PdfAnnotation.HIGHLIGHT_INVERT);
            stamper.addAnnotation(field, 1);
            */

            /*Utility.WriteFileError("Kiem loi Cert");
            if (this.Cert.ExternalSignature == null)
            {
                Utility.WriteFileError("this.Cert.ExternalSignature null");
            }
            if (this.Cert.Chain.Count == 0)
            {
                Utility.WriteFileError("this.Cert.Chain null");
            }*/
            MakeSignature.SignDetached(appearance, externalSignature, this.Cert.Chain, null, null, null, 0, CryptoStandard.CMS);
            Stamper.Close();
            reader.Close();
        }

        public void SignText(string sigReason, string sigContact, string sigLocation, string text,
          Rectangle rectangle, int page, string fieldName)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            IExternalSignature externalSignature = new PrivateKeySignature(this.Cert.Akp, DigestAlgorithms.SHA256);
            PdfReader.unethicalreading = true;
            PdfReader reader = new PdfReader(this.InputPDF);
            var output = new FileStream(this.OutputPDF, FileMode.Create, FileAccess.Write);

            PdfStamper Stamper = PdfStamper.CreateSignature(reader, output, '\0', null, true);
            int pageCount = reader.NumberOfPages;

            // Create New Layer for Watermark
            PdfLayer layer = new PdfLayer("Layer", Stamper.Writer);
            // Loop through each Page
            // Getting the Page Size
            Rectangle rect = reader.GetPageSize(page);

            // Get the ContentByte object
            PdfContentByte cb = Stamper.GetOverContent(page);

            // Tell the cb that the next commands should be "bound" to this new layer
            cb.BeginLayer(layer);
            BaseFont baseFont = BaseFont.CreateFont(this.FontPath, BaseFont.IDENTITY_H, true);
            cb.SetColorFill(BaseColor.BLUE);
            cb.SetFontAndSize(baseFont, 12);
            cb.BeginText();
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, text, rectangle.Width, rectangle.Height, 0);
            cb.EndText();

            // Close the layer
            cb.EndLayer();
            PdfSignatureAppearance appearance = Stamper.SignatureAppearance;
            appearance.Reason = sigReason;
            appearance.Location = sigLocation;
            appearance.Contact = sigContact;
            appearance.SignDate = DateTime.Now;
            /* appearance.SignatureGraphic = Image.GetInstance(imageFilePath);*/
            /* appearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.DESCRIPTION;*/

            appearance.Acro6Layers = true; // không hiển thị dấu valid
            /*appearance.SetVisibleSignature(rectangle, page, fieldName);*/
            Font NormalFont = FontFactory.GetFont("Arial", 11, Font.NORMAL, BaseColor.BLUE);
            BaseFont bf = BaseFont.CreateFont(this.FontPath, BaseFont.IDENTITY_H, true);
            appearance.Layer2Font = new iTextSharp.text.Font(bf, (float)11.5, Font.NORMAL, BaseColor.BLUE);



            appearance.CertificationLevel = PdfSignatureAppearance.NOT_CERTIFIED; // cho phép ký nhiều chữ ký
                                                                                  //appearance.CertificationLevel = PdfSignatureAppearance.CERTIFIED_FORM_FILLING_AND_ANNOTATIONS; // ký và khóa nội dung, sẽ bể những chữ ký có trước
                                                                                  //appearance.CertificationLevel
            MakeSignature.SignDetached(appearance, externalSignature, this.Cert.Chain, null, null, null, 0, CryptoStandard.CMS);
            Stamper.Close();
            reader.Close();
        }

        public static bool VerifySignatures(byte[] file)
        {

            PdfReader reader = new PdfReader(file);

            AcroFields af = reader.AcroFields;
            var names = af.GetSignatureNames();
            if (names.Count == 0)
                return false; // no signatures
            else
            {
                foreach (string name in names)
                {
                    if (!af.SignatureCoversWholeDocument(name))
                    {
                        return false;
                    }

                    PdfPKCS7 pk = af.VerifySignature(name);


                    if (!pk.Verify())
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
