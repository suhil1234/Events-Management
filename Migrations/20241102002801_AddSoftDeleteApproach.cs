using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Event_managment.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteApproach : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventParticipants_Event",
                table: "tb_event_participants");

            migrationBuilder.DropForeignKey(
                name: "FK_EventParticipants_Participant",
                table: "tb_event_participants");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_events_tb_locations",
                table: "tb_events");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "tb_participants",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "tb_locations",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "tb_events",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "tb_event_participants",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "tb_access_list",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_EventParticipants_Event",
                table: "tb_event_participants",
                column: "event_id",
                principalTable: "tb_events",
                principalColumn: "event_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EventParticipants_Participant",
                table: "tb_event_participants",
                column: "participant_id",
                principalTable: "tb_participants",
                principalColumn: "participant_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_events_tb_locations",
                table: "tb_events",
                column: "location_id",
                principalTable: "tb_locations",
                principalColumn: "location_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventParticipants_Event",
                table: "tb_event_participants");

            migrationBuilder.DropForeignKey(
                name: "FK_EventParticipants_Participant",
                table: "tb_event_participants");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_events_tb_locations",
                table: "tb_events");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "tb_participants");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "tb_locations");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "tb_events");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "tb_event_participants");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "tb_access_list");

            migrationBuilder.AddForeignKey(
                name: "FK_EventParticipants_Event",
                table: "tb_event_participants",
                column: "event_id",
                principalTable: "tb_events",
                principalColumn: "event_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventParticipants_Participant",
                table: "tb_event_participants",
                column: "participant_id",
                principalTable: "tb_participants",
                principalColumn: "participant_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_events_tb_locations",
                table: "tb_events",
                column: "location_id",
                principalTable: "tb_locations",
                principalColumn: "location_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
