﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RealWorldWebAPI.Data.Models;

namespace RealWorldWebAPI.Migrations
{
    [DbContext(typeof(RealWorldDbContext))]
    partial class RealWorldDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2");

            modelBuilder.Entity("RealWorldWebAPI.Data.Models.Article", b =>
                {
                    b.Property<Guid>("ArticleUID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("AuthorUserUID")
                        .HasColumnType("TEXT");

                    b.Property<string>("Body")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Favorited")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FavoritesCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Slug")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("ArticleUID");

                    b.HasIndex("AuthorUserUID");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("RealWorldWebAPI.Data.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("ArticleUID")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("AuthorUserUID")
                        .HasColumnType("TEXT");

                    b.Property<string>("Body")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ArticleUID");

                    b.HasIndex("AuthorUserUID");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("RealWorldWebAPI.Data.Models.Profile", b =>
                {
                    b.Property<Guid>("ProfileUID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Bio")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Following")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Image")
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .HasColumnType("TEXT");

                    b.HasKey("ProfileUID");

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("RealWorldWebAPI.Data.Models.Tag", b =>
                {
                    b.Property<Guid>("TagUID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ArticleUID")
                        .HasColumnType("TEXT");

                    b.HasKey("TagUID");

                    b.HasIndex("ArticleUID");

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("RealWorldWebAPI.Data.Models.User", b =>
                {
                    b.Property<Guid>("UserUID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Bio")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("Image")
                        .HasColumnType("TEXT");

                    b.Property<string>("Token")
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .HasColumnType("TEXT");

                    b.HasKey("UserUID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RealWorldWebAPI.Data.Models.Article", b =>
                {
                    b.HasOne("RealWorldWebAPI.Data.Models.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorUserUID");
                });

            modelBuilder.Entity("RealWorldWebAPI.Data.Models.Comment", b =>
                {
                    b.HasOne("RealWorldWebAPI.Data.Models.Article", null)
                        .WithMany("Comments")
                        .HasForeignKey("ArticleUID");

                    b.HasOne("RealWorldWebAPI.Data.Models.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorUserUID");
                });

            modelBuilder.Entity("RealWorldWebAPI.Data.Models.Tag", b =>
                {
                    b.HasOne("RealWorldWebAPI.Data.Models.Article", null)
                        .WithMany("TagList")
                        .HasForeignKey("ArticleUID");
                });
#pragma warning restore 612, 618
        }
    }
}