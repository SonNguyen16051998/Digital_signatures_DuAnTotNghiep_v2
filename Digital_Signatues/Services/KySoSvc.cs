using Digital_Signatues.Models;
using Digital_Signatues.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digital_Signatues.Services
{
    public interface IKySo
    {
        Task<bool> Sign(int ma_buocduyet,string filedaky);
        Task<bool> CheckPassCode(CheckPasscode checkPasscode);
        Task<int> GetIndexBuocDuyet(int ma_buocduyet);
        Task<List<KySoBuocDuyet>> GetBuocDuyetHienTai();
        Task<List<KySoBuocDuyet>> GetDaDuyet();
    }
    public class KySoSvc:IKySo 
    {
        private readonly DataContext _context;
        public KySoSvc(DataContext context)
        {
            _context = context;
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
                    kysodexuat.FileDaKy = filedaky;
                    kysodexuat.IsDaDuyet = true;
                    if(kysodexuat.isTaoVanBan)
                    {
                        VanBan add = new VanBan()
                        {
                            ChuDe = "Văn bản xác nhận từ ban lãnh đạo",
                            LoaiVanBan = kysodexuat.LoaiVanBan,
                            NgayTao = System.DateTime.Now,
                            File = filedaky,
                            Ten_FileGoc = kysodexuat.Ten_FileGoc,
                            Ma_NguoiTao = kysodexuat.Ma_NguoiDeXuat
                        };
                        await _context.VanBans.AddAsync(add);
                    }
                }
                _context.kySoDeXuats.Update(kysodexuat);
                await _context.SaveChangesAsync();
                
                ret=true;
            }
            catch { }
            return ret;
        }
        public async Task<bool> CheckPassCode(CheckPasscode checkPasscode)
        {
            bool ret=false;
            try
            {
                var check = await _context.KySoThongSos.Where(x => x.Ma_NguoiDung == checkPasscode.Ma_NguoiKy && x.PassCode == checkPasscode.Passcode)
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
            var dexuat = await _context.kySoDeXuats.Where(x => x.IsDaDuyet==false && x.TrangThai==true).ToListAsync();
            List<KySoBuocDuyet> buocduyets = new List<KySoBuocDuyet>();
            foreach(var item in dexuat)
            {
                if(item.IsDaDuyet==false)
                {
                    var buocduyet = await _context.kySoBuocDuyets
                    .Where(x => x.Ma_KySoDeXuat == item.Ma_KySoDeXuat && x.Order == item.CurentOrder && x.IsTuChoi == false)
                    .Include(x => x.KySoDeXuat)
                    .Include(x => x.KySoDeXuat.NguoiDung)
                    .Include(x => x.NguoiDung)
                    .FirstOrDefaultAsync();
                    if (buocduyet != null)
                    {
                        buocduyets.Add(buocduyet);
                    }
                } 
            }
            return buocduyets;
        }
        public async Task<List<KySoBuocDuyet>> GetDaDuyet()
        {
            return await _context.kySoBuocDuyets.Where(x => x.IsDaKy == true)
                .Include(x => x.KySoDeXuat)
                .Include(x => x.KySoDeXuat.NguoiDung)
                .Include(x => x.NguoiDung)
                .ToListAsync();
        }
    }
}
