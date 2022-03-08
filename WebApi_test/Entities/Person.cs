using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi_test.Entities
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        public string Biography { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Picture { get; set; }


        public List<MoviesActor> MoviesActors { get; set; }
    }
}
