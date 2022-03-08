using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApi_test.DTO;
using WebApi_test.Entities;
using WebApi_test.Services;

namespace WebApi_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IFileStorageService fileStorageService;
        private readonly string containerName = "movies";

        public MoviesController(ApplicationDbContext dbContext, IMapper mapper,
            IFileStorageService fileStorageService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.fileStorageService = fileStorageService;
        }

        [HttpGet]
        public async Task<ActionResult<List<MovieDTO>>> Get()
        {
            var top = 5;
            var today = DateTime.Today;
            var upcomingReleases = await dbContext.Movies.Where(x => x.ReleaseDate > today)
                .OrderBy(x => x.ReleaseDate).Take(top).ToListAsync();

            var movie = await dbContext.Movies.ToListAsync();
            return mapper.Map<List<MovieDTO>>(movie);
        }
        [HttpGet("{id}", Name = "getMovie")]
        public async Task<ActionResult<List<MovieDTO>>> GetByID(int id)
        {
            var movie = await dbContext.Movies.FirstOrDefaultAsync(x => x.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            return mapper.Map<List<MovieDTO>>(movie);
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] MovieCreationDTO movieCreationDTO)
        {
            var movie = mapper.Map<Movie>(movieCreationDTO);

            //return Ok();

            if (movieCreationDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await movieCreationDTO.Poster.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(movieCreationDTO.Poster.FileName);
                    movie.Poster = await fileStorageService.SaveFile(content, extension, containerName, movieCreationDTO.Poster.ContentType);
                }
            }
            AnnotateActorsOrder(movie);
            dbContext.Add(movie);
            await dbContext.SaveChangesAsync();
            var movieDTO = mapper.Map<MovieDTO>(movie);
            return new CreatedAtRouteResult("getMovie", new { id = movie.Id }, movieDTO);
        }
        private static void AnnotateActorsOrder(Movie movie)
        {
            if(movie.MoviesActors!=null)
            {
                for(int i = 0; i < movie.MoviesActors.Count; i++)
                {
                    movie.MoviesActors[i].Order = i;
                }
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] MovieCreationDTO movieCreationDTO)
        {
            var movieDb = await dbContext.Movies.FirstOrDefaultAsync(x => x.Id == id);
            if (movieDb == null)
            {
                return NotFound();
            }

            movieDb = mapper.Map(movieCreationDTO, movieDb);

            if (movieCreationDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await movieCreationDTO.Poster.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(movieCreationDTO.Poster.FileName);
                    movieDb.Poster = await fileStorageService.SaveFile(content, extension, containerName, movieCreationDTO.Poster.ContentType);
                }
            }

            await dbContext.Database.ExecuteSqlInterpolatedAsync($"delete from MoviesActors where MovieId = {movieDb.Id}; delete from MoviesGenres where MovieId ={movieDb.Id}" );
            AnnotateActorsOrder(movieDb);


            await dbContext.SaveChangesAsync();
            return NoContent();

        }
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<MoviePatchDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }
            var entityFromDb = await dbContext.Movies.FirstOrDefaultAsync(x => x.Id == id);
            if (entityFromDb == null)
            {
                return NotFound();
            }
            var entityDTO = mapper.Map<MoviePatchDTO>(entityFromDb);
            patchDocument.ApplyTo(entityDTO, (Microsoft.AspNetCore.JsonPatch.Adapters.IObjectAdapter)ModelState);
            var isValid = TryValidateModel(entityDTO);
            if (!isValid)
            {
                return BadRequest(ModelState);
            }
            mapper.Map(entityDTO, entityFromDb);

            await dbContext.SaveChangesAsync();

            return NoContent();

        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await dbContext.Movies.AnyAsync(x => x.Id == id);
            if(!exists)
            {
                return NotFound();
            }
            dbContext.Remove(new Movie() { Id = id });
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

      
    }
}
