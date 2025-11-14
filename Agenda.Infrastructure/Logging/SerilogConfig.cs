using Microsoft.Extensions.Configuration;
using Serilog;

namespace Agenda.Infrastructure.Logging;

public static class SerilogConfig
{
    public static void Configure(IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .CreateLogger();
    }
}
