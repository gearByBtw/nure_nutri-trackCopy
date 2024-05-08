using Serilog;

namespace NutritionalRecipeBook.Api.Configurations
{
    public static class ConfigureLogging
    {
        public static WebApplicationBuilder AddApplicationLogging(this WebApplicationBuilder builder, IConfiguration config)
        {
            builder.Host.UseSerilog((context, configuration) =>
               configuration.ReadFrom.Configuration(context.Configuration));
            return builder;
        }
    }
}
