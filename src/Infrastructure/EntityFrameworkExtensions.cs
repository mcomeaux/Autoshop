using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace Autoshop.Infrastructure
{
    public static class EntityFrameworkExtensions
    {
        public static IConfigurationBuilder AddEfConfiguration(this IConfigurationBuilder builder,
            Action<DbContextOptionsBuilder> optionsAction)
        {
            return builder.Add(new EntityFrameworkConfigurationSource(optionsAction));
        }
    }
}
