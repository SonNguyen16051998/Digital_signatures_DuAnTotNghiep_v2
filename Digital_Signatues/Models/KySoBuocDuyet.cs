using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models
{
    public class KySoBuocDuyet
    {
        [Key]
        public int Ma_BuocDuyet { get; set; }
        [Column(TypeName ="nvarchar(255)")]
        public string Ten_Buoc { get; set; }
        [ForeignKey("NguoiDung")]
        public int Ma_NguoiKy { get; set; }
        public int Ma_KySoDeXuat { get; set; }
        public int Order { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string FileDaKy { get; set; }
        public DateTime? NgayKy { get; set; }
        public bool IsDaKy { get; set; }
        public bool IsTuChoi { get; set; }
        public NguoiDung NguoiDung { get; set; }
    }
}
