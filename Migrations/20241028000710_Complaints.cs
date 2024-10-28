using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace scrubsAPI.Migrations
{
    /// <inheritdoc />
    public partial class Complaints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_parentCommentid",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Consultations_consultationId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultations_Specialities_specialityid",
                table: "Consultations");

            migrationBuilder.DropColumn(
                name: "code",
                table: "Diagnoses");

            migrationBuilder.RenameColumn(
                name: "icdDiagnosisId",
                table: "Diagnoses",
                newName: "icdDiagnosisid");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Diagnoses",
                newName: "inspectionid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Consultations",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "consultationId",
                table: "Comments",
                newName: "consultationid");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_consultationId",
                table: "Comments",
                newName: "IX_Comments_consultationid");

            migrationBuilder.AddColumn<string>(
                name: "complaints",
                table: "Inspections",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "doctorid",
                table: "Inspections",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "specialityid",
                table: "Consultations",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<Guid>(
                name: "parentCommentid",
                table: "Comments",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "modifiedTime",
                table: "Comments",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_doctorid",
                table: "Inspections",
                column: "doctorid");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnoses_icdDiagnosisid",
                table: "Diagnoses",
                column: "icdDiagnosisid");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnoses_inspectionid",
                table: "Diagnoses",
                column: "inspectionid");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_parentCommentid",
                table: "Comments",
                column: "parentCommentid",
                principalTable: "Comments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Consultations_consultationid",
                table: "Comments",
                column: "consultationid",
                principalTable: "Consultations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Consultations_Specialities_specialityid",
                table: "Consultations",
                column: "specialityid",
                principalTable: "Specialities",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Diagnoses_Icd10s_icdDiagnosisid",
                table: "Diagnoses",
                column: "icdDiagnosisid",
                principalTable: "Icd10s",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Diagnoses_Inspections_inspectionid",
                table: "Diagnoses",
                column: "inspectionid",
                principalTable: "Inspections",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inspections_Doctors_doctorid",
                table: "Inspections",
                column: "doctorid",
                principalTable: "Doctors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_parentCommentid",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Consultations_consultationid",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultations_Specialities_specialityid",
                table: "Consultations");

            migrationBuilder.DropForeignKey(
                name: "FK_Diagnoses_Icd10s_icdDiagnosisid",
                table: "Diagnoses");

            migrationBuilder.DropForeignKey(
                name: "FK_Diagnoses_Inspections_inspectionid",
                table: "Diagnoses");

            migrationBuilder.DropForeignKey(
                name: "FK_Inspections_Doctors_doctorid",
                table: "Inspections");

            migrationBuilder.DropIndex(
                name: "IX_Inspections_doctorid",
                table: "Inspections");

            migrationBuilder.DropIndex(
                name: "IX_Diagnoses_icdDiagnosisid",
                table: "Diagnoses");

            migrationBuilder.DropIndex(
                name: "IX_Diagnoses_inspectionid",
                table: "Diagnoses");

            migrationBuilder.DropColumn(
                name: "complaints",
                table: "Inspections");

            migrationBuilder.DropColumn(
                name: "doctorid",
                table: "Inspections");

            migrationBuilder.RenameColumn(
                name: "icdDiagnosisid",
                table: "Diagnoses",
                newName: "icdDiagnosisId");

            migrationBuilder.RenameColumn(
                name: "inspectionid",
                table: "Diagnoses",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Consultations",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "consultationid",
                table: "Comments",
                newName: "consultationId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_consultationid",
                table: "Comments",
                newName: "IX_Comments_consultationId");

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "Diagnoses",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<Guid>(
                name: "specialityid",
                table: "Consultations",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "parentCommentid",
                table: "Comments",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "modifiedTime",
                table: "Comments",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_parentCommentid",
                table: "Comments",
                column: "parentCommentid",
                principalTable: "Comments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Consultations_consultationId",
                table: "Comments",
                column: "consultationId",
                principalTable: "Consultations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Consultations_Specialities_specialityid",
                table: "Consultations",
                column: "specialityid",
                principalTable: "Specialities",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
