using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi_test.DTO;
using WebApi_test.Helpers;

namespace WebApi_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public CustomBaseController(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        protected async Task<List<TDTO>> Get<TEntity, TDTO>() where TEntity: class
        {
            var entity = await dbContext.Set<TEntity>().AsNoTracking().ToListAsync();

            var dtos = mapper.Map<List<TDTO>>(entity);

            return dtos;

        }

        protected async Task<List<TDTO>> Get<TEntity, TDTO>(PaginationDTO paginationDTO) where TEntity : class
        {
            var queryable = dbContext.Set<TEntity>().AsNoTracking().AsQueryable();
             await HttpContext.InsertPaginationParametersInResponse(queryable, paginationDTO.RecordPerPage);
            var entities = await queryable.Paginate(paginationDTO).ToListAsync();
            return mapper.Map<List<TDTO>>(entities);
        }
    }
}
