using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRatings.Api.Models
{
    public class MovieRatingModel
    {
        [Required(ErrorMessage ="Movie name is required")]
        public string MovieName { get; set; }
        [Required(ErrorMessage ="Username is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage ="Rating is required")]
        [Range(1,5, ErrorMessage = "Movie Rating should be between 1 and 5")]
        public int Rating { get; set; }
    }
}
