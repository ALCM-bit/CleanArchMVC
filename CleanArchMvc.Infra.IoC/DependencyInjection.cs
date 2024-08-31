using System.Reflection;
using CleanArchMvc.Application.Interfaces;
using CleanArchMvc.Application.Mappings;
using CleanArchMvc.Application.Services;
using CleanArchMvc.Domain.Account;
using CleanArchMvc.Domain.Interfaces;
using CleanArchMvc.Infra.Data.Context;
using CleanArchMvc.Infra.Data.Identity;
using CleanArchMvc.Infra.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchMvc.Infra.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddDbContext<ApplicationDbContext>(options => 
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), 
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        serviceCollection.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        serviceCollection.ConfigureApplicationCookie(options => 
            options.AccessDeniedPath = "/Account/Login");
        
        serviceCollection.AddScoped<IAuthenticate, AuthenticateService>();
        serviceCollection.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();

        serviceCollection.AddAutoMapper(typeof(DomainToDtoMappingProfile));
        serviceCollection.AddAutoMapper(typeof(DTOToCommandMappingProfile));

        serviceCollection.AddScoped<ICategoryRepository, CategoryRepository>();        
        serviceCollection.AddScoped<IProductRepository, ProductRepository>();

        serviceCollection.AddScoped<IProductService, ProductService>();
        serviceCollection.AddScoped<ICategoryService, CategoryService>();

        var myHandlers = AppDomain.CurrentDomain.Load("CleanArchMvc.Application");
        serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(myHandlers));

        

        return serviceCollection;

    }
}