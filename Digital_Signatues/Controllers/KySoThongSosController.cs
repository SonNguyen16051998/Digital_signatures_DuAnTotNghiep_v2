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
    public class KySoThongSosController : Controller
    {
        private readonly ILog _log;
        private readonly IKySoThongSo _thongso;
        public KySoThongSosController(IKySoThongSo thongso,ILog log)
        {
            _log = log;
            _thongso = thongso;
        }
        /// <summary>
        /// lấy toàn bộ danh sách thông số ký số
        /// </summary>
        /// <returns></returns>
        [HttpGet,ActionName("thongso")]
        public async Task<IActionResult> GetThongSosAsync()
        {
            return Ok(new
            {
                retCode=1,
                retText="Lấy danh sách thông số thành công",
                data=await _thongso.GetThongSoNguoiDungsAsync()
            });
        }
        /// <summary>
        /// lấy thông số chi tiết của người dùng
        /// </summary>
        /// <param name="id">mã người dùng</param>
        /// <returns></returns>
        [HttpGet("{id}"), ActionName("thongso")]
        public async Task<IActionResult> GetThongSoAsync(int id)
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy thông số người dùng thành công",
                data = await _thongso.GetThongSoNguoiDungAsync(id)
            });
        }
        /// <summary>
        /// thêm thông số cho người dùng. chỉ thêm thành công khi người dùng chưa có thông số
        /// </summary>
        /// <param name="thongSo"></param>
        /// <returns></returns>
        [HttpPost, ActionName("thongso")]
        [Authorize(Policy = "kiso")]
        public async Task<IActionResult> PostThongSoAsync([FromBody]PostKySoThongSo thongSo)
        {
            if(ModelState.IsValid)
            {
                if(await _thongso.CheckExistAsync(thongSo.Ma_NguoiDung))
                {
                    if (await _thongso.AddThongSoNguoiDungAsync(thongSo) > 0)
                    {
                        var data = await _thongso.GetThongSoNguoiDungAsync(thongSo.Ma_NguoiDung);
                        var id = User.FindFirstValue("Id");
                        var postlog = new PostLog()
                        {
                            Ten_Log = "Tạo thông số thành công",
                            Ma_NguoiThucHien = int.Parse(id),
                            Ma_TaiKhoan=thongSo.Ma_NguoiDung,
                            Ma_DeXuat=null
                        };
                        if (await _log.PostLogAsync(postlog) > 0)
                        { }
                        return Ok(new
                        {
                            retCode = 1,
                            retText = "Thêm thông số người dùng thành công",
                            data = data
                        });
                    }
                }
                return Ok(new
                {
                    retCode = 0,
                    retText = "Người dùng đã có thông số. thêm thất bại",
                    data = await _thongso.GetThongSoNguoiDungAsync(thongSo.Ma_NguoiDung)
                });
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Thêm thông số thất bại",
                data =""
            });
        }
        /// <summary>
        /// cập nhật thông số cho người dùng
        /// </summary>
        /// <param name="putThongSo"></param>
        /// <returns></returns>
        [HttpPut, ActionName("thongso")]
        public async Task<IActionResult> PutThongSoAsync([FromBody] PutThongSo putThongSo)
        {
            var id = User.FindFirstValue("Id");
            if (ModelState.IsValid)
            {
                if(await _thongso.UpdateThongSoAsync(putThongSo) > 0)
                {
                    var postlog = new PostLog()
                    {
                        Ten_Log = "Cập nhật thông số thành công",
                        Ma_NguoiThucHien = int.Parse(id),
                        Ma_TaiKhoan=putThongSo.Ma_NguoiDung,
                        Ma_DeXuat=null
                    };
                    if (await _log.PostLogAsync(postlog) > 0)
                    { }
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Cập nhật thông số người dùng thành công",
                        data = await _thongso.GetThongSoNguoiDungAsync(putThongSo.Ma_NguoiDung)
                    });
                }
                else
                {
                    var postlog = new PostLog()
                    {
                        Ten_Log = "Cập nhật thông số thất bại",
                        Ma_NguoiThucHien = int.Parse(id),
                        Ma_TaiKhoan = putThongSo.Ma_NguoiDung,
                        Ma_DeXuat = null
                    };
                    if (await _log.PostLogAsync(postlog) > 0)
                    { }
                }    
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Cập nhật thông số thất bại",
                data = ""
            });
        }
        /// <summary>
        /// cập nhật passcode cho người dùng
        /// </summary>
        /// <param name="putPasscode"></param>
        /// <returns></returns>
        [HttpPut, ActionName("passcode")]
        public async Task<IActionResult> ChangePasscode([FromBody] PutPasscode putPasscode)
        {
            var id = User.FindFirstValue("Id");
            if (ModelState.IsValid)
            {
                if(await _thongso.CheckPasscode(putPasscode.Ma_NguoiDung,putPasscode.PassCode))
                {
                    if (await _thongso.ChangePasscode(putPasscode))
                    {
                        var postlog = new PostLog()
                        {
                            Ten_Log = "Cập nhật passcode thành công",
                            Ma_NguoiThucHien = int.Parse(id),
                            Ma_TaiKhoan=putPasscode.Ma_NguoiDung,
                            Ma_DeXuat=null
                        };
                        if (await _log.PostLogAsync(postlog) > 0)
                        { }
                        return Ok(new
                        {
                            retCode = 1,
                            retText = "Cập nhật passcode thành công",
                            data = await _thongso.GetThongSoNguoiDungAsync(putPasscode.Ma_NguoiDung)
                        });
                    }
                    else
                    {
                        var postlog = new PostLog()
                        {
                            Ten_Log = "Cập nhật passcode thất bại",
                            Ma_NguoiThucHien = int.Parse(id),
                            Ma_TaiKhoan = putPasscode.Ma_NguoiDung,
                            Ma_DeXuat = null
                        };
                        if (await _log.PostLogAsync(postlog) > 0)
                        { }
                        return Ok(new
                        {
                            retCode = 0,
                            retText = "Cập nhật passcode thất bại",
                            data = ""
                        });
                    }
                }
                else
                {
                    var postlog = new PostLog()
                    {
                        Ten_Log = "Nhập passcode cũ không chính xác khi đổi passcode",
                        Ma_NguoiThucHien = int.Parse(id),
                        Ma_TaiKhoan = putPasscode.Ma_NguoiDung,
                        Ma_DeXuat = null
                    };
                    if (await _log.PostLogAsync(postlog) > 0)
                    { }
                    return Ok(new
                    {
                        retCode = 0,
                        retText = "Passcode cũ không chính xác",
                        data = ""
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
        /// cấu hình file chữ kí pfx cho người dùng
        /// </summary>
        /// <param name="fileChuKy"></param>
        /// <returns></returns>
        [HttpPut,ActionName("cauhinhfile")]
        public async Task<IActionResult> CauHinhFileChuKyAsync([FromBody] PostCauHinhFileChuKy fileChuKy)
        {
            if(ModelState.IsValid)
            {
                if(fileChuKy.LoaiChuKy==true)
                {
                    if(await _thongso.VerifyPassword(fileChuKy.FilePfx,fileChuKy.PasscodeFilePfx))
                    {
                        if (await _thongso.CauHinhChuKyAsync(fileChuKy))
                        {
                            var id = User.FindFirstValue("Id");
                            var postlog = new PostLog()
                            {
                                Ten_Log = "Cấu hình file chữ ký thành công",
                                Ma_NguoiThucHien = int.Parse(id),
                                Ma_TaiKhoan = fileChuKy.Ma_NguoiDung,
                                Ma_DeXuat = null
                            };
                            if (await _log.PostLogAsync(postlog) > 0)
                            { }
                            return Ok(new
                            {
                                retCode = 1,
                                retText = "Cấu hình file chữ kí người dùng thành công",
                                data = await _thongso.GetThongSoNguoiDungAsync(fileChuKy.Ma_NguoiDung)
                            });
                        }
                    }
                    return Ok(new
                    {
                        retCode = 0,
                        retText = "Passcode file PFX không chính xác",
                        data = ""
                    });
                }
                else
                {
                    if(SmartCaVNPT.SignSmartCa.CheckAccount(fileChuKy.Client_ID, fileChuKy.Client_Secret, fileChuKy.UID, fileChuKy.PasswordSmartSign))
                    {
                        if (await _thongso.CauHinhChuKyAsync(fileChuKy))
                        {
                            var id = User.FindFirstValue("Id");
                            var postlog = new PostLog()
                            {
                                Ten_Log = "Cấu hình file chữ ký thành công",
                                Ma_NguoiThucHien = int.Parse(id),
                                Ma_TaiKhoan = fileChuKy.Ma_NguoiDung,
                                Ma_DeXuat = null
                            };
                            if (await _log.PostLogAsync(postlog) > 0)
                            { }
                            return Ok(new
                            {
                                retCode = 1,
                                retText = "Cấu hình file chữ kí người dùng thành công",
                                data = await _thongso.GetThongSoNguoiDungAsync(fileChuKy.Ma_NguoiDung)
                            });
                        }
                    }
                    
                }
                return Ok(new
                {
                    retCode = 0,
                    retText = "Thông tin tài khoản không chính xác. Vui lòng kiểm tra lại",
                    data = ""
                });
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Cấu hình file chữ kí thất bại",
                data = ""
            });
        }
        /// <summary>
        /// xóa thông số người dùng
        /// </summary>
        /// <param name="id">mã người dùng</param>
        /// <returns></returns>
        [HttpDelete("{id}"),ActionName("thongso")]
        [Authorize(Policy = "kiso")]
        public async Task<IActionResult> DeleteThongSoAsync(int id)
        {
            var data = await _thongso.GetThongSoNguoiDungAsync(id);
            string ten = data.NguoiDung.HoTen;
            if (await _thongso.DeleteThongSoAsync(id))
            {
                var mauser = User.FindFirstValue("Id");
                var postlog = new PostLog()
                {
                    Ten_Log = "Xóa thông số thành công",
                    Ma_NguoiThucHien = int.Parse(mauser),
                    Ma_TaiKhoan=id,
                    Ma_DeXuat=null
                };
                if (await _log.PostLogAsync(postlog) > 0)
                { }
                return Ok(new
                {
                    retCode=1,
                    retText="Xóa thông số thành công",
                    data=""
                });
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Xóa thông số thất bại",
                data = ""
            });
        }
        /// <summary>
        /// lấy danh sách người dùng có quyền duyệt kí số
        /// </summary>
        /// <returns></returns>
        [HttpGet,ActionName("nguoidungduyet")]
        public async Task<IActionResult> GetNguoiDungDuyetAsync()
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy danh sách người duyệt thành công",
                data = await _thongso.GetNguoiDuyetsAsync()
            });
        }
        /// <summary>
        /// lấy toàn bộ người dùng có quyền được ký thật
        /// </summary>
        /// <returns></returns>
        [HttpGet,ActionName("nguoidungky")]
        public async Task<IActionResult> GetAllNguoiDungKyAsync()
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy danh sách người dùng được quyền ký thành công",
                data = await _thongso.GetAllNguoiDungDuocKyAsync()
            });
        }
    }
}
