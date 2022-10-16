﻿using Digital_Signatues.Models;
using Digital_Signatues.Models.ViewPost;
using Digital_Signatues.Models.ViewPut;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digital_Signatues.Services
{
    public interface IKySoDeXuat
    {
        Task<int> PostDeXuatAsync(PostKySoDeXuat kySoDeXuat);
        Task<bool> PutDeXuatAsync(PutKySoDeXuat kySoDeXuat);
        Task<bool> DeleteDeXuatAsync(int ma_dexuat);
        Task<List<KySoDeXuat>> GetDeXuatsByNguoiDeXuatAsync(int ma_nguoidexuat);
        Task<List<KySoDeXuat>> GetDeXuatsAsync();
        Task<KySoDeXuat> GetDeXuatAsync(int ma_dexuat);
        Task<bool> CheckDeleteAsync(int ma_dexuat);
        Task<bool> ChuyenDuyetAsync(int ma_dexuat);
    }
    public class KySoDeXuatSvc:IKySoDeXuat
    {
        private readonly DataContext _context;
        public KySoDeXuatSvc(DataContext context)
        {
           _context = context;
        }

        public async Task<int> PostDeXuatAsync(PostKySoDeXuat kySoDeXuat)
        {
            int ret = 0;
            try
            {
                var postdexuat = new KySoDeXuat()
                {
                    Ten_DeXuat = kySoDeXuat.Ten_DeXuat,
                    Ma_NguoiDeXuat = kySoDeXuat.Ma_NguoiDeXuat,
                    LoaiVanBan = kySoDeXuat.LoaiVanBan,
                    GhiChu = kySoDeXuat.GhiChu,
                    inputFile = kySoDeXuat.inputFile,
                    NgayDeXuat = System.DateTime.Now,
                    TrangThai = false
                };
                await _context.kySoDeXuats.AddAsync(postdexuat);
                await _context.SaveChangesAsync();
                ret = postdexuat.Ma_KySoDeXuat;
            }
            catch { }
            return ret;
        }
        public async Task<bool> PutDeXuatAsync(PutKySoDeXuat kySoDeXuat)
        {
            bool ret = false;
            try
            {
                var update = await _context.kySoDeXuats
                    .Where(x => x.Ma_KySoDeXuat == kySoDeXuat.Ma_KySoDeXuat).FirstOrDefaultAsync();
                update.Ten_DeXuat = kySoDeXuat.Ten_DeXuat;
                update.LoaiVanBan = kySoDeXuat.LoaiVanBan;
                update.GhiChu = kySoDeXuat.GhiChu;
                update.inputFile = kySoDeXuat.inputFile;
                _context.kySoDeXuats.Update(update);
                await _context.SaveChangesAsync();
                ret = true;
            }
            catch { }
            return ret;
        }

        public async Task<bool> DeleteDeXuatAsync(int ma_dexuat)
        { 
            bool ret = false;
            try
            {
                var delete = await _context.kySoDeXuats
                .Where(x => x.Ma_KySoDeXuat == ma_dexuat).FirstOrDefaultAsync();
                _context.kySoDeXuats.Remove(delete);
                await _context.SaveChangesAsync();
                ret = true;
            }
            catch { }
            return ret;
        }
        public async Task<List<KySoDeXuat>> GetDeXuatsByNguoiDeXuatAsync(int ma_nguoidexuat)
        {
            return await _context.kySoDeXuats.Where(x=>x.Ma_NguoiDeXuat==ma_nguoidexuat).ToListAsync();
        }
        public async Task<List<KySoDeXuat>> GetDeXuatsAsync()
        {
            return await _context.kySoDeXuats.ToListAsync();
        }
        public async Task<KySoDeXuat> GetDeXuatAsync(int ma_dexuat)
        {
            return await _context.kySoDeXuats.Where(x => x.Ma_KySoDeXuat == ma_dexuat).FirstOrDefaultAsync();
        }
        public async Task<bool> CheckDeleteAsync(int ma_dexuat)
        {
            var check = await _context.kySoDeXuats
                .Where(x => x.Ma_NguoiDeXuat == ma_dexuat && x.TrangThai == false).FirstOrDefaultAsync();
            if (check != null)
            {
                return true;
            }
            else
                return false;
        }
        public async Task<bool> ChuyenDuyetAsync(int ma_dexuat)
        {
            bool ret = false;
            try
            {
                var chuyenduyet = await _context.kySoDeXuats.Where(x => x.Ma_KySoDeXuat == ma_dexuat).FirstOrDefaultAsync();
                chuyenduyet.TrangThai = true;
                _context.kySoDeXuats.Update(chuyenduyet);
                await _context.SaveChangesAsync();
                ret = true;
            }
            catch { 
            }
            return ret;
            
        }
    }
}
