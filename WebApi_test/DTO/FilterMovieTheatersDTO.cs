using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace WebApi_test.DTO
{
    public class FilterMovieTheatersDTO
    {
        [BindRequired]
        [Range(-90,90)]
        public double Lat { get; set; }
        [BindRequired]
        [Range(-90, 90)]
        public double Long { get; set; }
        private int distanceInKms = 10;
        private int maxDistanceKms = 50;
        public int DistanceInKms
        {
            get { return distanceInKms; }
            set { distanceInKms = (value>maxDistanceKms)?maxDistanceKms:value; }
        }

    }
}
