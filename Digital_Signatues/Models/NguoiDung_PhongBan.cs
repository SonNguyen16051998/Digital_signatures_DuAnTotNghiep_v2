using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models
{
    public class NguoiDung_PhongBan
    {
        [ForeignKey("NguoiDung"),Required]
        public int Ma_NguoiDung { get; set; } 
        [ForeignKey("PhongBan"),Required]
        public int Ma_PhongBan { get; set; }
        [Column(TypeName ="Date")]
        public DateTime DateAdded { get; set; }= DateTime.Now;
        [Column(TypeName ="nvarchar(255)")]
        public string Ten_NguoiDung { get; set;}
        public NguoiDung NguoiDung { get; set; }
        public PhongBan PhongBan { get; set; }
    }
}
