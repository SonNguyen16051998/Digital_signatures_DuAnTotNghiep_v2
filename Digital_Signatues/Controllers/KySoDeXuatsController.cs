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
    public class KySoDeXuatsController : Controller
    {
        private readonly IKySoDeXuat _dexuat;
        private readonly ILog _log;
        public KySoDeXuatsController(IKySoDeXuat dexuat,ILog log)
        {
            _log=log;
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
        [Authorize(Policy = "dexuat")]
        public async Task<IActionResult> PostDeXuatAsync([FromBody] PostKySoDeXuat dexuat)
        {
            if (ModelState.IsValid)
            {
                int id_dexuat = await _dexuat.PostDeXuatAsync(dexuat);
                if (id_dexuat > 0)
                {
                    var ma_user = User.FindFirstValue("Id");
                    var postlog = new PostLog()
                    {
                        Ten_Log = "Tạo đề xuất thành công",
                        Ma_NguoiThucHien = int.Parse(ma_user),
                        Ma_TaiKhoan=null,
                        Ma_DeXuat=id_dexuat
                    };
                    if (await _log.PostLogAsync(postlog) > 0)
                    { }
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Tạo đề xuất thành công",
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
        [Authorize(Policy = "dexuat")]
        public async Task<IActionResult> PutDeXuatAsync([FromBody] PutKySoDeXuat kySoDeXuat)
        {
            if (ModelState.IsValid)
            {
                if (await _dexuat.PutDeXuatAsync(kySoDeXuat))
                {
                    var ma_user = User.FindFirstValue("Id");
                    var postlog = new PostLog()
                    {
                        Ten_Log = "Cập nhật đề xuất thành công",
                        Ma_NguoiThucHien = int.Parse(ma_user),
                        Ma_TaiKhoan=null,
                        Ma_DeXuat=kySoDeXuat.Ma_KySoDeXuat
                    };
                    if (await _log.PostLogAsync(postlog) > 0)
                    { }
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Cập nhật đề xuất thành công",
                        data = await _dexuat.GetDeXuatAsync(kySoDeXuat.Ma_KySoDeXuat)
                    });
                }
                else
                {
                    var ma_user = User.FindFirstValue("Id");
                    var postlog = new PostLog()
                    {
                        Ten_Log = "Cập nhật đề xuất thất bại",
                        Ma_NguoiThucHien = int.Parse(ma_user),
                        Ma_TaiKhoan=null,
                        Ma_DeXuat=kySoDeXuat.Ma_KySoDeXuat
                    };
                    if (await _log.PostLogAsync(postlog) > 0)
                    { }
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
        [Authorize(Policy = "dexuat")]
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
        [Authorize(Policy = "dexuat")]
        public async Task<IActionResult> ChuyenDuyetAsync(int id)
        {
            if(await _dexuat.ChuyenDuyetAsync(id))
            {
                var ma_user = User.FindFirstValue("Id");
                var postlog = new PostLog()
                {
                    Ten_Log = "Chuyển duyệt thành công",
                    Ma_NguoiThucHien = int.Parse(ma_user),
                    Ma_TaiKhoan=null,
                    Ma_DeXuat=id
                };
                if (await _log.PostLogAsync(postlog) > 0)
                { }
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
        /// <summary>
        /// lấy danh sách ký số chưa đề xuất của người dùng đang đăng nhập
        /// </summary>
        /// <param name="id">mã người dùng</param>
        /// <returns></returns>
        [HttpGet("{id}"),ActionName("chuadexuat")]
        public async Task<IActionResult> GetChuaDeXuatAsync(int id)
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy danh sách ký số chưa đề xuất thành công",
                data = await _dexuat.GetKySoChuaDeXuatAsync(id)
            });
        }
        /// <summary>
        /// lấy danh sách ký số chờ duyệt của người dùng đang đăng nhập
        /// </summary>
        /// <param name="id">mã người dùng</param>
        /// <returns></returns>
        [HttpGet("{id}"), ActionName("choduyet")]
        public async Task<IActionResult> GetChoDuyetAsync(int id)
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy danh sách ký số chờ duyệt thành công",
                data = await _dexuat.GetKySoChoDuyetAsync(id)
            });
        }
        /// <summary>
        /// lấy danh sách ký số đã duyệt của người dùng đang đăng nhập
        /// </summary>
        /// <param name="id">mã người dùng</param>
        /// <returns></returns>
        [HttpGet("{id}"), ActionName("daduyet")]
        public async Task<IActionResult> GetDaDuyetAsync(int id)
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy danh sách ký số đã duyệt thành công",
                data = await _dexuat.GetKySoDaDuyetAsync(id)
            });
        }
        /// <summary>
        /// lấy danh sách ký số đề xuất từ chối của người dùng đang đăng nhập
        /// </summary>
        /// <param name="id">mã người dùng</param>
        /// <returns></returns>
        [HttpGet("{id}"), ActionName("tuchoi")]
        public async Task<IActionResult> GetTuChoiAsync(int id)
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy danh sách ký số từ chối thành công",
                data = await _dexuat.GetKySoTuChoiAsync(id)
            });
        }
    }
}
