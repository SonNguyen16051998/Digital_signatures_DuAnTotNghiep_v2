using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models
{
    public class KySoVungKy
    {
        [Key]
        public int Ma_KySoVungKy { get; set; }
        [ForeignKey("KySoBuocDuyet")]
        public int Ma_BuocDuyet { get; set; }
        [Column(TypeName ="nvarchar(5000)")]
        public string Json { get; set; }
        public KySoBuocDuyet KySoBuocDuyet { set; get; }
    }
}
