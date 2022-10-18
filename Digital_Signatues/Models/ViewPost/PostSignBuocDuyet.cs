using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Digital_Signatues.Models.ViewPost
{
    public class PostSignBuocDuyet
    {
        [Required]
        public int Id_NguoiDung { get; set; }
        [Required]
        public string inputFile { get; set; }
        [Required]
        public int Ma_BuocDuyet { get; set; }
        [Required]
        public string passcode { get; set; }
        public List<PostPositionSign> PostPositionSigns { get; set; }
    }
}
