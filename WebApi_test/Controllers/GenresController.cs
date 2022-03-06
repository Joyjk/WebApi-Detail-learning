using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi_test.DTO;
using WebApi_test.Entities;
using WebApi_test.Services;

namespace WebApi_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
       // private readonly IRepository repository;
        private readonly ILogger<GenresController> logger;
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public GenresController( ILogger<GenresController> logger, ApplicationDbContext dbContext,
            IMapper mapper)
        {
            //this.repository = repository;
            this.logger = logger;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult< List<GenreDTO>>> Get()
        {
            //  logger.LogInformation("Getting all genres");

            var genre = await dbContext.Genres.AsNoTracking().ToListAsync();

            var genreDTOs = mapper.Map<List<GenreDTO>>(genre);


            return genreDTOs;
        }



        //[HttpGet("{id}")]
        [HttpGet("{id:int}", Name ="getGenre")]
        public async Task< IActionResult> Get(int id)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            var genre = await dbContext.Genres.FirstOrDefaultAsync(x => x.Id == id);

            if(genre == null)
            {
               // logger.LogWarning($"Genre ID: {id} not found");
                return NotFound();
            }
           // logger.LogDebug("Debug is opening.....");

            var genreDTO =  mapper.Map<GenreDTO>(genre);

            return Ok(genreDTO);
        }

        [HttpPost]
        public async Task <ActionResult> Post([FromBody]GenreCreatingDTO genreCreation)
        {
            
            var genre= mapper.Map<Genre>(genreCreation);

            dbContext.Genres.Add(genre);
           await dbContext.SaveChangesAsync();

            var genreDTO = mapper.Map<GenreDTO>(genre);

            return new CreatedAtRouteResult("getGenre", new { genreDTO.Id},genreDTO);
        }
        [HttpPut("{id}")]
        public async Task <ActionResult> Put(int id, [FromBody]GenreCreatingDTO genreUpdateDTO)
        {
            var genre = mapper.Map<Genre>(genreUpdateDTO);
            genre.Id = id;
            dbContext.Entry(genre).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
        public ActionResult Delete()
        {
            return NoContent();
        }

    }
}
