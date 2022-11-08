using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models
{
    public class MaQR
    {        
        [ForeignKey("KySoDeXuat")]
        public int Ma_DeXuat { get; set; }
        [Column(TypeName = "varchar(6)")]
        public string MaSo { get; set; }
        [Column(TypeName = "varchar(255)"),Required]
        public string NoiDung { get; set; }
        public DateTime NgayTao { get; set; }
        [Column(TypeName ="tinyint")]
        public int MucDo { get; set; }//1 xem file voi ma QR, 2 dang nhap + ma QR,3 khong cho xem
        [ForeignKey("NguoiDung")]
        public int Ma_NguoiTao { get; set; }
        public KySoDeXuat KySoDeXuat { get; set; }
        public NguoiDung NguoiDung { get; set; }
    }
}
