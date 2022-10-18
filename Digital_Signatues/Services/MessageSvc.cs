using Digital_Signatues.Models;
using Digital_Signatues.Models.ViewPost;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digital_Signatues.Services
{
    public interface IMessage
    {
        Task<int> PostMessageAsync(PostMessage message);
        Task<Message> GetMessageAsync(int ma_message);
        Task<List<Message>> GetMessagesAsync(int ma_dexuat);
        Task<bool> DeleteMessageAsync(int ma_message);
    }
    public class MessageSvc:IMessage
    {
        private readonly DataContext _context;
        public MessageSvc(DataContext context)
        {
            _context = context;
        }
        public async Task<int> PostMessageAsync(PostMessage message)
        {
            int ret = 0;
            try
            {
                Message post = new Message()
                {
                    Ma_NguoiDung=message.Ma_NguoiDung,
                    Y_Kien=message.Y_Kien,
                    ThoiGian=System.DateTime.Now,
                    FileDinhKem=message.FileDinhKem,
                    Ma_DeXuat=message.Ma_DeXuat
                };
                await _context.Messages.AddAsync(post);
                await _context.SaveChangesAsync();
                ret = post.Ma_Message;
            }
            catch { }
            return ret;
        }
        public async Task<Message> GetMessageAsync(int ma_message)
        {
            return await _context.Messages
                .Where(x => x.Ma_Message == ma_message)
                .Include(x=>x.NguoiDung)
                .FirstOrDefaultAsync();
        }
        public async Task<List<Message>> GetMessagesAsync(int ma_dexuat)
        {
            return await _context.Messages
                .Where(x => x.Ma_DeXuat == ma_dexuat)
                .Include(x => x.NguoiDung)
                .ToListAsync();
        }
        public async Task<bool> DeleteMessageAsync(int ma_message)
        {
            bool ret = false;
            try
            {
                var delete = await _context.Messages.Where(x=>x.Ma_Message==ma_message).FirstOrDefaultAsync();
                _context.Messages.Remove(delete);
                await _context.SaveChangesAsync();
                ret = true;
            }
            catch { }
            return ret;
        }
    }
}
