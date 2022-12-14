using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models.ViewPost
{
    public class PostPositionSign
    {
        public string imgSign { get; set; }
        public string textSign { get; set; }
        [Required]
        public int pageSign { get; set; }
        [Required]
        public float x { get; set; }
        [Required]
        public float y { get; set; }
        [Required]
        public float img_w { get; set; }
        [Required]
        public float img_h { get; set; }
    }
}
