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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchMvc.Infra.IoC;

public static class DependencyInjectionAPI
{
    public static IServiceCollection AddInfrastructureAPI(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        serviceCollection.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        serviceCollection.AddScoped<IAuthenticate, AuthenticateService>();

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
