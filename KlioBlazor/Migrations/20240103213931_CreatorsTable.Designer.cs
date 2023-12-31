﻿// <auto-generated />
using System;
using KlioBlazor.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace KlioBlazor.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240103213931_CreatorsTable")]
    partial class CreatorsTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("KlioBlazor.Shared.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("KlioBlazor.Shared.Entities.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("IndependenceDay")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("KlioBlazor.Shared.Entities.Creator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomeUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("Creators");
                });

            modelBuilder.Entity("KlioBlazor.Shared.Entities.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("KlioBlazor.Shared.Entities.Keyword", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Word")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Keywords");
                });

            modelBuilder.Entity("KlioBlazor.Shared.Entities.Movie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AgeLimit")
                        .HasColumnType("int");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<bool>("HasTrailer")
                        .HasColumnType("bit");

                    b.Property<int>("PatitionId")
                        .HasColumnType("int");

                    b.Property<DateTime>("PublicDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ReleaseDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<bool>("ReleaseDateExact")
                        .HasColumnType("bit");

                    b.Property<string>("Summary")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ViewCounter")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PatitionId");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("KlioBlazor.Shared.Entities.MovieInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Info")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MovieId")
                        .HasColumnType("int");

                    b.Property<string>("Remark")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MovieId");

                    b.ToTable("MovieInfos");
                });

            modelBuilder.Entity("KlioBlazor.Shared.Entities.MoviesActors", b =>
                {
                    b.Property<int>("MovieId")
                        .HasColumnType("int");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.Property<string>("Character")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActor")
                        .HasColumnType("bit");

                    b.Property<bool>("IsTranslator")
                        .HasColumnType("bit");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.HasKey("MovieId", "PersonId");

                    b.HasIndex("PersonId");

                    b.ToTable("MoviesActors");
                });

            modelBuilder.Entity("KlioBlazor.Shared.Entities.MoviesCountries", b =>
                {
                    b.Property<int>("MovieId")
                        .HasColumnType("int");

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.HasKey("MovieId", "CountryId");

                    b.HasIndex("CountryId");

                    b.ToTable("MoviesCountries");
                });

            modelBuilder.Entity("KlioBlazor.Shared.Entities.MoviesCreators", b =>
                {
                    b.Property<int>("MovieId")
                        .HasColumnType("int");

                    b.Property<int>("CreatorID")
                        .HasColumnType("int");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.HasKey("MovieId", "CreatorID");

                    b.HasIndex("CreatorID");

                    b.ToTable("MoviesCreators");
                });

            modelBuilder.Entity("KlioBlazor.Shared.Entities.MoviesGenres", b =>
                {
                    b.Property<int>("MovieId")
                        .HasColumnType("int");

                    b.Property<int>("GenreId")
                        .HasColumnType("int");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.HasKey("MovieId", "GenreId");

                    b.HasIndex("GenreId");

                    b.ToTable("MoviesGenres");
                });

            modelBuilder.Entity("KlioBlazor.Shared.Entities.MoviesKeywords", b =>
                {
                    b.Property<int>("MovieId")
                        .HasColumnType("int");

                    b.Property<int>("KeywordId")
                        .HasColumnType("int");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.HasKey("MovieId", "KeywordId");

                    b.HasIndex("KeywordId");

                    b.ToTable("MoviesKeywords");
                });

            modelBuilder.Entity("KlioBlazor.Shared.Entities.Partition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Partitions");
                });

            modelBuilder.Entity("KlioBlazor.Shared.Entities.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Biography")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<bool>("DateOfBirthExact")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("DateOfDeath")
                        .HasColumnType("datetime2");

                    b.Property<bool>("DateOfDeathExact")
                        .HasColumnType("bit");

                    b.Property<bool>("IsFemale")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("People");
                });

            modelBuilder.Entity("KlioBlazor.Shared.Entities.Creator", b =>
                {
                    b.HasOne("KlioBlazor.Shared.Entities.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("KlioBlazor.Shared.Entities.Movie", b =>
                {
                    b.HasOne("KlioBlazor.Shared.Entities.Partition", "Partition")
                        .WithMany("Movies")
                        .HasForeignKey("PatitionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Partition");
                });

            modelBuilder.Entity("KlioBlazor.Shared.Entities.MovieInfo", b =>
                {
                    b.HasOne("KlioBlazor.Shared.Entities.Movie", "Movie")
                        .WithMany("MovieInfos")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("KlioBlazor.Shared.Entities.MoviesActors", b =>
                {
                    b.HasOne("KlioBlazor.Shared.Entities.Movie", "Movie")
                        .WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KlioBlazor.Shared.Entities.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Movie");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("KlioBlazor.Shared.Entities.MoviesCountries", b =>
                {
                    b.HasOne("KlioBlazor.Shared.Entities.Country", "Country")
                        .WithMany("MoviesCountries")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KlioBlazor.Shared.Entities.Movie", "Movie")
                        .WithMany("MoviesCountries")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("KlioBlazor.Shared.Entities.MoviesCreators", b =>
                {
                    b.HasOne("KlioBlazor.Shared.Entities.Creator", "Creator")
                        .WithMany("MoviesCreators")
                        .HasForeignKey("CreatorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KlioBlazor.Shared.Entities.Movie", "Movie")
                        .WithMany("MoviesCreators")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("KlioBlazor.Shared.Entities.MoviesGenres", b =>
                {
                    b.HasOne("KlioBlazor.Shared.Entities.Genre", "Genre")
                        .WithMany("MoviesGenres")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KlioBlazor.Shared.Entities.Movie", "Movie")
                        .WithMany("MoviesGenres")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genre");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("KlioBlazor.Shared.Entities.MoviesKeywords", b =>
                {
                    b.HasOne("KlioBlazor.Shared.Entities.Keyword", "Keyword")
                        .WithMany("MoviesKeywords")
                        .HasForeignKey("KeywordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KlioBlazor.Shared.Entities.Movie", "Movie")
                        .WithMany("MoviesKeywords")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Keyword");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("KlioBlazor.Shared.Entities.Partition", b =>
                {
                    b.HasOne("KlioBlazor.Shared.Entities.Category", "Category")
                        .WithMany("Partitions")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("KlioBlazor.Shared.Entities.Category", b =>
                {
                    b.Navigation("Partitions");
                });

            modelBuilder.Entity("KlioBlazor.Shared.Entities.Country", b =>
                {
                    b.Navigation("MoviesCountries");
                });

            modelBuilder.Entity("KlioBlazor.Shared.Entities.Creator", b =>
                {
                    b.Navigation("MoviesCreators");
                });

            modelBuilder.Entity("KlioBlazor.Shared.Entities.Genre", b =>
                {
                    b.Navigation("MoviesGenres");
                });

            modelBuilder.Entity("KlioBlazor.Shared.Entities.Keyword", b =>
                {
                    b.Navigation("MoviesKeywords");
                });

            modelBuilder.Entity("KlioBlazor.Shared.Entities.Movie", b =>
                {
                    b.Navigation("MovieInfos");

                    b.Navigation("MoviesCountries");

                    b.Navigation("MoviesCreators");

                    b.Navigation("MoviesGenres");

                    b.Navigation("MoviesKeywords");
                });

            modelBuilder.Entity("KlioBlazor.Shared.Entities.Partition", b =>
                {
                    b.Navigation("Movies");
                });
#pragma warning restore 612, 618
        }
    }
}
