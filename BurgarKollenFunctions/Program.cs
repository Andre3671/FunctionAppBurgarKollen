using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using BurgarKollenFunctions;
using BurgarKollenFunctions.Services;
using Microsoft.Extensions.Configuration;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(s =>
    {
        s.AddDataService();
        s.AddTransient<DataService>();
        s.AddOptions<DataServiceOptions>()
    .Configure<IConfiguration>((settings, configuration) =>
    {
        configuration.GetSection(nameof(DataServiceOptions)).Bind(settings);
    });
    })
    .Build();

host.Run();
