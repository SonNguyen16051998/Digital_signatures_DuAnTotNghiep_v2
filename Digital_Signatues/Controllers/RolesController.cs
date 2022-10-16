using Digital_Signatues.Models;
using Digital_Signatues.Models.ViewPost;
using Digital_Signatues.Models.ViewPut;
using Digital_Signatues.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Digital_Signatues.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : Controller
    {
        private readonly IRole _role;
        public RolesController(IRole role)
        {
            _role = role;
        }
        /// <summary>
        /// trả về toàn bộ vai trò
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetRolesAsync()
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy danh sách vai trò thành công",
                data = await _role.GetRolesAsync()
            });
        }
        /// <summary>
        /// trả về vai trò được chọn
        /// </summary>
        /// <param name="id">mã vai trò</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleAsync(int id)
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy vai trò thành công",
                data = await _role.GetRoleAsync(id)
            });
        }
        /// <summary>
        /// thêm vai trò
        /// </summary>
        /// <param name="Role">tuyền về object role có tên và isdeleted=false</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostRoleAsync([FromBody] PostRole Role)
        {
            if (ModelState.IsValid)
            {
                Role addRole = new Role()
                {
                    IsDeleted = false,
                    Ten_Role = Role.Ten_Role
                };
                int id_Role = await _role.AddRoleAsync(addRole);
                if ( id_Role> 0)
                {
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Thêm role thành công",
                        data = await _role.GetRoleAsync(id_Role)
                    });
                }
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Dữ liệu không hợp lệ",
                data = ""
            });
        }
        /// <summary>
        /// cập nhật role
        /// </summary>
        /// <param name="putRole">truyền về tên và isdelted=false</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> PutRoleAsync([FromBody]PutRole putRole)
        {
            if (ModelState.IsValid)
            {
                int id_Role = await _role.UpdateRoleAsync(putRole);
                if ( id_Role> 0)
                {
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Cập nhật vai trò thành công",
                        data = await _role.GetRoleAsync(id_Role)
                    });
                }
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Dữ liệu không hợp lệ",
                data = ""
            });
        }
        /// <summary>
        /// xóa vai trò,trường isdeleted chuyển về true. đồng thời vai trò sẽ được xóa khỏi người dùng và tất cả quyền cũng sẽ bị xóa
        /// </summary>
        /// <param name="id">mã vai trò</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoleAsync(int id)
        {
            if (await _role.DeleteRoleAsync(id))
            {
                return Ok(new
                {
                    retCode = 1,
                    retText = "Xóa role thành công",
                    data = await _role.GetRoleAsync(id)
                });
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Xóa thất bại",
                data = ""
            });
        }
    }
}
