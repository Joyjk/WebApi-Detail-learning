using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using WebApi_test.Entities;

namespace WebApi_test
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
        }



        private void seedData(ModelBuilder modelBuilder)
        {
            var advan = new Genre() { Id = 4, Name = "Advanture0" };
            var animation = new Genre() { Id = 5, Name = "Animation" };
            var drama = new Genre() { Id = 6, Name = "Drama" };
            var romance = new Genre() { Id = 7, Name = "Romance" };
            modelBuilder.Entity<Genre>().HasData(new List<Genre>
            { advan,animation,drama,romance});

            var jimCarry = new Person() { Id = 8, Name = "Jim curry", DateOfBirth = new System.DateTime(1962, 02, 12) };
            var canndy = new Person() { Id = 9, Name = "candy canndy", DateOfBirth = new System.DateTime(1999, 04, 22) };
            var july = new Person() { Id = 10, Name = "july july", DateOfBirth = new System.DateTime(1998, 08, 24) };

            modelBuilder.Entity<Person>().HasData(new List<Person>() { jimCarry, canndy, july });


            var endGame = new Movie()
            {
                Id = 13,
                Title = "Avengers: Endgame",
                InTheaters = true,
                ReleaseDate = new System.DateTime(2020, 12, 12)
            };
            var infinityWar = new Movie()
            {
                Id = 14,
                Title = "Avengers: Infinity wars",
                InTheaters = false,
                ReleaseDate = new System.DateTime(2021, 02,25)
            };

            var sonic = new Movie()
            {
                Id = 17,
                Title = "Sonic",
                InTheaters = false,
                ReleaseDate = new System.DateTime(2019, 05, 15)
            };
            modelBuilder.Entity<Movie>().HasData(new List<Movie>() { infinityWar, sonic,endGame });

            modelBuilder.Entity<MoviesGenres>().HasData(new List<MoviesGenres>()
            {
                new MoviesGenres(){MovieId=endGame.Id,GenreId = drama.Id},
                new MoviesGenres(){MovieId=endGame.Id,GenreId = advan.Id},
                new MoviesGenres(){MovieId=infinityWar.Id,GenreId = drama.Id},
                new MoviesGenres(){MovieId=infinityWar.Id,GenreId = advan.Id},
                new MoviesGenres(){MovieId=sonic.Id,GenreId = advan.Id},

            });

            modelBuilder.Entity<MoviesActor>().HasData(
                new List<MoviesActor>()
                {
                    new MoviesActor(){MovieId=endGame.Id,PersonId=jimCarry.Id, Character="Tonny",Order=1},
                    new MoviesActor(){MovieId=infinityWar.Id,PersonId=canndy.Id, Character="Roger",Order=2},
                     new MoviesActor(){MovieId=infinityWar.Id,PersonId=jimCarry.Id, Character="Robot",Order=1},
                    new MoviesActor(){MovieId=sonic.Id,PersonId=july.Id, Character="Monny",Order=1},
                });
                
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MoviesGenres>().HasKey(x=>new {x.GenreId,x.MovieId});
            modelBuilder.Entity<MoviesActor>().HasKey(x => new { x.PersonId, x.MovieId });

            seedData(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Movie> Movies { get; set; }

        public DbSet<MoviesGenres> MoviesGenres { get; set; }
        public DbSet<MoviesActor> MoviesActors { get; set; }
    }
}
