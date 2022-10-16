using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Digital_Signatues.Helpers
{
    public enum Kieugiay : byte
    {
        Letter = 0,
        A4 = 1,
        A3 = 2,
        A5 = 3,
        Other = 100
    }
    public class PDFToImage
    {
        public static void PdfToJpg(string inputPDFFile, string outputImagesPath)
        {
            try
            {
                using (var document = PdfiumViewer.PdfDocument.Load(inputPDFFile))
                {
                    iTextSharp.text.Rectangle rectangle;
                    int widthImg = 827;
                    int heightImg = 1170;
                    string name = Path.GetFileNameWithoutExtension(inputPDFFile);
                    outputImagesPath = outputImagesPath + name;
                    for (int i = 0; i < document.PageCount; i++)
                    {
                        bool flagIsPortrait = CheckPageOrient(inputPDFFile, (i + 1), out rectangle);
                        Kieugiay kieuGiay = CheckLeterSize((int)rectangle.Width, (int)rectangle.Height);
                        if (kieuGiay == Kieugiay.Letter)
                        {
                            widthImg = 850;
                            heightImg = 1100;
                        }
                        else if (kieuGiay == Kieugiay.A4)
                        {
                            widthImg = 827;
                            heightImg = 1170;
                        }
                        else
                        {
                            float hangso = (float)1.388888889;
                            if (flagIsPortrait)
                            {
                                widthImg = (int)(rectangle.Width * hangso);
                                heightImg = (int)(rectangle.Height * hangso);
                            }
                            else
                            {
                                widthImg = (int)(rectangle.Height * hangso);
                                heightImg = (int)(rectangle.Width * hangso);
                            }
                        }


                        if (flagIsPortrait)
                        {
                            //var image = document.Render(i, 870, 1126, 300, 300, PdfiumViewer.PdfRenderFlags.Annotations);
                            var image = document.Render(i, widthImg, heightImg, 300, 300, PdfiumViewer.PdfRenderFlags.Annotations);
                            /*image.Save(outputImagesPath + (i + 1) + @".config", ImageFormat.Png);*/
                            image.Save(outputImagesPath + (i + 1) + @".config", ImageFormat.Png);
                        }
                        else
                        {
                            var image = document.Render(i, heightImg, widthImg, 300, 300, PdfiumViewer.PdfRenderFlags.Annotations);
                            image.Save(outputImagesPath + (i + 1) + @".config", ImageFormat.Png);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        public static bool CheckPageOrient(string inputPDFFile, int page, out iTextSharp.text.Rectangle rectangle)
        {
            PdfReader reader = new PdfReader(inputPDFFile);
            rectangle = reader.GetPageSize(page); //from 1
            bool flagIsPortrait = (rectangle.Height >= rectangle.Width);
            //ITextSharp.text.pdf.PdfReader 
            reader.Close();
            return flagIsPortrait;
        }
        public static Kieugiay CheckLeterSize(int w, int h)
        {
            Kieugiay flagLetterSize = Kieugiay.Letter;
            #region flagLetterSize
            //if (i == 0)
            {
                if ((w == 612 || h == 612))
                //if ((rect.Width == 592 || rect.Height == 592)) //Letter
                {
                    //flagLetterSize = false;
                    //widthImg = 850;  //A4
                    //heightImg = 1100;
                }
                else
                {
                    flagLetterSize = Kieugiay.A4;
                }
            }
            #endregion
            return flagLetterSize;
        }
    }
}
