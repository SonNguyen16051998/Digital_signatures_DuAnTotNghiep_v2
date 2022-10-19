using Digital_Signatues.Models;
using Digital_Signatues.Models.ViewPost;
using Digital_Signatues.Models.ViewPut;
using Digital_Signatues.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Claims;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Digital_Signatues.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class ChucDanhsController : Controller
    {
        private readonly IChucDanh _chucDanh;
        private readonly ILog _log;
        public ChucDanhsController(IChucDanh chucDanh,ILog log)
        {
            _log=log;
            _chucDanh = chucDanh;
        }
        /// <summary>
        /// hiển thị toàn bộ chức danh bao gồm người dùng thuộc chức danh tương ứng
        /// </summary>
        /// <returns></returns>
        [HttpGet,ActionName("chucdanh")]
        public async Task<IActionResult> GetChucDanhsAsync()
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy danh sách chức danh thành công",
                data = await _chucDanh.GetChucDanhsAsync()
            }); 
        }
        /// <summary>
        /// trả về chức danh vừa chọn bao gồm người dùng thuộc chức danh đó
        /// </summary>
        /// <param name="id">mã chức danh</param>
        /// <returns></returns>
        [HttpGet("{id}"), ActionName("chucdanh")]
        public async Task<IActionResult> GetChucDanhAsync(int id)
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy chức danh thành công",
                data = await _chucDanh.GetChucDanhAsync(id)
            });
        }
        /// <summary>
        /// thêm chức danh
        /// </summary>
        /// <param name="chucDanh">trong object chức danh chỉ cần truyền về tên chức danh còn lại để null</param>
        /// <returns></returns>
        [HttpPost, ActionName("chucdanh")]
        public async Task<IActionResult> PostChucDanhAsync([FromBody] PostChucDanh chucDanh)
        {// thêm chức danh chỉ cần gửi tên chức danh
            if (ModelState.IsValid)
            {
                
                ChucDanh addChucDanh = new ChucDanh()
                {
                    IsDeleted = false,
                    Order = 0,
                    Ten_ChucDanh = chucDanh.Ten_ChucDanh
                };
                int id_ChucDanh = await _chucDanh.AddChucDanhAsync(addChucDanh);
                if (id_ChucDanh> 0)
                {
                    var id = User.FindFirstValue("Id");
                    var postlog = new PostLog()
                    {
                        Ten_Log = "Thêm chức danh " + addChucDanh.Ten_ChucDanh + " thành công",
                        Ma_NguoiThucHien = int.Parse(id)
                    };
                    if(await _log.PostLogAsync(postlog)>0)
                    {}
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Thêm chức danh thành công",
                        data = await _chucDanh.GetChucDanhAsync(id_ChucDanh)
                    });
                }
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Thêm chức danh thất bại",
                data = ""
            });
        }
        /// <summary>
        /// cập nhật chức danh
        /// </summary>
        /// <param name="putChucDanh">truyền đầy đủ dữ liệu không được bỏ trống</param>
        /// <returns></returns>
        [HttpPut, ActionName("chucdanh")]
        public async Task<IActionResult> PutChucDanhAsync(PutChucDanh putChucDanh)
        {//cập nhật chức danh truyền đầy đủ dữ liệu
            if (ModelState.IsValid)
            {
                var chucdanhcu=await _chucDanh.GetChucDanhAsync(putChucDanh.Ma_ChucDanh);
                string tencu = chucdanhcu.Ten_ChucDanh;
                int id_chucDanh = await _chucDanh.UpdateChucDanhAsync(putChucDanh);
                if ( id_chucDanh> 0)
                {
                    var id = User.FindFirstValue("Id");
                    var postlog = new PostLog()
                    {
                        Ten_Log = "Cập nhật chức danh " + tencu + " thành " + putChucDanh.Ten_ChucDanh + " thành công",
                        Ma_NguoiThucHien = int.Parse(id)
                    };
                    if (await _log.PostLogAsync(postlog) > 0)
                    { }
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Cập nhật chức danh thành công",
                        data = await _chucDanh.GetChucDanhAsync(id_chucDanh)
                    });
                }
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Cập nhật chức danh thất bại",
                data = ""
            });
        }
        /// <summary>
        /// xóa chức danh: chuyển trường isdeleted về true. chỉ có thể xóa khi không có người dùng
        /// </summary>
        /// <param name="id">mã chức danh</param>
        /// <returns></returns>
        [HttpDelete("{id}"), ActionName("chucdanh")]
        public async Task<IActionResult> DeleteChucDanhAsync(int id)
        {
            var chucdanhcu = await _chucDanh.GetChucDanhAsync(id);
            string tencu = chucdanhcu.Ten_ChucDanh;
            if (await _chucDanh.DeleteChucDanhAsync(id))
            {
                var manguoidung = User.FindFirstValue("Id");
                var postlog = new PostLog()
                {
                    Ten_Log = "Xóa chức danh " + tencu + " thành công",
                    Ma_NguoiThucHien = int.Parse(manguoidung)
                };
                if (await _log.PostLogAsync(postlog) > 0)
                { }
                return Ok(new
                {
                    retCode = 1,
                    retText = "Xóa chức danh thành công",
                    data = await _chucDanh.GetChucDanhAsync(id)
                });
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Xóa chức danh thất bại. Chức danh đang có người sử dụng không được xóa",
                data = ""
            });
        }
        /// <summary>
        /// sắp xếp thứ tự chức danh
        /// </summary>
        /// <param name="chucDanhs"></param>
        /// <returns></returns>
        [HttpPut, ActionName("sapxep")]
        public async Task<IActionResult> SapXepThuTuAsync(List<PutSapXep> chucDanhs)
        {
            if(ModelState.IsValid)
            {
                if(await _chucDanh.SapXepThuTuAsync(chucDanhs))
                {
                    var id = User.FindFirstValue("Id");
                    var postlog = new PostLog()
                    {
                        Ten_Log = "Sắp xếp thứ tự chức danh thành công",
                        Ma_NguoiThucHien = int.Parse(id)
                    };
                    if (await _log.PostLogAsync(postlog) > 0)
                    { }
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Sắp xếp chức danh thành công",
                        data = await _chucDanh.GetChucDanhsAsync()
                    });
                }
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Sắp xếp chức danh thất bại",
                data = ""
            });
        }
    }
}
