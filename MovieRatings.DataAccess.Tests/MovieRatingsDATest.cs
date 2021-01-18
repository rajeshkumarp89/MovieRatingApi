using Microsoft.EntityFrameworkCore;
using Moq;
using MovieRatings.Common.Exceptions;
using MovieRatings.Context;
using MovieRatings.Context.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRatings.DataAccess.Tests
{
    public class MovieRatingsDATest
    {
        private List<Movie> Movies;
        private List<User> Users;
        private List<Genre> Genres;
        private List<MovieGenre> MovieGenres;
        private List<MovieRating> MovieRatings;
        private DbContextOptions<MovieRatingContext> options;
        public MovieRatingsDATest()
        {
            Users = new List<User>
            {
                new User { Id = 1, UserName = "Bob", IsActive = true },
                new User { Id = 2, UserName = "John", IsActive = true },
                new User { Id = 3, UserName = "Alex", IsActive = true },
                new User { Id = 4, UserName = "Smith", IsActive = true },
                new User { Id = 5, UserName = "Raj", IsActive = true },
            };
            Movies = new List<Movie>()
            {
                new Movie {  Id = 1, MovieName = "The Gentlemen", YearOfRelease = 2019, IsActive = true},
                new Movie {  Id = 2, MovieName = "Parasite", YearOfRelease = 2019, IsActive = true},
                new Movie {  Id = 3, MovieName = "Kingsman: The Golden Circle", YearOfRelease = 2017, IsActive = true},
                new Movie {  Id = 4, MovieName = "Avengers: Endgame", YearOfRelease = 2019, IsActive = true},
                new Movie {  Id = 5, MovieName = "Interstellar", YearOfRelease = 2020, IsActive = true},
                new Movie {  Id = 6, MovieName = "The Matrix", YearOfRelease = 1999, IsActive = true},
                new Movie {  Id = 7, MovieName = "Guardians of the Galaxy", YearOfRelease = 2014, IsActive = true},
                new Movie {  Id = 8, MovieName = "Back to the Future", YearOfRelease = 1985, IsActive = true},
                new Movie {  Id = 9, MovieName = "The Prestige", YearOfRelease = 2006, IsActive = true},
                new Movie {  Id = 10, MovieName = "Wonder Woman 1984", YearOfRelease = 2020, IsActive = true},
                new Movie {  Id = 11, MovieName = "Tenet", YearOfRelease = 2020, IsActive = true},
                new Movie {  Id = 12, MovieName = "Inception", YearOfRelease = 2020, IsActive = true},
                new Movie {  Id = 13, MovieName = "The Dark Knight", YearOfRelease = 2008, IsActive = true}
            };
            Genres = new List<Genre>
            {
                new Genre { Id = 1, GenreName = "Action", IsActive = true },
                new Genre { Id = 2, GenreName = "Comedy", IsActive = true },
                new Genre { Id = 3, GenreName = "Sci-Fi", IsActive = true }
            };
            MovieRatings = new List<MovieRating>
            {
                new MovieRating { MovieId = 1, UserId = 1, Rating = 2, IsActive = true },                
                new MovieRating { MovieId = 1, UserId = 3, Rating = 1, IsActive = true },
                new MovieRating { MovieId = 1, UserId = 4, Rating = 5, IsActive = true },
                new MovieRating { MovieId = 1, UserId = 5, Rating = 3, IsActive = true },
                new MovieRating { MovieId = 2, UserId = 1, Rating = 4, IsActive = true },                
                new MovieRating { MovieId = 2, UserId = 4, Rating = 5, IsActive = true },
                new MovieRating { MovieId = 2, UserId = 5, Rating = 3, IsActive = true },
                new MovieRating { MovieId = 3, UserId = 1, Rating = 2, IsActive = true },                
                new MovieRating { MovieId = 3, UserId = 4, Rating = 1, IsActive = true },
                new MovieRating { MovieId = 3, UserId = 5, Rating = 3, IsActive = true },
                new MovieRating { MovieId = 4, UserId = 1, Rating = 5, IsActive = true },
                new MovieRating { MovieId = 4, UserId = 3, Rating = 3, IsActive = true },
                new MovieRating { MovieId = 4, UserId = 4, Rating = 2, IsActive = true },
                new MovieRating { MovieId = 4, UserId = 5, Rating = 4, IsActive = true },
                new MovieRating { MovieId = 5, UserId = 1, Rating = 3, IsActive = true },
                new MovieRating { MovieId = 5, UserId = 4, Rating = 5, IsActive = true },
                new MovieRating { MovieId = 5, UserId = 5, Rating = 4, IsActive = true },
                new MovieRating { MovieId = 6, UserId = 1, Rating = 3, IsActive = true },
                new MovieRating { MovieId = 6, UserId = 3, Rating = 2, IsActive = true },
                new MovieRating { MovieId = 6, UserId = 4, Rating = 4, IsActive = true },
                new MovieRating { MovieId = 6, UserId = 5, Rating = 3, IsActive = true },
                new MovieRating { MovieId = 7, UserId = 1, Rating = 4, IsActive = true },
                new MovieRating { MovieId = 7, UserId = 4, Rating = 5, IsActive = true },
                new MovieRating { MovieId = 7, UserId = 5, Rating = 4, IsActive = true },
                new MovieRating { MovieId = 8, UserId = 1, Rating = 4, IsActive = true },
                new MovieRating { MovieId = 8, UserId = 4, Rating = 5, IsActive = true },
                new MovieRating { MovieId = 8, UserId = 5, Rating = 4, IsActive = true },
                new MovieRating { MovieId = 9, UserId = 1, Rating = 5, IsActive = true },
                new MovieRating { MovieId = 9, UserId = 4, Rating = 5, IsActive = true },
                new MovieRating { MovieId = 9, UserId = 5, Rating = 3, IsActive = true },
                new MovieRating { MovieId = 10, UserId = 1, Rating = 2, IsActive = true },
                new MovieRating { MovieId = 10, UserId = 4, Rating = 4, IsActive = true },
                new MovieRating { MovieId = 10, UserId = 5, Rating = 3, IsActive = true },
                new MovieRating { MovieId = 11, UserId = 1, Rating = 3, IsActive = true },
                new MovieRating { MovieId = 11, UserId = 4, Rating = 4, IsActive = true },
                new MovieRating { MovieId = 11, UserId = 5, Rating = 3, IsActive = true },
                new MovieRating { MovieId = 12, UserId = 1, Rating = 4, IsActive = true },
                new MovieRating { MovieId = 12, UserId = 4, Rating = 5, IsActive = true },
                new MovieRating { MovieId = 12, UserId = 5, Rating = 5, IsActive = true },
                new MovieRating { MovieId = 13, UserId = 1, Rating = 5, IsActive = true },
                new MovieRating { MovieId = 13, UserId = 3, Rating = 4, IsActive = true },
                new MovieRating { MovieId = 13, UserId = 4, Rating = 5, IsActive = true },
                new MovieRating { MovieId = 13, UserId = 5, Rating = 5, IsActive = true }
            };
            MovieGenres = new List<MovieGenre>
            {
                new MovieGenre { MovieId = 1, GenreId = 1, IsActive = true },
                new MovieGenre { MovieId = 2, GenreId = 1, IsActive = true },
                new MovieGenre { MovieId = 3, GenreId = 1, IsActive = true },
                new MovieGenre { MovieId = 4, GenreId = 1, IsActive = true },
                new MovieGenre { MovieId = 4, GenreId = 3, IsActive = true },
                new MovieGenre { MovieId = 5, GenreId = 3, IsActive = true },
                new MovieGenre { MovieId = 7, GenreId = 1, IsActive = true },
                new MovieGenre { MovieId = 7, GenreId = 2, IsActive = true },
                new MovieGenre { MovieId = 7, GenreId = 3, IsActive = true },
                new MovieGenre { MovieId = 6, GenreId = 3, IsActive = true },
                new MovieGenre { MovieId = 8, GenreId = 2, IsActive = true },
                new MovieGenre { MovieId = 8, GenreId = 3, IsActive = true },
                new MovieGenre { MovieId = 9, GenreId = 3, IsActive = true },
                new MovieGenre { MovieId = 10, GenreId = 1, IsActive = true },
                new MovieGenre { MovieId = 11, GenreId = 3, IsActive = true },
                new MovieGenre { MovieId = 12, GenreId = 3, IsActive = true },
                new MovieGenre { MovieId = 13, GenreId = 1, IsActive = true }
            };

            options = new DbContextOptionsBuilder<MovieRatingContext>()
                .UseInMemoryDatabase($"CourseDatabaseForTesting{Guid.NewGuid()}")
                .Options;
            SetupContext(options);
        }

        [Test]
        public void MovieNameFilterTest()
        {
            using (var context = new MovieRatingContext(options))
            {
                var target = new MovieRatingsDA(context);
                var result = target.GetMovies("the", null, null);

                Assert.IsNotNull(result);
                Assert.AreEqual(7, result.Count());
            }
        }

        [Test]
        public void YearFilterTest()
        {
            using (var context = new MovieRatingContext(options))
            {
                var target = new MovieRatingsDA(context);
                var result = target.GetMovies(null, 2019, null);

                Assert.IsNotNull(result);
                Assert.AreEqual(3, result.Count());
            }
        }

        [Test]
        public void GenreFilterTest()
        {
            using (var context = new MovieRatingContext(options))
            {
                var target = new MovieRatingsDA(context);
                var result = target.GetMovies(null, null, new List<string> { "Action", "Comedy" });

                Assert.IsNotNull(result);
                Assert.AreEqual(8, result.Count());
            }
        }

        [Test]
        public void MovieNameAndYearFilterTest()
        {
            using (var context = new MovieRatingContext(options))
            {
                var target = new MovieRatingsDA(context);
                var result = target.GetMovies("the", 2019, null);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Count());
            }
        }

        [Test]
        public void MovieNameAndGenreFilterTest()
        {
            using (var context = new MovieRatingContext(options))
            {
                var target = new MovieRatingsDA(context);
                var result = target.GetMovies("The", null, new List<string> { "Action", "Comedy" });

                Assert.IsNotNull(result);
                Assert.AreEqual(5, result.Count());
            }
        }

        [Test]
        public void YearAndGenreFilterTest()
        {
            using (var context = new MovieRatingContext(options))
            {
                var target = new MovieRatingsDA(context);
                var result = target.GetMovies(null, 2020, new List<string> { "Action", "Comedy" });

                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Count());
            }
        }

        [Test]
        public void AllFilterTest()
        {
            using (var context = new MovieRatingContext(options))
            {
                var target = new MovieRatingsDA(context);
                var result = target.GetMovies("The", 2019, new List<string> { "Action", "Comedy" });

                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Count());
            }
        }

        [Test]
        public void ValidateAverageForMovieName()
        {
            using (var context = new MovieRatingContext(options))
            {
                var target = new MovieRatingsDA(context);
                var result = target.GetMovies("inception", null, null);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Count());
                Assert.AreEqual(MovieRatings.Where(x => x.MovieId == result.First().Id).Average(x => x.Rating), result.First().AverageRating);
            }
        }

        [Test]
        public void MovieWithMultipleGenresReturnsAllGenresInCommaSeparatedString()
        {
            using (var context = new MovieRatingContext(options))
            {
                var target = new MovieRatingsDA(context);
                var result = target.GetMovies("Guardians of the Galaxy", null, null);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Count());
                Assert.IsTrue(result.First().Genres.Split(',').Count() == 3);
            }
        }

        [Test]
        public void NoResultReturnedWhenNoFilterMatch()
        {
            using (var context = new MovieRatingContext(options))
            {
                var target = new MovieRatingsDA(context);
                var result = target.GetMovies(null, 1960, null);

                Assert.IsNotNull(result);
                Assert.IsEmpty(result);
            }
        }

        [Test]
        public void OnlyTopFiveRatedMoviesAreReturnedForGetTopRatedMoviesWithRightOrder()
        {
            using (var context = new MovieRatingContext(options))
            {
                var target = new MovieRatingsDA(context);
                var result = target.GetTopRatedMovies();

                Assert.IsNotNull(result);
                Assert.AreEqual(5, result.Count());
                var listResult = result.ToList();
                Assert.AreEqual(listResult[2].AverageRating, listResult[3].AverageRating);
                Assert.AreEqual(listResult[3].AverageRating, listResult[4].AverageRating);
                Assert.AreEqual(listResult[2].Title, "Back to the Future");
                Assert.AreEqual(listResult[3].Title, "Guardians of the Galaxy");
                Assert.AreEqual(listResult[4].Title, "The Prestige");

            }
        }

        [Test]
        public void OnlyTopFiveRatedMoviesAreReturnedForGetTopRatedMoviesByUserWithRightOrder()
        {
            using (var context = new MovieRatingContext(options))
            {
                var target = new MovieRatingsDA(context);
                var result = target.GetTopRatedMoviesByUser("Raj");

                Assert.IsNotNull(result);
                Assert.AreEqual(5, result.Count());
                var listResult = result.ToList();
                Assert.AreEqual(listResult[0].AverageRating, listResult[1].AverageRating);
                Assert.AreEqual(listResult[0].Title, "Inception");
                Assert.AreEqual(listResult[1].Title, "The Dark Knight");
            }
        }

        [Test]
        public void IfThereAreNoRatingsProvidedByAnUserGetTopRatedMoviesByUserReturnsEmptyEnumerable()
        {
            using (var context = new MovieRatingContext(options))
            {
                var target = new MovieRatingsDA(context);
                var result = target.GetTopRatedMoviesByUser("John");

                Assert.IsNotNull(result);
                Assert.IsEmpty(result);
            }
        }

        [Test]
        public void WhenAnInvalidUsernameSentThenGetTopRatedMoviesByUserThrowsException()
        {
            using (var context = new MovieRatingContext(options))
            {
                var target = new MovieRatingsDA(context);
                _ = Assert.Throws<UserNotFoundException>(() => target.GetTopRatedMoviesByUser("Test"));
            }
        }

        [Test]
        public async Task AddUserRatingAsyncAddsNewRatingIfThereIsNoneGivenByTheUserAsync()
        {
            using(var context = new MovieRatingContext(options))
            {
                var target = new MovieRatingsDA(context);
                var result = await target.AddUserRatingAsync("Parasite", "Alex", 3);

                Assert.IsTrue(result);
            }
        }

        [Test]
        public async Task AddUserRatingAsyncUpdatesRatingIfThereAnExistingMovieGivenByTheUserAsync()
        {
            using (var context = new MovieRatingContext(options))
            {
                var target = new MovieRatingsDA(context);
                var result = await target.AddUserRatingAsync("Wonder Woman 1984", "Bob", 3);

                Assert.IsTrue(result);

            }
        }

        [Test]
        public void WhenInvalidUsernameSentToAddUserRatingAsyncThenExceptionThrown()
        {
            using (var context = new MovieRatingContext(options))
            {
                var target = new MovieRatingsDA(context);
                _ = Assert.ThrowsAsync<UserNotFoundException>(() => target.AddUserRatingAsync("Wonder Woman 1984", "Test", 3));
            }
        }

        [Test]
        public void WhenInvalidMovieSentToAddUserRatingAsyncThenExceptionThrown()
        {
            using (var context = new MovieRatingContext(options))
            {
                var target = new MovieRatingsDA(context);
                _ = Assert.ThrowsAsync<MovieNotFoundException>(() => target.AddUserRatingAsync("Test", "Raj", 3));
            }
        }

        private void SetupContext(DbContextOptions<MovieRatingContext> dbContextOptions)
        {
            using(var context = new MovieRatingContext(dbContextOptions))
            {
                context.Movies.AddRange(Movies);
                context.Genres.AddRange(Genres);
                context.Users.AddRange(Users);
                context.MovieRatings.AddRange(MovieRatings);
                context.MovieGenres.AddRange(MovieGenres);

                context.SaveChanges();
            }
        }
    }
}
