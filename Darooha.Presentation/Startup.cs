using AutoMapper;
using Darooha.Common.Helpers.AppSetting;
using Darooha.Common.Helpers.Helpers;
using Darooha.Common.Helpers.Interface;
using Darooha.Data.DatabaseContext;
using Darooha.Data.Models;
using Darooha.Presentation.Helpers.Filters;
using Darooha.Repo.Infrastructure;
using Darooha.Services.Seed.Service;
using Darooha.Services.Site.Admin.Auth.Interface;
using Darooha.Services.Site.Admin.Auth.Service;
using Darooha.Services.Site.Admin.User.Interface;
using Darooha.Services.Site.Admin.User.Service;
using Darooha.Services.Upload.Interface;
using Darooha.Services.Upload.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Darooha.Presentation
{
    public class Startup
    {
        private readonly int? _httpsPort;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;

            if (env.IsDevelopment())
            {
                var lunchJsonConf = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("Properties\\launchSettings.json")
                    .Build();

                _httpsPort = lunchJsonConf.GetValue<int>("iisSettings:iisExpress:sslPort");
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<DaroohaDbContext>(p => p.UseSqlServer(
                @"data source=.;initial catalog=Darooha;integrated security=True;multipleactiveresultsets=true"));

            services.AddMvc(config =>
            {
                config.EnableEndpointRouting = false;
                config.ReturnHttpNotAcceptable = true;
                config.SslPort = _httpsPort;
                config.Filters.Add(typeof(RequireHttpsAttribute));
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
                //var jsonFormatter = config.OutputFormatters.OfType<JsonOutputFormatter>().Single();
                //config.OutputFormatters.Remove(jsonFormatter);
                //config.OutputFormatters.Add(new IonOutputFormatter(jsonFormatter));
                //config.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                //config.InputFormatters.Add(new XmlSerializerInputFormatter(config));

            })
                .AddNewtonsoftJson(opt =>
                {
                    opt.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            services.AddResponseCaching();
            services.AddHsts(opt =>
            {
                opt.MaxAge = TimeSpan.FromDays(180);
                opt.IncludeSubDomains = true;
                opt.Preload = true;
            });

            services.AddOpenApiDocument(document =>
            {
                document.DocumentName = "v1_Site_Panel";
                document.ApiGroupNames = new[] { "v1_Site_Panel" };

                document.PostProcess = d =>
                {
                    d.Info.Title = "Darooha Api Docs";
                };


                document.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Type into the textbox: Bearer {your JWT token}."
                });

                document.OperationProcessors.Add(
                    new AspNetCoreOperationSecurityScopeProcessor("JWT"));
                //      new OperationSecurityScopeProcessor("JWT"));


            });
            services.AddOpenApiDocument(document =>
            {
                document.DocumentName = "v1_Site_App";
                document.ApiGroupNames = new[] { "v1_Site_App" };
                document.PostProcess = d =>
                {
                    d.Info.Title = "Api Docs For App";
                };
            });

            services.AddCors();

            //services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));

            services.AddAutoMapper(typeof(Startup));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IUnitOfWork<DaroohaDbContext>, UnitOfWork<DaroohaDbContext>>();
            services.AddTransient<SeedService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUploadService, UploadService>();
            services.AddScoped<IUtilities, Utilities>();
            services.AddScoped<UserCheckIdFilter>();


            IdentityBuilder builder = services.AddIdentityCore<Tbl_User>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 4;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
            });
            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
            builder.AddEntityFrameworkStores<DaroohaDbContext>();
            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddRoleManager<RoleManager<Role>>();
            builder.AddSignInManager<SignInManager<Tbl_User>>();
            builder.AddDefaultTokenProviders();

            var tokenSettingSection = Configuration.GetSection("TokenSetting");
            var tokenSetting = tokenSettingSection.Get<TokenSetting>();
            var key = Encoding.ASCII.GetBytes(tokenSetting.Secret);


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidIssuer = tokenSetting.Site,
                        ValidateAudience = true,
                        ValidAudience = tokenSetting.Audience,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
                opt.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
                opt.AddPolicy("RequireBlogsRole", policy => policy.RequireRole("Blog"));
                opt.AddPolicy("RequireAccountantRole", policy => policy.RequireRole("Accountant"));
                opt.AddPolicy("AccessBlog", policy => policy.RequireRole("Admin", "Blog"));
                opt.AddPolicy("AccessAccounting", policy => policy.RequireRole("Admin", "Accountant"));
                opt.AddPolicy("AccessProfile", policy => policy.RequireRole("Admin", "User", "Blog", "Accountant"));




            });

            //services.AddSpaStaticFiles(configuration =>
            //{
            //    configuration.RootPath = "ClientApp/dist";
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SeedService seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            context.Response.AddAppError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
                app.UseResponseCaching();
            }

            //app.UseHttpsRedirection();
            //app.UseResponseCompression();
            //app.UseResponseCaching();
            seeder.SeedUsers();
            app.UseCors(p => p.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader());
            //
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseOpenApi();
            app.UseSwaggerUi3(); // serve Swagger UI

            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/Files/Pic")),
            //    RequestPath = new PathString("/Files/Pic")
            //});
            //app.Use(async (context, next) =>
            //{
            //    await next();
            //    if (context.Response.StatusCode == 400 && !Path.HasExtension(context.Request.Path.Value))
            //    {
            //        context.Request.Path = "/ClientApp/index.html";
            //        await next();
            //    }
            //});
            //app.UseStaticFiles();
            //app.UseDefaultFiles();
//            app.UseSpa(spa =>
//            {
//                spa.Options.SourcePath = “ClientApp”;
//                if (env.IsDevelopment())
//                {
//                    spa.UseProxyToSpaDevelopmentServer(“http://localhost:4200");
//}
//            });
            app.UseStaticFiles(new StaticFileOptions()
            {
                RequestPath = new PathString("/wwwroot")
            });


            app.UseMvc();
        }
    }
}