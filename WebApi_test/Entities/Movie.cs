using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi_test.Entities
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public string Title { get; set; }
        public string Summery { get; set; }
        public bool InTheaters { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Poster { get; set; }
        public List<MoviesActor> MoviesActors { get; set; }
        public List<MoviesGenres> MoviesGenres { get; set; }
    }
}
