using System.ComponentModel.DataAnnotations;

namespace Digital_Signatues.Models.ViewPost
{
    public class PostQR
    {
        [Required]
        public int Ma_DeXuat { get; set; }
        public int Ma_NguoiTao { get; set; }
        [Required]
        public float Left { get; set; }
        [Required]
        public float Top { get; set; }
        [Required]
        public string inputFile { get; set; }
        [Required]
        public int MucDo { get; set; }
    }
}
