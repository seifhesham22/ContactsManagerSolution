using ContactsManager.Core.Domain.IdentityEntities;
using CRUDEXAMPLE.Filters.ActionFilters;
using Entities;
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
            Services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddUserStore<UserStore<ApplicationUser, ApplicationRole , ApplicationDbContext , Guid>>()
            .AddRoleStore<RoleStore<ApplicationRole , ApplicationDbContext , Guid>>();
            return Services;
        }
    }
}
