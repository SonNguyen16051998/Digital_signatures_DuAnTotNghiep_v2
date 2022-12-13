using System.ComponentModel.DataAnnotations;

namespace Digital_Signatues.Models.ViewModel
{
    public class ViewChangePass
    {
        [Key]
        [Required(ErrorMessage = "Bạn cần nhập email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Bạn cần nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }
        [Required(ErrorMessage = "Bạn cần nhập mật khẩu mới")]
        [DataType(DataType.Password)]
        public string NewPass { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Bạn cần nhập lại mật khẩu mới")]
        [Compare("NewPass", ErrorMessage = "Mật khẩu không trùng khớp!!!")]
        public string RetypePass { get; set; }
    }
}
