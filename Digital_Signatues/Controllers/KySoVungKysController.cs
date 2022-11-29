using Digital_Signatues.Services;
using Digital_Signatues.Models;
using Digital_Signatues.Models.ViewModel;
using Digital_Signatues.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Digital_Signatues.Models.ViewPost;
using Digital_Signatues.Models.ViewPut;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Digital_Signatues.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class KySoVungKysController : Controller
    {
        private readonly IVungKy _vungky;
        private readonly ILog _log;
        public KySoVungKysController(IVungKy vungky,ILog log)
        {
            _log=log;
            _vungky = vungky;
        }
        /// <summary>
        /// lấy vùng ký của buocs duyệt
        /// </summary>
        /// <param name="id">mã bước duyệt</param>
        /// <returns></returns>
        [HttpGet("{id}"),ActionName("vungky")]
        public async Task<IActionResult> GetBuocDuyetAsync(int id)
        {
            if(await _vungky.GetVungKyAsync(id)!=null)
            {
                return Ok(new
                {
                    retCode = 1,
                    retText = "Lấy vùng ký thành công",
                    data = await _vungky.GetVungKyAsync(id)
                });
            }
            return Ok(new
            {
                retCode = 0,
                retText = "bước duyệt không có vùng ký",
                data = ""
            });
        }
        /// <summary>
        /// kiểm tra bước duyệt có vùng ký hay không
        /// </summary>
        /// <param name="id">mã bước duyệt</param>
        /// <returns></returns>
        [HttpGet("{id}"),ActionName("check")]
        public async Task<IActionResult> CheckVungKy(int id)
        {
            if(await _vungky.isVungKyAsync(id))
            {
                return Ok(new
                {
                    retCode = 1,
                    retText = "Check vùng ký thành công",
                    data = true
                });
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Check vùng ký thành công",
                data = false
            });
        }
        /// <summary>
        /// chuẩn bị vùng ký
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost,ActionName("vungky")]
        [Authorize(Policy = "dexuat")]
        public async Task<IActionResult> PostVungKyAsync([FromBody] PostVungKy post)
        {
            if(ModelState.IsValid)
            {
                if(await _vungky.PostVungKyAsync(post))
                {
                    /*var ma_user = User.FindFirstValue("Id");
                    var postlog = new PostLog()
                    {
                        Ten_Log = "Gắn mã QR thành công",
                        Ma_NguoiThucHien = int.Parse(ma_user),
                        Ma_TaiKhoan = null,
                        Ma_DeXuat = qrcode.Ma_DeXuat
                    };
                    if (await _log.PostLogAsync(postlog) > 0) { }*/
                        return Ok(new
                    {
                        retCode = 1,
                        retText = "Thêm vùng ký thành công",
                        data = true
                    });
                }
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Thêm vùng ký thất bại",
                data = false
            });
        }
        /// <summary>
        /// lấy danh sách vùng ký bằng mã đề xuất
        /// </summary>
        /// <param name="id">mã đề xuất</param>
        /// <returns></returns>
        [HttpGet("{id}"),ActionName("vungkydexuat")]
        public async Task<IActionResult> GetVungKyByDeXuatAsync(int id)
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy vùng ký thành công",
                data = await _vungky.isVungKyByDeXuatAsync(id)
            });
        }
    }
}
