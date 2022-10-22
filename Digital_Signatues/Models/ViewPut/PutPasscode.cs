using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models.ViewPut
{
    public class PutPasscode
    {
        public int Ma_NguoiDung { get; set; }
        [Column(TypeName = "nvarchar(55)"), Required]
        public string PassCode { get; set; }
        [Column(TypeName = "nvarchar(55)"), Required]
        public string NewPassCode { get; set; }
    }
}
