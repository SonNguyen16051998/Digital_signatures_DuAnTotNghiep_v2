using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models.ViewPost
{
    public class PostNguoiDung
    {
        [Required(ErrorMessage = "Email không được để trống!!!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Họ tên")]
        public string HoTen { get; set; }
        [Required]
        [Display(Name = "Số điện thoại")]
        public string Sdt { get; set; }
        [Required]
        public string DiaChi { get; set; }
        [Required, Display(Name = "Giới tính")]
        public bool GioiTinh { get; set; }
        [Display(Name = "Hình ảnh")]
        public string Avatar { get; set; }
        public int Ma_ChucDanh { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }
        [Required(ErrorMessage = "Bạn cần nhập lại mật khẩu.")]
        [DataType(DataType.Password)]
        [Compare("PassWord", ErrorMessage = "Mật khẩu không trùng khớp!!!")]
        [NotMapped]
        [Display(Name = "Xác nhận mật khẩu")]
        public string RetypePassWord { get; set; }
    }
}
