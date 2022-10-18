using System;
using System.ComponentModel.DataAnnotations;

namespace Digital_Signatues.Models.ViewPost
{
    public class PostMessage
    {
        [Required]
        public int Ma_NguoiDung { get; set; }
        [Required]
        public string Y_Kien { get; set; }
        public string FileDinhKem { get; set; }
        [Required]
        public int Ma_DeXuat { get; set; }
    }
}
