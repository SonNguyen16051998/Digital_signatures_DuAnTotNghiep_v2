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
    [Route("api/[controller]")]
    public class QuyensController : Controller
    {
        private readonly IQuyen _quyen;
        public QuyensController(IQuyen quyen)
        {
            _quyen = quyen;
        }
        /// <summary>
        /// trả về toàn bộ quyền 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetQuyensAsync()
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy danh sách quyền thành công",
                data = await _quyen.GetQuyensAsync()
            });
        }
        /// <summary>
        /// trả về quyền được chọn
        /// </summary>
        /// <param name="id">mã quyền</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuyenAsync(int id)
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy quyền thành công",
                data = await _quyen.GetQuyenAsync(id)
            });
        }
        /// <summary>
        /// thêm quyền
        /// </summary>
        /// <param name="quyen">truyền về object quyen trả về tên quyền và isdeleted=false</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostQuyenAsync([FromBody] PostQuyen quyen)
        {
            if (ModelState.IsValid)
            {
                Quyen addQuyen = new Quyen()
                {
                    Isdeleted = false,
                    Ten_Quyen = quyen.Ten_Quyen,
                };
                int id_Quyen = await _quyen.AddQuyenAsync(addQuyen);
                if ( id_Quyen> 0)
                {
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Thêm quyền thành công",
                        data = await _quyen.GetQuyenAsync(id_Quyen)
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
        /// cập nhật quyền
        /// </summary>
        /// <param name="putQuyen">truyền đầy đủ dữ liệu tên và isdeleted=false</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> PutQuyenAsync([FromBody]PutQuyen putQuyen)
        {
            if (ModelState.IsValid)
            {
                int id_quyen = await _quyen.UpdateQuyenAsync(putQuyen);
                if ( id_quyen> 0)
                {
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Cập nhật quyền thành công",
                        data = await _quyen.GetQuyenAsync(id_quyen)
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
        /// xóa quyền. trường isdeleted trả về true và quyền sẽ được xóa khỏi role và người dùng
        /// </summary>
        /// <param name="id">mã quyền</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuyenAsync(int id)
        {
            if (await _quyen.DeleteQuyenAsync(id))
            {
                return Ok(new
                {
                    retCode = 1,
                    retText = "Xóa quyền thành công",
                    data = await _quyen.GetQuyenAsync(id)
                });
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Xóa quyền thất bại",
                data = ""
            });
        }
    }
}
