using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WebApi_test.DTO;
using WebApi_test.Entities;
using WebApi_test.Helpers;
using WebApi_test.Services;

namespace WebApi_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IFileStorageService fileStorageService;
        private readonly string containerName = "people";

        public PeopleController(ApplicationDbContext dbContext, IMapper mapper,
            IFileStorageService fileStorageService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.fileStorageService = fileStorageService;
        }

        [HttpGet]
        public async Task <ActionResult<List<PersonDTO>>> Get([FromQuery] PaginationDTO pagination)
        {
            var queryable = dbContext.People.AsQueryable();
            await HttpContext.InsertPaginationParametersInResponse(queryable, pagination.RecordPerPage);
            var people = await queryable.Paginate(pagination).ToListAsync();
            return mapper.Map<List<PersonDTO>>(people);
        }
        [HttpGet("{id}", Name ="getPerson")]
        public async Task<ActionResult<PersonDTO>> Get(int id)
        {
            var person = await dbContext.People.FirstOrDefaultAsync(x => x.Id == id);

            if (person==null)
            {
                return NotFound();
            }

            return  mapper.Map<PersonDTO>(person);
        }
        
         [HttpPost]
         public async Task<ActionResult<PersonDTO>> Post([FromForm] PersonCreationDTO personCreationDTO)
         {
            var person = mapper.Map<Person>(personCreationDTO);

            if(personCreationDTO.Picture!=null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await personCreationDTO.Picture.CopyToAsync(memoryStream);
                    var content  = memoryStream.ToArray();
                    var extension = Path.GetExtension(personCreationDTO.Picture.FileName);
                    person.Picture = await fileStorageService.SaveFile(content, extension, containerName, personCreationDTO.Picture.ContentType);
                }
            }

            dbContext.Add(person);
            await dbContext.SaveChangesAsync();
            var personDTO = mapper.Map<PersonDTO>(person);

            return new CreatedAtRouteResult("getPerson", new { id=person.Id }, personDTO);
         }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] PersonCreationDTO personUpdateDTO)
        {
            var personDB = await dbContext.People.FirstOrDefaultAsync(x => x.Id == id);
            if(personDB==null)
            {
                return NotFound();
            }

            personDB = mapper.Map(personUpdateDTO, personDB);


            if (personUpdateDTO.Picture != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await personUpdateDTO.Picture.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(personUpdateDTO.Picture.FileName);
                    personDB.Picture = 
                        await fileStorageService.SaveFile(content, extension, containerName,
                         personUpdateDTO.Picture.ContentType);
                }
            }
            await dbContext.SaveChangesAsync();

            return NoContent();

        }



        
    }
}
