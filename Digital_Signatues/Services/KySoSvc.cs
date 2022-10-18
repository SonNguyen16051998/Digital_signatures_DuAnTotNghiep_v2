using Digital_Signatues.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digital_Signatues.Services
{
    public interface IKySo
    {
        Task<int> AddKySoTest (KySoTest kySotest);
        Task<bool> Sign(int ma_buocduyet,string filedaky);
        Task<bool> CheckPassCode(int ma_nguoiky,string passcode);
        Task<int> GetIndexBuocDuyet(int ma_buocduyet);
        Task<List<KySoBuocDuyet>> GetBuocDuyetHienTai();
    }
    public class KySoSvc:IKySo 
    {
        private readonly DataContext _context;
        public KySoSvc(DataContext context)
        {
            _context = context;
        }
        public async Task<int> AddKySoTest(KySoTest kySotest)
        {
            int ret = 0;
            try
            {
                await _context.KySoTests.AddAsync(kySotest);
                await _context.SaveChangesAsync();
                ret = kySotest.Id_KySoTest;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }
        public async Task<bool> Sign(int ma_buocduyet,string filedaky)
        {
            bool ret = false;
            try
            {
                var buocduyet = await _context.kySoBuocDuyets
                    .Where(x => x.Ma_BuocDuyet == ma_buocduyet).FirstOrDefaultAsync();
                buocduyet.FileDaKy=filedaky;
                buocduyet.IsDaKy=true;
                buocduyet.NgayKy = System.DateTime.Now;
                _context.kySoBuocDuyets.Update(buocduyet);
                await _context.SaveChangesAsync();
                var toanbobuocduyet = await _context.kySoBuocDuyets
                    .Where(x => x.Ma_KySoDeXuat == buocduyet.Ma_KySoDeXuat)
                    .OrderBy(x=>x.Order)
                    .ToListAsync();
                int index = toanbobuocduyet.IndexOf(buocduyet);
                var buocduyettieptheo = toanbobuocduyet.Skip(index+1).Take(1).FirstOrDefault();
                var kysodexuat = await _context.kySoDeXuats
                    .Where(x => x.Ma_KySoDeXuat == buocduyet.Ma_KySoDeXuat).FirstOrDefaultAsync();
                if (toanbobuocduyet.Count != index + 1)
                {
                    kysodexuat.CurentOrder = buocduyettieptheo.Order;
                }
                if(toanbobuocduyet.Count==index+1)
                {

                    kysodexuat.IsDaDuyet = true;
                }
                _context.kySoDeXuats.Update(kysodexuat);
                await _context.SaveChangesAsync();
                
                ret=true;
            }
            catch { }
            return ret;
        }
        public async Task<bool> CheckPassCode(int ma_nguoiky,string passcode)
        {
            bool ret=false;
            try
            {
                var check = await _context.KySoThongSos.Where(x => x.Ma_NguoiDung == ma_nguoiky && x.PassCode == passcode)
                    .FirstOrDefaultAsync();
                ret = check == null ? false : true;
            }
            catch { }
            return ret;
        }
        public async Task<int> GetIndexBuocDuyet(int ma_buocduyet)
        {
            var buocduyet = await _context.kySoBuocDuyets
                    .Where(x => x.Ma_BuocDuyet == ma_buocduyet).FirstOrDefaultAsync();
            var toanbobuocduyet = await _context.kySoBuocDuyets
                    .Where(x => x.Ma_KySoDeXuat == buocduyet.Ma_KySoDeXuat)
                    .OrderBy(x => x.Order)
                    .ToListAsync();
            int index = toanbobuocduyet.IndexOf(buocduyet);
            return index+1;
        }
        public async Task<List<KySoBuocDuyet>> GetBuocDuyetHienTai()
        {
            var dexuat = await _context.kySoDeXuats.Where(x => x.IsDaDuyet==false).ToListAsync();
            List<KySoBuocDuyet> buocduyets = new List<KySoBuocDuyet>();
            foreach(var item in dexuat)
            {
                var buocduyet = await _context.kySoBuocDuyets
                    .Where(x => x.Ma_KySoDeXuat == item.Ma_KySoDeXuat && x.Order == item.CurentOrder)
                    .Include(x => x.KySoDeXuat)
                    .Include(x => x.NguoiDung)
                    .FirstOrDefaultAsync();
                if(buocduyet!=null)
                {
                    buocduyets.Add(buocduyet);
                }
            }
            return buocduyets;
        }
    }
}
