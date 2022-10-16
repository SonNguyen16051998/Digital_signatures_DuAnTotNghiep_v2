using Digital_Signatues.Models;
using Digital_Signatues.Models.ViewModel;
using Digital_Signatues.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Digital_Signatues.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class NguoiDung_RolesController : Controller
    {
        private readonly INguoiDung_Role _nguoiDung_Role;
        public NguoiDung_RolesController(INguoiDung_Role nguoiDung_Role)
        {
            _nguoiDung_Role= nguoiDung_Role;
        }

        /// <summary>
        /// cập nhật hoặc thêm mới role cho người dùng
        /// </summary>
        /// <param name="nguoiDung_Roles">truyền về mã người dùng và mã role</param>
        /// <returns></returns>
        [HttpPost(),ActionName("rolenguoidung")]
        public async Task<IActionResult> UpdateOrAddNguoiDung_RoleAsync(PostNguoiDung_Role nguoiDung_Roles)
        {
            if(ModelState.IsValid)
            {
                if (await _nguoiDung_Role.DeleteAllRoleNguoiDung_RoleAsync(nguoiDung_Roles.Id_NguoiDung))
                {
                    if(await _nguoiDung_Role.AddOrUpdateNguoiDung_RoleAsync(nguoiDung_Roles))
                    {
                        return Ok(new
                        {
                            retCode = 1,
                            retText = "Cập nhật role cho người dùng thành công",
                            data = await _nguoiDung_Role.GetNguoiDung_RolesAsync(nguoiDung_Roles.Id_NguoiDung)
                        });
                    }
                }
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Cập nhật role cho người dùng thất bại",
                data = ""
            });
        }
        /// <summary>
        /// trả về toàn bộ role người dùng đang có
        /// </summary>
        /// <param name="id">id người dùng</param>
        /// <returns></returns>
        [HttpGet("{id}"),ActionName("rolein")]
        public async Task<IActionResult> GetNguoiDung_RolesAsync(int id)//hiển thị toàn bộ role của người dùng có
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy danh sách vai trò thành công",
                data = await _nguoiDung_Role.GetNguoiDung_RolesAsync(id)
            });
        }
        /// <summary>
        /// trả về role mà người dùng chưa có
        /// </summary>
        /// <param name="id">id người dùng</param>
        /// <returns></returns>
        [HttpGet("{id}"),ActionName("rolenotin")]
        public async Task<List<Role>> GetRoleNguoiDungNotHaveAsync(int id)// hiển thị các role mà người dùng chưa có
        {
            return await _nguoiDung_Role.GetRoleNguoiDungNotHaveAsync(id);//id của người dùng
        }
        /// <summary>
        /// trả về các quyền mà người dùng có
        /// </summary>
        /// <param name="id">id của người dùng</param>
        /// <returns></returns>
        [HttpGet("{id}"),ActionName("nguoidungquyen")]
        public async Task<List<NguoiDung_Quyen>> GetNguoiDungQuyensAsync(int id)//id người dùng
        {
            return await _nguoiDung_Role.GetNguoiDung_QuyensAsync(id);
        }    
    }
}
