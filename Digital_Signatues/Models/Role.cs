using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models
{
    public class Role
    {
        [Key]
        public int Ma_Role { get; set; }
        [Required,Column(TypeName ="nvarchar(255)")]
        public string Ten_Role { get; set;}
        public bool IsDeleted { get; set; }
        public ICollection<NguoiDung_Role> NguoiDung_Role { get; set; }
    }
}
