using Digital_Signatues.Models;
using Digital_Signatues.Models.ViewPut;
using Digital_Signatues.Models.ViewPost;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QRCoder;
using System.IO;
using iTextSharp.text.pdf;

namespace Digital_Signatues.Services
{
    public interface IQR
    {
        Task<List<MaQR>> GetMaQRsAsync();
        Task<MaQR> GetMaQRAsync(string maso);
        /*Task<bool> DeleteMaQRAsync(int maso);*/
        Task<string> AddMaQRAsync(PostQR postMaQR);
        Task<string> UpdateMaQRAsync(PutQR putMaQR);
        void AddQRCodeToPdf(float left, float top, string data, string filePDF, string outputFile);
    }
    public class QRSvc:IQR
    {
        private readonly DataContext _context;
        public QRSvc(DataContext context)
        {
            _context = context;
        }
        public async Task<List<MaQR>> GetMaQRsAsync()
        {
            return await _context.MaQRs.OrderByDescending(x=>x.NgayTao).ToListAsync();
        }
        public async Task<MaQR> GetMaQRAsync(string maso)
        {
            return await _context.MaQRs.Where(x=>x.MaSo==maso).FirstOrDefaultAsync();
        }
        public async Task<string> AddMaQRAsync(PostQR postMaQR)
        {
            string ret = "";
            try
            {
                string maso = Helpers.RandomOTPHelper.randomQR();
                var add = new MaQR()
                {
                    Ma_DeXuat = postMaQR.Ma_DeXuat,
                    MaSo = maso,
                    NoiDung = "https://localhost:44388/" + maso,
                    NgayTao=System.DateTime.Now,
                    MucDo=1,
                    Ma_NguoiTao=postMaQR.Ma_NguoiTao
                };
                await _context.MaQRs.AddAsync(add);
                await _context.SaveChangesAsync();
                ret = maso;
            }
            catch
            {
                ret = "";
            }
            return ret;
        }
        public async Task<string> UpdateMaQRAsync(PutQR putMaQR)
        {
            string ret = "";
            try
            {
                var update = await GetMaQRAsync(putMaQR.MaSo);
                update.MucDo = putMaQR.MucDo;
                _context.MaQRs.Update(update);
                await _context.SaveChangesAsync();
                ret = putMaQR.MaSo;
            }
            catch
            {
                ret = "";
            }
            return ret;
        }
        public void AddQRCodeToPdf(float left, float top, string data, string filePDF,string outputFile)
        {
            QRCodeGenerator _qrcode = new QRCodeGenerator();
            QRCodeData _qrcodedata = _qrcode.CreateQrCode("12345", QRCodeGenerator.ECCLevel.Q);
            QRCode qRCode = new QRCode(_qrcodedata);
            System.Drawing.Bitmap qrcodeImage = qRCode.GetGraphic(3, System.Drawing.Color.Black, System.Drawing.Color.White, false);
            byte[] imgQR;
            using (MemoryStream ms = new MemoryStream())
            {
                qrcodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                imgQR = ms.ToArray();
            }
            PdfReader.unethicalreading = true;
            PdfReader reader = new PdfReader(filePDF);
            var output = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
            var stamper = new PdfStamper(reader, output);
            var pdfContentByte = stamper.GetOverContent(1);
            iTextSharp.text.Image PatientSign = iTextSharp.text.Image.GetInstance(imgQR); // image from database
            PatientSign.SetAbsolutePosition(left, top);
            pdfContentByte.AddImage(PatientSign);
            stamper.Close();
            reader.Close();
        }
    }
}
