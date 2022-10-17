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
    public class KySoBuocDuyetsController : Controller
    {
        private readonly IKySoBuocDuyet _buocduyet;
        public KySoBuocDuyetsController(IKySoBuocDuyet buocduyet)
        {
            _buocduyet = buocduyet;
        }
        /// <summary>
        /// hiển thị toàn bộ ký số bước duyệt của đề xuất
        /// </summary>
        /// <param name="id">mã đề xuất</param>
        /// <returns></returns>
        [HttpGet("{id}"), ActionName("buocduyet")]
        public async Task<IActionResult> GetBuocDuyetsAsync(int id)
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy danh sách bước duyệt thành công",
                data = await _buocduyet.GetAllBuocDuyetAsync(id)
            });
        }
        /// <summary>
        /// trả về buoc duyệt được chọn
        /// </summary>
        /// <param name="id">mã bước duyệt</param>
        /// <returns></returns>
        [HttpGet("{id}"), ActionName("chitietbuocduyet")]
        public async Task<IActionResult> GetBuocDuyetAsync(int id)
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy bước duyệt thành công",
                data = await _buocduyet.GetBuocDuyetAsync(id)
            });
        }
        /// <summary>
        /// thêm bước duyệt
        /// </summary>
        /// <param name="buocDuyet"></param>
        /// <returns></returns>
        [HttpPost, ActionName("buocduyet")]
        public async Task<IActionResult> PostBuocDuyetAsync([FromBody] PostKySoBuocDuyet buocDuyet)
        {
            if (ModelState.IsValid)
            {
                int id_buocduyet = await _buocduyet.PostBuocDuyetAsync(buocDuyet);
                if (id_buocduyet > 0)
                {
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Thêm bước duyệt thành công",
                        data = await _buocduyet.GetBuocDuyetAsync(id_buocduyet)
                    });
                }
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Thêm bước duyệt thất bại",
                data = ""
            });
        }
        /// <summary>
        /// cập nhật bước duyệt
        /// </summary>
        /// <param name="buocDuyet"></param>
        /// <returns></returns>
        [HttpPut, ActionName("buocduyet")]
        public async Task<IActionResult> PutBuocDuyetAsync([FromBody] PutKySoBuocDuyet buocDuyet)
        {
            if (ModelState.IsValid)
            {
                if (await _buocduyet.PutBuocDuyetAsync(buocDuyet))
                {
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Cập nhật đề xuất thành công",
                        data = await _buocduyet.GetBuocDuyetAsync(buocDuyet.Ma_BuocDuyet)
                    });
                }
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Cập nhật bước duyệt thất bại",
                data = ""
            });
        }
        /// <summary>
        /// xóa bước duyệt
        /// </summary>
        /// <param name="id">mã bước duyệt</param>
        /// <returns></returns>
        [HttpDelete("{id}"), ActionName("buocduyet")]
        public async Task<IActionResult> DeleteBuocDuyetAsync(int id)
        {
            if (await _buocduyet.CheckDeleteAsync(id))
            {
                if (await _buocduyet.DeleteBuocDuyetAsync(id))
                {
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Xóa bước duyệt thành công",
                        data = ""
                    });
                }
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Xóa bước duyệt thất bại. đề xuất đã được chuyển duyệt",
                data = await _buocduyet.GetBuocDuyetAsync(id)
            });
        }
    }
}
