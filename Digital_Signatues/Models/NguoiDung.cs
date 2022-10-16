using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models
{
    public class NguoiDung
    {
        [Key]
        public int Ma_NguoiDung { get; set; }
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
        /*[NotMapped]
        [Display(Name = "Chọn hình")]
        public IFormFile Image_File { get; set; }*/
        [ForeignKey("ChucDanh")]
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
        public bool Block { get; set; }
        public bool IsDeleted { get; set;}
        public bool IsThongSo { get; set;}
        public ChucDanh ChucDanh { get;set; }
        public ICollection<NguoiDung_PhongBan> NguoiDung_PhongBan { get; set; }
        public ICollection<NguoiDung_Role> NguoiDung_Role { get; set; }
        public ICollection<NguoiDung_Quyen> NguoiDung_Quyens { get; set; }
        public ICollection<KySoTest> kySoTests { get; set; }
        public KySoThongSo KySoNguoiDung { get; set; }
        public ICollection<KySoDeXuat> kySoDeXuats { get; set; }
        public ICollection<KySoBuocDuyet> kySoBuocDuyets { get; set; }
    }
}
