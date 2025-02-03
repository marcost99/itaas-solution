using ItaasSolution.Api.Api.Filters;
using ItaasSolution.Api.Application.Services.FileLog.Converter;
using ItaasSolution.Api.Application.Services.Log.Converter;
using ItaasSolution.Api.Application.UseCases.FileLog.Converter;
using ItaasSolution.Api.Application.UseCases.FileLog.GetAll;
using ItaasSolution.Api.Application.UseCases.FileLog.GetById;
using ItaasSolution.Api.Application.UseCases.Log.GetAll;
using ItaasSolution.Api.Application.UseCases.Log.GetById;
using ItaasSolution.Api.Application.UseCases.Log.Register;
using ItaasSolution.Api.Domain.Repositories;
using ItaasSolution.Api.Domain.Repositories.FileLogs;
using ItaasSolution.Api.Domain.Repositories.Logs;
using ItaasSolution.Api.Infraestructure.DataAccess;
using ItaasSolution.Api.Infraestructure.DataAccess.Repositories;
using ItaasSolution.Api.Infraestructure.Services.Cache;
using ItaasSolution.Api.Infraestructure.Services.File.Generator;
using ItaasSolution.Api.Infraestructure.Services.File.HandlerIO;
using ItaasSolution.Api.Infraestructure.Services.FileLog.Info;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            services.AddMemoryCache();

            services.AddHttpContextAccessor();

            // If have an exception redirect to class ExceptionFilter
            services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter))).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Sets the NSwag
            services.AddOpenApiDocument(config =>
            {
                config.Title = "My API";
            });

            // Sets the response caching
            services.AddResponseCaching();

            // Sets the dependency injection
            AddDependencyInjection(services);

            // Sets the DbContext
            AddDbContext(services, Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Sets the response caching
            app.UseResponseCaching();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Actives the middleware to server the NSwag
                app.UseOpenApi();
                app.UseSwaggerUi3();

                // Redirects to the page of the NSwag to default
                app.Use(async (context, next) =>
                {
                    if (context.Request.Path == "/")
                    {
                        context.Response.Redirect("/swagger");
                        return;
                    }

                    await next();
                });
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // Sets the static files
            AddStaticFiles(app);

            app.UseMvc();
        }

        // This method sets the dependency injection of the application and infraestructure
        public void AddDependencyInjection(IServiceCollection services)
        {
            // Application
            services.AddScoped<IConverterFileLogUseCase, ConverterFileLogUseCase>();
            services.AddScoped<IDataTypeLogConverter, DataTypeLogConverter>();
            services.AddScoped<IDataTypeFileLogConverter, DataTypeFileLogAgoraConverter>();
            services.AddScoped<IRegisterLogUseCase, RegisterLogUseCase>();
            services.AddScoped<IGetAllLogUseCase, GetAllLogUseCase>();
            services.AddScoped<IGetByIdLogUseCase, GetByIdLogUseCase>();
            services.AddScoped<IInfoFileLog, InfoFileLog>();
            services.AddScoped<IGetAllFileLogUseCase, GetAllFileLogUseCase>();
            services.AddScoped<IGetByIdFileLogUseCase, GetByIdFileLogUseCase>();
            services.AddScoped<IFileLogsReadOnlyRepository, FileLogsRepository>();

            // Infraestructure
            services.AddScoped<IHandlerIOFile, HandlerIOFile>();
            services.AddScoped<IGeneratorFile, GeneratorFile>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ILogsWriteOnlyRepository, LogsRepository>();
            services.AddScoped<ILogsReadOnlyRepository, LogsRepository>();
        }

        // This method sets the settings of the infraestructure
        public void AddStaticFiles(IApplicationBuilder app)
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

        public static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Connection");

            services.AddDbContext<ItaasSolutionDbContext>(config =>
                config.UseSqlServer(connectionString, sqlServerOptions =>
                    sqlServerOptions.MigrationsAssembly("ItaasSolution.Api")));
        }
    }
}
