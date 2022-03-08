using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi_test.DTO
{
    public class MoviePatchDTO
    {
        [Required]
        [StringLength(300)]
        public string Title { get; set; }
        public string Summery { get; set; }
        public bool InTheaters { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
