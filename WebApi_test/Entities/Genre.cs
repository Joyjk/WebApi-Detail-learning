using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApi_test.Validations;

namespace WebApi_test.Entities
{
    public class Genre 
    {
        [Key]
        public int Id { get; set; }
        ////[Required(ErrorMessage ="{0} is needed")]
        [Required]
        [StringLength(40)]
        [FirstLetterUppercase]
        public string Name { get; set; }





        //[CreditCard]
        //public string CreditCard { get; set; }
        //[Url]
        //public string Url { get; set; }


    }
}
