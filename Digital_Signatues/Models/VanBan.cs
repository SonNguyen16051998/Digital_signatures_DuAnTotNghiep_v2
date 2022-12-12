using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models
{
    public class VanBan
    {
        [Key]
        public int Ma_VanBan { get; set; }
        [Required,Column(TypeName ="nvarchar(255)")]
        public string ChuDe { get; set; }
        [Required, Column(TypeName = "nvarchar(255)")]
        public string LoaiVanBan { get; set; }
        [Column(TypeName ="date")]
        public DateTime NgayTao { get; set; }
        [Required, Column(TypeName = "nvarchar(255)")]
        public string File { get; set; }
        [Required, Column(TypeName = "nvarchar(255)")]
        public string Ten_FileGoc { get; set; }
        [ForeignKey("NguoiDung")]
        public int Ma_NguoiTao { get; set; }
        [Column(TypeName ="nvarchar(255)")]
        public string NguoiKy { get; set; }
        public DateTime Ngay_HieuLuc { get;set; }
        public NguoiDung NguoiDung { get; set; }
    }
}
