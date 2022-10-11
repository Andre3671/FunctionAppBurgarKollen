using FunctionAppBurgarKollen;
using BurgarKollen.Functions2.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BurgarKollen.Functions2;

[assembly: FunctionsStartup(typeof(Startup))]
namespace FunctionAppBurgarKollen
{
    internal class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<Startup>()
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration["DBCS"];
            if (connectionString is null)
            {
                connectionString = Environment.GetEnvironmentVariable("DBCS");
            }

            var serverVersion = ServerVersion.AutoDetect(connectionString);

            builder.Services.AddDbContext<AppDbContext>(
                dbContextOptions => dbContextOptions
                    .UseMySql(connectionString, serverVersion)
            );

            builder.Services.AddTransient<DataService>();
        }
    }
}
