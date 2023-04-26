using WebApi.Utilities.Formatters;

namespace WebApi.Extensions;

public static class MvBuilderExtensions
{
    public static IMvcBuilder AddCustomCsvFormetter(this IMvcBuilder builder) =>
        builder.AddMvcOptions(config => config.OutputFormatters.Add(new CsvOutputFormatter()));
}