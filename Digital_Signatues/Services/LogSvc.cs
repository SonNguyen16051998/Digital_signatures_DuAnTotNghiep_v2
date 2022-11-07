using Digital_Signatues.Models;
using Digital_Signatues.Models.ViewPost;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digital_Signatues.Services
{
    public interface ILog
    {
        Task<int> PostLogAsync(PostLog log);
        Task<List<Log>> GetAllLogAsync();
        Task<List<Log>> GetAllLogThongSoAsync(int ma_taikhoan);
        Task<List<Log>> GetAllLogDeXuatAsync(int ma_dexuat);
    }
    public class LogSvc:ILog
    {
        private readonly DataContext _context;
        public LogSvc(DataContext context)
        {
            _context = context;
        }
        public async Task<int> PostLogAsync(PostLog log)
        {
            int ret = 0;
            try
            {
                var post = new Log()
                {
                    Ten_Log = log.Ten_Log,
                    Ma_NguoiThucHien = log.Ma_NguoiThucHien,
                    ThoiGianThucHien = System.DateTime.Now,
                    Ma_TaiKhoan=log.Ma_TaiKhoan,
                    Ma_DeXuat=log.Ma_DeXuat
                };
                await _context.Logs.AddAsync(post);
                await _context.SaveChangesAsync();
                ret = post.Ma_Log;
            }
            catch { }
            return ret;
        }
        public async Task<List<Log>> GetAllLogAsync()
        {
            return await _context.Logs.OrderByDescending(x=>x.ThoiGianThucHien)
                .Include(x=>x.NguoiDung)
                .ToListAsync();
        }
        public async Task<List<Log>> GetAllLogThongSoAsync(int ma_taikhoan)
        {
            return await _context.Logs.Where(x=>x.Ma_TaiKhoan==ma_taikhoan && string.IsNullOrEmpty(x.Ma_DeXuat.ToString()))
                .OrderByDescending(x => x.ThoiGianThucHien)
                .Include(x => x.NguoiDung)
                .ToListAsync();
        }
        public async Task<List<Log>> GetAllLogDeXuatAsync(int ma_dexuat)
        {
            return await _context.Logs.Where(x => x.Ma_DeXuat == ma_dexuat && string.IsNullOrEmpty(x.Ma_TaiKhoan.ToString()))
                .OrderByDescending(x => x.ThoiGianThucHien)
                .Include(x => x.NguoiDung)
                .ToListAsync();
        }
    }
}
