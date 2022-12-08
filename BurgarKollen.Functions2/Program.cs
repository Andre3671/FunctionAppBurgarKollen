using BurgarKollenFunctions.Functions2.Services;
using BurgarKollenFunctions.Functions2;
using FunctionAppBurgarKollen;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    
    .Build();

host.Run();

    
