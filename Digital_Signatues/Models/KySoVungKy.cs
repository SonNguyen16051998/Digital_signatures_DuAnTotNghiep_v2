using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models
{
    public class KySoVungKy
    {
        [Key]
        [ForeignKey("KySoBuocDuyet")]
        public int Ma_BuocDuyet { get; set; }
        [Column(TypeName ="nvarchar(5000)")]
        /*public string img_Sign { get; set; }
        public int page { get; set; }
        public int x { get; set; }
        public int y { get; set; }*/
        public string Json { get; set; }
        [ForeignKey("NguoiDung")]
        public int Ma_NguoiTao { get; set; }
        public KySoBuocDuyet KySoBuocDuyet { set; get; }
        public NguoiDung NguoiDung { get; set; }
    }
}
