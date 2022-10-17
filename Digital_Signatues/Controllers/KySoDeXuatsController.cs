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
    [Route("api/[controller]/[action]")]
    public class KySoDeXuatsController : Controller
    {
        private readonly IKySoDeXuat _dexuat;
        public KySoDeXuatsController(IKySoDeXuat dexuat)
        {
            _dexuat = dexuat;
        }
        /// <summary>
        /// hiển thị toàn bộ ký số đề xuất
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("dexuat")]
        public async Task<IActionResult> GetDeXuatsAsync()
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy danh sách đề xuất thành công",
                data = await _dexuat.GetDeXuatsAsync()
            });
        }
        /// <summary>
        /// trả về đề xuất vừa chọn
        /// </summary>
        /// <param name="id">mã đề xuất</param>
        /// <returns></returns>
        [HttpGet("{id}"), ActionName("dexuat")]
        public async Task<IActionResult> GetDexuatAsync(int id)
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy ký số đề xuất thành công",
                data = await _dexuat.GetDeXuatAsync(id)
            });
        }
        /// <summary>
        /// lấy toàn bộ ký số đề xuất của người dùng
        /// </summary>
        /// <param name="id">mã người dùng</param>
        /// <returns></returns>
        [HttpGet("{id}"),ActionName("nguoidungdexuat")]
        public async Task<IActionResult> GetDexuatsAsync(int id)
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy danh sách ký số đề xuất thành công",
                data = await _dexuat.GetDeXuatsByNguoiDeXuatAsync(id)
            });
        }
        /// <summary>
        /// thêm ký số đề xuất
        /// </summary>
        /// <param name="dexuat"></param>
        /// <returns></returns>
        [HttpPost, ActionName("dexuat")]
        public async Task<IActionResult> PostDeXuatAsync([FromBody] PostKySoDeXuat dexuat)
        {
            if (ModelState.IsValid)
            {
                int id_dexuat = await _dexuat.PostDeXuatAsync(dexuat);
                if (id_dexuat > 0)
                {
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Đề xuất ký số thành công",
                        data = await _dexuat.GetDeXuatAsync(id_dexuat)
                    });
                }
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Đề xuất ký số thất bại",
                data = ""
            });
        }
        /// <summary>
        /// cập nhật ký số đề xuất
        /// </summary>
        /// <param name="kySoDeXuat"></param>
        /// <returns></returns>
        [HttpPut, ActionName("dexuat")]
        public async Task<IActionResult> PutDeXuatAsync([FromBody] PutKySoDeXuat kySoDeXuat)
        {
            if (ModelState.IsValid)
            {
                if (await _dexuat.PutDeXuatAsync(kySoDeXuat))
                {
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Cập nhật đề xuất thành công",
                        data = await _dexuat.GetDeXuatAsync(kySoDeXuat.Ma_KySoDeXuat)
                    });
                }
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Cập nhật đề xuất thất bại",
                data = ""
            });
        }
        /// <summary>
        /// xóa đề xuất
        /// </summary>
        /// <param name="id">mã đề xuất</param>
        /// <returns></returns>
        [HttpDelete("{id}"), ActionName("dexuat")]
        public async Task<IActionResult> DeleteDeXuatAsync(int id)
        {
            if(await _dexuat.CheckDeleteAsync(id))
            {
                if (await _dexuat.DeleteDeXuatAsync(id))
                {
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Xóa đề xuất thành công",
                        data = ""
                    });
                }
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Xóa đề xuất thất bại. Đề xuất đã được chuyển duyệt",
                data = await _dexuat.GetDeXuatAsync(id)
            });
        }
        /// <summary>
        /// chuyển duyệt đề xuất
        /// </summary>
        /// <param name="id">mã đề xuất</param>
        /// <returns></returns>
        [HttpPut("{id}"),ActionName("chuyenduyet")]
        public async Task<IActionResult> ChuyenDuyetAsync(int id)
        {
            if(await _dexuat.ChuyenDuyetAsync(id))
            {
                return Ok(new
                {
                    retCode = 1,
                    retText = "Chuyển duyệt đề xuất thành công",
                    data = await _dexuat.GetDeXuatAsync(id)
                });
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Chuyển duyệt thất bại",
                data = await _dexuat.GetDeXuatAsync(id)
            });
        }
    }
}
