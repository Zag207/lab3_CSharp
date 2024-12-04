﻿// <auto-generated />
using System;
using Lab3_CSharp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Lab3_CSharp.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20241127123744_Country in Philosophers is null")]
    partial class CountryinPhilosophersisnull
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Lab3_CSharp.Models.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CountryName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("Lab3_CSharp.Models.Philosopher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("Birth_date")
                        .HasColumnType("date");

                    b.Property<int>("CountryLivingId")
                        .HasColumnType("integer");

                    b.Property<DateOnly?>("Die_date")
                        .HasColumnType("date");

                    b.Property<bool>("IsDie")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CountryLivingId");

                    b.ToTable("Philosophers");
                });

            modelBuilder.Entity("Lab3_CSharp.Models.View", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ViewName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Views");
                });

            modelBuilder.Entity("Lab3_CSharp.Models.Work", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AuthorId")
                        .HasColumnType("integer");

                    b.Property<string>("WorkName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Works");
                });

            modelBuilder.Entity("PhilosopherView", b =>
                {
                    b.Property<int>("PhilosophersId")
                        .HasColumnType("integer");

                    b.Property<int>("ViewsId")
                        .HasColumnType("integer");

                    b.HasKey("PhilosophersId", "ViewsId");

                    b.HasIndex("ViewsId");

                    b.ToTable("PhilosopherView");
                });

            modelBuilder.Entity("Lab3_CSharp.Models.Philosopher", b =>
                {
                    b.HasOne("Lab3_CSharp.Models.Country", "CountryLiving")
                        .WithMany()
                        .HasForeignKey("CountryLivingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CountryLiving");
                });

            modelBuilder.Entity("Lab3_CSharp.Models.Work", b =>
                {
                    b.HasOne("Lab3_CSharp.Models.Philosopher", "Author")
                        .WithMany("Works")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("PhilosopherView", b =>
                {
                    b.HasOne("Lab3_CSharp.Models.Philosopher", null)
                        .WithMany()
                        .HasForeignKey("PhilosophersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Lab3_CSharp.Models.View", null)
                        .WithMany()
                        .HasForeignKey("ViewsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Lab3_CSharp.Models.Philosopher", b =>
                {
                    b.Navigation("Works");
                });
#pragma warning restore 612, 618
        }
    }
}
