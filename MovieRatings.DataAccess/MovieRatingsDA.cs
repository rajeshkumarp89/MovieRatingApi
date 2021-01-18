using MovieRatings.Common.Exceptions;
using MovieRatings.Context;
using MovieRatings.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieRatings.DataAccess
{
    /// <summary>
    /// Class <c>MovieDataAccess</c> connects to Database for adding and querying User rating for Movies
    /// </summary>
    public class MovieRatingsDA : IMovieRatingsDA
    {
        private readonly MovieRatingContext context;

        /// <summary>
        /// Constructor for class MovieDataAccess
        /// </summary>
        /// <param name="_context">MovieRating DBContext</param>
        public MovieRatingsDA(MovieRatingContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Returns list of User rating for Movies based on the filter
        /// </summary>
        /// <param name="movieName">Filter parameter for Movie Name field</param>
        /// <param name="year">Filter parameter for Movie's Year of Release field</param>
        /// <param name="genres">Filter parameter for Movie Genre(s) type</param>
        /// <returns>Matching User Movie ratings</returns>
        public IEnumerable<UserMovieRating> GetMovies(string movieName, int? year, List<string> genres)
        {
            var query = GetUserMovieRatingQuery();
            
            if (!string.IsNullOrEmpty(movieName))
            {
                query = query.Where(x => x.Title.ToLower().Contains(movieName.ToLower()));
            }
            if (year > 0)
            {
                query = query.Where(x => x.YearOfRelease == year);
            }
            if (genres != null && genres.Count > 0)
            {
                var genreIds = context.Genres.Where(x => genres.Contains(x.GenreName)).Select(x => x.Id).ToList();
                query = query.Where(x => genreIds.Contains(x.GenresId));
            }

            return GroupByGenre(query.AsEnumerable());
        }

        /// <summary>
        /// Returns Top 5 Movies based on Total user average ratings
        /// </summary>
        /// <returns>Top 5 Movies based on Total user average ratings</returns>
        public IEnumerable<UserMovieRating> GetTopRatedMovies()
        {
            var query = GetUserMovieRatingQuery();
            var result = GroupByGenre(query.AsEnumerable());
            return result.OrderByDescending(x => x.AverageRating).ThenBy(x => x.Title).Take(5);
        }

        /// <summary>
        /// Returns Top 5 Movies based on the highest ratings given by a specific user
        /// </summary>
        /// <param name="userName">Username for which top 5 rated movies to be returned</param>
        /// <returns>Top 5 Movies based on specific user average rating</returns>
        public IEnumerable<UserMovieRating> GetTopRatedMoviesByUser(string userName)
        {
            var user = context.Users.AsQueryable();

            if (!user.Any(x => x.UserName == userName && x.IsActive == true))
            {
                throw new UserNotFoundException("Provided Username does not exists, please enter a valid Username");
            }
            int userId = user.Where(x => x.UserName == userName).Select(x => x.Id).Single();
            var query = GetUserMovieRatingQuery(userId);
            var result = GroupByGenre(query.AsEnumerable());
            return result.OrderByDescending(x => x.AverageRating).ThenBy(x => x.Title).Take(5);
        }

        /// <summary>
        /// Adds or Updates Movie rating for a user
        /// </summary>
        /// <param name="movieName">Movie name for which the rating to be added or updated</param>
        /// <param name="userName">Username who is adding or updating the raiting</param>
        /// <param name="rating">Rating provided by user</param>
        /// <returns>true if Success, false if Failure</returns>
        public async System.Threading.Tasks.Task<bool> AddUserRatingAsync(string movieName, string userName, int rating)
        {
            var movie = context.Movies.AsQueryable();
            var user = context.Users.AsQueryable();
            if (!user.Any(x => x.UserName == userName && x.IsActive == true))
            {
                throw new UserNotFoundException("Entered Username does not exists, please enter a valid Username");
            }
            if (!movie.Any(x => x.MovieName == movieName && x.IsActive == true))
            {
                throw new MovieNotFoundException("Entered Movie name does not exists, please enter a valid Movie name");
            }

            int movieId = movie.Where(x => x.MovieName == movieName).Select(x => x.Id).SingleOrDefault();
            int userId = user.Where(x => x.UserName == userName).Select(x => x.Id).SingleOrDefault();

            MovieRating movieRating = context.MovieRatings.Find(movieId, userId);
            if (movieRating == null)
            {
                context.MovieRatings.Add(new MovieRating { MovieId = movieId, UserId = userId, Rating = rating });
            }
            else
            {
                movieRating.Rating = rating;
                context.MovieRatings.Update(movieRating);
            }
            return await context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Returns IQueryable UserMovieRating object for querying DB
        /// </summary>
        /// <returns>Instance of IQueryable UserMovieRating</returns>
        private IQueryable<UserMovieRating> GetUserMovieRatingQuery()
        {
            var movie = context.Movies.AsQueryable();
            var moviegenre = context.MovieGenres.AsQueryable();
            var genre = context.Genres.AsQueryable();
            var rating = context.MovieRatings.AsQueryable();

            return (from m in movie
                    join mg in moviegenre on m.Id equals mg.MovieId
                    join g in genre on mg.GenreId equals g.Id
                    join r in rating on m.Id equals r.MovieId
                    where m.IsActive == true && mg.IsActive == true && g.IsActive == true && r.IsActive == true
                    group new { r.Rating } by new { m.Id, m.MovieName, m.YearOfRelease, m.RunningTimeMins, GenreId = g.Id, g.GenreName } into grp
                    select new UserMovieRating
                    {
                        Id = grp.Key.Id,
                        Title = grp.Key.MovieName,
                        YearOfRelease = grp.Key.YearOfRelease,
                        RunningTime = grp.Key.RunningTimeMins,
                        GenresId = grp.Key.GenreId,
                        Genres = grp.Key.GenreName,                        
                        AverageRating = grp.Average(x => x.Rating)
                    }).AsQueryable();
        }

        private IQueryable<UserMovieRating> GetUserMovieRatingQuery(int userId)
        {
            var movie = context.Movies.AsQueryable();
            var moviegenre = context.MovieGenres.AsQueryable();
            var genre = context.Genres.AsQueryable();
            var rating = context.MovieRatings.AsQueryable();

            return (from m in movie
                    join mg in moviegenre on m.Id equals mg.MovieId
                    join g in genre on mg.GenreId equals g.Id
                    join r in rating on m.Id equals r.MovieId
                    where r.UserId == userId && m.IsActive == true && mg.IsActive == true && g.IsActive == true && r.IsActive == true
                    group new { r.Rating } by new { m.Id, m.MovieName, m.YearOfRelease, m.RunningTimeMins, GenreId = g.Id, g.GenreName, r.UserId } into grp
                    select new UserMovieRating
                    {
                        Id = grp.Key.Id,
                        Title = grp.Key.MovieName,
                        YearOfRelease = grp.Key.YearOfRelease,
                        RunningTime = grp.Key.RunningTimeMins,
                        GenresId = grp.Key.GenreId,
                        Genres = grp.Key.GenreName,
                        UserId = grp.Key.UserId,
                        AverageRating = grp.Average(x => x.Rating)
                    }).AsQueryable();
        }

        private IEnumerable<UserMovieRating> GroupByGenre(IEnumerable<UserMovieRating> userMovieRatings)
        {
            return userMovieRatings.GroupBy(x => new { x.Id, x.Title, x.RunningTime, x.YearOfRelease, x.AverageRating })
                .Select(x => new UserMovieRating
                {
                    Id = x.Key.Id,
                    Title = x.Key.Title,
                    YearOfRelease = x.Key.YearOfRelease,
                    RunningTime = x.Key.RunningTime,
                    AverageRating = x.Key.AverageRating,
                    Genres = string.Join(",", x.Select(x => x.Genres))
                });
        }
    }
}
