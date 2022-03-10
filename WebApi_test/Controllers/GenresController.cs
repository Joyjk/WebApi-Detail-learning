using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi_test.DTO;
using WebApi_test.Entities;
using WebApi_test.Helpers;
using WebApi_test.Services;

namespace WebApi_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : CustomBaseController
    {
       // private readonly IRepository repository;
        private readonly ILogger<GenresController> logger;
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public GenresController( ILogger<GenresController> logger, ApplicationDbContext dbContext,
            IMapper mapper):base(dbContext,mapper)
        {
            //this.repository = repository;
            this.logger = logger;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet(Name ="getGenres")]
       // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
       [EnableCors( PolicyName = "AllowAPIRequestIO")]
        public async Task<ActionResult<List<GenreDTO>>> Get(bool includeHatos = true)
        {
            //  logger.LogInformation("Getting all genres");

            // var genre = await dbContext.Genres.AsNoTracking().ToListAsync();

            //  var genreDTOs = mapper.Map<List<GenreDTO>>(genre);

            //if(includeHatos)
            //{
            //    var resourceCollection = new ResourceCollection<GenreDTO>(genreDTOs);

            //    genreDTOs.ForEach(genre => GenrateLinks(genre));

            //    resourceCollection.Links.Add(new Link(Url.Link("getGenres", new { }), "Self", "GET"));
            //    resourceCollection.Links.Add(new Link(Url.Link("createGenre", new { }), "create-genre", "POST"));

            //    return Ok(resourceCollection);

            //}
            //return Ok(genreDTOs);

            return await Get<Genre, GenreDTO>();
            
        }

        private void  GenrateLinks(GenreDTO genreDTO)
        {
            genreDTO.Links.Add(new Link(Url.Link
                ("getGenre", new { Id = genreDTO.Id }), "get-Genre", "GET"));
            genreDTO.Links.Add(new Link(Url.Link
                ("putGenre", new { Id = genreDTO.Id }), "put-Genre", "PUT"));
            genreDTO.Links.Add(new Link(Url.Link
                ("deleteGenre", new { Id = genreDTO.Id }), "delete-Genre", "DELETE"));
        }

        //[HttpGet("{id}")]
        [HttpGet("{id:int}", Name ="getGenre")]
        //[ServiceFilter(typeof(GenreHATEOASAttribute))]
        public async Task< IActionResult> Get(int id,bool includeHatos=true)
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

            if (includeHatos)
            {
                GenrateLinks(genreDTO);
            }



            return Ok(genreDTO);
        }

        [HttpPost(Name = "createGenre")]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme,Roles ="Admin")]
        public async Task <ActionResult> Post([FromBody]GenreCreatingDTO genreCreation)
        {
            
            var genre= mapper.Map<Genre>(genreCreation);

            dbContext.Genres.Add(genre);
           await dbContext.SaveChangesAsync();

            var genreDTO = mapper.Map<GenreDTO>(genre);

            return new CreatedAtRouteResult("getGenre", new { genreDTO.Id},genreDTO);
        }
        [HttpPut("{id}",Name ="putGenre")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task <ActionResult> Put(int id, [FromBody]GenreCreatingDTO genreUpdateDTO)
        {
            var genre = mapper.Map<Genre>(genreUpdateDTO);
            genre.Id = id;
            dbContext.Entry(genre).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}", Name ="deleteGenre")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task< ActionResult> Delete(int id)
        {
            var exists =  await dbContext.Genres.AnyAsync(x => x.Id == id);
            if (!exists)
            {
                return NotFound();
            }
            dbContext.Remove(new Genre { Id = id });
            await dbContext.SaveChangesAsync();


            return NoContent();
        }

    }
}
