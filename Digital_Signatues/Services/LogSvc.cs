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
        /*Task<List<Log>> GetLogByNguoiThayDoiAsync(int ma_nguoithaydoi);
        Task<List<Log>> GetLogByNguoiThucHienAsync(int ma_nguoithuchien);
        Task<List<Log>> GetAllLogAsync();*/
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
                    ThoiGianThucHien = System.DateTime.Now
                };
                await _context.Logs.AddAsync(post);
                await _context.SaveChangesAsync();
                ret = post.Ma_Log;
            }
            catch { }
            return ret;
        }
    }
}
