using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi_test.Migrations
{
    public partial class AdminRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"Insert into AspNetRoles (Id,[name], [NormalizedName]) 
values ('20a471d0-4b5b-440d-a507-03d4124d9258','Admin','Admin')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
             migrationBuilder.Sql(@"Delete AspNetRoles where Id = '20a471d0-4b5b-440d-a507-03d4124d9258'");
        }
    }
}
