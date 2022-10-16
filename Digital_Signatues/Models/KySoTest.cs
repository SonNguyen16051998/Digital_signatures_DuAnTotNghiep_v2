using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models
{
    public class KySoTest
    {
        [Key]
        public int Id_KySoTest { get; set; }
        [ForeignKey("NguoiDung")]
        public int Id_NguoiDung { get; set; }
        [Required,Column(TypeName ="nvarchar(1000)")]
        public string inputFile { get; set; }
        [Required, Column(TypeName = "nvarchar(1000)")]
        public string imgSign { get; set; }
        [Required]
        public int pageSign { get; set; }
        [Required]
        public float x { get; set; }
        [Required]
        public float y { get; set; }
        [Required]
        public float img_w { get; set; }
        [Required]
        public float img_h { get; set; }
        public DateTime NgayKyTest { get; set; }
        public NguoiDung NguoiDung { get; set; }
    }
}
