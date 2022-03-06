using System.ComponentModel.DataAnnotations;
using WebApi_test.Validations;

namespace WebApi_test.DTO
{
    public class GenreCreatingDTO
    {
        [Required]
        [StringLength(40)]
        [FirstLetterUppercase]
        public string Name { get; set; }
    }
}
