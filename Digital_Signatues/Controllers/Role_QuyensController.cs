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
    public class Role_QuyensController : Controller
    {
        private readonly IRole_Quyen _role_Quyen;
        public Role_QuyensController(IRole_Quyen role_Quyen)
        {
            _role_Quyen = role_Quyen;
        }
        /// <summary>
        ///thêm hoăc cập nhật quyền cho role
        /// </summary>
        /// <param name="Role_Quyens">truyền về danh sách object role_quyen</param>
        /// <returns></returns>
        [HttpPost(), ActionName("rolequyen")]
        public async Task<IActionResult> UpdateOrAddRole_QuyenAsync(PostRole_Quyen Role_Quyens)
        {
            if (ModelState.IsValid)
            {
                if (await _role_Quyen.DeleteAlNothavelRole_QuyenAsync(Role_Quyens.Id_Role))
                {
                    if (await _role_Quyen.AddOrUpdateRole_QuyenAsync(Role_Quyens))
                    {
                        return Ok(new
                        {
                            retCode = 1,
                            retText = "Cập nhật quyền cho role thành công",
                            data = await _role_Quyen.GetRole_QuyensAsync(Role_Quyens.Id_Role)
                        });
                    }
                }
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Cập nhật quyền cho role thất bại",
                data = ""
            });
        }
        /// <summary>
        /// hiển thị toàn bộ quyền mà role có
        /// </summary>
        /// <param name="id">mã role</param>
        /// <returns></returns>
        [HttpGet("{id}"), ActionName("quyenin")]
        public async Task<IActionResult> GetRole_QuyensAsync(int id)//hiển thị toàn bộ quyền của role có
        {
            var list = await _role_Quyen.GetRole_QuyensAsync(id);
            return Ok(new
            {
                retCode = 1,
                retText = "Danh sách quyền thuộc vai trò",
                data = list
            });
            //return await _role_Quyen.GetRole_QuyensAsync(id);//id của role
        }
        /// <summary>
        /// hiển thị các quyền mà role chưa có
        /// </summary>
        /// <param name="id">mã role</param>
        /// <returns></returns>
        [HttpGet("{id}"), ActionName("quyennotin")]
        public async Task<List<Quyen>> GetQuyenRoleNotHaveAsync(int id)// hiển thị các quyen mà role chưa có
        {
            return await _role_Quyen.GetRoleQuyenNotHaveAsync(id);//id của role
        }
    }
}
