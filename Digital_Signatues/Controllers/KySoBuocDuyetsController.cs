using Digital_Signatues.Models;
using Digital_Signatues.Models.ViewPost;
using Digital_Signatues.Models.ViewPut;
using Digital_Signatues.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Digital_Signatues.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class KySoBuocDuyetsController : Controller
    {
        private readonly IKySoBuocDuyet _buocduyet;
        private readonly ILog _log;
        public KySoBuocDuyetsController(IKySoBuocDuyet buocduyet,ILog log)
        {
            _log=log;
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
                    var id = User.FindFirstValue("Id");
                    var postlog = new PostLog()
                    {
                        Ten_Log = "Thêm bước duyệt " + buocDuyet.Ten_Buoc + " thành công",
                        Ma_NguoiThucHien = int.Parse(id),
                        Ma_TaiKhoan=null,
                        Ma_DeXuat=buocDuyet.Ma_KySoDeXuat
                    };
                    if (await _log.PostLogAsync(postlog) > 0)
                    { }
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
                    var data = await _buocduyet.GetBuocDuyetAsync(buocDuyet.Ma_BuocDuyet);
                    var id = User.FindFirstValue("Id");
                    var postlog = new PostLog()
                    {
                        Ten_Log = "Cập nhật bước duyệt " + buocDuyet.Ten_Buoc + " thành công",
                        Ma_NguoiThucHien = int.Parse(id),
                        Ma_TaiKhoan = null,
                        Ma_DeXuat =data.Ma_KySoDeXuat
                    };
                    if (await _log.PostLogAsync(postlog) > 0)
                    { }
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Cập nhật bước duyệt thành công",
                        data =data
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
            var buocduyet = await _buocduyet.GetBuocDuyetAsync(id);
            if (await _buocduyet.CheckDeleteAsync(id))
            {
                if (await _buocduyet.DeleteBuocDuyetAsync(id))
                {
                    var ma_user = User.FindFirstValue("Id");
                    var postlog = new PostLog()
                    {
                        Ten_Log = "Xóa bước duyệt " + buocduyet.Ten_Buoc + " thành công",
                        Ma_NguoiThucHien = int.Parse(ma_user),
                        Ma_TaiKhoan = null,
                        Ma_DeXuat = buocduyet.Ma_KySoDeXuat
                    };
                    if (await _log.PostLogAsync(postlog) > 0)
                    { }
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
        /// <summary>
        /// từ chối duyệt 
        /// </summary>
        /// <param name="id">mã bước duyệt</param>
        /// <returns></returns>
        [HttpPut("{id}"),ActionName("tuchoi")]
        public async Task<IActionResult> TuChoiDuyetAsync(int id)
        {
            if(await _buocduyet.TuChoiDuyetAsync(id))
            {
                var data = await _buocduyet.GetBuocDuyetAsync(id);
                var mauser = User.FindFirstValue("Id");
                var postlog = new PostLog()
                {
                    Ten_Log = "Từ chối duyệt " + data.Ten_Buoc,
                    Ma_NguoiThucHien = int.Parse(mauser),
                    Ma_TaiKhoan = null,
                    Ma_DeXuat = data.Ma_KySoDeXuat
                };
                if (await _log.PostLogAsync(postlog) > 0)
                { }
                return Ok(new
                {
                    retCode = 1,
                    retText = "Từ chối duyệt thành công",
                    data = data
                });
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Từ chối duyệt thất bại",
                data =""
            });
        }
    }
}
