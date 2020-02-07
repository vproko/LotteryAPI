using Lottery.DataAccess;
using Lottery.DataAccess.Interfaces;
using Lottery.DataAccess.Repositories;
using Lottery.DomainClasses.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Lottery.Services.Helpers
{
    public class DiModule
    {
        public static IServiceCollection RegisterModules(IServiceCollection services, string connectionString)
        {
            services.AddTransient<IRepository<User>, UserRepository>();
            services.AddTransient<IRepository<Ticket>, TicketRepository>();
            services.AddTransient<IRepository<Draw>, DrawRepository>();
            services.AddTransient<IRepository<Session>, SessionRepository>();
            services.AddTransient<IRepository<Winner>, WinnerRepository>();
            services.AddTransient<IRepository<Prize>, PrizeRepository>();
            services.AddDbContext<LotteryDbContext>(x => x.UseSqlServer(connectionString));

            return services;
        }
    }
}
