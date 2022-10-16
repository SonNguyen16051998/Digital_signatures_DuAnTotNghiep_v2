using Digital_Signatues.Helpers;
using Digital_Signatues.Models;
using Digital_Signatues.Models.ViewModel;
using Digital_Signatues.Models.ViewPut;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digital_Signatues.Services
{
    public interface INguoiDung
    {
        Task<List<NguoiDung>> GetNguoiDungsAsync();
        Task<NguoiDung> GetNguoiDungAsync(int id);
        Task<NguoiDung> LoginAsync(ViewLogin login);
        Task<bool> ChangePassAsync(ViewChangePass changePass);
        Task<bool> isPass(string email, string pass);
        Task<bool> isEmail(string email);
        Task<int> AddNguoiDungAsync(NguoiDung nguoiDung);
        Task<int> UpdateNguoiDungAsync(PutNguoiDung putNguoiDung);
        Task<bool> CreateOrUpdateOTPAsync(OTP oTP);//tạo mã otp hoặc cập nhật mã otp mới cho email đã tồn tại
        Task<bool> ConfirmOTPAsync(string email,string otp);//xác nhận OTP sau khi nhập mã OTP
        Task<bool> QuenMatKhauAsync(ViewQuenMatKhau quenMatKhau);//đổi mật khẩu mới khi chọn chức năng quên mật khẩu
        Task<bool> DeleteNguoiDungAsync(int id_NguoiDung);
    }
    public class NguoiDungSvc:INguoiDung
    {
        private readonly DataContext _context;
        public NguoiDungSvc(DataContext context)
        {
            _context = context;
        }
        public async Task<NguoiDung> LoginAsync(ViewLogin login)
        {
            var pass = MaHoaHelper.Mahoa(login.PassWord);
            var user =await _context.NguoiDungs.Where(x => x.Block == false && x.IsDeleted==false
                && x.PassWord == pass && x.Email == login.Email).FirstOrDefaultAsync();
            if (user != null)
                return user;
            else
                return null;
        }
        public async Task<bool> ChangePassAsync(ViewChangePass changePass)
        {
            bool result = false;
            try
            {
                NguoiDung nguoiDung = await _context.NguoiDungs.Where(x => x.Email == changePass.Email).FirstOrDefaultAsync();
                nguoiDung.PassWord =MaHoaHelper.Mahoa(changePass.NewPass);
                _context.NguoiDungs.Update(nguoiDung);
                await _context.SaveChangesAsync();
                result = true;
            }
            catch { }
            return result;
        }
        public async Task<bool> isPass(string email, string pass)
        {
            bool ret = false;
            try
            {
                NguoiDung nguoiDung = await _context.NguoiDungs.Where(x => x.Email == email && x.PassWord == pass)
                    .FirstOrDefaultAsync();
                if (nguoiDung != null)
                {
                    ret = true;
                }
                else
                {
                    ret = false;
                }
            }
            catch
            {
                ret = false;
            }
            return ret;
        }
        public async Task<bool> isEmail(string email)
        {
            bool ret = false;
            try
            {
                NguoiDung nguoiDung  = await _context.NguoiDungs.Where(x => x.Email == email).FirstOrDefaultAsync();
                if (nguoiDung != null)
                {
                    ret = true;
                }
                else
                {
                    ret = false;
                }
            }
            catch
            {
                ret = false;
            }
            return ret;
        }
        public async Task<int> AddNguoiDungAsync(NguoiDung nguoiDung)
        {
            int ret = 0;
            try
            {
                nguoiDung.PassWord = MaHoaHelper.Mahoa(nguoiDung.PassWord);
                await _context.NguoiDungs.AddAsync(nguoiDung);
                var chucdanh = await _context.ChucDanhs.Where(x => x.Ma_ChucDanh == nguoiDung.Ma_ChucDanh).FirstOrDefaultAsync();
                chucdanh.IsSelected = true;
                _context.ChucDanhs.Update(chucdanh);
                await _context.SaveChangesAsync();
                ret = nguoiDung.Ma_NguoiDung;
            }
            catch { ret = 0; }
            return ret;
        }
        public async Task<int> UpdateNguoiDungAsync(PutNguoiDung putNguoiDung)
        {
            int ret = 0;
            try
            {
                var update=await _context.NguoiDungs.Where(x=>x.Ma_NguoiDung==putNguoiDung.Ma_NguoiDung).FirstOrDefaultAsync();
                update.HoTen = putNguoiDung.HoTen;
                update.Sdt=putNguoiDung.Sdt;
                update.DiaChi = putNguoiDung.DiaChi;
                update.GioiTinh=putNguoiDung.GioiTinh;
                update.Block=putNguoiDung.Block;
                update.Avatar = putNguoiDung.Avatar;
                if(update.Ma_ChucDanh!=putNguoiDung.Ma_ChucDanh)
                {
                    var oldchucdanh = await _context.ChucDanhs.Where(x => x.Ma_ChucDanh == update.Ma_ChucDanh).FirstOrDefaultAsync();
                    oldchucdanh.IsSelected = false;
                    _context.ChucDanhs.Update(oldchucdanh);
                    var newchucdanh = await _context.ChucDanhs.Where(x => x.Ma_ChucDanh == putNguoiDung.Ma_ChucDanh).FirstOrDefaultAsync();
                    newchucdanh.IsSelected = true;
                    _context.ChucDanhs.Update(newchucdanh);
                    update.Ma_ChucDanh = putNguoiDung.Ma_ChucDanh;
                }
                _context.NguoiDungs.Update(update);
                await _context.SaveChangesAsync();
                ret = update.Ma_NguoiDung;
            }
            catch { ret = 0; }
            return ret;
        }
        public async Task<bool> CreateOrUpdateOTPAsync(OTP oTP)
        {
            bool result=false;
            try
            {
                OTP otp=new OTP();
                otp=await _context.OTPs.Where(x=>x.email==oTP.email).FirstOrDefaultAsync();
                if(otp!=null)
                {
                    otp.Otp = oTP.Otp;
                    otp.expiredAt = oTP.expiredAt;
                    otp.isUse = false;
                    _context.OTPs.Update(otp);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    await _context.OTPs.AddAsync(oTP);
                    await _context.SaveChangesAsync();
                }
                SendEmailHelper.SendEmail(oTP.email, oTP.Otp);
                result =true;
            }
            catch
            { 
                result = false; 
            }
            return result;
        }
        public async Task<bool> ConfirmOTPAsync(string email, string OTP)
        {
            bool result = false;
            try
            {
                OTP otp = new OTP();
                otp = await _context.OTPs.Where(x => x.email == email && x.Otp==OTP && x.isUse==false
                                && DateTime.Now<x.expiredAt).FirstOrDefaultAsync();
                if(otp!=null)
                {
                    otp.isUse = true;
                    _context.OTPs.Update(otp);
                    await _context.SaveChangesAsync();
                    result = true;
                }
                else
                {
                    result = false;
                }
                
            }
            catch
            {
                result = false;
            }
            return result;
        }
        public async Task<bool> QuenMatKhauAsync(ViewQuenMatKhau quenMatKhau)
        {
            bool result = false;
            try
            {
                NguoiDung nguoiDung = await _context.NguoiDungs.Where(x => x.Email == quenMatKhau.Email).FirstOrDefaultAsync();
                nguoiDung.PassWord = MaHoaHelper.Mahoa(quenMatKhau.NewPass);
                _context.NguoiDungs.Update(nguoiDung);
                await _context.SaveChangesAsync();
                result = true;
            }
            catch { }
            return result;
        }
        public async Task<NguoiDung> GetNguoiDungAsync(int id)
        {
            NguoiDung nguoiDung = new NguoiDung();
            nguoiDung = await _context.NguoiDungs.Where(x => x.Ma_NguoiDung == id)
                          .Include(x=>x.ChucDanh)
                          .Include(x=>x.NguoiDung_PhongBan)
                          .FirstOrDefaultAsync();
            nguoiDung.PassWord = String.Empty;
            return nguoiDung;
        }
        public async Task<List<NguoiDung>> GetNguoiDungsAsync()
        {
            List<NguoiDung> nguoiDungs=new List<NguoiDung>();
            nguoiDungs = await _context.NguoiDungs
                            .OrderBy(x=>x.ChucDanh.Order)
                            .Include(x=>x.ChucDanh)
                            .Include(x=>x.NguoiDung_PhongBan)
                            .ToListAsync();
            List<NguoiDung> ret=new List<NguoiDung>();
            foreach(var item in nguoiDungs)
            {
                item.PassWord = String.Empty;
                ret.Add(item);
            }
            return ret;
        }
        public async Task<bool> DeleteNguoiDungAsync(int id_NguoiDung)
        {
            bool ret = false;
            try
            {
                List<NguoiDung_PhongBan> nguoiDungs = new List<NguoiDung_PhongBan>();
                nguoiDungs = await _context.NguoiDung_PhongBans.Where(x => x.Ma_NguoiDung == id_NguoiDung).ToListAsync();
                foreach (var item in nguoiDungs)//xóa người dùng ở bảng người dùng_phòng ban
                {
                    _context.NguoiDung_PhongBans.Remove(item);
                }

                NguoiDung nguoidung = new NguoiDung();
                nguoidung = await _context.NguoiDungs.Where(x => x.Ma_NguoiDung == id_NguoiDung).FirstOrDefaultAsync();
                nguoidung.IsDeleted = true;
                nguoidung.Ma_ChucDanh = 1;
                var chucdanh = await _context.ChucDanhs.Where(x => x.Ma_ChucDanh == nguoidung.Ma_ChucDanh).FirstOrDefaultAsync();
                chucdanh.IsSelected = false;
                _context.NguoiDungs.Update(nguoidung);
                _context.ChucDanhs.Update(chucdanh);
                await _context.SaveChangesAsync();
                ret = true;
            }
            catch { }
            return ret;
        }
    }
}
