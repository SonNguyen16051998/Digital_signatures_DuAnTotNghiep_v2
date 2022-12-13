using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models.ViewPost
{
    public class PostKySoThongSo
    {
        [Required]
        public int Ma_NguoiDung { get; set; }
        [Required]
        public string Hinh1 { get; set; }
        public string Hinh2 { get; set; }
        public string Hinh3 { get; set; }
        public string LyDoMacDinh { get; set; }
        [Required]
        public string PassCode { get; set; }
        [Required]
        [Compare("PassCode", ErrorMessage = "Mật khẩu không trùng khớp!!!")]
        public string RetypePasscode { get; set; }
        public int Ma_NguoiCapNhatCuoi { get; set; }
        public bool TrangThai { get; set; }
        [Column(TypeName = "date")]
        public DateTime NgayChuKyHetHan { get; set; }
    }
}
