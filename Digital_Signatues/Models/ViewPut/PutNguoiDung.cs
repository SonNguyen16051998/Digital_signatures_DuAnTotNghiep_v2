using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models.ViewPut
{
    public class PutNguoiDung
    {

        [Key]
        public int Ma_NguoiDung { get; set; }
        [Required]
        public string HoTen { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Sdt { get; set; }
        [Required]
        public string DiaChi { get; set; }
        [Required]
        public bool GioiTinh { get; set; }
        public int Ma_ChucDanh { get; set; }
        public string Avatar { get; set; }
        public bool Block { get; set; }
    }
}
