using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieRatings.Api.Controllers;
using MovieRatings.Api.Models;
using MovieRatings.Common;
using MovieRatings.Common.Exceptions;
using MovieRatings.Context.Entities;
using MovieRatings.DataAccess;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieRating.Api.Tests
{
    public class MovieRatingControllerTest
    {
        private IMapper mapper;

        public MovieRatingControllerTest()
        {
            if (mapper == null)
            {
                mapper = new MapperConfiguration(x => x.CreateMap<UserMovieRating, UserMovieRatingModel>()
                .ForMember(dest => dest.AverageRating, m => m.MapFrom(u => Helpers.RoundRating(u.AverageRating)))).CreateMapper();
            }
        }

        [Test]
        public void WhenNoSearchParametersPassedOnMovieSearchThenBadRequestIsReturend()
        {
            Mock<IMovieRatingsDA> mockMovieRatingsDA = new Mock<IMovieRatingsDA>();            
            MovieRatingController target = new MovieRatingController(mockMovieRatingsDA.Object, mapper);

            var result = target.MovieRatingSearch(new MovieSearchModel()) as ObjectResult;

            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Test]
        public void WhenAtleastOneSearchPameterIsPassedOnMovieSearchThenCorrectResultIsReturned()
        {
            Mock<IMovieRatingsDA> mockMovieRatingsDA = new Mock<IMovieRatingsDA>();
            UserMovieRating userMovieRating = new UserMovieRating { Id = 1, Title = "TestMovie" };
            mockMovieRatingsDA.Setup(x => x.GetMovies(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<List<string>>())).Returns(new List<UserMovieRating> { userMovieRating });
            MovieRatingController target = new MovieRatingController(mockMovieRatingsDA.Object, mapper);

            var result = target.MovieRatingSearch(new MovieSearchModel { MovieName = "TestMovie" }) as ObjectResult;

            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.IsInstanceOf<List<UserMovieRatingModel>>(result.Value);
            var moviesReturned = result.Value as List<UserMovieRatingModel>;
            Assert.AreEqual(1, moviesReturned.Count);
            Assert.AreEqual(userMovieRating.Title, moviesReturned[0].Title);
        }

        [Test]
        public void WhenThereIsNoResultForMovieSearchThenNotFoundStatusCodeReturned()
        {
            Mock<IMovieRatingsDA> mockMovieRatingsDA = new Mock<IMovieRatingsDA>();
            MovieRatingController target = new MovieRatingController(mockMovieRatingsDA.Object, mapper);

            var result = target.MovieRatingSearch(new MovieSearchModel { MovieName = "TestMovie" }) as ObjectResult;

            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [Test]
        public void AverageRoundingMappingScenario1()
        {
            Mock<IMovieRatingsDA> mockMovieRatingsDA = new Mock<IMovieRatingsDA>();
            UserMovieRating userMovieRating = new UserMovieRating { Id = 1, Title = "TestMovie", AverageRating = 2.91 };
            mockMovieRatingsDA.Setup(x => x.GetMovies(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<List<string>>())).Returns(new List<UserMovieRating> { userMovieRating });
            MovieRatingController target = new MovieRatingController(mockMovieRatingsDA.Object, mapper);

            var result = target.MovieRatingSearch(new MovieSearchModel { MovieName = "TestMovie" }) as ObjectResult;

            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.IsInstanceOf<List<UserMovieRatingModel>>(result.Value);
            var moviesReturned = result.Value as List<UserMovieRatingModel>;
            Assert.AreEqual(1, moviesReturned.Count);
            Assert.AreEqual(3, moviesReturned[0].AverageRating);
        }

        [Test]
        public void AverageRoundingMappingScenario2()
        {
            Mock<IMovieRatingsDA> mockMovieRatingsDA = new Mock<IMovieRatingsDA>();
            UserMovieRating userMovieRating = new UserMovieRating { Id = 1, Title = "TestMovie", AverageRating = 3.249 };
            mockMovieRatingsDA.Setup(x => x.GetMovies(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<List<string>>())).Returns(new List<UserMovieRating> { userMovieRating });
            MovieRatingController target = new MovieRatingController(mockMovieRatingsDA.Object, mapper);

            var result = target.MovieRatingSearch(new MovieSearchModel { MovieName = "TestMovie" }) as ObjectResult;

            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.IsInstanceOf<List<UserMovieRatingModel>>(result.Value);
            var moviesReturned = result.Value as List<UserMovieRatingModel>;
            Assert.AreEqual(1, moviesReturned.Count);
            Assert.AreEqual(3, moviesReturned[0].AverageRating);
        }

        [Test]
        public void AverageRoundingMappingScenario3()
        {
            Mock<IMovieRatingsDA> mockMovieRatingsDA = new Mock<IMovieRatingsDA>();
            UserMovieRating userMovieRating = new UserMovieRating { Id = 1, Title = "TestMovie", AverageRating = 3.25 };
            mockMovieRatingsDA.Setup(x => x.GetMovies(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<List<string>>())).Returns(new List<UserMovieRating> { userMovieRating });
            MovieRatingController target = new MovieRatingController(mockMovieRatingsDA.Object, mapper);

            var result = target.MovieRatingSearch(new MovieSearchModel { MovieName = "TestMovie" }) as ObjectResult;

            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.IsInstanceOf<List<UserMovieRatingModel>>(result.Value);
            var moviesReturned = result.Value as List<UserMovieRatingModel>;
            Assert.AreEqual(1, moviesReturned.Count);
            Assert.AreEqual(3.5, moviesReturned[0].AverageRating);
        }

        [Test]
        public void AverageRoundingMappingScenario4()
        {
            Mock<IMovieRatingsDA> mockMovieRatingsDA = new Mock<IMovieRatingsDA>();
            UserMovieRating userMovieRating = new UserMovieRating { Id = 1, Title = "TestMovie", AverageRating = 3.6 };
            mockMovieRatingsDA.Setup(x => x.GetMovies(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<List<string>>())).Returns(new List<UserMovieRating> { userMovieRating });
            MovieRatingController target = new MovieRatingController(mockMovieRatingsDA.Object, mapper);

            var result = target.MovieRatingSearch(new MovieSearchModel { MovieName = "TestMovie" }) as ObjectResult;

            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.IsInstanceOf<List<UserMovieRatingModel>>(result.Value);
            var moviesReturned = result.Value as List<UserMovieRatingModel>;
            Assert.AreEqual(1, moviesReturned.Count);
            Assert.AreEqual(3.5, moviesReturned[0].AverageRating);
        }

        [Test]
        public void AverageRoundingMappingScenario5()
        {
            Mock<IMovieRatingsDA> mockMovieRatingsDA = new Mock<IMovieRatingsDA>();
            UserMovieRating userMovieRating = new UserMovieRating { Id = 1, Title = "TestMovie", AverageRating = 3.75 };
            mockMovieRatingsDA.Setup(x => x.GetMovies(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<List<string>>())).Returns(new List<UserMovieRating> { userMovieRating });
            MovieRatingController target = new MovieRatingController(mockMovieRatingsDA.Object, mapper);

            var result = target.MovieRatingSearch(new MovieSearchModel { MovieName = "TestMovie" }) as ObjectResult;

            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.IsInstanceOf<List<UserMovieRatingModel>>(result.Value);
            var moviesReturned = result.Value as List<UserMovieRatingModel>;
            Assert.AreEqual(1, moviesReturned.Count);
            Assert.AreEqual(4, moviesReturned[0].AverageRating);
        }

        [Test]
        public void WhenThereIsNoGetTopRatedMoviesThenNotFoundStatusCodeReturned()
        {
            Mock<IMovieRatingsDA> mockMovieRatingsDA = new Mock<IMovieRatingsDA>();
            MovieRatingController target = new MovieRatingController(mockMovieRatingsDA.Object, mapper);

            var result = target.GetTopRatedMovies() as ObjectResult;

            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [Test]
        public void WhenThereIsLessThanFiveMoviesForGetTopRatedMoviesThenAllAvailableMoviesAreReturned()
        {
            Mock<IMovieRatingsDA> mockMovieRatingsDA = new Mock<IMovieRatingsDA>();
            UserMovieRating userMovieRating1 = new UserMovieRating { Id = 1, Title = "TestMovie1", AverageRating = 3.25 };
            UserMovieRating userMovieRating2 = new UserMovieRating { Id = 2, Title = "TestMovie2", AverageRating = 4.23 };
            mockMovieRatingsDA.Setup(x => x.GetTopRatedMovies()).Returns(new List<UserMovieRating> { userMovieRating1, userMovieRating2 });
            MovieRatingController target = new MovieRatingController(mockMovieRatingsDA.Object, mapper);

            var result = target.GetTopRatedMovies() as ObjectResult;

            Assert.IsInstanceOf<List<UserMovieRatingModel>>(result.Value);
            var moviesReturned = result.Value as List<UserMovieRatingModel>;
            Assert.AreEqual(2, moviesReturned.Count);
        }

        [Test]
        public void WhenThereAreFiveMoviesForGetTopRatedMoviesThenAllAvailableMoviesAreReturned()
        {
            Mock<IMovieRatingsDA> mockMovieRatingsDA = new Mock<IMovieRatingsDA>();
            UserMovieRating userMovieRating1 = new UserMovieRating { Id = 1, Title = "TestMovie1", AverageRating = 3.25 };
            UserMovieRating userMovieRating2 = new UserMovieRating { Id = 2, Title = "TestMovie2", AverageRating = 4.23 };
            UserMovieRating userMovieRating3 = new UserMovieRating { Id = 3, Title = "TestMovie2", AverageRating = 2.98 };
            UserMovieRating userMovieRating4 = new UserMovieRating { Id = 4, Title = "TestMovie2", AverageRating = 5 };
            UserMovieRating userMovieRating5 = new UserMovieRating { Id = 5, Title = "TestMovie2", AverageRating = 3.78 };
            mockMovieRatingsDA.Setup(x => x.GetTopRatedMovies()).Returns(new List<UserMovieRating> { userMovieRating1, userMovieRating2, userMovieRating3, userMovieRating4, userMovieRating5 });
            MovieRatingController target = new MovieRatingController(mockMovieRatingsDA.Object, mapper);

            var result = target.GetTopRatedMovies() as ObjectResult;

            Assert.IsInstanceOf<List<UserMovieRatingModel>>(result.Value);
            var moviesReturned = result.Value as List<UserMovieRatingModel>;
            Assert.AreEqual(5, moviesReturned.Count);
        }

        [Test]
        public void WhenThereIsNoGetTopRatedMoviesByUserThenNotFoundStatusCodeReturned()
        {
            Mock<IMovieRatingsDA> mockMovieRatingsDA = new Mock<IMovieRatingsDA>();
            MovieRatingController target = new MovieRatingController(mockMovieRatingsDA.Object, mapper);

            var result = target.GetTopRatedMoviesByUser("testuser") as ObjectResult;

            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [Test]
        public void WhenUserDoesNotExistsForGetTopRatedMoviesByUserThenBadRequestCodeReturned()
        {
            Mock<IMovieRatingsDA> mockMovieRatingsDA = new Mock<IMovieRatingsDA>();
            mockMovieRatingsDA.Setup(x => x.GetTopRatedMoviesByUser(It.IsAny<string>())).Throws(new UserNotFoundException(""));
            MovieRatingController target = new MovieRatingController(mockMovieRatingsDA.Object, mapper);

            var result = target.GetTopRatedMoviesByUser("testuser") as ObjectResult;

            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Test]
        public void WhenThereIsLessThanFiveMoviesForGetTopRatedMoviesByUserThenAllAvailableMoviesAreReturned()
        {
            Mock<IMovieRatingsDA> mockMovieRatingsDA = new Mock<IMovieRatingsDA>();
            UserMovieRating userMovieRating1 = new UserMovieRating { Id = 1, Title = "TestMovie1", AverageRating = 3.25 };
            UserMovieRating userMovieRating2 = new UserMovieRating { Id = 2, Title = "TestMovie2", AverageRating = 4.23 };
            mockMovieRatingsDA.Setup(x => x.GetTopRatedMoviesByUser(It.IsAny<string>())).Returns(new List<UserMovieRating> { userMovieRating1, userMovieRating2 });
            MovieRatingController target = new MovieRatingController(mockMovieRatingsDA.Object, mapper);

            var result = target.GetTopRatedMoviesByUser("") as ObjectResult;

            Assert.IsInstanceOf<List<UserMovieRatingModel>>(result.Value);
            var moviesReturned = result.Value as List<UserMovieRatingModel>;
            Assert.AreEqual(2, moviesReturned.Count);
        }

        [Test]
        public void WhenThereAreFiveMoviesForGetTopRatedMoviesByUserThenAllAvailableMoviesAreReturned()
        {
            Mock<IMovieRatingsDA> mockMovieRatingsDA = new Mock<IMovieRatingsDA>();
            UserMovieRating userMovieRating1 = new UserMovieRating { Id = 1, Title = "TestMovie1", AverageRating = 3.25 };
            UserMovieRating userMovieRating2 = new UserMovieRating { Id = 2, Title = "TestMovie2", AverageRating = 4.23 };
            UserMovieRating userMovieRating3 = new UserMovieRating { Id = 3, Title = "TestMovie2", AverageRating = 2.98 };
            UserMovieRating userMovieRating4 = new UserMovieRating { Id = 4, Title = "TestMovie2", AverageRating = 5 };
            UserMovieRating userMovieRating5 = new UserMovieRating { Id = 5, Title = "TestMovie2", AverageRating = 3.78 };
            mockMovieRatingsDA.Setup(x => x.GetTopRatedMoviesByUser(It.IsAny<string>())).Returns(new List<UserMovieRating> { userMovieRating1, userMovieRating2, userMovieRating3, userMovieRating4, userMovieRating5 });
            MovieRatingController target = new MovieRatingController(mockMovieRatingsDA.Object, mapper);

            var result = target.GetTopRatedMoviesByUser("") as ObjectResult;

            Assert.IsInstanceOf<List<UserMovieRatingModel>>(result.Value);
            var moviesReturned = result.Value as List<UserMovieRatingModel>;
            Assert.AreEqual(5, moviesReturned.Count);
        }

        [Test]
        public async Task WhenUserDoesNotExistsForAddUserRatingAsyncThenBadRequestCodeReturnedAsync()
        {
            Mock<IMovieRatingsDA> mockMovieRatingsDA = new Mock<IMovieRatingsDA>();
            mockMovieRatingsDA.Setup(x => x.AddUserRatingAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Throws(new UserNotFoundException(""));
            MovieRatingController target = new MovieRatingController(mockMovieRatingsDA.Object, mapper);

            var result = await target.AddUserRatingAsync(new MovieRatingModel { UserName = "test", MovieName = "test", Rating = 3 }) as ObjectResult;

            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Test]
        public async Task WhenMovieDoesNotExistsForAddUserRatingAsyncThenBadRequestCodeReturnedAsync()
        {
            Mock<IMovieRatingsDA> mockMovieRatingsDA = new Mock<IMovieRatingsDA>();
            mockMovieRatingsDA.Setup(x => x.AddUserRatingAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Throws(new MovieNotFoundException(""));
            MovieRatingController target = new MovieRatingController(mockMovieRatingsDA.Object, mapper);

            var result = await target.AddUserRatingAsync(new MovieRatingModel { UserName = "test", MovieName = "test", Rating = 3 }) as ObjectResult;

            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Test]
        public async Task WhenValidValuesSentToAddUserRatingAsyncThenOkResultCodeReturnedAsync()
        {
            Mock<IMovieRatingsDA> mockMovieRatingsDA = new Mock<IMovieRatingsDA>();
            mockMovieRatingsDA.Setup(x => x.AddUserRatingAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns(Task.FromResult(true));
            MovieRatingController target = new MovieRatingController(mockMovieRatingsDA.Object, mapper);

            var result = await target.AddUserRatingAsync(new MovieRatingModel { UserName = "test", MovieName = "test", Rating = 3 }) as OkResult;

            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        }
    }
}
