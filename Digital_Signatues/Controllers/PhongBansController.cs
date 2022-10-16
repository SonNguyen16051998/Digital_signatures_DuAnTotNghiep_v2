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

namespace Digital_Signatues.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PhongBansController : Controller
    {
        private readonly IPhongBan _phongBan;
        public PhongBansController(IPhongBan phongBan)
        {
            _phongBan = phongBan;
        }
        /// <summary>
        /// lấy toàn bộ phòng ban bao gồm nhân viên thuộc vào
        /// </summary>
        /// <returns></returns>
        [HttpGet,ActionName("phongban")]
        public async Task<IActionResult> GetPhongBansAsync()
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy danh sách phòng ban thành công",
                data = await _phongBan.GetPhongBansAsync()
            });
        }
        /// <summary>
        /// hiển thị phòng ban được chọn bao gồm nhân viên thuộc vào
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}"), ActionName("phongban")]
        public async Task<IActionResult> GetPhongBanAsync(int id)
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy danh sách phòng ban thành công",
                data = await _phongBan.GetPhongBanAsync(id)
            });
        }
        /// <summary>
        /// thêm phòng ban
        /// </summary>
        /// <param name="phongBan">truyền về object phongban.trường isdeleted,order và ngaytao để null</param>
        /// <returns></returns>
        [HttpPost, ActionName("phongban")]
        public async Task<IActionResult> PostPhongBanAsync([FromBody]PostPhongBan phongBan)
        {
            if(ModelState.IsValid)
            {
                PhongBan addPhongBan = new PhongBan()
                {
                    NgayTao = DateTime.Now,
                    Order = 0,
                    IsDeleted = false,
                    Ten_PhongBan = phongBan.Ten_PhongBan
                };
                int id_Phongban = await _phongBan.AddPhongBanAsync(addPhongBan);
                if (id_Phongban>0)
                {
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Thêm phòng ban thành công",
                        data = await _phongBan.GetPhongBanAsync(id_Phongban)
                    }) ;
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
        /// cập nhật phòng ban
        /// </summary>
        /// <param name="putPhongBan">truyền đầy đủ dữ liệu của object phongban</param>
        /// <returns></returns>
        [HttpPut, ActionName("phongban")]
        public async Task<IActionResult> PutPhongbanAsync([FromBody] PutPhongBan putPhongBan)
        {
            if (ModelState.IsValid)
            {
                int id_Phongban = await _phongBan.UpdatePhongBanAsync(putPhongBan);
                if ( id_Phongban> 0)
                {
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Cập nhật phòng ban thành công",
                        data = await _phongBan.GetPhongBanAsync(id_Phongban)
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
        /// xóa phòng ban. chuyển trường isdeleted về true. chỉ xóa khi không có nhân viên
        /// </summary>
        /// <param name="id">mã phòng ban</param>
        /// <returns></returns>
        [HttpDelete("{id}"), ActionName("phongban")]
        public async Task<IActionResult> DeletePhongbanAsync(int id)//id phòng ban
        {
            if(await _phongBan.DeletePhongBanAsync(id))
            {
                return Ok(new
                {
                    retCode = 1,
                    retText = "Xóa phòng ban thành công",
                    data = await _phongBan.GetPhongBanAsync(id)
                });
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Phòng ban đang có người làm việc không thể xóa",
                data = ""
            });
        }
        /// <summary>
        /// sắp xếp phòng ban
        /// </summary>
        /// <param name="phongBans"></param>
        /// <returns></returns>
        [HttpPut, ActionName("sapxep")]
        public async Task<IActionResult> SapXepPhongBanAsync(List<PutSapXep> phongBans)
        {
            if (ModelState.IsValid)
            {
                if (await _phongBan.SapXepPhongBanAsync(phongBans))
                {
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Sắp xếp phòng ban thành công",
                        data = await _phongBan.GetPhongBansAsync()
                    });
                }
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Sắp xếp phòng ban thất bại",
                data = ""
            });
        }
    }
}
