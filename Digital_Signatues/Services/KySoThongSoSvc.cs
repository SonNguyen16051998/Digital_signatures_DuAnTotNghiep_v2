using Digital_Signatues.Models;
using Digital_Signatues.Models.ViewPost;
using Digital_Signatues.Models.ViewPut;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace Digital_Signatues.Services
{
    public interface IKySoThongSo
    {
        Task<int> AddThongSoNguoiDungAsync(PostKySoThongSo PostKySoThongSo);//them thong so cho nguoi dung
        Task<KySoThongSo> GetThongSoNguoiDungAsync(int ma_nguoidung);//lay thong so cua mot nguoi dung
        Task<List<KySoThongSo>> GetThongSoNguoiDungsAsync();//lay toan bo nguoi dung co thong so
        Task<int> UpdateThongSoAsync(PutThongSo PutThongSo);//cap nhat thong so  nguoi dung
        Task<bool> ChangePasscode (PutPasscode putPasscode);//doi passcode
        Task<bool> CauHinhChuKyAsync(PostCauHinhFileChuKy cauHinhFileChuKy);//cấu hình file pfx
        Task<bool> CapNhatThongSoFileAsync(PostThongSoFilePfx ThongSofilePfx);//cap nhat thong so file pfx  
        Task<bool> CheckExistAsync(int ma_nguoidung);//kiểm tra người dùng đã có thông số hay chưa
        Task<string> CheckSubjectFileAsync(int ma_nguoidung);//kiểm tra subject của người dùng có trùng không
        Task<bool> DeleteThongSoAsync(int ma_nguoidung);//xóa thông số người dùng
        Task<List<NguoiDung>> GetNguoiDuyetsAsync();//danh sach nguoi dung co quyen duyet ki so
        Task<bool> CheckPasscode(int ma_nguoidung, string passcode);
        Task<List<KySoThongSo>> GetAllNguoiDungDuocKyAsync();//lấy toàn bộ người dùng có quyền được kí
        Task<bool> VerifyPassword(string filepfx, string password);
    }
    public class KySoThongSoSvc:IKySoThongSo
    {
        private readonly DataContext _context;
        public KySoThongSoSvc(DataContext context)
        {
            _context = context;
        }
        public async Task<int> AddThongSoNguoiDungAsync(PostKySoThongSo PostKySoThongSo)
        {
            int ret = 0;
            try
            {
                string hinh1 = null, hinh2 = null, hinh3 = null;
                if(!string.IsNullOrEmpty(PostKySoThongSo.Hinh1))
                {
                    hinh1 = await GetChuKy(PostKySoThongSo.Hinh1);
                }
                if (!string.IsNullOrEmpty(PostKySoThongSo.Hinh2))
                {
                    hinh2 = await GetChuKy(PostKySoThongSo.Hinh2);
                }
                if (!string.IsNullOrEmpty(PostKySoThongSo.Hinh3))
                {
                    hinh3 = await GetChuKy(PostKySoThongSo.Hinh3);
                }
                var thongso = new KySoThongSo()
                {
                    Ma_NguoiDung = PostKySoThongSo.Ma_NguoiDung,
                    Hinh1 = hinh1,
                    Hinh2 = hinh2,
                    Hinh3 = hinh3,
                    LyDoMacDinh = PostKySoThongSo.LyDoMacDinh,
                    PassCode = PostKySoThongSo.PassCode,
                    Ma_NguoiCapNhatCuoi = PostKySoThongSo.Ma_NguoiCapNhatCuoi,
                    NgayCapNhatCuoi = System.DateTime.Now,
                    TrangThai = PostKySoThongSo.TrangThai,
                    LoaiChuKy = true,
                    NgayChuKyHetHan = PostKySoThongSo.NgayChuKyHetHan,
                    Serial = null,
                    Subject = null,
                    FilePfx =null,
                    PasscodeFilePfx =null,
                    Client_ID = null,
                    Client_Secret = null,
                    UID=null,
                    PasswordSmartSign = null,
                    isDislayValid = false
                };
                await _context.KySoThongSos.AddAsync(thongso);
                await _context.SaveChangesAsync();
                ret = PostKySoThongSo.Ma_NguoiDung;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }
        public async Task<KySoThongSo> GetThongSoNguoiDungAsync(int ma_nguoidung)
        {
            return await _context.KySoThongSos.Where(x => x.Ma_NguoiDung == ma_nguoidung)
                .Include(x=>x.NguoiDung)
                .Include(x=>x.NguoiDung.ChucDanh)
                .FirstOrDefaultAsync();
        }
        public async Task<List<KySoThongSo>> GetThongSoNguoiDungsAsync()
        {
            return await _context.KySoThongSos.Include(x => x.NguoiDung)
                .Include(x => x.NguoiDung.ChucDanh)
                .ToListAsync();
        }
        public async Task<int> UpdateThongSoAsync(PutThongSo PutThongSo)
        {
            int ret = 0;
            try
            {
                
                var capnhatthongso = await _context.KySoThongSos
                    .Where(x => x.Ma_NguoiDung == PutThongSo.Ma_NguoiDung).FirstOrDefaultAsync();
                string hinh1 = capnhatthongso.Hinh1, hinh2 = capnhatthongso.Hinh2, hinh3 = capnhatthongso.Hinh3;
                if (!string.IsNullOrEmpty(PutThongSo.Hinh1))
                {
                    string name = Path.GetFileNameWithoutExtension(PutThongSo.Hinh1) + ".png";
                    string check = "ImgChuKy\\" + name;
                    if(capnhatthongso.Hinh1!=check)
                    {
                        hinh1 = await GetChuKy(PutThongSo.Hinh1);
                    }
                }
                if (!string.IsNullOrEmpty(PutThongSo.Hinh2))
                {
                    string name = Path.GetFileNameWithoutExtension(PutThongSo.Hinh2) + ".png";
                    string check = "ImgChuKy\\" + name;
                    if (capnhatthongso.Hinh2 != check)
                    {
                        hinh2 = await GetChuKy(PutThongSo.Hinh2);
                    }
                }
                if (!string.IsNullOrEmpty(PutThongSo.Hinh3))
                {
                    string name = Path.GetFileNameWithoutExtension(PutThongSo.Hinh3) + ".png";
                    string check = "ImgChuKy\\" + name;
                    if (capnhatthongso.Hinh3 != check)
                    {
                        hinh3 =await  GetChuKy(PutThongSo.Hinh3);
                    }
                }
                capnhatthongso.Hinh1 = hinh1;
                capnhatthongso.Hinh2 = hinh2;
                capnhatthongso.Hinh3 = hinh3;
                capnhatthongso.LyDoMacDinh = PutThongSo.LyDoMacDinh;
                capnhatthongso.Ma_NguoiCapNhatCuoi = PutThongSo.Ma_NguoiCapNhatCuoi;
                capnhatthongso.NgayCapNhatCuoi = System.DateTime.Now;
                capnhatthongso.TrangThai = PutThongSo.TrangThai;
                capnhatthongso.NgayChuKyHetHan = PutThongSo.NgayChuKyHetHan;
                _context.KySoThongSos.Update(capnhatthongso);
                await _context.SaveChangesAsync();
                ret = PutThongSo.Ma_NguoiDung;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }
        public async Task<bool> ChangePasscode(PutPasscode putPasscode)
        {
            bool result = false;
            try
            {
                var thongso = await _context.KySoThongSos.Where(x => x.Ma_NguoiDung == putPasscode.Ma_NguoiDung).FirstOrDefaultAsync();
                thongso.PassCode = putPasscode.NewPassCode;
                _context.KySoThongSos.Update(thongso);
                await _context.SaveChangesAsync();
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }
        public async Task<bool> CauHinhChuKyAsync(PostCauHinhFileChuKy cauHinhFileChuKy)
        {
            bool result = false;
            try
            {
                var thongso = await _context.KySoThongSos
                        .Where(x => x.Ma_NguoiDung == cauHinhFileChuKy.Ma_NguoiDung).FirstOrDefaultAsync();
                if (!string.IsNullOrEmpty(cauHinhFileChuKy.FilePfx))
                {
                    string namePfx = Path.GetFileNameWithoutExtension(cauHinhFileChuKy.FilePfx).Replace("%","") + ".pfx";
                    string remoteUri = cauHinhFileChuKy.FilePfx;
                    string fileName = Path.Combine("wwwroot\\FilePfx", namePfx);
                    using (var webpage = new WebClient())
                    {
                        webpage.DownloadFileAsync(new System.Uri(remoteUri), fileName);
                    }
                    thongso.FilePfx = "wwwroot\\FilePfx\\" + namePfx;
                    thongso.PasscodeFilePfx = cauHinhFileChuKy.PasscodeFilePfx;
                }
                else
                {
                    thongso.Client_ID = cauHinhFileChuKy.Client_ID;
                    thongso.Client_Secret = cauHinhFileChuKy.Client_Secret;
                    thongso.UID = cauHinhFileChuKy.UID;
                    thongso.PasswordSmartSign = cauHinhFileChuKy.PasswordSmartSign;
                }
                thongso.LoaiChuKy = cauHinhFileChuKy.LoaiChuKy;
                thongso.isDislayValid = cauHinhFileChuKy.isDislayValid;
                _context.KySoThongSos.Update(thongso);
                await _context.SaveChangesAsync();
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }
        public async Task<bool> CapNhatThongSoFileAsync(PostThongSoFilePfx ThongSofilePfx)
        {
            bool result = false;
            try
            {
                var thongso = await _context.KySoThongSos
                    .Where(x => x.Ma_NguoiDung == ThongSofilePfx.Ma_NguoiDung).FirstOrDefaultAsync();
                thongso.Subject = ThongSofilePfx.Subject;
                thongso.Serial = ThongSofilePfx.Serial;
                var nguoidung = await _context.NguoiDungs.Where(x => x.Ma_NguoiDung == ThongSofilePfx.Ma_NguoiDung).FirstOrDefaultAsync();
                nguoidung.IsThongSo = true;
                _context.NguoiDungs.Update(nguoidung);
                _context.KySoThongSos.Update(thongso);
                await _context.SaveChangesAsync();
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }
        public async Task<bool> CheckExistAsync(int ma_nguoidung)
        {
            var thongso = await _context.KySoThongSos.Where(c => c.Ma_NguoiDung == ma_nguoidung).FirstOrDefaultAsync();
            if(thongso==null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<string> CheckSubjectFileAsync(int ma_nguoidung)
        {
            var thongso = await _context.KySoThongSos.Where(x => x.Ma_NguoiDung == ma_nguoidung).FirstOrDefaultAsync();
            return thongso.Subject;
        }
        public async Task<bool> DeleteThongSoAsync(int ma_nguoidung)
        {
            bool result = false;
            try
            {
                var thongso=await _context.KySoThongSos.Where(x=>x.Ma_NguoiDung==ma_nguoidung).FirstOrDefaultAsync();
                _context.KySoThongSos.Remove(thongso);
                var nguoidung = await _context.NguoiDungs.Where(x => x.Ma_NguoiDung == ma_nguoidung).FirstOrDefaultAsync();
                nguoidung.IsThongSo = false;
                _context.NguoiDungs.Update(nguoidung);
                await _context.SaveChangesAsync();
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }
        public async Task<List<NguoiDung>> GetNguoiDuyetsAsync()
        {
            var nguoidungs= await _context.NguoiDungs
                .Include(x=>x.NguoiDung_Quyens)
                .ToListAsync();
            List<NguoiDung> nguoidung_duyet=new List<NguoiDung>();
            foreach (var item in nguoidungs)
            {
                var quyen = await _context.NguoiDung_Quyens
                    .Where(x => x.Ma_NguoiDung == item.Ma_NguoiDung && x.Ma_Quyen==4)
                    .FirstOrDefaultAsync();
                var nguoidung_thongso = await _context.KySoThongSos
                    .Where(x => x.Ma_NguoiDung == item.Ma_NguoiDung).FirstOrDefaultAsync();
                if(quyen!=null && nguoidung_thongso==null)
                {
                    item.PassWord = string.Empty;
                    nguoidung_duyet.Add(item);
                }
            }
            return nguoidung_duyet;
        }
        public async Task<bool> CheckPasscode(int ma_nguoidung, string passcode)
        {
            var check = await _context.KySoThongSos.Where(x => x.Ma_NguoiDung == ma_nguoidung && x.PassCode == passcode).FirstOrDefaultAsync();
            return check==null? false: true;
        }
        public async Task<List<KySoThongSo>> GetAllNguoiDungDuocKyAsync()
        {
            var thongso = await _context.KySoThongSos.Include(x => x.NguoiDung)
                .Include(x => x.NguoiDung.ChucDanh)
                .ToListAsync();
            List<KySoThongSo> ret = new List<KySoThongSo>();
            foreach (var item in thongso)
            {
                if(System.DateTime.Compare(System.DateTime.Now,item.NgayChuKyHetHan)<0)
                {
                    if (item.NguoiDung.IsThongSo == true)
                    {
                        var ng_duyet = await _context.NguoiDung_Quyens
                       .Where(x => x.Ma_NguoiDung == item.Ma_NguoiDung && x.Ma_Quyen == 4)
                       .FirstOrDefaultAsync();
                        if (ng_duyet != null)
                        {
                            ret.Add(item);
                        }
                    }
                }
            }
            return ret;
        }
        public async Task<string> GetChuKy(string chuky)
        {
            string nameimg = Path.GetFileNameWithoutExtension(chuky).Replace("%","") + ".png";
            string remoteUri = chuky;
            string fileName = Path.Combine("wwwroot\\ImgChuKy", nameimg);
            using (var webpage = new WebClient())
            {
               await webpage.DownloadFileTaskAsync(new System.Uri(remoteUri, System.UriKind.Absolute), fileName);
            }
            string hinhanh = "ImgChuKy\\" + nameimg;
            return hinhanh;
        }
        public async Task<bool> VerifyPassword(string filepfx, string password)
        {
            try
            {
                string namePfx = Path.GetFileNameWithoutExtension(filepfx).Replace("%", "") + ".pfx";
                string remoteUri = filepfx;
                string fileName = Path.Combine("wwwroot\\FilePfx", namePfx);
           
               /* using (var webpage = new WebClient())
                {
                    webpage.DownloadFileAsync(new System.Uri(remoteUri), fileName);
                    
                }*/
                try
                {
                    var webpage = new WebClient();
                    await webpage.DownloadFileTaskAsync(new System.Uri(remoteUri), fileName);
                }
                catch
                {

                }
                finally
                {
                    byte[]  data = File.ReadAllBytes("wwwroot\\FilePfx\\"+namePfx);
                    var certificate = new X509Certificate2(data, password);
                }
                // ReSharper disable once UnusedVariable
                
            }
            catch (CryptographicException ex)
            {
                if ((ex.HResult & 0xFFFF) == 0x56)
                {
                    return false;
                };

                throw;
            }

            return true;
        }
       /* public async Task<string> down(string filepfx, string password)
        {

        }*/
    }
}
