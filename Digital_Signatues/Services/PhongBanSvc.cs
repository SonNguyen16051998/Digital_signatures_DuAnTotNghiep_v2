using Digital_Signatues.Models;
using Digital_Signatues.Models.ViewPut;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digital_Signatues.Services
{
    public interface IPhongBan
    {
        Task<List<PhongBan>> GetPhongBansAsync();
        Task<PhongBan> GetPhongBanAsync(int id);
        Task<bool> DeletePhongBanAsync(int id);
        Task<int> AddPhongBanAsync(PhongBan phongBan);
        Task<int> UpdatePhongBanAsync(PutPhongBan putPhongBan);
        Task<bool> SapXepPhongBanAsync(List<PutSapXep> phongBans);
    }
    public class PhongBanSvc:IPhongBan
    {
        private readonly DataContext _context;
        public PhongBanSvc(DataContext context)
        {
            _context = context;
        }
        public async Task<List<PhongBan>> GetPhongBansAsync()
        {
            List<PhongBan> phongBans = new List<PhongBan>();
            phongBans = await _context.PhongBans
                         .OrderBy(x=>x.Order)
                        .Include(x=>x.NguoiDung_PhongBan)
                        .ToListAsync();
            return phongBans;
        }

        public async Task<PhongBan> GetPhongBanAsync(int id)
        {
            PhongBan phongBan = new PhongBan();
            phongBan = await _context.PhongBans.Where(x => x.Ma_PhongBan == id)
                .Include(x=>x.NguoiDung_PhongBan)
                .FirstOrDefaultAsync();
            return phongBan;
        }

        public async Task<bool> DeletePhongBanAsync(int id)
        {
            bool ret = false;
            try
            {
                List<NguoiDung_PhongBan> nguoiDungs = new List<NguoiDung_PhongBan>();
                nguoiDungs = await _context.NguoiDung_PhongBans.Where(x => x.Ma_PhongBan == id).ToListAsync();
                if (nguoiDungs.Count > 0)
                {
                    ret = false;
                }
                else
                {
                    PhongBan phongBan = new PhongBan();
                    phongBan = await _context.PhongBans.Where(x => x.Ma_PhongBan == id).FirstOrDefaultAsync();
                    phongBan.IsDeleted = true;
                    _context.PhongBans.Update(phongBan);
                    await _context.SaveChangesAsync();
                    ret = true;
                }
            }
            catch { }
            return ret;
        }

        public async Task<int> AddPhongBanAsync(PhongBan phongBan)
        {
            int ret = 0;
            try
            {
                var phongban = await _context.PhongBans.OrderByDescending(x => x.Order).Take(1).FirstOrDefaultAsync();
                if(phongban==null)
                {
                    phongBan.Order = 1;
                }
                else
                {
                    phongBan.Order = phongban.Order + 1;
                }
                await _context.PhongBans.AddAsync(phongBan);
                await _context.SaveChangesAsync();
                ret = phongBan.Ma_PhongBan;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }

        public async Task<int> UpdatePhongBanAsync(PutPhongBan putPhongBan)
        {
            int ret = 0;
            try
            {
                var update=await _context.PhongBans.Where(x=>x.Ma_PhongBan==putPhongBan.Ma_PhongBan).FirstOrDefaultAsync();
                update.Ten_PhongBan = putPhongBan.Ten_PhongBan;
                _context.PhongBans.Update(update);
                await _context.SaveChangesAsync();
                ret = update.Ma_PhongBan;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }
        public async Task<bool> SapXepPhongBanAsync(List<PutSapXep> phongBans)
        {
            bool ret = false;
            try
            {
                foreach (var item in phongBans)
                {
                    PhongBan phongban = await _context.PhongBans.Where(x => x.Ma_PhongBan == item.Id).FirstOrDefaultAsync();
                    if (phongban.Order == item.Order)
                    {
                        continue;
                    }
                    else
                    {
                        phongban.Order = item.Order;
                        _context.PhongBans.Update(phongban);
                    }
                }
                await _context.SaveChangesAsync();
                ret = true;
            }
            catch { }
            return ret;
        }
    }
}
