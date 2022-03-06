using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi_test.DTO;
using WebApi_test.Entities;

namespace WebApi_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public PeopleController(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task <ActionResult<List<PersonDTO>>> Get()
        {
            var people = await dbContext.People.ToListAsync();
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
            dbContext.Add(person);
            await dbContext.SaveChangesAsync();
            var personDTO = mapper.Map<PersonDTO>(person);

            return new CreatedAtRouteResult("getPerson", new { id=person.Id }, personDTO);
        }



        
    }
}
