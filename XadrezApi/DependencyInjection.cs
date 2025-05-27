using Microsoft.Extensions.DependencyInjection;
using Xadrez.API.Services;

namespace Xadrez.API;

public static class DependencyInjection
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<JogoDeXadrezService>();
        return services;
    }
}
