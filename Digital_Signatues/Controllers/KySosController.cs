using Digital_Signatues.Helpers;
using Digital_Signatues.Models;
using Digital_Signatues.Models.ViewPost;
using Digital_Signatues.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.util;

namespace Digital_Signatues.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KySosController : Controller
    {
        private readonly IKySo _kyso;
        private readonly IKySoThongSo _thongso;
        private readonly IHostingEnvironment _environment;
        public KySosController(IKySo kyso, IHostingEnvironment environment, IKySoThongSo thongso)
        {
            _kyso = kyso;
            _environment = environment;
            _thongso = thongso;
        }
        /// <summary>
        /// ký thử
        /// </summary>
        /// <param name="signs"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SignTest([FromBody] PostSign signs)
        {
            if (ModelState.IsValid)
            {
                string fileName = "";
                foreach (var item in signs.PostPositionSigns)
                {
                    string outputFile = "";
                    string inputNewFile = "";
                    string fieldName = "";
                    string name = Path.GetFileNameWithoutExtension(signs.inputFile);
                    string fontPath = Path.Combine(_environment.WebRootPath, "Font", "ARIALUNI.TTF");
                    var thongso = await _thongso.GetThongSoNguoiDungAsync(signs.Id_NguoiDung);
                    Certicate myCert = new Certicate(thongso.FilePfx, thongso.PasscodeFilePfx);
                    X509Certificate cert = new X509Certificate(thongso.FilePfx, thongso.PasscodeFilePfx);

                    if (await _thongso.CheckSubjectFileAsync(signs.Id_NguoiDung) != cert.Subject)
                    {
                        PostThongSoFilePfx thongsofilepfx = new PostThongSoFilePfx();
                        thongsofilepfx.Ma_NguoiDung = signs.Id_NguoiDung;
                        thongsofilepfx.Subject = cert.Subject;
                        thongsofilepfx.Serial = cert.GetSerialNumberString();
                        await _thongso.CapNhatThongSoFileAsync(thongsofilepfx);
                    }
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
                return Ok(new
                {
                    retCode = 1,
                    retText = "Ký thử thành công",
                    data = Path.Combine("Filedaky", fileName)
                });
            }
            return Ok(new
            {
                retCode = 0,
                retText = "Ky test thất bại",
                data = ""
            });
        }
    }
}