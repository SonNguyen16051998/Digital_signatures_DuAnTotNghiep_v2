using Digital_Signatues.Models;
using Digital_Signatues.Models.ViewPost;
using Digital_Signatues.Models.ViewPut;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digital_Signatues.Services
{
    public interface IKySoBuocDuyet
    {
        Task<int> PostBuocDuyetAsync(PostKySoBuocDuyet kySoBuocDuyet);
        Task<bool> PutBuocDuyetAsync(PutKySoBuocDuyet kySoBuocDuyet);
        Task<bool> DeleteBuocDuyetAsync(int ma_BuocDuyet);
        Task<KySoBuocDuyet> GetBuocDuyetAsync(int ma_BuocDuyet);
        Task<List<KySoBuocDuyet>> GetAllBuocDuyetAsync(int ma_dexuat);
        Task<bool> CheckDeleteAsync(int ma_BuocDuyet);
    }
    public class KySoBuocDuyetSvc
    {
        private readonly DataContext _context;
        public KySoBuocDuyetSvc(DataContext context)
        {
            _context = context;
        }
        public async Task<int> PostBuocDuyetAsync(PostKySoBuocDuyet kySoBuocDuyet)
        {
            int ret = 0;
            try
            {
                var buocduyet = await _context.kySoBuocDuyets
                    .Where(x => x.Ma_KySoDeXuat == kySoBuocDuyet.Ma_KySoDeXuat)
                    .OrderBy(x => x.Order).Take(1).FirstOrDefaultAsync() ;
                int order = buocduyet!=null ? buocduyet.Order+1 : 1;
                var post = new KySoBuocDuyet()
                {
                    Ten_Buoc = kySoBuocDuyet.Ten_Buoc,
                    Ma_NguoiKy = kySoBuocDuyet.Ma_NguoiKy,
                    Ma_KySoDeXuat = kySoBuocDuyet.Ma_KySoDeXuat,
                    Order = order,
                    FileDaKy = null,
                    NgayKy = null,
                    IsDaKy = false,
                    IsTuChoi=false,
                };
                if (order == 1)
                {
                    var dexuat = await _context.kySoDeXuats
                    .Where(x => x.Ma_KySoDeXuat == kySoBuocDuyet.Ma_KySoDeXuat).FirstOrDefaultAsync();
                    dexuat.CurentOrder = order;
                    _context.kySoDeXuats.Update(dexuat);
                }
                await _context.kySoBuocDuyets.AddAsync(post);
                await _context.SaveChangesAsync();  
            }
            catch { }
            return ret;
        }
        public async Task<bool> PutBuocDuyetAsync(PutKySoBuocDuyet kySoBuocDuyet)
        {
            bool ret = false;
            try
            {
                var put = await _context.kySoBuocDuyets.Where(x => x.Ma_BuocDuyet == kySoBuocDuyet.Ma_BuocDuyet)
                    .FirstOrDefaultAsync();
                put.Ten_Buoc = kySoBuocDuyet.Ten_Buoc;
                put.Ma_NguoiKy = kySoBuocDuyet.Ma_NguoiKy;
                _context.kySoBuocDuyets.Update(put);
                await _context.SaveChangesAsync();
                ret = true;
            }
            catch { }
            return ret;
        }
        public async Task<bool> DeleteBuocDuyetAsync(int ma_BuocDuyet)
        {
            bool ret = false;
            try
            {
                var delete = await _context.kySoBuocDuyets.Where(x => x.Ma_BuocDuyet == ma_BuocDuyet)
                    .FirstOrDefaultAsync();
                _context.kySoBuocDuyets.Remove(delete);
                await _context.SaveChangesAsync();
                ret = true;
            }
            catch { }
            return ret;
        }
    }
}
