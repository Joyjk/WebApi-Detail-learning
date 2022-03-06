using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi_test.DTO
{
    public class MovieDTO
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        public string Summery { get; set; }
        public bool InTheaters { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Poster { get; set; }
    }
}
