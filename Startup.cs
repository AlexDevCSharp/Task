using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Task.Services;

namespace Task
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddSwaggerGen();
            services.AddControllers();
            //services.AddSwaggerGen();
            services.AddTransient<BlogpostService>();
            services.AddTransient<CommentService>();
            services.AddTransient<ProfileService>();
            // configure DI for DB
            services.AddDbContext<BaseDbContext>();
            
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            app.UseRouting();

            app.UseAuthorization();
            //app.UseSwagger();
            //app.UseSwaggerUI();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
            
        }
    }
}
