using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi_test.DTO;

namespace WebApi_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieTheatersController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext dbContext;

        public MovieTheatersController(IMapper mapper, ApplicationDbContext dbContext)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public async Task<ActionResult<List<MovieTheaterDTO>>> Get([FromQuery] FilterMovieTheatersDTO filterMovieTheatersDTO)
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            var usersLocation = geometryFactory
                .CreatePoint(new Coordinate(filterMovieTheatersDTO.Long,
                filterMovieTheatersDTO.Lat));

            var theaters = await dbContext
                .MovieTheaters.OrderBy(x => x.Location
                .Distance(usersLocation)).Where(x => x.Location
                .IsWithinDistance(usersLocation, filterMovieTheatersDTO.DistanceInKms * 1000))
                .Select(x => new MovieTheaterDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    DistanceInMeters = Math.Round(x.Location.Distance(usersLocation))
                }).ToListAsync();
              

            return theaters;
        }
    }
}
