using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models.ViewPost
{
    public class PostKySoThongSo
    {
        public int Ma_NguoiDung { get; set; }
        public string Hinh1 { get; set; }
        public string Hinh2 { get; set; }
        public string LyDoMacDinh { get; set; }
        [Column(TypeName = "nvarchar(55)")]
        public string PassCode { get; set; }
        [Column(TypeName = "nvarchar(55)")]
        [Compare("PassCode", ErrorMessage = "Mật khẩu không trùng khớp!!!")]
        public string RetypePasscode { get; set; }
        public int Ma_NguoiCapNhatCuoi { get; set; }
        public bool TrangThai { get; set; }
        [Column(TypeName = "date")]
        public DateTime NgayChuKyHetHan { get; set; }
    }
}
