using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models.ViewPost
{
    public class PostSign
    {
        public int Id_NguoiDung { get; set; }
        [Required, Column(TypeName = "nvarchar(1000)")]
        public string inputFile { get; set; }
        public List<PostPositionSign> PostPositionSigns { get; set; }
    }
}
