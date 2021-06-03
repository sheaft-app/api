using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Migrations
{
    public partial class Update_Orders_Transactions_Statuses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("update app.orders set Status = 6 where Status = 2");
            
            migrationBuilder.Sql("update app.donations set Processed = 1 where Processed is null");
            migrationBuilder.Sql("update app.payins set Processed = 1 where Processed is null");
            migrationBuilder.Sql("update app.payouts set Processed = 1 where Processed is null");
            migrationBuilder.Sql("update app.preAuthorizations set Processed = 1 where Processed is null");
            migrationBuilder.Sql("update app.refunds set Processed = 1 where Processed is null");
            migrationBuilder.Sql("update app.transfers set Processed = 1 where Processed is null");
            migrationBuilder.Sql("update app.withholdings set Processed = 1 where Processed is null");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
