﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using scrubsAPI;

#nullable disable

namespace scrubsAPI.Migrations
{
    [DbContext(typeof(ScrubsDbContext))]
    [Migration("20241027172144_entities")]
    partial class entities
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("scrubsAPI.BannedToken", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("token")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("BannedTokens");
                });

            modelBuilder.Entity("scrubsAPI.Doctor", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("birthday")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("createTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("gender")
                        .HasColumnType("INTEGER");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("phone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("specialityid")
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.HasIndex("specialityid");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("scrubsAPI.Models.Comment", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("authorid")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("consultationId")
                        .HasColumnType("TEXT");

                    b.Property<string>("content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("createTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("modifiedTime")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("parentCommentid")
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.HasIndex("authorid");

                    b.HasIndex("consultationId");

                    b.HasIndex("parentCommentid");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("scrubsAPI.Models.Consultation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("createTime")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("inspectionid")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("specialityid")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("inspectionid");

                    b.HasIndex("specialityid");

                    b.ToTable("Consultations");
                });

            modelBuilder.Entity("scrubsAPI.Models.Diagnosis", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("createTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("icdDiagnosisId")
                        .HasColumnType("TEXT");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("type")
                        .HasColumnType("INTEGER");

                    b.HasKey("id");

                    b.ToTable("Diagnoses");
                });

            modelBuilder.Entity("scrubsAPI.Models.Icd10", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("createTime")
                        .HasColumnType("TEXT");

                    b.Property<int?>("idFromJson")
                        .HasColumnType("INTEGER");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("parentId")
                        .HasColumnType("TEXT");

                    b.Property<int?>("parentIdFromJson")
                        .HasColumnType("INTEGER");

                    b.HasKey("id");

                    b.HasIndex("parentId");

                    b.ToTable("Icd10s");
                });

            modelBuilder.Entity("scrubsAPI.Models.Inspection", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("anamesis")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("conclusion")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("date")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("deathTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("nextVisitDate")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("patientid")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("previousInspectionid")
                        .HasColumnType("TEXT");

                    b.Property<string>("treatment")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.HasIndex("patientid");

                    b.HasIndex("previousInspectionid");

                    b.ToTable("Inspections");
                });

            modelBuilder.Entity("scrubsAPI.Patient", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("birthDay")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("creationTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("gender")
                        .HasColumnType("INTEGER");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("scrubsAPI.Speciality", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("creationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("Specialities");
                });

            modelBuilder.Entity("scrubsAPI.Doctor", b =>
                {
                    b.HasOne("scrubsAPI.Speciality", "speciality")
                        .WithMany()
                        .HasForeignKey("specialityid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("speciality");
                });

            modelBuilder.Entity("scrubsAPI.Models.Comment", b =>
                {
                    b.HasOne("scrubsAPI.Doctor", "author")
                        .WithMany()
                        .HasForeignKey("authorid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("scrubsAPI.Models.Consultation", "consultation")
                        .WithMany()
                        .HasForeignKey("consultationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("scrubsAPI.Models.Comment", "parentComment")
                        .WithMany()
                        .HasForeignKey("parentCommentid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("author");

                    b.Navigation("consultation");

                    b.Navigation("parentComment");
                });

            modelBuilder.Entity("scrubsAPI.Models.Consultation", b =>
                {
                    b.HasOne("scrubsAPI.Models.Inspection", "inspection")
                        .WithMany()
                        .HasForeignKey("inspectionid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("scrubsAPI.Speciality", "speciality")
                        .WithMany()
                        .HasForeignKey("specialityid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("inspection");

                    b.Navigation("speciality");
                });

            modelBuilder.Entity("scrubsAPI.Models.Icd10", b =>
                {
                    b.HasOne("scrubsAPI.Models.Icd10", "parent")
                        .WithMany()
                        .HasForeignKey("parentId");

                    b.Navigation("parent");
                });

            modelBuilder.Entity("scrubsAPI.Models.Inspection", b =>
                {
                    b.HasOne("scrubsAPI.Patient", "patient")
                        .WithMany()
                        .HasForeignKey("patientid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("scrubsAPI.Models.Inspection", "previousInspection")
                        .WithMany()
                        .HasForeignKey("previousInspectionid");

                    b.Navigation("patient");

                    b.Navigation("previousInspection");
                });
#pragma warning restore 612, 618
        }
    }
}
