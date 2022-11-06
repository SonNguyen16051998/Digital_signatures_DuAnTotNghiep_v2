using System;

namespace Digital_Signatues.Models.ViewPost
{
    public class PostLog
    {
        public string Ten_Log { get; set; }
        public int Ma_NguoiThucHien { get; set; }
        public int? Ma_TaiKhoan { get; set; }//Log thông số người dùng
        public int? Ma_DeXuat { get; set; }//log ký thật
    }
}
