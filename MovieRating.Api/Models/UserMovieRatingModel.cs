using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRatings.Api.Models
{
    public class UserMovieRatingModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public int YearOfRelease { get; set; }
        public int RunningTime { get; set; }
        public string Genres { get; set; }
        public double AverageRating { get; set; }
    }
}
