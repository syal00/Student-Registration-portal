using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CourseLedger.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    Code = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", unicode: false, nullable: true),
                    HoursPerWeek = table.Column<int>(type: "integer", nullable: true),
                    FeeBase = table.Column<decimal>(type: "numeric(6,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Course__A25C5AA694384533", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Employee__3214EC070CC93D07", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Role = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Student__3214EC0749778D6A", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employee_Role",
                columns: table => new
                {
                    Employee_Id = table.Column<int>(type: "integer", nullable: false),
                    Role_Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee_Role", x => new { x.Employee_Id, x.Role_Id });
                    table.ForeignKey(
                        name: "FK_ToEmployee",
                        column: x => x.Employee_Id,
                        principalTable: "Employee",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ToRole",
                        column: x => x.Role_Id,
                        principalTable: "Role",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AcademicRecord",
                columns: table => new
                {
                    CourseCode = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    StudentId = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Grade = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Academic__3D052599C63BA3FB", x => new { x.StudentId, x.CourseCode });
                    table.ForeignKey(
                        name: "FK_AcademicRecord_Course",
                        column: x => x.CourseCode,
                        principalTable: "Course",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_AcademicRecord_Student",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicRecord_CourseCode",
                table: "AcademicRecord",
                column: "CourseCode");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_UserName",
                table: "Employee",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_Role_Role_Id",
                table: "Employee_Role",
                column: "Role_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcademicRecord");

            migrationBuilder.DropTable(
                name: "Employee_Role");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
