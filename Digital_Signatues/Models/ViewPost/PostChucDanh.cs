using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models.ViewPost
{
    public class PostChucDanh
    {
        [Required(ErrorMessage = "Bạn cần nhập tên chức vụ!!!")]
        public string Ten_ChucDanh { get; set; }
    }
}
