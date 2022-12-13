using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models.ViewPost
{
    public class PostSign
    {
        public int Id_NguoiDung { get; set; }
        public string inputFile { get; set; }
        public List<PostPositionSign> PostPositionSigns { get; set; }
    }
}
