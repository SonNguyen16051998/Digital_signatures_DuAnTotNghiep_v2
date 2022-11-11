﻿using Digital_Signatues.Models;
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
        /// <summary>
        /// lấy toàn bộ mã QR
        /// </summary>
        /// <returns></returns>
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
        /// <param name="id">mã số QR code</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQRCodeAsync(string id)
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy chi tiết QRCode thành công",
                data = await _QR.GetMaQRbyMaSoAsync(id)
            });
        }
        /// <summary>
        /// gắn mã qr vào file pdf
        /// </summary>
        /// <param name="qrcode"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddQRCode([FromBody] PostQR qrcode)
        {

            if (ModelState.IsValid)
            {
                if (await _QR.AddQRCodeToPdf(qrcode))
                {
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Gắn mã QR thành công",
                        data = await _QR.GetMaQRbyDeXuatAsync(qrcode.Ma_DeXuat)
                    });
                }
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Gắn mã QR thất bại",
                data = ""
            });
        }
        /// <summary>
        /// cập nhật mức độ của mã qr
        /// </summary>
        /// <param name="qrcode"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> PutQRCode([FromBody]PutQR qrcode)
        {
            if(ModelState.IsValid)
            {
                var maso = await _QR.UpdateMaQRAsync(qrcode);
                if (maso.Length>0)
                {
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Cập nhật QR thành công",
                        data = await _QR.GetMaQRbyMaSoAsync(qrcode.MaSo)
                    });
                }
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Cập nhật QR thất bại",
                data = ""
            });
        }
    }
}
