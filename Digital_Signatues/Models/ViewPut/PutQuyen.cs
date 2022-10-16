using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models.ViewPut
{
    public class PutQuyen
    {
        [Key]
        public int Ma_Quyen { get; set; }
        [Required, Column(TypeName = "nvarchar(255)")]
        public string Ten_Quyen { get; set; }
    }
}
