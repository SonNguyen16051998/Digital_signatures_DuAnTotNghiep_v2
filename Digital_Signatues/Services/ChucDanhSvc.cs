using Digital_Signatues.Models;
using Digital_Signatues.Models.ViewPut;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digital_Signatues.Services
{
    public interface IChucDanh
    {
        Task<List<ChucDanh>> GetChucDanhsAsync();
        Task<ChucDanh> GetChucDanhAsync(int id);
        Task<bool> DeleteChucDanhAsync(int id);
        Task<int> AddChucDanhAsync(ChucDanh chucDanh);
        Task<int> UpdateChucDanhAsync(PutChucDanh putChucDanh);
        Task<bool> SapXepThuTuAsync(List<PutSapXep> chucDanhs);
    }
    public class ChucDanhSvc:IChucDanh
    {
       
        private readonly DataContext _context;
        public ChucDanhSvc(DataContext context)
        {
            _context = context;
        }
        public async Task<List<ChucDanh>> GetChucDanhsAsync()
        {
            List<ChucDanh> chucDanhs = new List<ChucDanh>();
            chucDanhs = await _context.ChucDanhs
                        .OrderBy(x=>x.Order)    
                        .Include(x=>x.NguoiDung)
                        .ToListAsync();
            return chucDanhs;
        }

        public async Task<ChucDanh> GetChucDanhAsync(int id)
        {
            ChucDanh chucDanh = new ChucDanh();
            chucDanh = await _context.ChucDanhs.Where(x => x.Ma_ChucDanh == id)
                    .Include(x=>x.NguoiDung)
                    .FirstOrDefaultAsync();
            return chucDanh;
        }

        public async Task<bool> DeleteChucDanhAsync(int id)
        {
            bool ret = false;
            try
            {
                NguoiDung nguoiDung_ChucDanh=new NguoiDung();
                nguoiDung_ChucDanh=await _context.NguoiDungs.Where(x=>x.Ma_ChucDanh==id).FirstOrDefaultAsync();
                if (nguoiDung_ChucDanh!=null)
                {
                    ret = false;
                }
                else
                {
                    ChucDanh chucDanh = new ChucDanh();
                    chucDanh = await _context.ChucDanhs.Where(x => x.Ma_ChucDanh == id).FirstOrDefaultAsync();
                    chucDanh.IsDeleted = true;
                    _context.ChucDanhs.Update(chucDanh);
                    await _context.SaveChangesAsync();
                    ret = true;
                }
            }
            catch { }
            return ret;
        }

        public async Task<int> AddChucDanhAsync(ChucDanh chucDanh)
        {
            int ret = 0;
            try
            {
                var chucdanh = await _context.ChucDanhs.OrderByDescending(x => x.Order).Take(1).FirstOrDefaultAsync();
                if(chucdanh==null)
                {
                    chucDanh.Order = 1;
                }
                else
                {
                    chucDanh.Order = chucdanh.Order + 1;
                }
                await _context.ChucDanhs.AddAsync(chucDanh);
                await _context.SaveChangesAsync();
                ret = chucDanh.Ma_ChucDanh;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }

        public async Task<int> UpdateChucDanhAsync(PutChucDanh putChucDanh)
        {
            int ret = 0;
            try
            {
                var update=await _context.ChucDanhs.Where(x=>x.Ma_ChucDanh== putChucDanh.Ma_ChucDanh).FirstOrDefaultAsync();
                update.Ten_ChucDanh = putChucDanh.Ten_ChucDanh;
                _context.ChucDanhs.Update(update);
                await _context.SaveChangesAsync();
                ret = update.Ma_ChucDanh;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }
        public async Task<bool> SapXepThuTuAsync(List<PutSapXep> chucDanhs)
        {
            bool ret = false;
            try
            {
                foreach(var item in chucDanhs)
                {
                    ChucDanh chucDanh = await _context.ChucDanhs.Where(x => x.Ma_ChucDanh == item.Id).FirstOrDefaultAsync();
                    if(chucDanh.Order==item.Order)
                    {
                        continue;
                    }
                    else
                    {
                        chucDanh.Order = item.Order;
                        _context.ChucDanhs.Update(chucDanh);
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
