using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models.ViewPut
{
    public class PutNguoiDung
    {

        [Key]
        public int Ma_NguoiDung { get; set; }
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
        public int Ma_ChucDanh { get; set; }
        [StringLength(255)]
        [Display(Name = "Hình ảnh")]
        public string Avatar { get; set; }
        public bool Block { get; set; }
    }
}
