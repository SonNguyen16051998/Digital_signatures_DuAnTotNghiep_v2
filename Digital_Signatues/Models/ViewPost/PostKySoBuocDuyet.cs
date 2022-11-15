using System.ComponentModel.DataAnnotations;

namespace Digital_Signatues.Models.ViewPost
{
    public class PostKySoBuocDuyet
    {
        [Required]
        public string Ten_Buoc { get; set; }
        [Required]
        public int Ma_NguoiKy { get; set; }
        [Required]
        public int Ma_KySoDeXuat { get; set; }
    }
}
