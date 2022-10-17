﻿// <auto-generated />
using System;
using Digital_Signatues.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Digital_Signatues.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Digital_Signatues.Models.ChucDanh", b =>
                {
                    b.Property<int>("Ma_ChucDanh")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSelected")
                        .HasColumnType("bit");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<string>("Ten_ChucDanh")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Ma_ChucDanh");

                    b.ToTable("ChucDanhs");
                });

            modelBuilder.Entity("Digital_Signatues.Models.KySoBuocDuyet", b =>
                {
                    b.Property<int>("Ma_BuocDuyet")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FileDaKy")
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("IsDaKy")
                        .HasColumnType("bit");

                    b.Property<bool>("IsTuChoi")
                        .HasColumnType("bit");

                    b.Property<int?>("KySoDeXuatMa_KySoDeXuat")
                        .HasColumnType("int");

                    b.Property<int>("Ma_KySoDeXuat")
                        .HasColumnType("int");

                    b.Property<int>("Ma_NguoiKy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("NgayKy")
                        .HasColumnType("datetime2");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<string>("Ten_Buoc")
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Ma_BuocDuyet");

                    b.HasIndex("KySoDeXuatMa_KySoDeXuat");

                    b.HasIndex("Ma_NguoiKy");

                    b.ToTable("kySoBuocDuyets");
                });

            modelBuilder.Entity("Digital_Signatues.Models.KySoDeXuat", b =>
                {
                    b.Property<int>("Ma_KySoDeXuat")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CurentOrder")
                        .HasColumnType("int");

                    b.Property<string>("GhiChu")
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("IsDaDuyet")
                        .HasColumnType("bit");

                    b.Property<string>("LoaiVanBan")
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("Ma_NguoiDeXuat")
                        .HasColumnType("int");

                    b.Property<DateTime>("NgayDeXuat")
                        .HasColumnType("datetime2");

                    b.Property<string>("Ten_DeXuat")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("TrangThai")
                        .HasColumnType("bit");

                    b.Property<string>("inputFile")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Ma_KySoDeXuat");

                    b.HasIndex("Ma_NguoiDeXuat");

                    b.ToTable("kySoDeXuats");
                });

            modelBuilder.Entity("Digital_Signatues.Models.KySoTest", b =>
                {
                    b.Property<int>("Id_KySoTest")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Id_NguoiDung")
                        .HasColumnType("int");

                    b.Property<DateTime>("NgayKyTest")
                        .HasColumnType("datetime2");

                    b.Property<string>("imgSign")
                        .IsRequired()
                        .HasColumnType("nvarchar(1000)");

                    b.Property<float>("img_h")
                        .HasColumnType("real");

                    b.Property<float>("img_w")
                        .HasColumnType("real");

                    b.Property<string>("inputFile")
                        .IsRequired()
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int>("pageSign")
                        .HasColumnType("int");

                    b.Property<float>("x")
                        .HasColumnType("real");

                    b.Property<float>("y")
                        .HasColumnType("real");

                    b.HasKey("Id_KySoTest");

                    b.HasIndex("Id_NguoiDung");

                    b.ToTable("KySoTests");
                });

            modelBuilder.Entity("Digital_Signatues.Models.KySoThongSo", b =>
                {
                    b.Property<int>("Ma_NguoiDung")
                        .HasColumnType("int");

                    b.Property<string>("FilePfx")
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Hinh1")
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Hinh2")
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Hinh3")
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("LoaiChuKy")
                        .HasColumnType("bit");

                    b.Property<string>("LyDoMacDinh")
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("Ma_NguoiCapNhatCuoi")
                        .HasColumnType("int");

                    b.Property<DateTime>("NgayCapNhatCuoi")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("NgayChuKyHetHan")
                        .HasColumnType("date");

                    b.Property<string>("PassCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(55)");

                    b.Property<string>("PasscodeFilePfx")
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Serial")
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("TrangThai")
                        .HasColumnType("bit");

                    b.HasKey("Ma_NguoiDung");

                    b.ToTable("KySoThongSos");
                });

            modelBuilder.Entity("Digital_Signatues.Models.NguoiDung", b =>
                {
                    b.Property<int>("Ma_NguoiDung")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Avatar")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("Block")
                        .HasColumnType("bit");

                    b.Property<string>("DiaChi")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(40)");

                    b.Property<bool>("GioiTinh")
                        .HasColumnType("bit");

                    b.Property<string>("HoTen")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsThongSo")
                        .HasColumnType("bit");

                    b.Property<int>("Ma_ChucDanh")
                        .HasColumnType("int");

                    b.Property<string>("PassWord")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Sdt")
                        .IsRequired()
                        .HasColumnType("varchar(15)");

                    b.HasKey("Ma_NguoiDung");

                    b.HasIndex("Ma_ChucDanh");

                    b.ToTable("NguoiDungs");
                });

            modelBuilder.Entity("Digital_Signatues.Models.NguoiDung_PhongBan", b =>
                {
                    b.Property<int>("Ma_NguoiDung")
                        .HasColumnType("int");

                    b.Property<int>("Ma_PhongBan")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("Date");

                    b.Property<string>("Ten_NguoiDung")
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Ma_NguoiDung", "Ma_PhongBan");

                    b.HasIndex("Ma_PhongBan");

                    b.ToTable("NguoiDung_PhongBans");
                });

            modelBuilder.Entity("Digital_Signatues.Models.NguoiDung_Quyen", b =>
                {
                    b.Property<int>("Ma_NguoiDung")
                        .HasColumnType("int");

                    b.Property<int>("Ma_Quyen")
                        .HasColumnType("int");

                    b.HasKey("Ma_NguoiDung", "Ma_Quyen");

                    b.HasIndex("Ma_Quyen");

                    b.ToTable("NguoiDung_Quyens");
                });

            modelBuilder.Entity("Digital_Signatues.Models.NguoiDung_Role", b =>
                {
                    b.Property<int>("Ma_NguoiDung")
                        .HasColumnType("int");

                    b.Property<int>("Ma_Role")
                        .HasColumnType("int");

                    b.HasKey("Ma_NguoiDung", "Ma_Role");

                    b.HasIndex("Ma_Role");

                    b.ToTable("NguoiDung_Roles");
                });

            modelBuilder.Entity("Digital_Signatues.Models.OTP", b =>
                {
                    b.Property<string>("email")
                        .HasColumnType("varchar(30)");

                    b.Property<string>("Otp")
                        .IsRequired()
                        .HasColumnType("varchar(6)");

                    b.Property<DateTime>("expiredAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("isUse")
                        .HasColumnType("bit");

                    b.HasKey("email");

                    b.ToTable("OTPs");
                });

            modelBuilder.Entity("Digital_Signatues.Models.PhongBan", b =>
                {
                    b.Property<int>("Ma_PhongBan")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("NgayTao")
                        .HasColumnType("date");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<string>("Ten_PhongBan")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Ma_PhongBan");

                    b.ToTable("PhongBans");
                });

            modelBuilder.Entity("Digital_Signatues.Models.Quyen", b =>
                {
                    b.Property<int>("Ma_Quyen")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Isdeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Ten_Quyen")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Ma_Quyen");

                    b.ToTable("Quyens");
                });

            modelBuilder.Entity("Digital_Signatues.Models.Role", b =>
                {
                    b.Property<int>("Ma_Role")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Ten_Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Ma_Role");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Digital_Signatues.Models.Role_Quyen", b =>
                {
                    b.Property<int>("Ma_Role")
                        .HasColumnType("int");

                    b.Property<int>("Ma_Quyen")
                        .HasColumnType("int");

                    b.HasKey("Ma_Role", "Ma_Quyen");

                    b.HasIndex("Ma_Quyen");

                    b.ToTable("Role_Quyens");
                });

            modelBuilder.Entity("Digital_Signatues.Models.KySoBuocDuyet", b =>
                {
                    b.HasOne("Digital_Signatues.Models.KySoDeXuat", "KySoDeXuat")
                        .WithMany("KySoBuocDuyets")
                        .HasForeignKey("KySoDeXuatMa_KySoDeXuat");

                    b.HasOne("Digital_Signatues.Models.NguoiDung", "NguoiDung")
                        .WithMany("kySoBuocDuyets")
                        .HasForeignKey("Ma_NguoiKy")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("KySoDeXuat");

                    b.Navigation("NguoiDung");
                });

            modelBuilder.Entity("Digital_Signatues.Models.KySoDeXuat", b =>
                {
                    b.HasOne("Digital_Signatues.Models.NguoiDung", "NguoiDung")
                        .WithMany("kySoDeXuats")
                        .HasForeignKey("Ma_NguoiDeXuat")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NguoiDung");
                });

            modelBuilder.Entity("Digital_Signatues.Models.KySoTest", b =>
                {
                    b.HasOne("Digital_Signatues.Models.NguoiDung", "NguoiDung")
                        .WithMany("kySoTests")
                        .HasForeignKey("Id_NguoiDung")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NguoiDung");
                });

            modelBuilder.Entity("Digital_Signatues.Models.KySoThongSo", b =>
                {
                    b.HasOne("Digital_Signatues.Models.NguoiDung", "NguoiDung")
                        .WithOne("KySoNguoiDung")
                        .HasForeignKey("Digital_Signatues.Models.KySoThongSo", "Ma_NguoiDung")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NguoiDung");
                });

            modelBuilder.Entity("Digital_Signatues.Models.NguoiDung", b =>
                {
                    b.HasOne("Digital_Signatues.Models.ChucDanh", "ChucDanh")
                        .WithMany("NguoiDung")
                        .HasForeignKey("Ma_ChucDanh")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChucDanh");
                });

            modelBuilder.Entity("Digital_Signatues.Models.NguoiDung_PhongBan", b =>
                {
                    b.HasOne("Digital_Signatues.Models.NguoiDung", "NguoiDung")
                        .WithMany("NguoiDung_PhongBan")
                        .HasForeignKey("Ma_NguoiDung")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Digital_Signatues.Models.PhongBan", "PhongBan")
                        .WithMany("NguoiDung_PhongBan")
                        .HasForeignKey("Ma_PhongBan")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NguoiDung");

                    b.Navigation("PhongBan");
                });

            modelBuilder.Entity("Digital_Signatues.Models.NguoiDung_Quyen", b =>
                {
                    b.HasOne("Digital_Signatues.Models.NguoiDung", "NguoiDung")
                        .WithMany("NguoiDung_Quyens")
                        .HasForeignKey("Ma_NguoiDung")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Digital_Signatues.Models.Quyen", "Quyen")
                        .WithMany("NguoiDung_Quyens")
                        .HasForeignKey("Ma_Quyen")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NguoiDung");

                    b.Navigation("Quyen");
                });

            modelBuilder.Entity("Digital_Signatues.Models.NguoiDung_Role", b =>
                {
                    b.HasOne("Digital_Signatues.Models.NguoiDung", "NguoiDung")
                        .WithMany("NguoiDung_Role")
                        .HasForeignKey("Ma_NguoiDung")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Digital_Signatues.Models.Role", "Role")
                        .WithMany("NguoiDung_Role")
                        .HasForeignKey("Ma_Role")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NguoiDung");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Digital_Signatues.Models.Role_Quyen", b =>
                {
                    b.HasOne("Digital_Signatues.Models.Quyen", "Quyen")
                        .WithMany("Role_Quyen")
                        .HasForeignKey("Ma_Quyen")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Digital_Signatues.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("Ma_Role")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Quyen");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Digital_Signatues.Models.ChucDanh", b =>
                {
                    b.Navigation("NguoiDung");
                });

            modelBuilder.Entity("Digital_Signatues.Models.KySoDeXuat", b =>
                {
                    b.Navigation("KySoBuocDuyets");
                });

            modelBuilder.Entity("Digital_Signatues.Models.NguoiDung", b =>
                {
                    b.Navigation("kySoBuocDuyets");

                    b.Navigation("kySoDeXuats");

                    b.Navigation("KySoNguoiDung");

                    b.Navigation("kySoTests");

                    b.Navigation("NguoiDung_PhongBan");

                    b.Navigation("NguoiDung_Quyens");

                    b.Navigation("NguoiDung_Role");
                });

            modelBuilder.Entity("Digital_Signatues.Models.PhongBan", b =>
                {
                    b.Navigation("NguoiDung_PhongBan");
                });

            modelBuilder.Entity("Digital_Signatues.Models.Quyen", b =>
                {
                    b.Navigation("NguoiDung_Quyens");

                    b.Navigation("Role_Quyen");
                });

            modelBuilder.Entity("Digital_Signatues.Models.Role", b =>
                {
                    b.Navigation("NguoiDung_Role");
                });
#pragma warning restore 612, 618
        }
    }
}
