using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApi_test.Validations;

namespace WebApi_test.Entities
{
    public class Genre: IValidatableObject
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="{0} is needed")]
        [StringLength(5)]
        //[FirstLetterUppercase]
        public string Name { get; set; }
        //[CreditCard]
        //public string CreditCard { get; set; }
        //[Url]
        //public string Url { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(!string.IsNullOrEmpty(Name))
            {
                var firstLetter = Name[0].ToString();
                if(firstLetter != firstLetter.ToUpper())
                {
                    yield return new ValidationResult("First letter should be upper case", new string[] { nameof(Name) });
                }
            }
        }
    }
}
