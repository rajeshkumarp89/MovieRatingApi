using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieRatings.Api.Models;
using MovieRatings.Common.Exceptions;
using MovieRatings.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRatings.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieRatingController : ControllerBase
    {
        private readonly IMovieRatingsDA movieRatingsDA;
        private readonly IMapper mapper;

        public MovieRatingController(IMovieRatingsDA movieRatingsDA, IMapper mapper)
        {
            this.movieRatingsDA = movieRatingsDA;
            this.mapper = mapper;
        }

        /// <summary>
        /// Gets list of User rating for Movies based on the filter
        /// </summary>
        /// <param name="movieSearch">Movie search filter</param>
        /// <returns>Matching User Movie ratings</returns>
        [HttpGet]
        public IActionResult MovieRatingSearch([FromQuery] MovieSearchModel movieSearch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Search criteria contains invalid value!");
                }

                if(string.IsNullOrEmpty(movieSearch.MovieName) && !movieSearch.Year.HasValue && (movieSearch.Genres == null || 
                    (movieSearch.Genres != null && movieSearch.Genres.Where(x => !string.IsNullOrEmpty(x)).Count() == 0)))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Atleast one search criteria is required!");
                }

                var result = mapper.Map<List<UserMovieRatingModel>>(movieRatingsDA.GetMovies(movieSearch.MovieName, movieSearch.Year, movieSearch.Genres));

                if(result == null || result.Count == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "No movie(s) found for the entered criteria");
                }
                return Ok(result);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Error occurred, please contact administrator!");
            }
        }
        /// <summary>
        /// Gets Top 5 Movies based on Total user average ratings
        /// </summary>
        /// <returns>Top 5 Movies based on Total user average ratings</returns>
        [Route("top")]
        [HttpGet]
        public IActionResult GetTopRatedMovies()
        {
            try
            {

                var result = mapper.Map<List<UserMovieRatingModel>>(movieRatingsDA.GetTopRatedMovies());

                if (result == null || result.Count == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "No movie(s) found for the entered criteria");
                }
                return Ok(result);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Error occurred, please contact administrator!");
            }
        }

        /// <summary>
        /// Gets Top 5 Movies based on the highest ratings given by a specific user
        /// </summary>
        /// <param name="userName">Username for which top 5 rated movies to be returned</param>
        /// <returns>Top 5 Movies based on specific user average rating</returns>
        [Route("top/{userName}")]
        [HttpGet]
        public IActionResult GetTopRatedMoviesByUser(string userName)
        {
            try
            {

                var result = mapper.Map<List<UserMovieRatingModel>>(movieRatingsDA.GetTopRatedMoviesByUser(userName));

                if (result == null || result.Count == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "No movie(s) found for the entered criteria");
                }
                return Ok(result);
            }
            catch(UserNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Error occurred, please contact administrator!");
            }
        }

        /// <summary>
        /// Adds or Updates User Rating
        /// </summary>
        /// <param name="movieRating">Movie Rating to be added or updated</param>
        /// <returns>OkResult if Success</returns>
        [HttpPost]
        public async Task<IActionResult> AddUserRatingAsync([FromBody]MovieRatingModel movieRating)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                var result = await movieRatingsDA.AddUserRatingAsync(movieRating.MovieName, movieRating.UserName, movieRating.Rating);
                if (result)
                {
                    return Ok();
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Internal Error occurred, please contact administrator!");
                }
            }
            catch (UserNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (MovieNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Error occurred, please contact administrator!");
            }
        }
    }
}
