using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models
{
    public class Quyen
    {
        [Key]
        public int Ma_Quyen { get; set; }
        [Required,Column(TypeName ="nvarchar(255)")]
        public string Ten_Quyen { get; set; }
        public bool Isdeleted { get; set; }
        public ICollection<Role_Quyen> Role_Quyen { get; set; } 
        public ICollection<NguoiDung_Quyen> NguoiDung_Quyens { get; set; }
    }
}
