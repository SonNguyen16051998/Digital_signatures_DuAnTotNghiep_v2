using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models
{
    public class PhongBan
    {
        [Key]
        public int Ma_PhongBan { get; set; }
        [Required(ErrorMessage = "Bạn cần nhập tên phòng ban!!!")]
        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "Tên phòng ban")]
        public string Ten_PhongBan { get; set; }
        [Column(TypeName = "date"), Display(Name = "Ngày tạo")]
        public DateTime NgayTao { get; set; }
        public int Order { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<NguoiDung_PhongBan> NguoiDung_PhongBan { get; set; }
    }
}
