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
    [Migration("20230420195724_ContactSheetSplit")]
    partial class ContactSheetSplit
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.2");

            modelBuilder.Entity("VideoPlayer.Entities.ContactSheet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Image")
                        .HasColumnType("TEXT");

                    b.Property<int>("VideoId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("VideoId")
                        .IsUnique();

                    b.ToTable("ContactSheets");
                });

            modelBuilder.Entity("VideoPlayer.Entities.Directory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("DirectoryName")
                        .HasColumnType("TEXT");

                    b.Property<string>("DirectoryPath")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsIncludeSubdirectories")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MediaType")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Directories");
                });

            modelBuilder.Entity("VideoPlayer.Entities.PlayListItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Order")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PlaylistId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("VideoId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PlaylistId");

                    b.HasIndex("VideoId");

                    b.ToTable("PlayListItem");
                });

            modelBuilder.Entity("VideoPlayer.Entities.Playlist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Playlists");
                });

            modelBuilder.Entity("VideoPlayer.Entities.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("VideoPlayer.Entities.TagVideo", b =>
                {
                    b.Property<int>("TagId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("VideoId")
                        .HasColumnType("INTEGER");

                    b.HasKey("TagId", "VideoId");

                    b.HasIndex("VideoId");

                    b.ToTable("TagVideo");
                });

            modelBuilder.Entity("VideoPlayer.Entities.Thumbnail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Image")
                        .HasColumnType("TEXT");

                    b.Property<int>("VideoId")
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

                    b.Property<int>("DirectoryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FileName")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastPlayed")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("Length")
                        .HasColumnType("TEXT");

                    b.Property<int>("NumberOfViews")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("Rating")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DirectoryId");

                    b.ToTable("Videos");
                });

            modelBuilder.Entity("VideoPlayer.Entities.ContactSheet", b =>
                {
                    b.HasOne("VideoPlayer.Entities.Video", "Video")
                        .WithOne("ContactSheet")
                        .HasForeignKey("VideoPlayer.Entities.ContactSheet", "VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Video");
                });

            modelBuilder.Entity("VideoPlayer.Entities.PlayListItem", b =>
                {
                    b.HasOne("VideoPlayer.Entities.Playlist", "Playlist")
                        .WithMany("Items")
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VideoPlayer.Entities.Video", "Video")
                        .WithMany()
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Playlist");

                    b.Navigation("Video");
                });

            modelBuilder.Entity("VideoPlayer.Entities.TagVideo", b =>
                {
                    b.HasOne("VideoPlayer.Entities.Tag", "Tag")
                        .WithMany("TagVideos")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VideoPlayer.Entities.Video", "Video")
                        .WithMany("TagVideos")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tag");

                    b.Navigation("Video");
                });

            modelBuilder.Entity("VideoPlayer.Entities.Thumbnail", b =>
                {
                    b.HasOne("VideoPlayer.Entities.Video", "Video")
                        .WithMany("Thumbnails")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Video");
                });

            modelBuilder.Entity("VideoPlayer.Entities.Video", b =>
                {
                    b.HasOne("VideoPlayer.Entities.Directory", "Directory")
                        .WithMany("Videos")
                        .HasForeignKey("DirectoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Directory");
                });

            modelBuilder.Entity("VideoPlayer.Entities.Directory", b =>
                {
                    b.Navigation("Videos");
                });

            modelBuilder.Entity("VideoPlayer.Entities.Playlist", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("VideoPlayer.Entities.Tag", b =>
                {
                    b.Navigation("TagVideos");
                });

            modelBuilder.Entity("VideoPlayer.Entities.Video", b =>
                {
                    b.Navigation("ContactSheet");

                    b.Navigation("TagVideos");

                    b.Navigation("Thumbnails");
                });
#pragma warning restore 612, 618
        }
    }
}