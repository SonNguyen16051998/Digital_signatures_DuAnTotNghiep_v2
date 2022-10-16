using Digital_Signatues.Models;
using Digital_Signatues.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digital_Signatues.Services
{
    public interface INguoiDung_PhongBan
    {
        Task<bool> AddOrUpdateNguoiDung_PhongBanAsync(PostNguoiDung_PhongBan nguoiDung_PhongBans);
        Task<bool> DeleteAlNothavelNguoiDung_PhongBanAsync(int id_nguoiDung);
        Task<List<NguoiDung_PhongBan>> GetNguoiDung_PhongBansAsync(int id_nguoiDung);//Hiển thị toàn bộ phòng ban đã có của người dùng
        Task<List<PhongBan>> GetNguoiDung_PhongBanNotHaveAsync(int id_nguoiDung);//lấy các phòng ban mà người dùng chưa có
    }
    public class NguoiDung_PhongBanSvc:INguoiDung_PhongBan
    {
        private readonly DataContext _context;
        public NguoiDung_PhongBanSvc(DataContext context)
        {
            _context = context; 
        }

        public async Task<bool> AddOrUpdateNguoiDung_PhongBanAsync(PostNguoiDung_PhongBan nguoiDung_PhongBans)
        {
            bool ret = false;
            try
            {
                foreach (var item in nguoiDung_PhongBans.PhongBans)
                {
                    var name = await _context.NguoiDungs.Where(x => x.Ma_NguoiDung == nguoiDung_PhongBans.Id_NguoiDung).FirstOrDefaultAsync();
                    var nguoidung = new NguoiDung_PhongBan()
                    {
                        Ma_NguoiDung = nguoiDung_PhongBans.Id_NguoiDung,
                        Ma_PhongBan = item.Id_PhongBan,
                        Ten_NguoiDung = name.HoTen
                    };
                    await _context.NguoiDung_PhongBans.AddAsync(nguoidung);
                }
                await _context.SaveChangesAsync();
                ret = true;
            }
            catch
            {
                ret = false;
            }
            return ret;
        }

        public async Task<bool> DeleteAlNothavelNguoiDung_PhongBanAsync(int id_nguoiDung)
        {
            bool ret = false;
            try
            {
                var nguoidung = await _context.NguoiDung_PhongBans.Where(x => x.Ma_NguoiDung == id_nguoiDung).ToListAsync();
                if(nguoidung.Count > 0)
                {
                    foreach (var item in nguoidung)
                    {
                        _context.NguoiDung_PhongBans.Remove(item);
                    }
                    await _context.SaveChangesAsync();
                }
                ret = true;
            }
            catch
            {
                ret = false;
            }
            return ret;
        }

        public async Task<List<NguoiDung_PhongBan>> GetNguoiDung_PhongBansAsync(int id_nguoiDung)
        {
            List<NguoiDung_PhongBan> nguoiDung_phongbans = new List<NguoiDung_PhongBan>();
            nguoiDung_phongbans = await _context.NguoiDung_PhongBans.Where(x => x.Ma_NguoiDung == id_nguoiDung)
                            .Include(x => x.PhongBan).ToListAsync();
            return nguoiDung_phongbans;
        }

        public async Task<List<PhongBan>> GetNguoiDung_PhongBanNotHaveAsync(int id_nguoiDung)
        {
            List<NguoiDung_PhongBan> nguoiDung_phongbans = new List<NguoiDung_PhongBan>();
            nguoiDung_phongbans = await _context.NguoiDung_PhongBans.Where(x => x.Ma_NguoiDung == id_nguoiDung)
                                .ToListAsync();
            List<int> Id_Phongban = new List<int>();
            Id_Phongban = (from p in nguoiDung_phongbans
                       select p.Ma_PhongBan).ToList();
            List<PhongBan> phongbans = new List<PhongBan>();
            phongbans = await _context.PhongBans.Where(x => !Id_Phongban.Contains(x.Ma_PhongBan)).ToListAsync();
            return phongbans;
        }
    }
}
