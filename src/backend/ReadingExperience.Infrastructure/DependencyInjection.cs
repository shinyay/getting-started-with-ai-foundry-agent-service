using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReadingExperience.Core.Interfaces;
using ReadingExperience.Core.Services;
using ReadingExperience.Infrastructure.Data;
using ReadingExperience.Infrastructure.Repositories;
using ReadingExperience.Infrastructure.Services;

namespace ReadingExperience.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<ReadingExperienceDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

        // Infrastructure Services
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddHttpClient<IExternalBookService, GoogleBooksService>();

        // Application Services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IBookService, BookService>();

        return services;
    }
}