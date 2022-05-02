using Serilog;
using Serilog.Core;

namespace GFL.Infra
{
    public static class AppSerilogConfiguration
    {
        public static Logger Configure() =>
               new LoggerConfiguration()
                   .WriteTo.Console()
#if DEBUG
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Debug)
                   .MinimumLevel.Is(Serilog.Events.LogEventLevel.Debug)
#else
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
#endif
                .MinimumLevel.Is(Serilog.Events.LogEventLevel.Information)
                   .CreateLogger();
    }
}

