using Digital_Signatues.Models.ViewModel;
using Digital_Signatues.Services;
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
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private IConfiguration _config;
        private readonly INguoiDung _nguoiDung;
        public TokenController(IConfiguration config,INguoiDung nguoiDung)
        {
            _config=config;
            _nguoiDung=nguoiDung;
        }
        /// <summary>
        /// đăng nhập
        /// </summary>
        /// <param name="login">trả về object login gồm email và mật khẩu</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CusLogin([FromBody] ViewLogin login)
        {
            if (ModelState.IsValid)
            {
                var user =await _nguoiDung.LoginAsync(login);
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
                        _config["Jwt:Audience"], Claims, expires: DateTime.UtcNow.AddDays(1),
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
    }
}
