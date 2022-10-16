using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Signatues.Models
{
    public class Role_Quyen
    {
        [ForeignKey("Role")]
        public int Ma_Role { get; set; }
        [ForeignKey("Quyen")]
        public int Ma_Quyen { get; set; }
        public Role Role { get; set; }
        public Quyen Quyen { get; set; }
    }
}
