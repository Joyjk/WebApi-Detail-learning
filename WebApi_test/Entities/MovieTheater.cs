using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace WebApi_test.Entities
{
    public class MovieTheater
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public Point Location { get; set; }

    }
}
