using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi_test.Entities;
using WebApi_test.Services;

namespace WebApi_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IRepository repository;

        public GenresController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult< List<Genre>>> Get()
        {
            return await repository.getAllGenre();
        }
        //[HttpGet("{id}")]
        [HttpGet("{id:int}/{param2=fff}")]
        public IActionResult Get(int id,[BindRequired] string param2)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var genre = repository.getGenreByID(id);

            if(genre == null)
            {
                return NotFound();
            }
            return Ok(genre);
        }

        public ActionResult Post()
        {
            return NoContent();
        }

        public ActionResult Put()
        {
            return NoContent();
        }
        public ActionResult Delete()
        {
            return NoContent();
        }

    }
}
