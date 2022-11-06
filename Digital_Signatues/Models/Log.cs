using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models
{
    public class Log
    {
        [Key]
        public int Ma_Log { get; set; }
        [Column(TypeName ="nvarchar(255)")]
        public string Ten_Log { get; set; }
        [ForeignKey("NguoiDung")]
        public int Ma_NguoiThucHien { get; set; }
        public DateTime ThoiGianThucHien { get; set; }
        public int? Ma_TaiKhoan { get; set; }//Log thông số người dùng
        public int? Ma_DeXuat { get; set; }//log ký thật
        public NguoiDung NguoiDung { get; set; }
    }
}
