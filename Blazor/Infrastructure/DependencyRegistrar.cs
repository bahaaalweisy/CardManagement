using Microsoft.AspNetCore.Identity;

using CardManagement.Core;
using CardManagement.Core.Domain.Users;
using CardManagement.Infrastructure.Context;
using CardManagement.Services.Interfaces;
using CardManagement.Services.Contacts;
using CardManagement.Services.Users;
using CardManagement.Services.Common;
using CardManagement.Services.Cards;

namespace CardManagement.Web.Api.Infrastructure
{
    public static class DependencyRegistrar
    {
        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Program));

            services.AddIdentity<User, UserRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<CardManagementDbDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ICommonService, CommonService>();

            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICardService, CardService>();           
            services.AddHttpClient();
        }
    }
}
