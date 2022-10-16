using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models.ViewPost
{
    public class PostRole
    {
        [Required, Column(TypeName = "nvarchar(255)")]
        public string Ten_Role { get; set; }
    }
}
