using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using WebApi_test.Validations;

namespace WebApi_test.DTO
{
    public class MovieCreationDTO
    {
      
        [Required]
        [StringLength(300)]
        public string Title { get; set; }
        public string Summery { get; set; }
        public bool InTheaters { get; set; }
        public DateTime ReleaseDate { get; set; }
        [FileSizeValidator(4)]
        [ContentTypeValidator(ContentTypeValidator.ContentTypeGroup.Image)]
        public IFormFile Poster { get; set; }
    }
}
