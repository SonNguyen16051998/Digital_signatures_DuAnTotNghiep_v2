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
                var delete = await _context.kySoVungKys.Where(x => x.Ma_KySoDeXuat == postVungKy.Ma_DeXuat).ToListAsync();
                foreach (var item in delete)
                {
                    _context.kySoVungKys.Remove(item);
                    await _context.SaveChangesAsync();
                }
                if(postVungKy.VungKies.Count>0)
                {
                    foreach (var item in postVungKy.VungKies)
                    {
                        var add = new KySoVungKy()
                        {
                            Ma_BuocDuyet = item.Ma_BuocDuyet,
                            Json = item.Json,
                            Ma_NguoiTao = postVungKy.Ma_NguoiTao
                        };
                        await _context.AddAsync(add);
                        await _context.SaveChangesAsync();
                    }
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
            var listvungky = await _context.kySoVungKys.Where(x => x.Ma_KySoDeXuat == ma_dexuat).ToListAsync();
            var vungky = new List<GetVungKy>();
            foreach (var item in listvungky)
            {
                vungky.Add(new GetVungKy { Ma_BuocDuyet = item.Ma_BuocDuyet, json = item.Json });
            }
            return vungky;
        }
    }
}
