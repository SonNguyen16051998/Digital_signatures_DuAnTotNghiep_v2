using Digital_Signatues.Models;
using Digital_Signatues.Models.ViewPost;
using Digital_Signatues.Models.ViewPut;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
                var thongso = new KySoThongSo()
                {
                    Ma_NguoiDung = PostKySoThongSo.Ma_NguoiDung,
                    Hinh1 = PostKySoThongSo.Hinh1,
                    Hinh2 = PostKySoThongSo.Hinh2,
                    Hinh3 = PostKySoThongSo.Hinh3,
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
                    PasscodeFilePfx =null
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
                capnhatthongso.Hinh1 = PutThongSo.Hinh1;
                capnhatthongso.Hinh2 = PutThongSo.Hinh2;
                capnhatthongso.Hinh3 = PutThongSo.Hinh3;
                capnhatthongso.LyDoMacDinh = PutThongSo.LyDoMacDinh;
                capnhatthongso.Ma_NguoiCapNhatCuoi = PutThongSo.Ma_NguoiCapNhatCuoi;
                capnhatthongso.NgayCapNhatCuoi = System.DateTime.Now;
                capnhatthongso.TrangThai = PutThongSo.TrangThai;

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
                if(cauHinhFileChuKy.FilePfx != null)
                {
                    thongso.FilePfx = cauHinhFileChuKy.FilePfx;
                    thongso.PasscodeFilePfx = cauHinhFileChuKy.PasscodeFilePfx;
                }
                thongso.LoaiChuKy = cauHinhFileChuKy.LoaiChuKy;
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
    }
}
