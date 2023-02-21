﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TechnoEgypt.Models;

#nullable disable

namespace TechnoEgypt.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20230217140145_CreateCertificates")]
    partial class CreateCertificates
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TechnoEgypt.Models.ChildCVData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ChildId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("FileURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("stationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ChildId");

                    b.HasIndex("stationId");

                    b.ToTable("childCVData");
                });

            modelBuilder.Entity("TechnoEgypt.Models.ChildCertificate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ChildCourseId")
                        .HasColumnType("int");

                    b.Property<int>("ChildId")
                        .HasColumnType("int");

                    b.Property<string>("FileURL")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ChildCourseId");

                    b.HasIndex("ChildId");

                    b.ToTable("ChildCertificates");
                });

            modelBuilder.Entity("TechnoEgypt.Models.ChildCourse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ChildId")
                        .HasColumnType("int");

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ChildId");

                    b.HasIndex("CourseId");

                    b.ToTable("childCourses");
                });

            modelBuilder.Entity("TechnoEgypt.Models.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CourseCategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Descripttion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ToolId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CourseCategoryId");

                    b.HasIndex("ToolId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("TechnoEgypt.Models.CourseCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StageId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StageId");

                    b.ToTable("CourseCategories");
                });

            modelBuilder.Entity("TechnoEgypt.Models.CourseTool", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CourseToolsMyProperty");
                });

            modelBuilder.Entity("TechnoEgypt.Models.Parent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Parents");
                });

            modelBuilder.Entity("TechnoEgypt.Models.Stage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AgeFrom")
                        .HasColumnType("int");

                    b.Property<int>("AgeTo")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Stages");
                });

            modelBuilder.Entity("TechnoEgypt.Models.child", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ParentId")
                        .HasColumnType("int");

                    b.Property<string>("SchoolName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("children");
                });

            modelBuilder.Entity("TechnoEgypt.Models.station", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAvilable")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("station");
                });

            modelBuilder.Entity("TechnoEgypt.Models.ChildCVData", b =>
                {
                    b.HasOne("TechnoEgypt.Models.child", "Child")
                        .WithMany("ChildCVs")
                        .HasForeignKey("ChildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TechnoEgypt.Models.station", "station")
                        .WithMany("childCVDatas")
                        .HasForeignKey("stationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Child");

                    b.Navigation("station");
                });

            modelBuilder.Entity("TechnoEgypt.Models.ChildCertificate", b =>
                {
                    b.HasOne("TechnoEgypt.Models.ChildCourse", "ChildCourse")
                        .WithMany("ChildCertificates")
                        .HasForeignKey("ChildCourseId");

                    b.HasOne("TechnoEgypt.Models.child", "Child")
                        .WithMany("ChildCertificates")
                        .HasForeignKey("ChildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Child");

                    b.Navigation("ChildCourse");
                });

            modelBuilder.Entity("TechnoEgypt.Models.ChildCourse", b =>
                {
                    b.HasOne("TechnoEgypt.Models.child", "Child")
                        .WithMany("ChildCourses")
                        .HasForeignKey("ChildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TechnoEgypt.Models.Course", "Course")
                        .WithMany("ChildCourses")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Child");

                    b.Navigation("Course");
                });

            modelBuilder.Entity("TechnoEgypt.Models.Course", b =>
                {
                    b.HasOne("TechnoEgypt.Models.CourseCategory", "CourseCategory")
                        .WithMany("Courses")
                        .HasForeignKey("CourseCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TechnoEgypt.Models.CourseTool", "courseTool")
                        .WithMany("Courses")
                        .HasForeignKey("ToolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CourseCategory");

                    b.Navigation("courseTool");
                });

            modelBuilder.Entity("TechnoEgypt.Models.CourseCategory", b =>
                {
                    b.HasOne("TechnoEgypt.Models.Stage", "Stage")
                        .WithMany("CourseCategories")
                        .HasForeignKey("StageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Stage");
                });

            modelBuilder.Entity("TechnoEgypt.Models.child", b =>
                {
                    b.HasOne("TechnoEgypt.Models.Parent", "parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("parent");
                });

            modelBuilder.Entity("TechnoEgypt.Models.ChildCourse", b =>
                {
                    b.Navigation("ChildCertificates");
                });

            modelBuilder.Entity("TechnoEgypt.Models.Course", b =>
                {
                    b.Navigation("ChildCourses");
                });

            modelBuilder.Entity("TechnoEgypt.Models.CourseCategory", b =>
                {
                    b.Navigation("Courses");
                });

            modelBuilder.Entity("TechnoEgypt.Models.CourseTool", b =>
                {
                    b.Navigation("Courses");
                });

            modelBuilder.Entity("TechnoEgypt.Models.Parent", b =>
                {
                    b.Navigation("Children");
                });

            modelBuilder.Entity("TechnoEgypt.Models.Stage", b =>
                {
                    b.Navigation("CourseCategories");
                });

            modelBuilder.Entity("TechnoEgypt.Models.child", b =>
                {
                    b.Navigation("ChildCVs");

                    b.Navigation("ChildCertificates");

                    b.Navigation("ChildCourses");
                });

            modelBuilder.Entity("TechnoEgypt.Models.station", b =>
                {
                    b.Navigation("childCVDatas");
                });
#pragma warning restore 612, 618
        }
    }
}
