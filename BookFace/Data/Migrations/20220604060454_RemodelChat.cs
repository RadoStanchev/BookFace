using Microsoft.EntityFrameworkCore.Migrations;

namespace BookFace.Data.Migrations
{
    public partial class RemodelChat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_AspNetUsers_Chats_ChatId",
            //    table: "AspNetUsers");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Messages_Chats_ChatId",
            //    table: "Messages");

            //migrationBuilder.DropIndex(
            //    name: "IX_AspNetUsers_ChatId",
            //    table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Messages",
                newName: "Context");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Chats",
                newName: "FriendshipId");

            migrationBuilder.DropColumn(
              name: "Id",
              table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Messages",
                type: "nvarchar(450)",
                nullable: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                column: "Id");

            //migrationBuilder.AlterColumn<string>(
            //    name: "Id",
            //    table: "Messages",
            //    type: "nvarchar(450)",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "ChatId",
                table: "Friendships",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Chats");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Chats",
                type: "nvarchar(450)",
                nullable: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chats",
                table: "Chats",
                column: "Id");

            //migrationBuilder.AlterColumn<string>(
            //    name: "Id",
            //    table: "Chats",
            //    type: "nvarchar(450)",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "ChatId",
                table: "Messages",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_ChatId",
                table: "Friendships",
                column: "ChatId",
                unique: true,
                filter: "[ChatId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Chats_ChatId",
                table: "Friendships",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Chats_ChatId",
                table: "Messages",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Chats_ChatId",
                table: "Friendships");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Chats_ChatId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Friendships_ChatId",
                table: "Friendships");

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "Friendships");

            migrationBuilder.RenameColumn(
                name: "Context",
                table: "Messages",
                newName: "Text");

            migrationBuilder.RenameColumn(
                name: "FriendshipId",
                table: "Chats",
                newName: "Name");

            migrationBuilder.AlterColumn<int>(
                name: "ChatId",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Messages",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Chats",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Chats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ChatId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ChatId",
                table: "AspNetUsers",
                column: "ChatId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Chats_ChatId",
                table: "AspNetUsers",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Chats_ChatId",
                table: "Messages",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
