using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Option;
public static class Extensions
{
    public static T LoadOptions<T>(this IConfiguration configuration, string sectionName) where T : IOptionsRoot
    {
        T options = configuration.GetSection(sectionName).Get<T>() ?? throw new Exception(sectionName);
        return options;
    }

    public static T BindValidateReturn<T>(this IServiceCollection services, IConfiguration configuration) where T : class, IOptionsRoot
    {
        services.AddOptions<T>()
            .BindConfiguration(typeof(T).Name)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        return configuration.LoadOptions<T>(typeof(T).Name);
    }
    public static void BindValidate<T>(this IServiceCollection services) where T : class, IOptionsRoot
    {
        services.AddOptions<T>()
            .BindConfiguration(typeof(T).Name)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}
