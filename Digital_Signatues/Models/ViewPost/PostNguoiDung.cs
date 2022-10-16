using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models.ViewPost
{
    public class PostNguoiDung
    {
        [Required(ErrorMessage = "Email không được để trống!!!")]
        [Column(TypeName = "varchar(40)")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Họ tên")]
        public string HoTen { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber), Column(TypeName = "varchar(15)")]
        [Display(Name = "Số điện thoại")]
        public string Sdt { get; set; }
        [Required]
        [Display(Name = "Địa chỉ"), Column(TypeName = "nvarchar(255)")]
        public string DiaChi { get; set; }
        [Required, Display(Name = "Giới tính")]
        public bool GioiTinh { get; set; }
        [StringLength(255)]
        [Display(Name = "Hình ảnh")]
        public string Avatar { get; set; }
        public int Ma_ChucDanh { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [Column(TypeName = "varchar(50)"), MinLength(6, ErrorMessage = "Mật khẩu từ 6-12 kí tự")]
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }
        [Required(ErrorMessage = "Bạn cần nhập lại mật khẩu.")]
        [Column(TypeName = "varchar(50)"), MinLength(6, ErrorMessage = "Mật khẩu từ 6-12 kí tự")]
        [DataType(DataType.Password)]
        [Compare("PassWord", ErrorMessage = "Mật khẩu không trùng khớp!!!")]
        [NotMapped]
        [Display(Name = "Xác nhận mật khẩu")]
        public string RetypePassWord { get; set; }
    }
}
