﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VideoPlayer.Database.Repository.SQLite;

#nullable disable

namespace VideoPlayer.Database.Repository.SQLite.Migrations
{
    [DbContext(typeof(VideoPlayerContext))]
    [Migration("20230124200731_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.2");

            modelBuilder.Entity("VideoPlayer.Entities.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.Property<int?>("VideoId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("VideoId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("VideoPlayer.Entities.Thumbnail", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Image")
                        .HasColumnType("TEXT");

                    b.Property<int?>("VideoId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("VideoId");

                    b.ToTable("Thumbnails");
                });

            modelBuilder.Entity("VideoPlayer.Entities.Video", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("TEXT");

                    b.Property<string>("DateAddedString")
                        .HasColumnType("TEXT");

                    b.Property<string>("Directory")
                        .HasColumnType("TEXT");

                    b.Property<string>("FileName")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastPlayed")
                        .HasColumnType("TEXT");

                    b.Property<string>("LengthString")
                        .HasColumnType("TEXT");

                    b.Property<int>("NumberOfViews")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Preview")
                        .HasColumnType("TEXT");

                    b.Property<uint>("Rating")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Videos");
                });

            modelBuilder.Entity("VideoPlayer.Entities.Tag", b =>
                {
                    b.HasOne("VideoPlayer.Entities.Video", null)
                        .WithMany("Tags")
                        .HasForeignKey("VideoId");
                });

            modelBuilder.Entity("VideoPlayer.Entities.Thumbnail", b =>
                {
                    b.HasOne("VideoPlayer.Entities.Video", null)
                        .WithMany("Thumbnails")
                        .HasForeignKey("VideoId");
                });

            modelBuilder.Entity("VideoPlayer.Entities.Video", b =>
                {
                    b.Navigation("Tags");

                    b.Navigation("Thumbnails");
                });
#pragma warning restore 612, 618
        }
    }
}
