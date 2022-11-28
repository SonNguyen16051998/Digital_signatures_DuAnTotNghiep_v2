using Digital_Signatues.Models.ViewModel;
using Digital_Signatues.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
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
        private readonly INguoiDung_Role _nguoidung_role;
        public TokenController(IConfiguration config, INguoiDung nguoiDung, ILog log,INguoiDung_Role nguoiDung_Role)
        {
            _nguoidung_role = nguoiDung_Role;
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
                    var nguoidung_quyen = await _nguoidung_role.GetNguoiDung_QuyensAsync(user.Ma_NguoiDung);
                    var Claims = new List<Claim>();
                    Claims.Add(new Claim(JwtRegisteredClaimNames.Sub, _config["Jwt:Subject"]));
                    Claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                    Claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()));
                    Claims.Add(new Claim("Id", user.Ma_NguoiDung.ToString()));
                    Claims.Add(new Claim("Name", user.HoTen));
                    Claims.Add(new Claim("Email", user.Email));
                    Claims.Add(new Claim("Address", user.DiaChi));
                    foreach(var item in (await ClaimRole(user.Ma_NguoiDung)))
                    {
                        Claims.Add(item);
                    }
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                        _config["Jwt:Audience"], Claims.ToArray(), expires: DateTime.UtcNow.AddYears(1),
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
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> GetAllLogDeXuatAsync(int id)
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy lịch sử đề xuất thành công",
                data = await _log.GetAllLogDeXuatAsync(id)
            });
        }

        private async Task<List<Claim>> ClaimRole(int ma_nguoidung)
        {
            List<Claim> claim=new List<Claim>();
            var ng_quyen=await _nguoidung_role.GetNguoiDung_QuyensAsync(ma_nguoidung);
            foreach(var item in ng_quyen)
            {
                if(item.Ma_Quyen==1)
                {
                    claim.Add(new Claim(ClaimTypes.Role, "quantrihethong"));
                }
                else if(item.Ma_Quyen==2)
                {
                    claim.Add(new Claim(ClaimTypes.Role, "quantrikiso"));
                }
                else if (item.Ma_Quyen == 3)
                {
                    claim.Add(new Claim(ClaimTypes.Role, "dexuatkiso"));
                }
                else if (item.Ma_Quyen == 4)
                {
                    claim.Add(new Claim(ClaimTypes.Role, "duyetkiso"));
                }
                else if (item.Ma_Quyen == 5)
                {
                    claim.Add(new Claim(ClaimTypes.Role, "xemdanhsachkiso"));
                }
                else if (item.Ma_Quyen == 6)
                {
                    claim.Add(new Claim(ClaimTypes.Role, "quantrivanban"));
                }
                else if (item.Ma_Quyen == 8)
                {
                    claim.Add(new Claim(ClaimTypes.Role, "quantriqr"));
                }
            }
            return claim;
        }
    }
}
