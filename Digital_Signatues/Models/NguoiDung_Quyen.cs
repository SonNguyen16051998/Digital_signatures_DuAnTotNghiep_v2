using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models
{
    public class NguoiDung_Quyen
    {
        [ForeignKey("NguoiDung")]
        public int Ma_NguoiDung { get; set; }
        [ForeignKey("Quyen")]
        public int Ma_Quyen { get; set; }
        public NguoiDung NguoiDung { get; set; }
        public Quyen Quyen { get; set; }
    }
}
