using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models.ViewModel
{
    public class ViewLogin
    {
        [Key]
        [Required(ErrorMessage = "Bạn cần nhập email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Bạn cần nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }
    }
}
