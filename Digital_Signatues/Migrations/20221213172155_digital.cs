using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Digital_Signatues.Migrations
{
    public partial class digital : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChucDanhs",
                columns: table => new
                {
                    Ma_ChucDanh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten_ChucDanh = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsSelected = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChucDanhs", x => x.Ma_ChucDanh);
                });

            migrationBuilder.CreateTable(
                name: "OTPs",
                columns: table => new
                {
                    email = table.Column<string>(type: "varchar(30)", nullable: false),
                    Otp = table.Column<string>(type: "varchar(6)", nullable: false),
                    expiredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isUse = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OTPs", x => x.email);
                });

            migrationBuilder.CreateTable(
                name: "PhongBans",
                columns: table => new
                {
                    Ma_PhongBan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten_PhongBan = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "date", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhongBans", x => x.Ma_PhongBan);
                });

            migrationBuilder.CreateTable(
                name: "Quyens",
                columns: table => new
                {
                    Ma_Quyen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten_Quyen = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Isdeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quyens", x => x.Ma_Quyen);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Ma_Role = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten_Role = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Ma_Role);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDungs",
                columns: table => new
                {
                    Ma_NguoiDung = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "varchar(100)", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Sdt = table.Column<string>(type: "varchar(15)", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    GioiTinh = table.Column<bool>(type: "bit", nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Ma_ChucDanh = table.Column<int>(type: "int", nullable: false),
                    PassWord = table.Column<string>(type: "varchar(50)", nullable: false),
                    Block = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsThongSo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDungs", x => x.Ma_NguoiDung);
                    table.ForeignKey(
                        name: "FK_NguoiDungs_ChucDanhs_Ma_ChucDanh",
                        column: x => x.Ma_ChucDanh,
                        principalTable: "ChucDanhs",
                        principalColumn: "Ma_ChucDanh",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Role_Quyens",
                columns: table => new
                {
                    Ma_Role = table.Column<int>(type: "int", nullable: false),
                    Ma_Quyen = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role_Quyens", x => new { x.Ma_Role, x.Ma_Quyen });
                    table.ForeignKey(
                        name: "FK_Role_Quyens_Quyens_Ma_Quyen",
                        column: x => x.Ma_Quyen,
                        principalTable: "Quyens",
                        principalColumn: "Ma_Quyen",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Role_Quyens_Roles_Ma_Role",
                        column: x => x.Ma_Role,
                        principalTable: "Roles",
                        principalColumn: "Ma_Role",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "kySoDeXuats",
                columns: table => new
                {
                    Ma_KySoDeXuat = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten_DeXuat = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Ma_NguoiDeXuat = table.Column<int>(type: "int", nullable: false),
                    LoaiVanBan = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    inputFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ten_FileGoc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayDeXuat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false),
                    IsDaDuyet = table.Column<bool>(type: "bit", nullable: false),
                    CurentOrder = table.Column<int>(type: "int", nullable: false),
                    FileDaKy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isQR = table.Column<bool>(type: "bit", nullable: false),
                    isTaoVanBan = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kySoDeXuats", x => x.Ma_KySoDeXuat);
                    table.ForeignKey(
                        name: "FK_kySoDeXuats_NguoiDungs_Ma_NguoiDeXuat",
                        column: x => x.Ma_NguoiDeXuat,
                        principalTable: "NguoiDungs",
                        principalColumn: "Ma_NguoiDung",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KySoThongSos",
                columns: table => new
                {
                    Ma_NguoiDung = table.Column<int>(type: "int", nullable: false),
                    Hinh1 = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    Hinh2 = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    Hinh3 = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    LyDoMacDinh = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    PassCode = table.Column<string>(type: "nvarchar(55)", nullable: false),
                    Ma_NguoiCapNhatCuoi = table.Column<int>(type: "int", nullable: false),
                    NgayCapNhatCuoi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false),
                    LoaiChuKy = table.Column<bool>(type: "bit", nullable: false),
                    NgayChuKyHetHan = table.Column<DateTime>(type: "date", nullable: false),
                    Serial = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    FilePfx = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    PasscodeFilePfx = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    Client_ID = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Client_Secret = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    UID = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    PasswordSmartSign = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    isDislayValid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KySoThongSos", x => x.Ma_NguoiDung);
                    table.ForeignKey(
                        name: "FK_KySoThongSos_NguoiDungs_Ma_NguoiDung",
                        column: x => x.Ma_NguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "Ma_NguoiDung",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Ma_Log = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten_Log = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Ma_NguoiThucHien = table.Column<int>(type: "int", nullable: false),
                    ThoiGianThucHien = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ma_TaiKhoan = table.Column<int>(type: "int", nullable: true),
                    Ma_DeXuat = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Ma_Log);
                    table.ForeignKey(
                        name: "FK_Logs_NguoiDungs_Ma_NguoiThucHien",
                        column: x => x.Ma_NguoiThucHien,
                        principalTable: "NguoiDungs",
                        principalColumn: "Ma_NguoiDung",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDung_PhongBans",
                columns: table => new
                {
                    Ma_NguoiDung = table.Column<int>(type: "int", nullable: false),
                    Ma_PhongBan = table.Column<int>(type: "int", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "Date", nullable: false),
                    Ten_NguoiDung = table.Column<string>(type: "nvarchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDung_PhongBans", x => new { x.Ma_NguoiDung, x.Ma_PhongBan });
                    table.ForeignKey(
                        name: "FK_NguoiDung_PhongBans_NguoiDungs_Ma_NguoiDung",
                        column: x => x.Ma_NguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "Ma_NguoiDung",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NguoiDung_PhongBans_PhongBans_Ma_PhongBan",
                        column: x => x.Ma_PhongBan,
                        principalTable: "PhongBans",
                        principalColumn: "Ma_PhongBan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDung_Quyens",
                columns: table => new
                {
                    Ma_NguoiDung = table.Column<int>(type: "int", nullable: false),
                    Ma_Quyen = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDung_Quyens", x => new { x.Ma_NguoiDung, x.Ma_Quyen });
                    table.ForeignKey(
                        name: "FK_NguoiDung_Quyens_NguoiDungs_Ma_NguoiDung",
                        column: x => x.Ma_NguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "Ma_NguoiDung",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NguoiDung_Quyens_Quyens_Ma_Quyen",
                        column: x => x.Ma_Quyen,
                        principalTable: "Quyens",
                        principalColumn: "Ma_Quyen",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDung_Roles",
                columns: table => new
                {
                    Ma_NguoiDung = table.Column<int>(type: "int", nullable: false),
                    Ma_Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDung_Roles", x => new { x.Ma_NguoiDung, x.Ma_Role });
                    table.ForeignKey(
                        name: "FK_NguoiDung_Roles_NguoiDungs_Ma_NguoiDung",
                        column: x => x.Ma_NguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "Ma_NguoiDung",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NguoiDung_Roles_Roles_Ma_Role",
                        column: x => x.Ma_Role,
                        principalTable: "Roles",
                        principalColumn: "Ma_Role",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VanBans",
                columns: table => new
                {
                    Ma_VanBan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChuDe = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    LoaiVanBan = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "date", nullable: false),
                    File = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Ten_FileGoc = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Ma_NguoiTao = table.Column<int>(type: "int", nullable: false),
                    NguoiKy = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Ngay_HieuLuc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VanBans", x => x.Ma_VanBan);
                    table.ForeignKey(
                        name: "FK_VanBans_NguoiDungs_Ma_NguoiTao",
                        column: x => x.Ma_NguoiTao,
                        principalTable: "NguoiDungs",
                        principalColumn: "Ma_NguoiDung",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "kySoBuocDuyets",
                columns: table => new
                {
                    Ma_BuocDuyet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten_Buoc = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Ma_NguoiKy = table.Column<int>(type: "int", nullable: false),
                    Ma_KySoDeXuat = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    FileDaKy = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    NgayKy = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDaKy = table.Column<bool>(type: "bit", nullable: false),
                    IsTuChoi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kySoBuocDuyets", x => x.Ma_BuocDuyet);
                    table.ForeignKey(
                        name: "FK_kySoBuocDuyets_kySoDeXuats_Ma_KySoDeXuat",
                        column: x => x.Ma_KySoDeXuat,
                        principalTable: "kySoDeXuats",
                        principalColumn: "Ma_KySoDeXuat",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_kySoBuocDuyets_NguoiDungs_Ma_NguoiKy",
                        column: x => x.Ma_NguoiKy,
                        principalTable: "NguoiDungs",
                        principalColumn: "Ma_NguoiDung",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "MaQRs",
                columns: table => new
                {
                    Ma_DeXuat = table.Column<int>(type: "int", nullable: false),
                    MaSo = table.Column<string>(type: "varchar(6)", nullable: false),
                    NoiDung = table.Column<string>(type: "varchar(255)", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MucDo = table.Column<byte>(type: "tinyint", nullable: false),
                    Ma_NguoiTao = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaQRs", x => new { x.Ma_DeXuat, x.MaSo });
                    table.ForeignKey(
                        name: "FK_MaQRs_kySoDeXuats_Ma_DeXuat",
                        column: x => x.Ma_DeXuat,
                        principalTable: "kySoDeXuats",
                        principalColumn: "Ma_KySoDeXuat",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaQRs_NguoiDungs_Ma_NguoiTao",
                        column: x => x.Ma_NguoiTao,
                        principalTable: "NguoiDungs",
                        principalColumn: "Ma_NguoiDung",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Ma_Message = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ma_NguoiDung = table.Column<int>(type: "int", nullable: false),
                    Y_Kien = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    ThoiGian = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileDinhKem = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Ma_DeXuat = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Ma_Message);
                    table.ForeignKey(
                        name: "FK_Messages_kySoDeXuats_Ma_DeXuat",
                        column: x => x.Ma_DeXuat,
                        principalTable: "kySoDeXuats",
                        principalColumn: "Ma_KySoDeXuat",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_NguoiDungs_Ma_NguoiDung",
                        column: x => x.Ma_NguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "Ma_NguoiDung",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "kySoVungKys",
                columns: table => new
                {
                    Ma_BuocDuyet = table.Column<int>(type: "int", nullable: false),
                    Json = table.Column<string>(type: "nvarchar(4000)", nullable: true),
                    Ma_NguoiTao = table.Column<int>(type: "int", nullable: false),
                    Ma_KySoDeXuat = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kySoVungKys", x => x.Ma_BuocDuyet);
                    table.ForeignKey(
                        name: "FK_kySoVungKys_kySoBuocDuyets_Ma_BuocDuyet",
                        column: x => x.Ma_BuocDuyet,
                        principalTable: "kySoBuocDuyets",
                        principalColumn: "Ma_BuocDuyet",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_kySoVungKys_kySoDeXuats_Ma_KySoDeXuat",
                        column: x => x.Ma_KySoDeXuat,
                        principalTable: "kySoDeXuats",
                        principalColumn: "Ma_KySoDeXuat",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_kySoVungKys_NguoiDungs_Ma_NguoiTao",
                        column: x => x.Ma_NguoiTao,
                        principalTable: "NguoiDungs",
                        principalColumn: "Ma_NguoiDung",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_kySoBuocDuyets_Ma_KySoDeXuat",
                table: "kySoBuocDuyets",
                column: "Ma_KySoDeXuat");

            migrationBuilder.CreateIndex(
                name: "IX_kySoBuocDuyets_Ma_NguoiKy",
                table: "kySoBuocDuyets",
                column: "Ma_NguoiKy");

            migrationBuilder.CreateIndex(
                name: "IX_kySoDeXuats_Ma_NguoiDeXuat",
                table: "kySoDeXuats",
                column: "Ma_NguoiDeXuat");

            migrationBuilder.CreateIndex(
                name: "IX_kySoVungKys_Ma_KySoDeXuat",
                table: "kySoVungKys",
                column: "Ma_KySoDeXuat");

            migrationBuilder.CreateIndex(
                name: "IX_kySoVungKys_Ma_NguoiTao",
                table: "kySoVungKys",
                column: "Ma_NguoiTao");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_Ma_NguoiThucHien",
                table: "Logs",
                column: "Ma_NguoiThucHien");

            migrationBuilder.CreateIndex(
                name: "IX_MaQRs_Ma_DeXuat",
                table: "MaQRs",
                column: "Ma_DeXuat",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaQRs_Ma_NguoiTao",
                table: "MaQRs",
                column: "Ma_NguoiTao");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_Ma_DeXuat",
                table: "Messages",
                column: "Ma_DeXuat");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_Ma_NguoiDung",
                table: "Messages",
                column: "Ma_NguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDung_PhongBans_Ma_PhongBan",
                table: "NguoiDung_PhongBans",
                column: "Ma_PhongBan");

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDung_Quyens_Ma_Quyen",
                table: "NguoiDung_Quyens",
                column: "Ma_Quyen");

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDung_Roles_Ma_Role",
                table: "NguoiDung_Roles",
                column: "Ma_Role");

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDungs_Ma_ChucDanh",
                table: "NguoiDungs",
                column: "Ma_ChucDanh");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Quyens_Ma_Quyen",
                table: "Role_Quyens",
                column: "Ma_Quyen");

            migrationBuilder.CreateIndex(
                name: "IX_VanBans_Ma_NguoiTao",
                table: "VanBans",
                column: "Ma_NguoiTao");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KySoThongSos");

            migrationBuilder.DropTable(
                name: "kySoVungKys");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "MaQRs");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "NguoiDung_PhongBans");

            migrationBuilder.DropTable(
                name: "NguoiDung_Quyens");

            migrationBuilder.DropTable(
                name: "NguoiDung_Roles");

            migrationBuilder.DropTable(
                name: "OTPs");

            migrationBuilder.DropTable(
                name: "Role_Quyens");

            migrationBuilder.DropTable(
                name: "VanBans");

            migrationBuilder.DropTable(
                name: "kySoBuocDuyets");

            migrationBuilder.DropTable(
                name: "PhongBans");

            migrationBuilder.DropTable(
                name: "Quyens");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "kySoDeXuats");

            migrationBuilder.DropTable(
                name: "NguoiDungs");

            migrationBuilder.DropTable(
                name: "ChucDanhs");
        }
    }
}
