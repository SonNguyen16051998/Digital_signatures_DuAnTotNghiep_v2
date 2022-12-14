using Digital_Signatues.Models;
using Digital_Signatues.Models.ViewPost;
using Digital_Signatues.Models.ViewPut;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Digital_Signatues.Services
{
    public interface IKySoDeXuat
    {
        Task<int> PostDeXuatAsync(PostKySoDeXuat kySoDeXuat);
        Task<bool> PutDeXuatAsync(PutKySoDeXuat kySoDeXuat);
        Task<bool> DeleteDeXuatAsync(int ma_dexuat);//xoa de xuat
        Task<List<KySoDeXuat>> GetDeXuatsByNguoiDeXuatAsync(int ma_nguoidexuat);
        Task<List<KySoDeXuat>> GetDeXuatsAsync();//hien thi toan bo de xuat
        Task<KySoDeXuat> GetDeXuatAsync(int ma_dexuat);//hien thi chi tiet de xuat hien tai
        Task<bool> CheckDeleteAsync(int ma_dexuat);
        Task<bool> ChuyenDuyetAsync(int ma_dexuat);
        Task<List<KySoDeXuat>> GetKySoChuaDeXuatAsync(int ma_nguoidung);
        Task<List<KySoDeXuat>> GetKySoChoDuyetAsync(int ma_nguoidung);
        Task<List<KySoDeXuat>> GetKySoDaDuyetAsync(int ma_nguoidung);
        Task<List<KySoDeXuat>> GetKySoTuChoiAsync(int ma_nguoidung);
        Task<int> TaoVanBanAsync(PostVanBan postVanBan);
        Task<List<string>> GetAllImgKy(int ma_dexuat);
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
                string namePdf = Path.GetFileNameWithoutExtension(kySoDeXuat.inputFile).Replace("%","") + ".pdf";
                /*using var client = new HttpClient();
                using var s = await client.GetStreamAsync(kySoDeXuat.inputFile);
                using var fs = new FileStream(Path.Combine("FileDeXuat", namePdf), FileMode.OpenOrCreate);
                await s.CopyToAsync(fs);*/
                string remoteUri = kySoDeXuat.inputFile;
                string fileName = Path.Combine("wwwroot\\FileDeXuat", namePdf);
                using (var webpage = new WebClient())
                {
                    await webpage.DownloadFileTaskAsync(new System.Uri(remoteUri,System.UriKind.Absolute), fileName);
                }
                var postdexuat = new KySoDeXuat()
                {
                    Ten_DeXuat = kySoDeXuat.Ten_DeXuat,
                    Ma_NguoiDeXuat = kySoDeXuat.Ma_NguoiDeXuat,
                    LoaiVanBan = kySoDeXuat.LoaiVanBan,
                    GhiChu = kySoDeXuat.GhiChu,
                    inputFile = "FileDeXuat\\" + namePdf,
                    Ten_FileGoc = kySoDeXuat.Ten_FileGoc,
                    NgayDeXuat = System.DateTime.Now,
                    FileDaKy=null,
                    TrangThai = false,
                    isQR=false,
                    isTaoVanBan=false,
                    CurentOrder = 0
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
                update.isTaoVanBan = false;
                if(!string.IsNullOrEmpty(kySoDeXuat.inputFile))
                {
                    if (File.Exists(Path.Combine("wwwroot", update.inputFile)))
                    {
                        File.Delete(Path.Combine("wwwroot", update.inputFile));
                    }
                    string namePdf = Path.GetFileNameWithoutExtension(kySoDeXuat.inputFile).Replace("%", "") + ".pdf";
                    string remoteUri = kySoDeXuat.inputFile;
                    string fileName = Path.Combine("wwwroot\\FileDeXuat", namePdf);
                    using (var webpage = new WebClient())
                    {
                        await webpage.DownloadFileTaskAsync(new System.Uri(remoteUri, System.UriKind.Absolute), fileName);
                    }
                    update.inputFile = "FileDeXuat\\" + namePdf;
                    update.Ten_FileGoc = kySoDeXuat.Ten_FileGoc;
                }
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
                var buocduyet = await _context.kySoBuocDuyets
                    .Where(x => x.Ma_KySoDeXuat == ma_dexuat).ToListAsync();
                foreach (var item in buocduyet)
                {
                    _context.kySoBuocDuyets.Remove(item);
                }
                await _context.SaveChangesAsync();
                var delete = await _context.kySoDeXuats
                .Where(x => x.Ma_KySoDeXuat == ma_dexuat).FirstOrDefaultAsync();
                if(File.Exists(Path.Combine("wwwroot",delete.inputFile)))
                {
                    File.Delete(Path.Combine("wwwroot", delete.inputFile));
                }
                _context.kySoDeXuats.Remove(delete);
                await _context.SaveChangesAsync();
                ret = true;
            }
            catch { }
            return ret;
        }
        public async Task<List<KySoDeXuat>> GetDeXuatsByNguoiDeXuatAsync(int ma_nguoidexuat)
        {
            return await _context.kySoDeXuats.Where(x=>x.Ma_NguoiDeXuat==ma_nguoidexuat)
                .Include(x=>x.KySoBuocDuyets)
                .Include(x=>x.NguoiDung)
                .ToListAsync();
        }
        public async Task<List<KySoDeXuat>> GetDeXuatsAsync()
        {
            return await _context.kySoDeXuats.Include(x => x.KySoBuocDuyets).ToListAsync();
        }
        public async Task<KySoDeXuat> GetDeXuatAsync(int ma_dexuat)
        {
            return await _context.kySoDeXuats.Where(x => x.Ma_KySoDeXuat == ma_dexuat)
                .Include(x => x.KySoBuocDuyets)
                .Include(x=>x.NguoiDung)
                .FirstOrDefaultAsync();
        }
        public async Task<bool> CheckDeleteAsync(int ma_dexuat)
        {
            var check = await _context.kySoDeXuats
                .Where(x => x.Ma_KySoDeXuat == ma_dexuat && x.TrangThai == false).FirstOrDefaultAsync();
            return check == null ? false : true;
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
        public async Task<List<KySoDeXuat>> GetKySoChuaDeXuatAsync(int ma_nguoidung)
        {
            return await _context.kySoDeXuats
                .Where(x=>x.TrangThai==false && x.Ma_NguoiDeXuat==ma_nguoidung).ToListAsync();
        }
        public async Task<List<KySoDeXuat>> GetKySoChoDuyetAsync(int ma_nguoidung)
        {
            var dexuat = await _context.kySoDeXuats
                .Where(x => x.TrangThai == true && x.Ma_NguoiDeXuat == ma_nguoidung && x.IsDaDuyet == false)
                .Include(x => x.KySoBuocDuyets)
                .ToListAsync();
            List<KySoDeXuat> danhsachchoduyet = new List<KySoDeXuat>();
            foreach (var item in dexuat)
            {
                if (item.KySoBuocDuyets.Count > 0)
                {
                    var tuchoi = await _context.kySoBuocDuyets
                    .Where(x => x.Ma_KySoDeXuat == item.Ma_KySoDeXuat && x.IsTuChoi == true).FirstOrDefaultAsync();
                    if (tuchoi == null)
                    {
                        danhsachchoduyet.Add(item);
                    }
                }
            }
            return danhsachchoduyet;
        }
        public async Task<List<KySoDeXuat>> GetKySoDaDuyetAsync(int ma_nguoidung)
        {
            return await _context.kySoDeXuats
                .Where(x => x.IsDaDuyet == true && x.Ma_NguoiDeXuat == ma_nguoidung).ToListAsync();
        }
        public async Task<List<KySoDeXuat>> GetKySoTuChoiAsync(int ma_nguoidung)
        {
            var dexuat= await _context.kySoDeXuats.Where(x => x.Ma_NguoiDeXuat == ma_nguoidung)
                .Include(x=>x.KySoBuocDuyets)
                .ToListAsync();
            List<KySoDeXuat> danhsachtuchoi= new List<KySoDeXuat>();
            foreach(var item in dexuat)
            {
                if(item.KySoBuocDuyets.Count>0)
                {
                    var tuchoi = await _context.kySoBuocDuyets
                    .Where(x => x.Ma_KySoDeXuat == item.Ma_KySoDeXuat && x.IsTuChoi == true).FirstOrDefaultAsync();
                    if (tuchoi != null)
                    {
                        danhsachtuchoi.Add(item);
                    }
                }
            }
            return danhsachtuchoi;
        }

        public async Task<int> TaoVanBanAsync(PostVanBan postvanban)
        {
            int ret = 0;
            try
            {
                VanBan add = new VanBan()
                {
                    ChuDe = postvanban.ChuDe,
                    LoaiVanBan = postvanban.LoaiVanBan,
                    NgayTao = System.DateTime.Now,
                    File = postvanban.File,
                    Ten_FileGoc = postvanban.Ten_FileGoc,
                    Ma_NguoiTao = postvanban.Ma_NguoiTao,
                    NguoiKy = postvanban.NguoiKy,
                    Ngay_HieuLuc = postvanban.Ngay_HieuLuc
                };
                await _context.VanBans.AddAsync(add);
                await _context.SaveChangesAsync();
                ret = add.Ma_VanBan;
            }
            catch
            {
            }
            return ret;
        }
        public async Task<List<string>> GetAllImgKy(int ma_dexuat)
        {
            List<string> imgky = new List<string>();
            var dexuat=await _context.kySoDeXuats.Where(x=>x.Ma_KySoDeXuat==ma_dexuat).FirstOrDefaultAsync();
            var listBuocduyet = await _context.kySoBuocDuyets.Where(x => x.Ma_KySoDeXuat == ma_dexuat).ToListAsync();
            if(listBuocduyet.Count==0)
            {
                return imgky=null;
            }
            else
            {
                var buocduyethientai = await _context.kySoBuocDuyets
                                .Where(x => x.Ma_KySoDeXuat == ma_dexuat && x.Order == dexuat.CurentOrder).FirstOrDefaultAsync();
                int indexbuocduyethientai = listBuocduyet.IndexOf(buocduyethientai);
                if (indexbuocduyethientai == 0)
                {
                    return imgky = null;
                }
                else
                {
                    var buocduyettruoc = listBuocduyet[indexbuocduyethientai - 1];
                    imgky = Helpers.PDFToImage.PdfToJpg(Path.Combine("wwwroot", buocduyettruoc.FileDaKy));
                    return imgky;
                }
            }
            
        }
    }
}
