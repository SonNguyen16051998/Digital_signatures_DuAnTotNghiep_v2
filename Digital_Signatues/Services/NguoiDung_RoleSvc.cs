using Digital_Signatues.Models;
using Digital_Signatues.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digital_Signatues.Services
{
    public interface INguoiDung_Role
    {
        Task<bool> AddOrUpdateNguoiDung_RoleAsync(PostNguoiDung_Role nguoiDung_Roles);
        Task<bool> DeleteAllRoleNguoiDung_RoleAsync(int id_nguoiDung);
        Task<List<NguoiDung_Role>> GetNguoiDung_RolesAsync(int id_nguoiDung);//Hiển thị toàn bộ role đã chọn của người dùng
        Task<List<Role>> GetRoleNguoiDungNotHaveAsync(int id_nguoiDung);//lấy các role mà người dùng chưa có
        Task<List<NguoiDung_Quyen>> GetNguoiDung_QuyensAsync(int id_nguoiDung);
    }
    public class NguoiDung_RoleSvc : INguoiDung_Role
    {
        private readonly DataContext _context;
        public NguoiDung_RoleSvc(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> AddOrUpdateNguoiDung_RoleAsync(PostNguoiDung_Role nguoiDung_Roles)
        {
            bool ret = false;
            try
            {
                foreach(var item in nguoiDung_Roles.Roles)
                {
                    var nguoidung_role = new NguoiDung_Role()
                    {
                        Ma_NguoiDung = nguoiDung_Roles.Id_NguoiDung,
                        Ma_Role = item.id_role
                    };
                    await _context.NguoiDung_Roles.AddAsync(nguoidung_role);
                    await _context.SaveChangesAsync();
                    var quyen = await _context.Role_Quyens.Where(x => x.Ma_Role == item.id_role).ToListAsync();
                    foreach(var item1 in quyen)
                    {
                        var nguoidung_quyen = await _context.NguoiDung_Quyens
                            .Where(x => x.Ma_NguoiDung == nguoiDung_Roles.Id_NguoiDung
                                    && x.Ma_Quyen == item1.Ma_Quyen).FirstOrDefaultAsync();
                        if(nguoidung_quyen!=null)//kiem tra quyen nay nguoi dung da co chua
                        {
                            continue;
                        }
                        else//chua co thi them moi
                        {
                            var postnguoidung_quyen = new NguoiDung_Quyen()
                            {
                                Ma_NguoiDung=nguoiDung_Roles.Id_NguoiDung,
                                Ma_Quyen=item1.Ma_Quyen
                            };
                            await _context.NguoiDung_Quyens.AddAsync(postnguoidung_quyen);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                ret = true;
            }
            catch
            {
                ret = false;
            }
            return ret;
        }
        public async Task<bool> DeleteAllRoleNguoiDung_RoleAsync(int id_Nguoidung)
        {
            bool ret = false;
            try
            {
                var nguoidung=await _context.NguoiDung_Roles.Where(x=>x.Ma_NguoiDung==id_Nguoidung).ToListAsync();
                if(nguoidung!=null)
                {
                    foreach (var item in nguoidung)
                    {
                        _context.NguoiDung_Roles.Remove(item);
                    }
                    var nguoidung_quyen = await _context.NguoiDung_Quyens.Where(x => x.Ma_NguoiDung == id_Nguoidung).ToListAsync();
                    if (nguoidung_quyen != null)
                    {
                        foreach (var item1 in nguoidung_quyen)
                        {
                            _context.NguoiDung_Quyens.Remove(item1);
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                ret = true;
            }
            catch
            {
                ret = false;
            }
            return ret;
        }

        public async Task<List<NguoiDung_Role>> GetNguoiDung_RolesAsync(int id_nguoiDung)
        {
            var nguoiDung_Roles=await _context.NguoiDung_Roles.Where(x=>x.Ma_NguoiDung==id_nguoiDung)
                            .Include(x=>x.Role).ToListAsync();
            return nguoiDung_Roles;
        }

        public async Task<List<Role>> GetRoleNguoiDungNotHaveAsync(int id_nguoiDung)
        {
            List<NguoiDung_Role> nguoiDung_Roles = new List<NguoiDung_Role>();
            nguoiDung_Roles = await _context.NguoiDung_Roles.Where(x => x.Ma_NguoiDung == id_nguoiDung)
                                .ToListAsync();
            List<int> Id_Role=new List<int>();
            Id_Role= (from p in nguoiDung_Roles
                      select p.Ma_Role).ToList();
            List<Role> roles=new List<Role>();
            roles=await _context.Roles.Where(x=>!Id_Role.Contains(x.Ma_Role)).ToListAsync();
            return roles;
        }

        public async Task<List<NguoiDung_Quyen>> GetNguoiDung_QuyensAsync(int id_nguoiDung)
        {
            List<NguoiDung_Quyen> nguoiDung_Quyens = new List<NguoiDung_Quyen>();
            nguoiDung_Quyens = await _context.NguoiDung_Quyens.Where(x => x.Ma_NguoiDung == id_nguoiDung)
                                .Include(x => x.Quyen)
                                .ToListAsync();
            return nguoiDung_Quyens;
        }
    }
}
