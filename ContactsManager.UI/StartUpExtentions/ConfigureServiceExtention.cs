using ContactsManager.Core.Domain.IdentityEntities;
using CRUDEXAMPLE.Filters.ActionFilters;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using Services;

namespace CRUDEXAMPLE
{
    public static class ConfigureServiceExtention
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection Services , IConfiguration Configuration , string Enviroment)
        {
            Services.AddScoped<ICountriesRepository, CountriesRepository>();
            Services.AddScoped<IPersonsRepository, PersonsRepository>();
            Services.AddScoped<ICountriesService, CountriesServies>();
            Services.AddScoped<IPersonsGetterService, PersonGetterServiceWithFewExcelFields>();
            Services.AddScoped<PersonsGetterService, PersonsGetterService>();
            Services.AddScoped<IPersonsAdderService, PersonsAdderService>();
            Services.AddScoped<IPersonsDeleterService, PersonsDeleterService>();
            Services.AddScoped<IPersonsUpdaterService, PersonsUpdaterService>();
            Services.AddScoped<IPersonsSorterService, PersonsSorterService>();
            Services.AddTransient<PersonListActionFilter>();
            Services.AddTransient<ResponseHeaderActionFilter>();
            if(Enviroment != "test")
            Services.AddDbContext<ApplicationDbContext>(options => {
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));});
            Services.AddIdentity<ApplicationUser, ApplicationRole>(options => { options.Password.RequiredLength = 5; options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredUniqueChars = 3;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddUserStore<UserStore<ApplicationUser, ApplicationRole , ApplicationDbContext , Guid>>()
            .AddRoleStore<RoleStore<ApplicationRole , ApplicationDbContext , Guid>>();
            Services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build(); // enforces authorization policy (user must be authinticated) for all action method
                options.AddPolicy("NotAuthed", policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        return !context.User.Identity.IsAuthenticated;
                    });
                });
            });
            Services.ConfigureApplicationCookie(options => options.LoginPath = "/Account/Login");
            return Services;
        }
    }
}
