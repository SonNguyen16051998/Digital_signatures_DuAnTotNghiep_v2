using System.ComponentModel.DataAnnotations;

namespace Digital_Signatues.Models.ViewPut
{
    public class PutSapXep
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int Order { get; set; }
    }
}
