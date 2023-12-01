using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LoadBalancer.DataBase;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        SessionControl.Register(services, Configuration);
        // Interceptor?
    }


}