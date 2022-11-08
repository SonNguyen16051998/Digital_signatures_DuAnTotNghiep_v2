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
    [Route("api/[controller]")]
    [Authorize]
    public class MaQRsController : Controller
    {
        private readonly ILog _log;
        private readonly IQR _QR;
        public MaQRsController(ILog log,IQR QR)
        {
            _log = log;
            _QR = QR;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllQRCode()
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy danh sách QRCode thành công",
                data = await _QR.GetMaQRsAsync()
            });
        }
        /// <summary>
        /// lấy chi tiết mã qr
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQRCodeAsync(string id)
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy chi tiết QRCode thành công",
                data = await _QR.GetMaQRAsync(id)
            });
        }
        /*[HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] PostQR postQR)
        {
            if(ModelState.IsValid)
            {
                string maso = await _QR.AddMaQRAsync(postQR);
                if (maso.Length>0)
                {
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Thêm QRCode thành công",
                        data = await _QR.GetMaQRAsync(id)
                    });
                }
            }
        }*/
    }
}
