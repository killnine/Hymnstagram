using AspNetCoreRateLimit;
using AutoMapper;
using DataAccess.Memory.Daos;
using FluentValidation.AspNetCore;
using Hymnstagram.Model.DataAccess;
using Hymnstagram.Model.Services;
using Hymnstagram.Web.Helpers;
using Hymnstagram.Web.Mapping;
using Hymnstagram.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Hymnstogram.Web
{
    #pragma warning disable CS1591 
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
                
        public void ConfigureServices(IServiceCollection services)
        {            
            services.AddHttpCacheHeaders((expirationModelOptions) =>
            {                
                expirationModelOptions.MaxAge = 600;
                expirationModelOptions.CacheLocation = Marvin.Cache.Headers.CacheLocation.Private;
            }, 
            (validationModelOptions) =>
            {
                validationModelOptions.MustRevalidate = true;
            });            

            services.AddControllers()                    
                    .ConfigureApiBehaviorOptions(setupAction =>
                    {                        
                        setupAction.InvalidModelStateResponseFactory = context =>
                        {
                            return InvalidModelStateResponseFactory.GenerateResponseForInvalidModelState(context.ModelState, context.HttpContext);
                        };
                    });

            services.AddMvc(setupAction =>
            {
                //Return 406 'Not Acceptable' for any unsupported media types
                setupAction.ReturnHttpNotAcceptable = true;

                //Create default response types for all controllers
                setupAction.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
                setupAction.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
                setupAction.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
            }).AddFluentValidation(setupAction => 
            {
                setupAction.RunDefaultMvcValidationAfterFluentValidationExecutes = true;
            });

            services.AddSingleton<ISongDao,SongDao>();
            services.AddSingleton<ISongbookDao, SongbookDao>();
            services.AddSingleton<ICreatorDao, CreatorDao>();
            services.AddSingleton<ISongbookRepository, SongbookRepository>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();            

            services.AddTransient<IPropertyMappingService, PropertyMappingService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new SongbookProfile());
                mc.AddProfile(new CreatorProfile());
                mc.AddProfile(new SongProfile());
                mc.AddProfile(new ParametersProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("HymnstagramOpenAPISpecification", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "Library API",
                    Description = "Create, read, and delete songbooks and songs your congregation uses.",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "administrator@hymnstagram.com",
                        Name = "Hymnstagram Development",
                        Url = new Uri("https://www.hymnstagram.com")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    },
                    //TermsOfService = new Uri(""),
                    Version = "1"
                });

                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                setupAction.IncludeXmlComments(xmlCommentsFullPath);
            });

            services.AddMemoryCache();

            services.Configure<IpRateLimitOptions>((options) =>
            {
                options.GeneralRules = new List<RateLimitRule>()
                {
                    new RateLimitRule()
                    {
                        Endpoint = "*",
                        Limit = 50,
                        Period = "5m"
                    }
                };                
            });            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault occurred. Try again later."); //log here
                    });
                });

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }            

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint("/swagger/HymnstagramOpenAPISpecification/swagger.json", "Library API");
                setupAction.RoutePrefix = "";
            });

            app.UseStaticFiles();
          
            app.UseHttpCacheHeaders();

            app.UseIpRateLimiting();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });            
        }
    }
    #pragma warning restore CS1591
}
