using System.Collections.Generic;

namespace WebApi_test.DTO
{
    public class IndexMoviePageDTO
    {
        public List<MovieDTO> UpcomingRealeses { get; set; }
        public List<MovieDTO> InTheaters { get; set; }

    }
}
