using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models.ViewPut
{
    public class PutPhongBan
    {
        [Key]
        public int Ma_PhongBan { get; set; }
        [Required(ErrorMessage = "Bạn cần nhập tên phòng ban!!!")]
        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "Tên phòng ban")]
        public string Ten_PhongBan { get; set; }
    }
}
