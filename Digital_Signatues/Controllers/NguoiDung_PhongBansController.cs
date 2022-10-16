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
    public class NguoiDung_PhongBansController : Controller
    {
        private readonly INguoiDung_PhongBan _nguoiDung_PhongBan;
        public NguoiDung_PhongBansController(INguoiDung_PhongBan nguoiDung_PhongBan)
        {
            _nguoiDung_PhongBan = nguoiDung_PhongBan;
        }

        /// <summary>
        /// cập nhật hoặc thêm mới phòng ban cho người dùng
        /// </summary>
        /// <param name="NguoiDung_PhongBans">truyền về đầy đủ dữ liệu object nguoidung_phongban</param>
        /// <returns></returns>
        [HttpPost(), ActionName("nguoidungphongban")]
        public async Task<IActionResult> UpdateOrAddNguoiDung_PhongBanAsync(PostNguoiDung_PhongBan NguoiDung_PhongBans)
        {
            if (ModelState.IsValid)
            {
                if (await _nguoiDung_PhongBan.DeleteAlNothavelNguoiDung_PhongBanAsync(NguoiDung_PhongBans.Id_NguoiDung))
                {
                    if (await _nguoiDung_PhongBan.AddOrUpdateNguoiDung_PhongBanAsync(NguoiDung_PhongBans))
                    {
                        return Ok(new
                        {
                            retCode = 1,
                            retText = "Cập nhật phòng ban cho người dùng thành công",
                            data = await _nguoiDung_PhongBan.GetNguoiDung_PhongBansAsync(NguoiDung_PhongBans.Id_NguoiDung)
                        });
                    }
                }
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Cập nhật phòng ban cho người dùng thất bại",
                data = ""
            });
        }
        /// <summary>
        /// trả về toàn bộ phòng ban người dùng đang có
        /// </summary>
        /// <param name="id">id người dùng</param>
        /// <returns></returns>
        [HttpGet("{id}"), ActionName("phongbanin")]
        public async Task<IActionResult> GetNguoiDung_PhongBansAsync(int id)
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy danh sách phòng ban của người dùng thành công",
                data = await _nguoiDung_PhongBan.GetNguoiDung_PhongBansAsync(id)
        });
        }
        /// <summary>
        /// trả về phòng ban mà người dùng chưa có
        /// </summary>
        /// <param name="id">id người dùng</param>
        /// <returns></returns>
        [HttpGet("{id}"), ActionName("phongbannotin")]
        public async Task<List<PhongBan>> GetRoleNguoiDungNotHaveAsync(int id)
        {
            return await _nguoiDung_PhongBan.GetNguoiDung_PhongBanNotHaveAsync(id);
        }
    }
}
