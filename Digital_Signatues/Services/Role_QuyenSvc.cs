using Digital_Signatues.Models;
using Digital_Signatues.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digital_Signatues.Services
{
    public interface IRole_Quyen
    {
        Task<bool> AddOrUpdateRole_QuyenAsync(PostRole_Quyen role_Quyens);
        Task<bool> DeleteAlNothavelRole_QuyenAsync(int id_role);
        Task<List<Role_Quyen>> GetRole_QuyensAsync(int id_role);//Hiển thị toàn bộ quyền đã có của role
        Task<List<Quyen>> GetRoleQuyenNotHaveAsync(int id_role);//lấy các quyền mà role chưa có
    }
    public class Role_QuyenSvc:IRole_Quyen
    {
        private readonly DataContext _context;
        public Role_QuyenSvc(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> AddOrUpdateRole_QuyenAsync(PostRole_Quyen role_Quyens)
        {
            bool ret = false;
            try
            {
                foreach(var item in role_Quyens.Quyens)
                {
                    var role_quyen = new Role_Quyen()
                    {
                        Ma_Quyen = item.id_Quyen,
                        Ma_Role = role_Quyens.Id_Role
                    };
                    await _context.Role_Quyens.AddAsync(role_quyen);
                }
                await _context.SaveChangesAsync();
                var nguoidung_role= await _context.NguoiDung_Roles
                                    .Where(x=>x.Ma_Role==role_Quyens.Id_Role).ToListAsync();
                if(nguoidung_role.Count>0)
                {
                    var idNguoidung_quyens = nguoidung_role.GroupBy(x => x.Ma_NguoiDung)
                                            .Select(p => new { Id_nguoidung = p.Key });
                    foreach (var item in idNguoidung_quyens)//xóa toàn bộ người dùng quyền có role vừa chỉnh sửa
                    {
                        var remove = await _context.NguoiDung_Quyens.Where(x => x.Ma_NguoiDung == item.Id_nguoidung).ToListAsync();
                        foreach(var removeNguoiDung in remove)
                        {
                            _context.NguoiDung_Quyens.Remove(removeNguoiDung);
                        }
                    }
                    await _context.SaveChangesAsync();
                    foreach (var item in idNguoidung_quyens)
                    {
                        var role_nguoidung = await _context.NguoiDung_Roles
                            .Where(x => x.Ma_NguoiDung == item.Id_nguoidung).ToListAsync();
                        foreach (var role in role_nguoidung)
                        {
                            var quyen = await _context.Role_Quyens.Where(x => x.Ma_Role == role.Ma_Role).ToListAsync();
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
                }
                ret = true;
            }
            catch
            {
                ret = false;
            }
            return ret;
        }
        public async Task<bool> DeleteAlNothavelRole_QuyenAsync(int id_role)
        {
            bool ret = false;
            try
            {
                var role_quyen=await _context.Role_Quyens.Where(x=>x.Ma_Role==id_role).ToListAsync();
                if(role_quyen.Count>0)
                {
                    foreach (var role in role_quyen)
                    {
                        _context.Role_Quyens.Remove(role);
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

        public async Task<List<Role_Quyen>> GetRole_QuyensAsync(int id_Role)
        {
            List<Role_Quyen> Role_Quyens = new List<Role_Quyen>();
            Role_Quyens = await _context.Role_Quyens.Where(x => x.Ma_Role == id_Role)
                            .Include(x => x.Quyen).ToListAsync();
            return Role_Quyens;
        }

        public async Task<List<Quyen>> GetRoleQuyenNotHaveAsync(int id_Role)
        {
            List<Role_Quyen> Role_Quyens = new List<Role_Quyen>();
            Role_Quyens = await _context.Role_Quyens.Where(x => x.Ma_Role == id_Role)
                                .ToListAsync();
            List<int> id_Quyen = new List<int>();
            id_Quyen = (from p in Role_Quyens
                       select p.Ma_Quyen).ToList();
            List<Quyen> quyens = new List<Quyen>();
            quyens = await _context.Quyens.Where(x => !id_Quyen.Contains(x.Ma_Quyen)).ToListAsync();
            return quyens;
        }
    }
}
