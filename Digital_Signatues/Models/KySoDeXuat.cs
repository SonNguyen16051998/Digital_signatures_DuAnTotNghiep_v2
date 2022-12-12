using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models
{
    public class KySoDeXuat
    {
        [Key]
        public int Ma_KySoDeXuat { get; set; }
        [Column(TypeName ="nvarchar(255)"),Required]
        public string Ten_DeXuat { get; set; }
        [ForeignKey("NguoiDung")]
        public int Ma_NguoiDeXuat { get; set; }
        [Column(TypeName ="nvarchar(255)")]
        public string LoaiVanBan { get; set; }
        [Column(TypeName ="nvarchar(500)")]
        public string GhiChu { get; set; }
        public string inputFile { get; set; }
        public string Ten_FileGoc { get; set; }
        public DateTime NgayDeXuat { get; set; }
        public bool TrangThai { get; set; }//đã chuyển duyệt hay chưa
        public bool IsDaDuyet { get; set; }//đã ký xong hay chưa
        public int CurentOrder { get; set; }//bước duyệt hiện tại
        public string FileDaKy { get; set; }//file ky cuoi cung
        public bool isQR { get; set; }//da gan ma QR
        public bool isTaoVanBan { get; set; }
        public ICollection<KySoBuocDuyet> KySoBuocDuyets { get; set; }
        public NguoiDung NguoiDung { get; set;}
        public ICollection<Message> Messages { get; set; }
        public MaQR MaQR { get; set; }
        public ICollection<KySoVungKy> KySoVungKys { get; set; }
    }
}
