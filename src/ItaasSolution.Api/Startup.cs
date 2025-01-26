﻿using ItaasSolution.Api.Api.Filters;
using ItaasSolution.Api.Application.Formatting.Log;
using ItaasSolution.Api.Application.UseCases.Log.Converter;
using ItaasSolution.Api.Infraestructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;
using System.IO;

namespace ItaasSolution.Api
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
            // Sets the response caching
            services.AddResponseCaching();

            // Sets the dependency injection
            AddApplication(services);

            // If have an exception redirect to class ExceptionFilter
            services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter))).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Sets the response caching
            app.UseResponseCaching();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // Sets the infraestructure
            AddInfraestructure(app);

            app.UseMvc();
        }

        // This method sets the dependency injection of the application
        public void AddApplication(IServiceCollection services)
        {
            services.AddScoped<IConverterLogUseCase, ConverterLogUseCase>();
            services.AddScoped<IFileGenerator, FileGenerator>();
            services.AddScoped<ILogListFormatter, LogListFormatter>();
        }

        // This method sets the settings of the infraestructure
        public void AddInfraestructure(IApplicationBuilder app)
        {
            // Sets the middleware to permits files statics
            var fileRepositories = Configuration.GetSection("Settings:FileRepository").Get<List<Dictionary<string, string>>>();

            foreach (var repo in fileRepositories)
            {
                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(
                        Path.Combine(Directory.GetCurrentDirectory(), repo["PhysicalPath"])),
                    RequestPath = repo["RequestPath"]
                });
            }
        }


    }
}
