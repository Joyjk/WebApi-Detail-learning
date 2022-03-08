using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApi_test.Helpers;
using WebApi_test.Validations;

namespace WebApi_test.DTO
{
    public class MovieCreationDTO:MoviePatchDTO
    {
      
        //[Required]
        //[StringLength(300)]
        //public string Title { get; set; }
        //public string Summery { get; set; }
        //public bool InTheaters { get; set; }
        //public DateTime ReleaseDate { get; set; }
        [FileSizeValidator(4)]
        [ContentTypeValidator(ContentTypeValidator.ContentTypeGroup.Image)]
        public IFormFile Poster { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> GenresIDs { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder<List<ActorDTO>>))]
        public List<ActorDTO> Actors { get; set; }
    }
}
