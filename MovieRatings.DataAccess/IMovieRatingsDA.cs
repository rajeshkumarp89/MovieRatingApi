using MovieRatings.Context.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieRatings.DataAccess
{
    public interface IMovieRatingsDA
    {
        IEnumerable<UserMovieRating> GetMovies(string movieName, int? year, List<string> genres);
        IEnumerable<UserMovieRating> GetTopRatedMovies();
        IEnumerable<UserMovieRating> GetTopRatedMoviesByUser(string userName);
        Task<bool> AddUserRatingAsync(string movieName, string userName, int rating);
    }
}
