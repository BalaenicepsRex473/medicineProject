using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace scrubsAPI.Migrations
{
    /// <inheritdoc />
    public partial class innitial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Icd10s",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    createTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    parentId = table.Column<Guid>(type: "uuid", nullable: true),
                    parentIdFromJson = table.Column<int>(type: "integer", nullable: true),
                    idFromJson = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Icd10s", x => x.id);
                    table.ForeignKey(
                        name: "FK_Icd10s_Icd10s_parentId",
                        column: x => x.parentId,
                        principalTable: "Icd10s",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    birthDay = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    gender = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Specialities",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    creationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialities", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    gender = table.Column<int>(type: "integer", nullable: false),
                    birthday = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    specialityid = table.Column<Guid>(type: "uuid", nullable: false),
                    createTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.id);
                    table.ForeignKey(
                        name: "FK_Doctors_Specialities_specialityid",
                        column: x => x.specialityid,
                        principalTable: "Specialities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inspections",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    patientid = table.Column<Guid>(type: "uuid", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    anamesis = table.Column<string>(type: "text", nullable: false),
                    treatment = table.Column<string>(type: "text", nullable: false),
                    complaints = table.Column<string>(type: "text", nullable: false),
                    conclusion = table.Column<int>(type: "integer", nullable: false),
                    nextVisitDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deathTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    previousInspectionid = table.Column<Guid>(type: "uuid", nullable: true),
                    doctorid = table.Column<Guid>(type: "uuid", nullable: false),
                    notified = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspections", x => x.id);
                    table.ForeignKey(
                        name: "FK_Inspections_Doctors_doctorid",
                        column: x => x.doctorid,
                        principalTable: "Doctors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    createTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    inspectionid = table.Column<Guid>(type: "uuid", nullable: false),
                    specialityid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultations", x => x.id);
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
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Diagnoses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    icdDiagnosisid = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    createTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    inspectionid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnoses", x => x.id);
                    table.ForeignKey(
                        name: "FK_Diagnoses_Icd10s_icdDiagnosisid",
                        column: x => x.icdDiagnosisid,
                        principalTable: "Icd10s",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Diagnoses_Inspections_inspectionid",
                        column: x => x.inspectionid,
                        principalTable: "Inspections",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    createTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modifiedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    content = table.Column<string>(type: "text", nullable: false),
                    authorid = table.Column<Guid>(type: "uuid", nullable: false),
                    parentCommentid = table.Column<Guid>(type: "uuid", nullable: true),
                    consultationid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.id);
                    table.ForeignKey(
                        name: "FK_Comments_Comments_parentCommentid",
                        column: x => x.parentCommentid,
                        principalTable: "Comments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Comments_Consultations_consultationid",
                        column: x => x.consultationid,
                        principalTable: "Consultations",
                        principalColumn: "id",
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
                name: "IX_Comments_consultationid",
                table: "Comments",
                column: "consultationid");

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
                name: "IX_Diagnoses_icdDiagnosisid",
                table: "Diagnoses",
                column: "icdDiagnosisid");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnoses_inspectionid",
                table: "Diagnoses",
                column: "inspectionid");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_specialityid",
                table: "Doctors",
                column: "specialityid");

            migrationBuilder.CreateIndex(
                name: "IX_Icd10s_parentId",
                table: "Icd10s",
                column: "parentId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_doctorid",
                table: "Inspections",
                column: "doctorid");

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
                name: "Icd10s");

            migrationBuilder.DropTable(
                name: "Inspections");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Specialities");
        }
    }
}
