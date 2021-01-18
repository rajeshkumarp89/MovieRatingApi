using System;
using System.Collections.Generic;
using System.Text;

namespace MovieRatings.Context.Entities
{
    public class Movie : BaseEntity
    {
        public int Id { get; set; }
        public string MovieName { get; set; }
        public int YearOfRelease { get; set; }
        public int RunningTimeMins { get; set; }        
    }
}
