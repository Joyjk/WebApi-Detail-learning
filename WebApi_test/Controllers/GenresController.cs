using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<GenresController> logger;

        public GenresController(IRepository repository, ILogger<GenresController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult< List<Genre>>> Get()
        {
            logger.LogInformation("Getting all genres");
            return await repository.getAllGenre();
        }



        //[HttpGet("{id}")]
        [HttpGet("{id:int}", Name ="getGenre")]
        public IActionResult Get(int id,[FromHeader] string param2)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            var genre = repository.getGenreByID(id);

            if(genre == null)
            {
                logger.LogWarning($"Genre ID: {id} not found");
                return NotFound();
            }
            logger.LogDebug("Debug is opening.....");
            return Ok(genre);
        }

        [HttpPost]
        public ActionResult Post([FromBody]Genre genre)
        {
            repository.AddGenre(genre);

            return new CreatedAtRouteResult("getGenre", new { Id = genre.Id},genre);
        }
        [HttpPut]
        public ActionResult Put([FromBody]Genre genre)
        {
            return NoContent();
        }
        public ActionResult Delete()
        {
            return NoContent();
        }

    }
}
