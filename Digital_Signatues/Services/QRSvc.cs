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
        Task<MaQR> GetMaQRbyDeXuatAsync(int madexuat);
        Task<MaQR> GetMaQRbyMaSoAsync(string maso);
        /*Task<bool> DeleteMaQRAsync(int maso);*/
        Task<string> AddMaQRAsync(PostQR postMaQR);
        Task<string> UpdateMaQRAsync(PutQR putMaQR);
        Task<bool> AddQRCodeToPdf(PostQR qr);
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
        public async Task<MaQR> GetMaQRbyDeXuatAsync(int madexuat)
        {
            return await _context.MaQRs.Where(x=>x.Ma_DeXuat==madexuat).FirstOrDefaultAsync();
        }
        public async Task<MaQR> GetMaQRbyMaSoAsync(string maso)
        {
            return await _context.MaQRs.Where(x => x.MaSo == maso).FirstOrDefaultAsync();
        }
        public async Task<string> AddMaQRAsync(PostQR postMaQR)
        {
            string ret = "";
            try
            {
                string maso = "";
                do
                {
                    maso = Helpers.RandomOTPHelper.randomQR();
                } while (await GetMaQRbyMaSoAsync(maso) != null);
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
                var update = await _context.MaQRs.Where(x=>x.MaSo==putMaQR.MaSo).FirstOrDefaultAsync();
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
        public async Task<bool> AddQRCodeToPdf(PostQR qr)
        {
            bool ret = false;
            try
            {
                var dexuat = await _context.kySoDeXuats.Where(x => x.Ma_KySoDeXuat == qr.Ma_DeXuat).FirstOrDefaultAsync();
                string masoQR=await AddMaQRAsync(qr);
                var qrcode = await _context.MaQRs.Where(x => x.MaSo == masoQR).FirstOrDefaultAsync();
                string name = Path.GetFileNameWithoutExtension(Path.Combine(qr.inputFile));
                string outputFile = Path.Combine("FileDeXuat",name+"_QR.pdf");
                QRCodeGenerator _qrcode = new QRCodeGenerator();
                QRCodeData _qrcodedata = _qrcode.CreateQrCode(qrcode.NoiDung, QRCodeGenerator.ECCLevel.Q);
                QRCode qRCode = new QRCode(_qrcodedata);
                System.Drawing.Bitmap qrcodeImage = qRCode.GetGraphic(3, System.Drawing.Color.Black, System.Drawing.Color.White, false);
                byte[] imgQR;
                using (MemoryStream ms = new MemoryStream())
                {
                    qrcodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    imgQR = ms.ToArray();
                }
                PdfReader.unethicalreading = true;
                PdfReader reader = new PdfReader(qr.inputFile);
                var output = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
                var stamper = new PdfStamper(reader, output);
                var pdfContentByte = stamper.GetOverContent(1);
                iTextSharp.text.Image PatientSign = iTextSharp.text.Image.GetInstance(imgQR); // image from database
                PatientSign.SetAbsolutePosition(qr.Left, qr.Top);
                pdfContentByte.AddImage(PatientSign);
                stamper.Close();
                reader.Close();
                dexuat.inputFile = outputFile;
                _context.kySoDeXuats.Update(dexuat);
                await _context.SaveChangesAsync();
                ret = true;
            }
            catch { ret = false; }
            return ret;
        }
    }
}
