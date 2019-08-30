﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Olivia.Web.Models.Data;

namespace Olivia.Web.Migrations
{
    [DbContext(typeof(OliviaContext))]
    [Migration("20190830154235_CodeFirstAnnotations")]
    partial class CodeFirstAnnotations
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("Olivia.Web.Models.Data.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<DateTime>("Date");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.HasKey("Id");

                    b.HasIndex("Username");

                    b.ToTable("Post","sheeminc_olivia");
                });

            modelBuilder.Entity("Olivia.Web.Models.Data.User", b =>
                {
                    b.Property<string>("Username")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<byte>("IsEmailConfirmed");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Username");

                    b.ToTable("User","sheeminc_olivia");
                });

            modelBuilder.Entity("Olivia.Web.Models.Data.Post", b =>
                {
                    b.HasOne("Olivia.Web.Models.Data.User", "User")
                        .WithMany("Post")
                        .HasForeignKey("Username")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}