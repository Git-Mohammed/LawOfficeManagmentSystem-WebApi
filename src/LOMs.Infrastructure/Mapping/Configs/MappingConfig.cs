using System.Reflection;
using Mapster;

namespace LOMs.Infrastructure.Mapping.Configs
{
    public class MappingConfig
    {
        public static void Configure()
        {
            TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
        }
    }
}