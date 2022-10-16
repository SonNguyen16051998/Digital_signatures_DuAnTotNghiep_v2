using System.Collections.Generic;

namespace Digital_Signatues.Models.ViewModel
{
    public class PostNguoiDung_PhongBan
    {
        public int Id_NguoiDung { get; set; }
        public List<ViewPhongBan> PhongBans { get; set; }
    }
}
