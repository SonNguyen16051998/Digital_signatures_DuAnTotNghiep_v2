using Digital_Signatues.Helpers;
using Digital_Signatues.Models;
using Digital_Signatues.Models.ViewModel;
using Digital_Signatues.Models.ViewPost;
using Digital_Signatues.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.util;

namespace Digital_Signatues.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class KySosController : Controller
    {
        private readonly IKySo _kyso;
        private readonly IKySoThongSo _thongso;
        private readonly IHostingEnvironment _environment;
        private readonly ILog _log;
        private readonly IKySoBuocDuyet _buocduyet;
        public KySosController(
            IKySo kyso, IHostingEnvironment environment, IKySoThongSo thongso,ILog log, IKySoBuocDuyet buocduyet)
        {
            _buocduyet=buocduyet;
            _log = log;
            _kyso = kyso;
            _environment = environment;
            _thongso = thongso;
        }
        /// <summary>
        /// ký thử
        /// </summary>
        /// <param name="signs"></param>
        /// <returns></returns>
        [HttpPost,ActionName("signtest")]
        public async Task<IActionResult> SignTest([FromBody] PostSign signs)
        {
            var id = User.FindFirstValue("Id");
            if (ModelState.IsValid)
            {
                string fileName = "";
                var thongso = await _thongso.GetThongSoNguoiDungAsync(signs.Id_NguoiDung);
                if(string.IsNullOrEmpty(thongso.FilePfx) && thongso.LoaiChuKy==true)
                { 
                    var postlog = new PostLog()
                    {
                        Ten_Log = "Kí thử thất bại",
                        Ma_NguoiThucHien = int.Parse(id),
                        Ma_TaiKhoan=signs.Id_NguoiDung,
                        Ma_DeXuat=null,
                    };
                    if (await _log.PostLogAsync(postlog) > 0)
                    { }
                    return Ok(new
                    {
                        retCode = 0,
                        retText = "Ký thử thất bại. Người dùng chưa cấu hình file chữ kí",
                        data = ""
                    });
                }
                else
                {
                    X509Certificate cert = new X509Certificate(thongso.FilePfx, thongso.PasscodeFilePfx);

                    if (await _thongso.CheckSubjectFileAsync(signs.Id_NguoiDung) != cert.Subject)
                    {
                        PostThongSoFilePfx thongsofilepfx = new PostThongSoFilePfx();
                        thongsofilepfx.Ma_NguoiDung = signs.Id_NguoiDung;
                        thongsofilepfx.Subject = cert.Subject;
                        thongsofilepfx.Serial = cert.GetSerialNumberString();
                        await _thongso.CapNhatThongSoFileAsync(thongsofilepfx);
                    }
                    foreach (var item in signs.PostPositionSigns)
                    {
                        string outputFile = "";
                        string inputNewFile = "";
                        string fieldName = "";
                        string name = Path.GetFileNameWithoutExtension(signs.inputFile);
                        string fontPath = Path.Combine(_environment.WebRootPath, "Font", "ARIALUNI.TTF");
                        Certicate myCert = new Certicate(thongso.FilePfx, thongso.PasscodeFilePfx);

                        PDFSigner pdfs = new PDFSigner();
                        for (int i = 0; i < 1000; i++)
                        {
                            fileName = name + "_" + i + "_daky.pdf";
                            fieldName = "filedName_" + i;
                            outputFile = Path.Combine(_environment.WebRootPath, "Filedaky") + @"\" + name + "_" + i + "_daky.pdf";
                            if (!System.IO.File.Exists(outputFile))
                            {
                                inputNewFile = Path.Combine(_environment.WebRootPath, "Filedaky") + @"\" + name + "_" + (i - 1) + "_daky.pdf";
                                break;
                            }
                        }
                        if (System.IO.File.Exists(inputNewFile))
                        {
                            pdfs = new PDFSigner(inputNewFile, outputFile, myCert, fontPath);
                        }
                        else
                        {
                            pdfs = new PDFSigner(signs.inputFile, outputFile, myCert, fontPath);
                        }
                        if (!string.IsNullOrEmpty(item.textSign))
                        {
                            var rectangle = new iTextSharp.text.Rectangle((int)item.x, (int)item.y);
                            pdfs.SignText(thongso.LyDoMacDinh, "", "", item.textSign, rectangle, item.pageSign, fieldName);
                        }
                        else
                        {
                            var recJ = new RectangleJ((int)item.x, (int)item.y, (int)item.img_w, (int)item.img_h);

                            string inputImg = item.imgSign;
                            var rectangle = new iTextSharp.text.Rectangle(recJ);
                            pdfs.SignImage(thongso.LyDoMacDinh, "", "", inputImg, rectangle, item.pageSign, fieldName, false);
                        }
                    }
                    var postlog = new PostLog()
                    {
                        Ten_Log = "Kí thử thành công",
                        Ma_NguoiThucHien = int.Parse(id),
                        Ma_TaiKhoan=signs.Id_NguoiDung,
                        Ma_DeXuat=null
                    };
                    if (await _log.PostLogAsync(postlog) > 0)
                    { }
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Ký thử thành công",
                        data = Path.Combine("Filedaky", fileName)
                    });
                }
            }    
            else
            {
                var postlog = new PostLog()
                {
                    Ten_Log = "Kí thử thất bại",
                    Ma_NguoiThucHien = int.Parse(id),
                    Ma_TaiKhoan = signs.Id_NguoiDung,
                    Ma_DeXuat = null,
                };
                if (await _log.PostLogAsync(postlog) > 0)
                { }
                return Ok(new
                {
                    retCode = 0,
                    retText = "Ky thử thất bại",
                    data = ""
                });
            }    
        }
        /// <summary>
        /// ký thật
        /// </summary>
        /// <param name="signs"></param>
        /// <returns></returns>
        [HttpPost,ActionName("sign")]
        public async Task<IActionResult> SignBuocDuyet([FromBody] PostSignBuocDuyet signs)
        {
            if (ModelState.IsValid)
            {
                string fileName = "";
                string name = Path.GetFileNameWithoutExtension(signs.inputFile);
                string outputFile = "";
                string inputNewFile = "";
                string fieldName = "filedName_0";
                int buocduyethientai = await _kyso.GetIndexBuocDuyet(signs.Ma_BuocDuyet);
                if (buocduyethientai> 1)
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        fileName = name + "_" + i + "_daky.pdf";
                        fieldName = "filedName_" + i;
                        outputFile = Path.Combine(_environment.WebRootPath, "Filedaky") + @"\" + name + "_" + i + "_daky.pdf";
                        if (!System.IO.File.Exists(outputFile))
                        {
                            inputNewFile = Path.Combine(_environment.WebRootPath, "Filedaky") + @"\" + name + "_" + (i - 1) + "_daky.pdf";
                            break;
                        }
                    }
                }
                else
                {
                    fileName = name + "_" + 0 + "_daky.pdf";
                    outputFile = Path.Combine(_environment.WebRootPath, "Filedaky") + @"\" + name + "_" + 0 + "_daky.pdf";
                }
                foreach (var item in signs.PostPositionSigns)
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        fileName = name + "_" + i + "_daky.pdf";
                        fieldName = "filedName_" + i;
                        outputFile = Path.Combine(_environment.WebRootPath, "Filedaky") + @"\" + name + "_" + i + "_daky.pdf";
                        if (!System.IO.File.Exists(outputFile))
                        {
                            inputNewFile = Path.Combine(_environment.WebRootPath, "Filedaky") + @"\" + name + "_" + (i - 1) + "_daky.pdf";
                            break;
                        }
                    }
                    string fontPath = Path.Combine(_environment.WebRootPath, "Font", "ARIALUNI.TTF");
                    var thongso = await _thongso.GetThongSoNguoiDungAsync(signs.Id_NguoiDung);
                    Certicate myCert = new Certicate(thongso.FilePfx, thongso.PasscodeFilePfx);
                    PDFSigner pdfs = new PDFSigner();

                    if (System.IO.File.Exists(inputNewFile))
                    {
                        pdfs = new PDFSigner(inputNewFile, outputFile, myCert, fontPath);
                    }
                    else
                    {
                        pdfs = new PDFSigner(signs.inputFile, outputFile, myCert, fontPath);
                    }
                    if (!string.IsNullOrEmpty(item.textSign))
                    {
                        var rectangle = new iTextSharp.text.Rectangle((int)item.x, (int)item.y);
                        pdfs.SignText(thongso.LyDoMacDinh, "", "", item.textSign, rectangle, item.pageSign, fieldName);
                    }
                    else
                    {
                        var recJ = new RectangleJ((int)item.x, (int)item.y, (int)item.img_w, (int)item.img_h);

                        string inputImg = item.imgSign;
                        var rectangle = new iTextSharp.text.Rectangle(recJ);
                        pdfs.SignImage(thongso.LyDoMacDinh, "", "", inputImg, rectangle, item.pageSign, fieldName, false);
                    }
                }
                string fileReturn = Path.Combine("Filedaky", fileName);
                string filedaky = Path.Combine("wwwroot\\Filedaky", fileName);
                if (await _kyso.Sign(signs.Ma_BuocDuyet, filedaky))
                {
                    var id = User.FindFirstValue("Id");
                    var buoc = await _buocduyet.GetBuocDuyetAsync(signs.Ma_BuocDuyet);
                    var postlog = new PostLog()
                    {
                        Ten_Log = "Đã ký thành công",
                        Ma_NguoiThucHien = int.Parse(id),
                        Ma_TaiKhoan=null,
                        Ma_DeXuat=buoc.Ma_KySoDeXuat
                    };
                    if (await _log.PostLogAsync(postlog) > 0)
                    { }
                    return Ok(new
                    {
                        retCode = 1,
                        retText = "Ký thành công",
                        data = fileReturn
                    });
                }
                else
                {
                    System.IO.File.Delete(filedaky);
                    return Ok(new
                    {
                        retCode = 0,
                        retText = "Ký thất bại",
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
        /// lấy bước duyệt hiện tại thuộc người dùng nào
        /// </summary>
        /// <returns></returns>
        [HttpGet,ActionName("buocduyet")]
        public async Task<IActionResult> GetBuocDuyetHienTai()
        {
            return Ok(new
            {
                retCode = 1,
                retText = "Lấy bước duyệt hiện tại thành công",
                data = await _kyso.GetBuocDuyetHienTai()
            });
        }
        /// <summary>
        /// kiểm tra passcode trước khi kí
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        [HttpPut,ActionName("checkpasscode")]
        public async Task<IActionResult> CheckPasscodeAsync([FromBody]CheckPasscode check)
        {
            if(await _kyso.CheckPassCode(check))
            {
                return Ok(new
                {
                    retCode = 1,
                    retText = "Passcode chính xác",
                    data = ""
                });
            }
            return Ok(new
            {
                retCode = 0,
                retText = "passcode không chính xác",
                data = ""
            });
        }
    }
}