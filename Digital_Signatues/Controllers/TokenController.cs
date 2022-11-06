﻿using Digital_Signatues.Models.ViewModel;
using Digital_Signatues.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Digital_Signatues.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [AllowAnonymous]
    public class TokenController : Controller
    {
        private IConfiguration _config;
        private readonly INguoiDung _nguoiDung;
        private readonly ILog _log;
        public TokenController(IConfiguration config, INguoiDung nguoiDung, ILog log)
        {
            _log = log;
            _config = config;
            _nguoiDung = nguoiDung;
        }
        /// <summary>
        /// đăng nhập
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost, ActionName("login")]
        public async Task<IActionResult> CusLogin([FromBody] ViewLogin login)
        {
            if (ModelState.IsValid)
            {
                var user = await _nguoiDung.LoginAsync(login);
                if (user != null)
                {
                    var Claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub,_config["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                        new Claim("Id",user.Ma_NguoiDung.ToString()),
                        new Claim("Name",user.HoTen),
                        new Claim("Email",user.Email),
                        new Claim("Address",user.DiaChi),
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                        _config["Jwt:Audience"], Claims, expires: DateTime.UtcNow.AddYears(1),
                        signingCredentials: signIn);
                    ViewToken viewToken = new ViewToken() { Token = new JwtSecurityTokenHandler().WriteToken(token), NguoiDung = user };
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Đăng nhập thành công",
                        data = viewToken
                    });
                }
                else
                {
                    return Ok(new
                    {
                        retCode = 0,
                        retText = "Tài khoản hoặc mật khẩu không chính xác",
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
        /*/// <summary>
        /// lấy toàn bộ lịch sử của hệ thống
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllLogAsync()
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy lịch sử hệ thống thành công",
                data = await _log.GetAllLogAsync()
            });
        }*/
        /// <summary>
        /// lấy toàn bộ lịch sử thông số của tài khoản hiện tại
        /// </summary>
        /// <param name="id">mã tài khoản</param>
        /// <returns></returns>
        [HttpGet("{id}"),ActionName("logthongso")]
        public async Task<IActionResult> GetAllLogThongSoAsync(int id)
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy lịch sử thông số người dùng thành công",
                data = await _log.GetAllLogThongSoAsync(id)
            });
        }
        /// <summary>
        /// lấy toàn bộ lịch sử của đề xuất
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}"),ActionName("logdexuat")]
        public async Task<IActionResult> GetAllLogDeXuatAsync(int id)
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy lịch sử đề xuất thành công",
                data = await _log.GetAllLogDeXuatAsync(id)
            });
        }
    }
}
