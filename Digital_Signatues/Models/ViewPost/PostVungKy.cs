﻿using System.Collections.Generic;

namespace Digital_Signatues.Models.ViewPost
{
    public class VungKy
    {
        public int Ma_BuocDuyet { get; set; }
        public string Json { get; set; }
        public int Ma_NguoiTao { get; set; }
    }
    public class PostVungKy
    {
        public List<VungKy> VungKies { get; set; }
    }
}
