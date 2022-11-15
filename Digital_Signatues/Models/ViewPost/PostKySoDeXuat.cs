using System.ComponentModel.DataAnnotations;

namespace Digital_Signatues.Models.ViewPost
{
    public class PostKySoDeXuat
    {
        [Required]
        public string Ten_DeXuat { get; set; }
        public string LoaiVanBan { get; set; }
        public string GhiChu { get; set; }
        [Required]
        public string inputFile { get; set; }
        [Required]
        public string Ten_FileGoc { get; set; }
        [Required]
        public int Ma_NguoiDeXuat { get; set; }
    }
}
