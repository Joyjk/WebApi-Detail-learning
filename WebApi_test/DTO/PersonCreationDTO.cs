using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using WebApi_test.Validations;

namespace WebApi_test.DTO
{
    public class PersonCreationDTO
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        public string Biography { get; set; }
        public DateTime DateOfBirth { get; set; }

        [FileSizeValidator(4)]
        [ContentTypeValidator(ContentTypeValidator.ContentTypeGroup.Image)]
        public IFormFile Picture { get; set; }

    }
}
