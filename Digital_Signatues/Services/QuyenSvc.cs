using Digital_Signatues.Models;
using Digital_Signatues.Models.ViewPut;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digital_Signatues.Services
{
    public interface IQuyen
    {
        Task<List<Quyen>> GetQuyensAsync();
        Task<Quyen> GetQuyenAsync(int id);
        Task<bool> DeleteQuyenAsync(int id); 
        Task<int> AddQuyenAsync (Quyen quyen);
        Task<int> UpdateQuyenAsync(PutQuyen putQuyen);
       
    }
    public class QuyenSvc:IQuyen
    {
        private readonly DataContext _context;
        public QuyenSvc(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Quyen>> GetQuyensAsync()
        {
            List<Quyen> quyens= new List<Quyen>();
            quyens = await _context.Quyens.ToListAsync();
            return quyens;
        }

        public async Task<Quyen> GetQuyenAsync(int id)
        {
            Quyen quyen= new Quyen();
            quyen = await _context.Quyens.Where(x => x.Ma_Quyen == id).FirstOrDefaultAsync();
            return quyen;
        }

        public async Task<bool> DeleteQuyenAsync(int id)
        {
            bool ret = false;
            try
            {
                List<NguoiDung_Quyen> nguoiDungs= new List<NguoiDung_Quyen>();
                nguoiDungs=await _context.NguoiDung_Quyens.Where(x=>x.Ma_Quyen==id).ToListAsync();
                foreach (var item in nguoiDungs)//xóa quyền ở bảng người dùng_quyền
                {
                    _context.NguoiDung_Quyens.Remove(item);
                }

                List<Role_Quyen> role_Quyens= new List<Role_Quyen>();
                role_Quyens=await _context.Role_Quyens.Where(x=>x.Ma_Quyen== id).ToListAsync();
                foreach (var item in role_Quyens)//xóa quyền ở bảng role_quyền
                {
                    _context.Role_Quyens.Remove(item);
                }

                Quyen quyen = new Quyen();
                quyen = await _context.Quyens.Where(x => x.Ma_Quyen == id).FirstOrDefaultAsync();
                quyen.Isdeleted = true;
                _context.Quyens.Update(quyen);
                await _context.SaveChangesAsync();
                ret = true;
            }
            catch { }
            return ret;
        }

        public async Task<int> AddQuyenAsync(Quyen quyen)
        {
            int ret = 0;
            try
            {
                await _context.Quyens.AddAsync(quyen);
                await _context.SaveChangesAsync();
                ret = quyen.Ma_Quyen;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }

        public async Task<int> UpdateQuyenAsync(PutQuyen putQuyen)
        {
            int ret = 0;
            try
            {
                var update = await _context.Quyens.Where(x => x.Ma_Quyen == putQuyen.Ma_Quyen).FirstOrDefaultAsync();
                update.Ten_Quyen=putQuyen.Ten_Quyen;
                _context.Quyens.Update(update);
                await _context.SaveChangesAsync();
                ret = update.Ma_Quyen;   
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }
        
    }
}
