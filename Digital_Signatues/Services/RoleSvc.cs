using Digital_Signatues.Models;
using Digital_Signatues.Models.ViewPut;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digital_Signatues.Services
{
    public interface IRole
    {
        Task<List<Role>> GetRolesAsync();
        Task<Role> GetRoleAsync(int id);
        Task<bool> DeleteRoleAsync(int id);
        Task<int> AddRoleAsync(Role role);
        Task<int> UpdateRoleAsync(PutRole putRole);
    }
    public class RoleSvc:IRole
    {
        private readonly DataContext _context;
        public RoleSvc(DataContext context)
        {
            _context = context;
        }
        public async Task<List<Role>> GetRolesAsync()
        {
            List<Role> roles = new List<Role>();
            roles = await _context.Roles.ToListAsync();
            return roles;
        }

        public async Task<Role> GetRoleAsync(int id)
        {
            Role role = new Role();
            role = await _context.Roles.Where(x =>x.Ma_Role == id).FirstOrDefaultAsync();
            return role;
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            bool ret = false;
            try
            {
                List<NguoiDung_Role> nguoiDungs = new List<NguoiDung_Role>();
                nguoiDungs = await _context.NguoiDung_Roles.Where(x => x.Ma_Role == id).ToListAsync();
                foreach(var item in nguoiDungs)//xoas role của người dùng
                {
                    _context.NguoiDung_Roles.Remove(item);
                }
                await _context.SaveChangesAsync();

                var idNguoidung_quyens = nguoiDungs.GroupBy(x => x.Ma_NguoiDung)
                                            .Select(p => new { Id_nguoidung = p.Key });
                foreach (var item in idNguoidung_quyens)//xóa toàn bộ người dùng quyền có role vừa chỉnh sửa
                {
                    var remove = await _context.NguoiDung_Quyens.Where(x => x.Ma_NguoiDung == item.Id_nguoidung).ToListAsync();
                    foreach (var removeNguoiDung in remove)
                    {
                        _context.NguoiDung_Quyens.Remove(removeNguoiDung);
                    }
                }
                await _context.SaveChangesAsync();

                foreach (var item in idNguoidung_quyens)
                {
                    var role_nguoidung = await _context.NguoiDung_Roles
                        .Where(x => x.Ma_NguoiDung == item.Id_nguoidung).ToListAsync();
                    foreach (var item2 in role_nguoidung)
                    {
                        var quyen = await _context.Role_Quyens.Where(x => x.Ma_Role == item2.Ma_Role).ToListAsync();
                        foreach (var item3 in quyen)//thêm lại quyền cho các user vừa xóa
                        {
                            var nguoidung_quyen = await _context.NguoiDung_Quyens
                                .Where(x => x.Ma_NguoiDung == item.Id_nguoidung
                                        && x.Ma_Quyen == item3.Ma_Quyen).FirstOrDefaultAsync();
                            if (nguoidung_quyen != null)//kiem tra quyen nay nguoi dung da co chua
                            {
                                continue;
                            }
                            else//chua co thi them moi
                            {
                                var postnguoidung_quyen = new NguoiDung_Quyen()
                                {
                                    Ma_NguoiDung = item.Id_nguoidung,
                                    Ma_Quyen = item3.Ma_Quyen
                                };
                                await _context.NguoiDung_Quyens.AddAsync(postnguoidung_quyen);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                }

                List<Role_Quyen> role_Quyens=new List<Role_Quyen>();
                role_Quyens=await _context.Role_Quyens.Where(x=>x.Ma_Role==id).ToListAsync();
                foreach(var item in role_Quyens)//xóa quyền của role
                {
                    _context.Role_Quyens.Remove(item);
                }

                Role role = new Role();
                role = await _context.Roles.Where(x => x.Ma_Role == id).FirstOrDefaultAsync();
                role.IsDeleted = true;
                _context.Roles.Update(role);
                await _context.SaveChangesAsync();
                ret = true;
            }
            catch { }
            return ret;
        }

        public async Task<int> AddRoleAsync(Role role)
        {
            int ret = 0;
            try
            {
                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();
                ret = role.Ma_Role;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }

        public async Task<int> UpdateRoleAsync(PutRole putRole)
        {
            int ret = 0;
            try
            {
                var update=await _context.Roles.Where(x=>x.Ma_Role==putRole.Ma_Role).FirstOrDefaultAsync();
                update.Ten_Role=putRole.Ten_Role;
                _context.Roles.Update(update);
                await _context.SaveChangesAsync();
                ret = update.Ma_Role;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }
    }
}
