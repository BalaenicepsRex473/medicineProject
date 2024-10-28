using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace scrubsAPI.Migrations
{
    /// <inheritdoc />
    public partial class entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Diagnoses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    icdDiagnosisId = table.Column<Guid>(type: "TEXT", nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: false),
                    createTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    type = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    code = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnoses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Inspections",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    patientid = table.Column<Guid>(type: "TEXT", nullable: false),
                    date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    anamesis = table.Column<string>(type: "TEXT", nullable: false),
                    treatment = table.Column<string>(type: "TEXT", nullable: false),
                    conclusion = table.Column<int>(type: "INTEGER", nullable: false),
                    nextVisitDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    deathTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    previousInspectionid = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspections", x => x.id);
                    table.ForeignKey(
                        name: "FK_Inspections_Inspections_previousInspectionid",
                        column: x => x.previousInspectionid,
                        principalTable: "Inspections",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Inspections_Patients_patientid",
                        column: x => x.patientid,
                        principalTable: "Patients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Consultations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    createTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    inspectionid = table.Column<Guid>(type: "TEXT", nullable: false),
                    specialityid = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Consultations_Inspections_inspectionid",
                        column: x => x.inspectionid,
                        principalTable: "Inspections",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Consultations_Specialities_specialityid",
                        column: x => x.specialityid,
                        principalTable: "Specialities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    createTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    modifiedTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    content = table.Column<string>(type: "TEXT", nullable: false),
                    authorid = table.Column<Guid>(type: "TEXT", nullable: false),
                    parentCommentid = table.Column<Guid>(type: "TEXT", nullable: false),
                    consultationId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.id);
                    table.ForeignKey(
                        name: "FK_Comments_Comments_parentCommentid",
                        column: x => x.parentCommentid,
                        principalTable: "Comments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Consultations_consultationId",
                        column: x => x.consultationId,
                        principalTable: "Consultations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Doctors_authorid",
                        column: x => x.authorid,
                        principalTable: "Doctors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_authorid",
                table: "Comments",
                column: "authorid");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_consultationId",
                table: "Comments",
                column: "consultationId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_parentCommentid",
                table: "Comments",
                column: "parentCommentid");

            migrationBuilder.CreateIndex(
                name: "IX_Consultations_inspectionid",
                table: "Consultations",
                column: "inspectionid");

            migrationBuilder.CreateIndex(
                name: "IX_Consultations_specialityid",
                table: "Consultations",
                column: "specialityid");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_patientid",
                table: "Inspections",
                column: "patientid");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_previousInspectionid",
                table: "Inspections",
                column: "previousInspectionid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Diagnoses");

            migrationBuilder.DropTable(
                name: "Consultations");

            migrationBuilder.DropTable(
                name: "Inspections");
        }
    }
}
