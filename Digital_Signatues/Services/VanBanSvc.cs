using Digital_Signatues.Models;
using Digital_Signatues.Models.ViewPost;
using Digital_Signatues.Models.ViewPut;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digital_Signatues.Services
{
    public interface IVanBan
    {
        Task<List<VanBan>> GetVanBansAsync();
        Task<VanBan> GetVanBanAsync(int ma_vanban);
        Task<bool> DeleteVanBanAsync(int ma_vanban);
        Task<int> AddVanBanAsync(PostVanBan postvanban);
        Task<int> UpdateVanBanAsync(PutVanBan putVanBan);
    }
    public class VanBanSvc:IVanBan
    {
        private readonly DataContext _context;
        public VanBanSvc(DataContext context)
        {
            _context = context;
        }

        public async Task<List<VanBan>> GetVanBansAsync()
        {
            return await _context.VanBans.OrderByDescending(x=>x.NgayTao).ToListAsync();
        }
        public async Task<VanBan> GetVanBanAsync(int ma_vanban)
        {
            return await _context.VanBans.Where(x => x.Ma_VanBan == ma_vanban).FirstOrDefaultAsync();
        }
        public async Task<bool> DeleteVanBanAsync(int ma_vanban)
        {
            var delete = await GetVanBanAsync(ma_vanban);
            if (delete != null)
            {
                _context.VanBans.Remove(delete);
                await _context.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }
        public async Task<int> AddVanBanAsync(PostVanBan postvanban)
        {
            int ret = 0;
            try
            {
                VanBan add = new VanBan()
                {
                    ChuDe=postvanban.ChuDe,
                    LoaiVanBan=postvanban.LoaiVanBan,
                    NgayTao=System.DateTime.Now,
                    File=postvanban.File,
                    Ma_NguoiTao=postvanban.Ma_NguoiTao 
                };
                await _context.VanBans.AddAsync(add);
                await _context.SaveChangesAsync();
                ret = add.Ma_VanBan;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }
        public async Task<int> UpdateVanBanAsync(PutVanBan putVanBan)
        {
            int ret = 0;
            try
            {
                var update=await GetVanBanAsync(putVanBan.Ma_VanBan);
                if(update!=null)
                {
                    update.LoaiVanBan = putVanBan.LoaiVanBan;
                    update.ChuDe=putVanBan.ChuDe;
                    update.File=putVanBan.File;
                    update.Ma_NguoiTao = putVanBan.Ma_NguoiTao;
                    _context.VanBans.Update(update);
                    await _context.SaveChangesAsync();
                    ret = putVanBan.Ma_VanBan;
                }
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }
    }
}
