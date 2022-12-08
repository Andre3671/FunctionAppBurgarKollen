using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using BurgarKollenFunctions;
using BurgarKollenFunctions.Services;

var configuration = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .AddEnvironmentVariables()
                .Build();

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(s =>
    {


        var connectionString = configuration["DBCS"];
        if (connectionString is null)
        {
            connectionString = Environment.GetEnvironmentVariable("DBCS");
        }

        //var serverVersion = ServerVersion.AutoDetect(connectionString);
        s.AddDbContext<AppDbContext>(dbContextOptions => 
            dbContextOptions.UseMySQL(connectionString)
        );
        s.AddTransient<DataService>();
    })
    .Build();


host.Run();
