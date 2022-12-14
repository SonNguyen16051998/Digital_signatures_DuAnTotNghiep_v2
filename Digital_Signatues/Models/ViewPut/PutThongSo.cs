using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models.ViewPut
{
    public class PutThongSo
    {
        public int Ma_NguoiDung { get; set; }
        public string Hinh1 { get; set; }
        public string Hinh2 { get; set; }
        public string Hinh3 { get; set; }
        public string LyDoMacDinh { get; set; }
        public int Ma_NguoiCapNhatCuoi { get; set; }
        public bool TrangThai { get; set; }
        [Column(TypeName = "date")]
        public DateTime NgayChuKyHetHan { get; set; }
    }
}
