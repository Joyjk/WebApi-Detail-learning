using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var movie = await dbContext.Movies.ToListAsync();
            return mapper.Map<List<MovieDTO>>(movie);
        }
        [HttpGet("{id}", Name = "getMovie")]
        public async Task<ActionResult<List<MovieDTO>>> GetByID(int id)
        {
            var movie =  await dbContext.Movies.FirstOrDefaultAsync(x=>x.Id == id); 
            if(movie == null)
            {
                return NotFound();
            }
            return mapper.Map<List<MovieDTO>>(movie);
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromForm]MovieCreationDTO movieCreationDTO)
        {
            var movie = mapper.Map<Movie>(movieCreationDTO);

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

            dbContext.Add(movie);
            await dbContext.SaveChangesAsync();
            var movieDTO = mapper.Map<MovieDTO>(movie);
            return new CreatedAtRouteResult("getMovie", new {id=movie.Id}, movieDTO);
        }

      
    }
}
