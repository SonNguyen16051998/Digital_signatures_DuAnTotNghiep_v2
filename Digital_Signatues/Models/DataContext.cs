using Microsoft.EntityFrameworkCore;

namespace Digital_Signatues.Models
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
        protected override void OnModelCreating(ModelBuilder model)
        {
            //ràng buộc 2 khóa chính  1 bảng cần sử dụng fluent API
            model.Entity<NguoiDung_PhongBan>().HasKey(e => new { e.Ma_NguoiDung, e.Ma_PhongBan });

            model.Entity<NguoiDung_Role>().HasKey(e => new { e.Ma_NguoiDung, e.Ma_Role });

            model.Entity<Role_Quyen>().HasKey(e => new { e.Ma_Role, e.Ma_Quyen });

            model.Entity<NguoiDung_Quyen>().HasKey(e => new { e.Ma_NguoiDung, e.Ma_Quyen });

        }

        public DbSet<ChucDanh> ChucDanhs { get; set; }
        public DbSet<NguoiDung> NguoiDungs { get; set; }
        public DbSet<NguoiDung_PhongBan> NguoiDung_PhongBans { get; set; }
        public DbSet<NguoiDung_Quyen> NguoiDung_Quyens { get; set; }
        public DbSet<NguoiDung_Role> NguoiDung_Roles { get; set; }
        public DbSet<PhongBan> PhongBans { get; set; }
        public DbSet<Quyen> Quyens { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Role_Quyen> Role_Quyens { get; set; }
        public DbSet<OTP> OTPs { get; set; }
        public DbSet<KySoTest> KySoTests { get; set;}
        public DbSet<KySoThongSo> KySoThongSos { get; set; }
        public DbSet<KySoDeXuat> kySoDeXuats { get; set; }
        public DbSet<KySoBuocDuyet> kySoBuocDuyets { get; set; }
    }
}
