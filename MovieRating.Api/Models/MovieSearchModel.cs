using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRatings.Api.Models
{
    public class MovieSearchModel
    {
        public string MovieName { get; set; }
        [ValidateYear(ErrorMessage ="Year must be greater than 1950 and less or equal to current year")]
        public int? Year { get; set; }
        public List<string> Genres { get; set; }
    }
}
