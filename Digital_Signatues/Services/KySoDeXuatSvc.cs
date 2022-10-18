using Digital_Signatues.Models;
using Digital_Signatues.Models.ViewPost;
using Digital_Signatues.Models.ViewPut;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digital_Signatues.Services
{
    public interface IKySoDeXuat
    {
        Task<int> PostDeXuatAsync(PostKySoDeXuat kySoDeXuat);
        Task<bool> PutDeXuatAsync(PutKySoDeXuat kySoDeXuat);
        Task<bool> DeleteDeXuatAsync(int ma_dexuat);//xoa de xuat
        Task<List<KySoDeXuat>> GetDeXuatsByNguoiDeXuatAsync(int ma_nguoidexuat);
        Task<List<KySoDeXuat>> GetDeXuatsAsync();//hien thi toan bo de xuat
        Task<KySoDeXuat> GetDeXuatAsync(int ma_dexuat);//hien thi chi tiet de xuat hien tai
        Task<bool> CheckDeleteAsync(int ma_dexuat);
        Task<bool> ChuyenDuyetAsync(int ma_dexuat);
        Task<List<KySoDeXuat>> GetKySoChuaDeXuatAsync(int ma_nguoidung);
        Task<List<KySoDeXuat>> GetKySoChoDuyetAsync(int ma_nguoidung);
        Task<List<KySoDeXuat>> GetKySoDaDuyetAsync(int ma_nguoidung);
        Task<List<KySoDeXuat>> GetKySoTuChoiAsync(int ma_nguoidung);
    }
    public class KySoDeXuatSvc:IKySoDeXuat
    {
        private readonly DataContext _context;
        public KySoDeXuatSvc(DataContext context)
        {
           _context = context;
        }

        public async Task<int> PostDeXuatAsync(PostKySoDeXuat kySoDeXuat)
        {
            int ret = 0;
            try
            {
                var postdexuat = new KySoDeXuat()
                {
                    Ten_DeXuat = kySoDeXuat.Ten_DeXuat,
                    Ma_NguoiDeXuat = kySoDeXuat.Ma_NguoiDeXuat,
                    LoaiVanBan = kySoDeXuat.LoaiVanBan,
                    GhiChu = kySoDeXuat.GhiChu,
                    inputFile = kySoDeXuat.inputFile,
                    NgayDeXuat = System.DateTime.Now,
                    TrangThai = false,
                    CurentOrder = 0
                };
                await _context.kySoDeXuats.AddAsync(postdexuat);
                await _context.SaveChangesAsync();
                ret = postdexuat.Ma_KySoDeXuat;
            }
            catch { }
            return ret;
        }
        public async Task<bool> PutDeXuatAsync(PutKySoDeXuat kySoDeXuat)
        {
            bool ret = false;
            try
            {
                var update = await _context.kySoDeXuats
                    .Where(x => x.Ma_KySoDeXuat == kySoDeXuat.Ma_KySoDeXuat).FirstOrDefaultAsync();
                update.Ten_DeXuat = kySoDeXuat.Ten_DeXuat;
                update.LoaiVanBan = kySoDeXuat.LoaiVanBan;
                update.GhiChu = kySoDeXuat.GhiChu;
                update.inputFile = kySoDeXuat.inputFile;
                _context.kySoDeXuats.Update(update);
                await _context.SaveChangesAsync();
                ret = true;
            }
            catch { }
            return ret;
        }

        public async Task<bool> DeleteDeXuatAsync(int ma_dexuat)
        { 
            bool ret = false;
            try
            {
                var delete = await _context.kySoDeXuats
                .Where(x => x.Ma_KySoDeXuat == ma_dexuat).FirstOrDefaultAsync();
                _context.kySoDeXuats.Remove(delete);
                await _context.SaveChangesAsync();
                var buocduyet = await _context.kySoBuocDuyets
                    .Where(x => x.Ma_KySoDeXuat == ma_dexuat).ToListAsync();
                foreach(var item in buocduyet)
                {
                    _context.kySoBuocDuyets.Remove(item);
                }
                await _context.SaveChangesAsync();
                ret = true;
            }
            catch { }
            return ret;
        }
        public async Task<List<KySoDeXuat>> GetDeXuatsByNguoiDeXuatAsync(int ma_nguoidexuat)
        {
            return await _context.kySoDeXuats.Where(x=>x.Ma_NguoiDeXuat==ma_nguoidexuat)
                .Include(x=>x.KySoBuocDuyets)
                .ToListAsync();
        }
        public async Task<List<KySoDeXuat>> GetDeXuatsAsync()
        {
            return await _context.kySoDeXuats.Include(x => x.KySoBuocDuyets).ToListAsync();
        }
        public async Task<KySoDeXuat> GetDeXuatAsync(int ma_dexuat)
        {
            return await _context.kySoDeXuats.Where(x => x.Ma_KySoDeXuat == ma_dexuat)
                .Include(x => x.KySoBuocDuyets)
                .FirstOrDefaultAsync();
        }
        public async Task<bool> CheckDeleteAsync(int ma_dexuat)
        {
            var check = await _context.kySoDeXuats
                .Where(x => x.Ma_KySoDeXuat == ma_dexuat && x.TrangThai == false).FirstOrDefaultAsync();
            return check == null ? false : true;
        }
        public async Task<bool> ChuyenDuyetAsync(int ma_dexuat)
        {
            bool ret = false;
            try
            {
                var chuyenduyet = await _context.kySoDeXuats.Where(x => x.Ma_KySoDeXuat == ma_dexuat).FirstOrDefaultAsync();
                chuyenduyet.TrangThai = true;
                _context.kySoDeXuats.Update(chuyenduyet);
                await _context.SaveChangesAsync();
                ret = true;
            }
            catch { 
            }
            return ret;
            
        }
        public async Task<List<KySoDeXuat>> GetKySoChuaDeXuatAsync(int ma_nguoidung)
        {
            return await _context.kySoDeXuats
                .Where(x=>x.TrangThai==false && x.Ma_NguoiDeXuat==ma_nguoidung).ToListAsync();
        }
        public async Task<List<KySoDeXuat>> GetKySoChoDuyetAsync(int ma_nguoidung)
        {
            return await _context.kySoDeXuats
                .Where(x=>x.TrangThai==true && x.Ma_NguoiDeXuat == ma_nguoidung && x.IsDaDuyet==false)
                .ToListAsync();
        }
        public async Task<List<KySoDeXuat>> GetKySoDaDuyetAsync(int ma_nguoidung)
        {
            return await _context.kySoDeXuats
                .Where(x => x.IsDaDuyet == true && x.Ma_NguoiDeXuat == ma_nguoidung).ToListAsync();
        }
        public async Task<List<KySoDeXuat>> GetKySoTuChoiAsync(int ma_nguoidung)
        {
            var dexuat= await _context.kySoDeXuats.Where(x => x.Ma_NguoiDeXuat == ma_nguoidung)
                .Include(x=>x.KySoBuocDuyets)
                .ToListAsync();
            List<KySoDeXuat> danhsachtuchoi= new List<KySoDeXuat>();
            foreach(var item in dexuat)
            {
                if(item.KySoBuocDuyets.Count>0)
                {
                    var tuchoi = await _context.kySoBuocDuyets
                    .Where(x => x.Ma_KySoDeXuat == item.Ma_KySoDeXuat && x.IsTuChoi == true).FirstOrDefaultAsync();
                    if (tuchoi != null)
                    {
                        danhsachtuchoi.Add(item);
                    }
                }
            }
            return danhsachtuchoi;
        }
    }
}
