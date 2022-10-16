using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models
{
    public class KySoThongSo
    {
        [Key,ForeignKey("NguoiDung")]
        public int Ma_NguoiDung { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string Hinh1 { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string Hinh2 { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string Hinh3 { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string LyDoMacDinh { get; set; }
        [Column(TypeName = "nvarchar(55)"),Required]
        public string PassCode { get; set; }
        public int Ma_NguoiCapNhatCuoi { get; set; }
        public DateTime NgayCapNhatCuoi { get; set; }
        public bool TrangThai { get; set; }//0 là không hiệu lực, 1 là hiệu lực
        public bool LoaiChuKy { get; set; }//0 là kí file, 1 là kí smartSign
        [Column(TypeName ="date")]
        public DateTime NgayChuKyHetHan { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string Serial { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string Subject { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string FilePfx { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string PasscodeFilePfx { get; set; }
        public NguoiDung NguoiDung { get; set; }
    }
}
