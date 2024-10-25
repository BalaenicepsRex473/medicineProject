﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using scrubsAPI;

#nullable disable

namespace scrubsAPI.Migrations
{
    [DbContext(typeof(ScrubsDbContext))]
    partial class ScrubsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

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

                    b.Property<Guid>("speciality")
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("Doctors");
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

            modelBuilder.Entity("scrubsAPI.Models.Icd10", b =>
                {
                    b.HasOne("scrubsAPI.Models.Icd10", "parent")
                        .WithMany()
                        .HasForeignKey("parentId");

                    b.Navigation("parent");
                });
#pragma warning restore 612, 618
        }
    }
}
