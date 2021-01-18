using System;
using System.Collections.Generic;
using System.Text;

namespace MovieRatings.Context.Entities
{
    public class UserMovieRating
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int YearOfRelease { get; set; }
        public int RunningTime { get; set; }
        public int GenresId { get; set; }
        public string Genres { get; set; }
        public double AverageRating { get; set; }
        public int UserId { get; set; }
    }
}
