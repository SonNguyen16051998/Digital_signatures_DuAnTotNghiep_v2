using Digital_Signatues.Services;
using Digital_Signatues.Models;
using Digital_Signatues.Models.ViewModel;
using Digital_Signatues.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Digital_Signatues.Models.ViewPost;

namespace Digital_Signatues.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MessagesController : Controller
    {
        private readonly IMessage _message;
        public MessagesController(IMessage message)
        {
            _message = message;
        }
        /// <summary>
        /// lấy toàn bộ tin nhắn trao đổi của 1 ký số đề xuất
        /// </summary>
        /// <param name="id">mã ký số đề xuất</param>
        /// <returns></returns>
        [HttpGet("{id}"),ActionName("message")]
        public async Task<IActionResult> GetMessagesAsync(int id)
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy danh sách trao đổi thành công",
                data =    await _message.GetMessagesAsync(id)
            });
        }
        /// <summary>
        /// chi tiết của một trao đổi
        /// </summary>
        /// <param name="id">mã message</param>
        /// <returns></returns>
        [HttpGet("{id}"),ActionName("details")]
        public async Task<IActionResult> GetMessageAsync(int id)
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy chi tiết trao đổi thành công",
                data = await _message.GetMessageAsync(id)
            });
        }
        /// <summary>
        /// thêm một tin trao đổi trong ký số đề xuất
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost,ActionName("message")]
        public async Task<IActionResult> PostMessageAsync([FromBody] PostMessage message)
        {
            if(ModelState.IsValid)
            {
                int id = await _message.PostMessageAsync(message);
                if (id>0)
                {
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Thêm message thành công",
                        data = await _message.GetMessagesAsync(id)
                    });
                }
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Thêm message thất bại",
                data =""
            });
        }
        /// <summary>
        /// xóa message
        /// </summary>
        /// <param name="id">mã message</param>
        /// <returns></returns>
        [HttpDelete("{id}"),ActionName("message")]
        public async Task<IActionResult> DeleteMessageAsync(int id)
        {
            if(await _message.DeleteMessageAsync(id))
            {
                return Ok(new
                {
                    retCode = 1,
                    retText = "Xóa message thành công",
                    data = ""
                });
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Xóa message thất bại",
                data = await _message.GetMessagesAsync(id)
            });
        }
    }
}
