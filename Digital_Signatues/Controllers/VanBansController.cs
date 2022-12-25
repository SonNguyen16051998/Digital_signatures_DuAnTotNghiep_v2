using Digital_Signatues.Models;
using Digital_Signatues.Models.ViewPost;
using Digital_Signatues.Models.ViewPut;
using Digital_Signatues.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Digital_Signatues.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VanBansController : Controller
    {
        private readonly IVanBan _vanban;
        public VanBansController(IVanBan vanban)
        {
            _vanban = vanban;
        }
        /// <summary>
        /// trả về toàn bộ văn bản
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetVanBansAsync()
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy danh sách văn bản thành công",
                data = await _vanban.GetVanBansAsync()
            });
        }
        /// <summary>
        /// trả về văn bản được chọn
        /// </summary>
        /// <param name="id">mã văn bản</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVanBanAsync(int id)
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy văn bản thành công",
                data = await _vanban.GetVanBanAsync(id)
            });
        }
        /// <summary>
        /// thêm văn bản
        /// </summary>
        /// <param name="postvanban"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "vanban")]
        public async Task<IActionResult> PostVanBanAsync([FromBody] PostVanBan postvanban)
        {
            if (ModelState.IsValid)
            {
                int id_VanBan = await _vanban.AddVanBanAsync(postvanban);
                if (id_VanBan > 0)
                {
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Thêm văn bản thành công",
                        data = await _vanban.GetVanBanAsync(id_VanBan)
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
        /// cập nhật văn bản
        /// </summary>
        /// <param name="putVanBan"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Policy = "vanban")]
        public async Task<IActionResult> PutVanBanAsync([FromBody] PutVanBan putVanBan)
        {
            if (ModelState.IsValid)
            {
                int id_VanBan = await _vanban.UpdateVanBanAsync(putVanBan);
                if (id_VanBan > 0)
                {
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Cập nhật văn bản thành công",
                        data = await _vanban.GetVanBanAsync(id_VanBan)
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
        /// Xóa văn bản
        /// </summary>
        /// <param name="id">mã văn bản</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Policy = "vanban")]
        public async Task<IActionResult> DeleteVanBanAsync(int id)
        {
            if (await _vanban.DeleteVanBanAsync(id))
            {
                return Ok(new
                {
                    retCode = 1,
                    retText = "Xóa văn bản thành công",
                    data = await _vanban.GetVanBanAsync(id)
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
