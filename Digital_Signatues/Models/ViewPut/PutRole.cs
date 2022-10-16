using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models.ViewPut
{
    public class PutRole
    {
        [Key]
        public int Ma_Role { get; set; }
        [Required, Column(TypeName = "nvarchar(255)")]
        public string Ten_Role { get; set; }
    }
}
