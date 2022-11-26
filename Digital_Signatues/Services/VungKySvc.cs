using Digital_Signatues.Models;
using Digital_Signatues.Models.ViewModel;
using Digital_Signatues.Models.ViewPost;
using Digital_Signatues.Models.ViewPut;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Digital_Signatues.Services
{
    public interface IVungKy
    {
        Task<bool> isVungKyAsync(int ma_buocduyet);
        Task<bool> PostVungKyAsync(PostVungKy postVungKy);
        /*Task<int> PutVungKyAsync();*/
        Task<KySoVungKy> GetVungKyAsync(int ma_buocduyet);
        Task<List<GetVungKy>> isVungKyByDeXuatAsync(int ma_dexuat);
    }
    public class VungKySvc:IVungKy
    {
        private readonly DataContext _context;
        public VungKySvc(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> isVungKyAsync(int ma_buocduyet)
        {
            var check = await _context.kySoVungKys.Where(x => x.Ma_BuocDuyet == ma_buocduyet).FirstOrDefaultAsync();
            return check == null ? false : true;
        }
        public async Task<bool> PostVungKyAsync(PostVungKy postVungKy)
        {
            bool ret = false;
            try
            {
                foreach (var item in postVungKy.VungKies)
                {
                    var check = await _context.kySoVungKys.Where(x => x.Ma_BuocDuyet == item.Ma_BuocDuyet).FirstOrDefaultAsync();
                    if(check!=null)
                    {
                        _context.kySoVungKys.Remove(check);
                        await _context.SaveChangesAsync();
                    }
                    var add = new KySoVungKy()
                    {
                        Ma_BuocDuyet = item.Ma_BuocDuyet,
                        Json = item.Json,
                        Ma_NguoiTao = postVungKy.Ma_NguoiTao
                    };
                    await _context.AddAsync(add);
                    await _context.SaveChangesAsync();
                }
                ret = true;
            }
            catch { }

            return ret;
        }
        public async Task<KySoVungKy> GetVungKyAsync(int ma_buocduyet)
        {
            return await _context.kySoVungKys.Where(x=>x.Ma_BuocDuyet==ma_buocduyet)
                .Include(x=>x.KySoBuocDuyet)
                .FirstOrDefaultAsync();
        }
        public async Task<List<GetVungKy>> isVungKyByDeXuatAsync(int ma_dexuat)
        {
            var listbuocduyet = await _context.kySoBuocDuyets.Where(x => x.Ma_KySoDeXuat == ma_dexuat).ToListAsync();
            var vungky = new List<GetVungKy>();
            foreach (var item in listbuocduyet)
            {
                var ksvungky=await _context.kySoVungKys.Where(x=>x.Ma_BuocDuyet==item.Ma_BuocDuyet).FirstOrDefaultAsync();
                if(ksvungky!=null)
                {
                    vungky.Add(new GetVungKy { Ma_BuocDuyet=ksvungky.Ma_BuocDuyet,json=ksvungky.Json });
                }
            }
            return vungky;
        }
    }
}
