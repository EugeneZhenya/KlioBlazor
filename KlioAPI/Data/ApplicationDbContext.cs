using KlioBlazor.Shared.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KlioAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasMany(e => e.Partitions).WithOne(e => e.Category).HasForeignKey(e => e.CategoryId).IsRequired();
            modelBuilder.Entity<Movie>().HasMany(e => e.MovieInfos).WithOne(e => e.Movie).HasForeignKey(e => e.MovieId).IsRequired();
            modelBuilder.Entity<Partition>().HasMany(e => e.Movies).WithOne(e => e.Partition).HasForeignKey(e => e.PatitionId).IsRequired();
            modelBuilder.Entity<MoviesActors>().HasKey(x => new { x.MovieId, x.PersonId });
            modelBuilder.Entity<MoviesCountries>().HasKey(x => new { x.MovieId, x.CountryId });
            modelBuilder.Entity<MoviesCreators>().HasKey(x => new { x.MovieId, x.CreatorID });
            modelBuilder.Entity<MoviesGenres>().HasKey(x => new { x.MovieId, x.GenreId });
            modelBuilder.Entity<MoviesKeywords>().HasKey(x => new { x.MovieId, x.KeywordId });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Creator> Creators { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieInfo> MovieInfos { get; set; }
        public DbSet<MoviesActors> MoviesActors { get; set; }
        public DbSet<MoviesCountries> MoviesCountries { get; set; }
        public DbSet<MoviesCreators> MoviesCreators { get; set; }
        public DbSet<MoviesGenres> MoviesGenres { get; set; }
        public DbSet<MoviesKeywords> MoviesKeywords { get; set; }
        public DbSet<Partition> Partitions { get; set; }
        public DbSet<Person> People { get; set; }
    }
}
