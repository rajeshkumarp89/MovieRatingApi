using System;
using System.Collections.Generic;
using System.Text;

namespace MovieRatings.Context.Entities
{
    public class Genre : BaseEntity
    {
        public int Id { get; set; }
        public string GenreName { get; set; }
    }
}
