using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RealWorldWebAPI.Data.Models;
using Newtonsoft;
using Newtonsoft.Json;
using System.IO;
using RealWorldWebAPI.Controllers;
using RealWorldWebAPI.Services;

namespace RealWorldWebAPI
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
            services
                .AddDbContext<RealWorldContext>(options =>
                    options.UseSqlite(Configuration.GetConnectionString("Sqlite")))
                .AddScoped<UserService>()
                .AddScoped<ArticleService>()
                .AddScoped<FavoritesService>()
                .AddScoped<CommentsService>()
                .AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Use(async (context, next) =>
            {
                // Do loging
                // Do work that doesn't write to the Response.
                //var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
                //var bodyObect = JsonConvert.DeserializeObject<UserRequest>(body);

                await next.Invoke();
                // Do logging or other work that doesn't write to the Response.
            });
        }
    }
}
