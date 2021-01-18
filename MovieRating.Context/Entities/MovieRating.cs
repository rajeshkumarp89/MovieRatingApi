using System;
using System.Collections.Generic;
using System.Text;

namespace MovieRatings.Context.Entities
{
    public class MovieRating : BaseEntity
    {
        public int MovieId { get; set; }
        public int UserId { get; set; }        
        public int Rating { get; set; }
        public Movie Movie { get; set; }
        public User User { get; set; }
    }
}
