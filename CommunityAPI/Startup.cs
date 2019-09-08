using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CommunityAPI.Options;
using CommunityAPI.Installers;
using CommunityAPI.Data;
using CommunityAPI.Domain;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using CommunityAPI.Extensions;
using CommunityAPI.HubConfig;

namespace CommunityAPI
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
            services.AddCors();
            services.AddSignalR();
            services.AddSingleton<IUserIdProvider, UserIdHubProvider>();
            services.InstallServicesInAssembly(Configuration);
            services.AddAutoMapper(typeof(Startup));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseCors(builder => builder.WithOrigins("http://localhost:4200").WithOrigins("https://batogits.github.io")
                   .AllowAnyMethod()
                   .AllowAnyHeader().AllowCredentials());

            app.UseHttpsRedirection();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthentication();

            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

            app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });

            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description);
            });

            app.UseSignalR(routes =>
            {
                routes.MapHub<NotifyHub>("/api/notify");
            });

            app.UseMvc();
        }
    }
}
