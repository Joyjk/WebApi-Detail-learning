using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi_test.Helpers;
using WebApi_test.Services;

namespace WebApi_test
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
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
            sqlserver=>sqlserver.UseNetTopologySuite()));

            services.AddControllers();

            //services.AddCors();
            services.AddCors(options=>
            {
                options.AddPolicy("AllowAPIRequestIO",
                    builder => builder.WithOrigins("http://apirequest.io/").WithMethods("GET", "POST").AllowAnyHeader());
            });

            services.AddDataProtection();

            services.AddAutoMapper(typeof(Startup));

            services.AddTransient<HashServices>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddTransient<GenreHATEOASAttribute>();
            services.AddTransient<LinksGenerator>();

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters =
                new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration["jwt:key"])),
                    ClockSkew = TimeSpan.Zero

                });

            services.AddTransient<IFileStorageService, InAppStorageService>();

            services.AddTransient<IHostedService, MovieInTheatersService>();

            services.AddHttpContextAccessor();


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "MoviesAPI" });
            });

            //services.AddSingleton<IRepository, InMemoryRepository>();
            //services.AddScoped<>
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "MoviesAPI");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            //app.UseCors(builder =>
            //builder.WithOrigins("http://apirequest.io/")
            //.WithMethods("GET","POST").AllowAnyHeader());
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
