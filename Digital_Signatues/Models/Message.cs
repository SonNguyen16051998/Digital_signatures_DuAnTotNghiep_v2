using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models
{
    public class Message
    {
        [Key]
        public int Ma_Message { get; set; }
        [ForeignKey("NguoiDung")]
        public int Ma_NguoiDung { get; set;}
        [Column(TypeName ="nvarchar(255)"),Required]
        public string Y_Kien { get; set; }
        public DateTime ThoiGian { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string FileDinhKem { get; set; }
        [ForeignKey("KySoDeXuat")]
        public int Ma_DeXuat { get; set; }
        public KySoDeXuat KySoDeXuat { get; set; }
        public NguoiDung NguoiDung { get; set;}
    }
}
