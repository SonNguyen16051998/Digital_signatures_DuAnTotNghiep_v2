using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models.ViewPut
{
    public class PutThongSo
    {
        public int Ma_NguoiDung { get; set; }
        public string Hinh1 { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string Hinh2 { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string LyDoMacDinh { get; set; }
        public int Ma_NguoiCapNhatCuoi { get; set; }
        public bool TrangThai { get; set; }
    }
}
