using System.ComponentModel.DataAnnotations;

namespace WebApi_test.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="{0} is needed")]
        [StringLength(5)]
        public string Name { get; set; }
    }
}
