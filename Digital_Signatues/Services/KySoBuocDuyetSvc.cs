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
        Task<bool> CheckDeleteAsync(int ma_buocduyet);
        Task<bool> TuChoiDuyetAsync(int ma_buocduyet);
        Task<bool> isCheckTenBuoc(int ma_dexuat, string tenbuoc);
    }
    public class KySoBuocDuyetSvc:IKySoBuocDuyet
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
                    .OrderByDescending(x => x.Order).Take(1).FirstOrDefaultAsync() ;
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
                ret = post.Ma_BuocDuyet;
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

                var buocduyet = await _context.kySoBuocDuyets
                    .Where(x=>x.Ma_KySoDeXuat==delete.Ma_KySoDeXuat).ToListAsync();

                var dexuat=await _context.kySoDeXuats
                    .Where(x=>x.Ma_KySoDeXuat==delete.Ma_KySoDeXuat).FirstOrDefaultAsync();

                if(dexuat.CurentOrder==delete.Order && buocduyet.Count>1)
                {
                    dexuat.CurentOrder= delete.Order+1;
                    _context.kySoDeXuats.Update(dexuat);
                }
                if(buocduyet.Count==1)
                {
                    dexuat.CurentOrder = 0;
                    _context.kySoDeXuats.Update(dexuat);
                }
                _context.kySoBuocDuyets.Remove(delete);
                await _context.SaveChangesAsync();
                ret = true;
            }
            catch { }
            return ret;
        }
        public async Task<KySoBuocDuyet> GetBuocDuyetAsync(int ma_BuocDuyet)
        {
            return await _context.kySoBuocDuyets
                .Where(x => x.Ma_BuocDuyet == ma_BuocDuyet)
                .Include(x=>x.KySoDeXuat)
                .Include(x=>x.NguoiDung)
                .FirstOrDefaultAsync();
        }
        public async Task<List<KySoBuocDuyet>> GetAllBuocDuyetAsync(int ma_dexuat)
        {
            return await _context.kySoBuocDuyets
                .Where(x=>x.Ma_KySoDeXuat==ma_dexuat)
                .Include(x => x.NguoiDung)
                .Include(x=>x.KySoDeXuat)
                .ToListAsync();
        }
        public async Task<bool> CheckDeleteAsync(int ma_buocduyet)
        {
            bool ret = false;
            try
            {
                var buocduyet = await _context.kySoBuocDuyets.Where(x => x.Ma_BuocDuyet == ma_buocduyet).FirstOrDefaultAsync(); 
                var dexuat= await _context.kySoDeXuats.Where(x=>x.Ma_KySoDeXuat==buocduyet.Ma_KySoDeXuat).FirstOrDefaultAsync();
                ret = dexuat.TrangThai == true ? false : true;
            }
            catch { }
            return ret;
        }
        public async Task<bool> TuChoiDuyetAsync(int ma_buocduyet)
        {
            bool ret = false;
            try
            {
                var buocduyet = await _context.kySoBuocDuyets.Where(x => x.Ma_BuocDuyet == ma_buocduyet).FirstOrDefaultAsync();
                buocduyet.IsTuChoi = true;
                _context.kySoBuocDuyets.Update(buocduyet);
                await _context.SaveChangesAsync();
                ret = true;
            }
            catch { }
            return ret;
        }
        public async Task<bool> isCheckTenBuoc(int ma_dexuat, string tenbuoc)
        {
            var check=await _context.kySoBuocDuyets.Where(x=>x.Ma_KySoDeXuat == ma_dexuat && x.Ten_Buoc==tenbuoc).FirstOrDefaultAsync();
            return check != null ? false : true;
        }
    }
}
