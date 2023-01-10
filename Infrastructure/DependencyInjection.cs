using Application.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Application.Repository;
using Infrastructure.Repository;
using Application;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration) {

            services.AddDbContext<ApplicationDBContext>(options => 
                options.UseSqlServer(configuration.GetConnectionString("TinyUrlDatabase"), 
                b => b.MigrationsAssembly(typeof(ApplicationDBContext).Assembly.FullName)), ServiceLifetime.Transient);
            
            services.AddScoped<IApplicationDBContext>(provider => provider.GetService<ApplicationDBContext>());

            services.AddScoped<ITinyUrlRepository,TinyUrlRepository>();
            services.AddScoped<ITinyUrlCreator, TinyUrlCreator>();
            services.AddScoped<IUserRepository, UserRepository>();



            //        services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //.AddEntityFrameworkStores<ApplicationDBContext>();
            return services;
        }
    }
}
