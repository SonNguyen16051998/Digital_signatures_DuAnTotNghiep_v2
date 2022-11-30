using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Digital_Signatues.Models.ViewPost
{
    public class VungKy
    {
        public int Ma_BuocDuyet { get; set; }
        public string Json { get; set; }
    }
    public class PostVungKy
    {
        [Required]
        public int Ma_DeXuat { get; set; }
        public int Ma_NguoiTao { get; set; }
        public List<VungKy> VungKies { get; set; }
    }
}
