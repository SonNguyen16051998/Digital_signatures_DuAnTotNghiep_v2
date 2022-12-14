using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models.ViewPut
{
    public class PutChucDanh
    {
        [Key]
        public int Ma_ChucDanh { get; set; }
        [Required(ErrorMessage = "Bạn cần nhập tên chức vụ!!!")]
        public string Ten_ChucDanh { get; set; }
    }
}
