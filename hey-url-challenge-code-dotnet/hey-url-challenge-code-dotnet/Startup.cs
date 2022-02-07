using FluentValidation;
using hey_url_challenge_code_dotnet.Mapping;
using HeyUrl.Application.Abstraction;
using HeyUrl.Application.Click;
using HeyUrl.Application.Url;
using HeyUrl.Application.Url.Validators;
using HeyUrl.Dto.Click;
using HeyUrl.Dto.Url;
using HeyUrl.IApplication.Base;
using HeyUrl.Mapping;
using HeyUrl.Persistence;
using HeyUrl.Repository;
using HeyUrl.Repository.Abstraction;
using HeyUrl.UnitOfWork;
using HeyUrl.UnitOfWork.Abstraction;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HeyUrlChallengeCodeDotnet
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
            services.AddBrowserDetection();
            services.AddControllersWithViews();
            services.AddAutoMapper(typeof(HeyUrlWebMapping));
            services.AddAutoMapper(typeof(HeyUrlMapping));
            ConfigureInfraestructureServices(services);
            ConfigureApplicationServices(services);
        }

        public void ConfigureInfraestructureServices(IServiceCollection services)
        {
            services.AddDbContext<HeyUrlDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default"), b => b.MigrationsAssembly("HeyUrl.Persistence")));
            services.AddTransient<IUrlRepository, UrlRepository>();
            services.AddTransient<IShortUrlRepository, ShortUrlRepository>();
            services.AddTransient<IClickRepository, ClickRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork<HeyUrlDbContext>>();
        }

        public void ConfigureApplicationServices(IServiceCollection services)
        {
            services.AddTransient<IUrlApplication, UrlApplication>();
            services.AddTransient<IClickApplication, ClickApplication>();

            services.AddTransient<IValidatorFactory, ValidatorFactory>();
            services.AddTransient<IValidator<CreateClickRequestDto>, CreateClickRequestDtoValidator>();
            services.AddTransient<IValidator<CreateUrlRequestDto>, CreateUrlRequestDtoValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
				//Just for testing exception handling
                app.UseExceptionHandler("/Home/Error");
                //app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            if (env.IsDevelopment())
            {
                using var scope = app.ApplicationServices.CreateScope();
                var context = scope.ServiceProvider.GetService<HeyUrlDbContext>();
                context.Database.Migrate();
            }
        }
    }
}
