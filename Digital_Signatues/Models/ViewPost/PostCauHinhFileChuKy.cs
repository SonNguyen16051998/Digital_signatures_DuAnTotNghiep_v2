using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models.ViewPost
{
    public class PostCauHinhFileChuKy
    {
        [Required]
        public int Ma_NguoiDung { get; set; }
        [Required]
        public bool LoaiChuKy { get; set; }
        public string FilePfx { get; set; }
        public string PasscodeFilePfx { get; set; }
        public string Client_ID { get; set; }
        public string Client_Secret { get; set; }
        public string UID { get; set; }
        public string PasswordSmartSign { get; set; }
        public bool isDislayValid { get; set; }
    }
}
