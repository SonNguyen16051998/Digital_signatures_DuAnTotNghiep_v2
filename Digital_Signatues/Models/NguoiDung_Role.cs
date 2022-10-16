using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models
{
    public class NguoiDung_Role
    {
        [ForeignKey("NguoiDung")]
        public int Ma_NguoiDung { get; set; }
        [ForeignKey("Role")]
        public int Ma_Role { get; set; }
        public NguoiDung NguoiDung { get; set; }
        public Role Role { get; set; }
    }
}
