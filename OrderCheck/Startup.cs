using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OrderCheck.DAL.Contexts;
using OrderCheck.Models;
using OrderCheck.DAL.Services;
using OrderCheck.DAL.Interfaces;
using OrderCheck.DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using OrderCheck.Web.Services;

namespace OrderCheck.Web
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

            #region DbContexts

            var connectionString = Configuration.GetConnectionString("OrderCheckConnection");

            services.AddDbContext<OrderCheckContext>(opt =>
            {
                opt.UseSqlServer(connectionString);
            });

            #endregion

            #region Identity

            services.TryAddSingleton<ISystemClock, SystemClock>();

            var builder = services.AddIdentityCore<User>();
            var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);

            identityBuilder.AddEntityFrameworkStores<OrderCheckContext>();
            identityBuilder.AddSignInManager<SignInManager<User>>();

            #endregion

            #region Services

            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IImageService, ImageService>();

            #endregion

            #region Repositories

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEstateRepository, EstateRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            
            #endregion

            services.AddMvc(option =>
            {
                option.EnableEndpointRouting = false;
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                option.Filters.Add(new AuthorizeFilter(policy));
            }).SetCompatibilityVersion(CompatibilityVersion.Latest);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenKey"]));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                    };
                });

            services.AddHttpContextAccessor();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            //app.UseRouting();

            //app.UseAuthorization();
            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");
            //});
        }
    }
}
