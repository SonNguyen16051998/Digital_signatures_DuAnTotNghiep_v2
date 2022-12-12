﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Digital_Signatues.Models.ViewPost
{
    public class PostVanBan
    {
        [Required]
        public string ChuDe { get; set; }
        [Required]
        public string LoaiVanBan { get; set; }
        public string File { get; set; }
        public string Ten_FileGoc { get; set; }
        [Required]
        public int Ma_NguoiTao { get; set; }
        public string NguoiKy { get; set; }
        public DateTime Ngay_HieuLuc { get; set; }
    }
}
