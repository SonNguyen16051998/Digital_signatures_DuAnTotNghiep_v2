using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models
{
    public class OTP
    {
        [Key,Column(TypeName ="varchar(30)")]
        public string email { get; set; }
        [Column(TypeName ="varchar(6)"),Required]
        public string Otp { get; set; }
        public DateTime expiredAt { get; set; }
        public bool isUse { get; set; }
    }
}
