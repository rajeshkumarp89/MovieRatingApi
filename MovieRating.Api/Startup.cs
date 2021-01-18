using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MovieRatings.Api.Models;
using MovieRatings.Common;
using MovieRatings.Context;
using MovieRatings.Context.Entities;
using MovieRatings.DataAccess;

namespace MovieRating.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MovieRatingContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("MovieRating"));
            });
            services.AddScoped<IMovieRatingsDA, MovieRatingsDA>();
            services.AddAutoMapper(cfg => cfg.CreateMap<UserMovieRating, UserMovieRatingModel>()
                .ForMember(dest => dest.AverageRating, m => m.MapFrom(u => Helpers.RoundRating(u.AverageRating))));
            services.AddControllers();
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
