using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models.ViewPost
{
    public class PostPhongBan
    {
        [Required(ErrorMessage = "Bạn cần nhập tên phòng ban!!!")]
        [Column(TypeName = "nvarchar(50)")]
        public string Ten_PhongBan { get; set; }
    }
}
