using Challenge.Application.Categories;
using Challenge.Domain.Interfaces;
using Challenge.Infraestructure.DataAccess.Categories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);


builder.Services.AddScoped<ICategoryApplication, CategoryApplication>();
builder.Services.AddScoped<ICategoryReadRepository, SampleCategoryReadRepository>();

using var host = builder.Build();

// I create a Scope to work with the required example and share the same instance of the servicies.
using var scope = host.Services.CreateScope();

var categoryApplication = scope.ServiceProvider.GetRequiredService<ICategoryApplication>();

Console.Write("Enter category id: ");
var input = Console.ReadLine();

if (!int.TryParse(input, out var categoryId))
{
    Console.WriteLine("Invalid category id.");
    return;
}

try
{
    var properties = categoryApplication.GetProperiesByCategoryId(categoryId);
    Console.WriteLine(properties);
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
finally
{
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey(true);
}
