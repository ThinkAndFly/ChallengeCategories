using Challenge.Application.Categories;
using Challenge.Domain.Interfaces;
using Challenge.Infraestructure.DataAccess.Categories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);


builder.Services.AddScoped<ICategoryApplication, CategoryApplication>();
builder.Services.AddScoped<ICategoryReadRepository, SampleCategoryReadRepository>();

using var host = builder.Build();

// This Scope is to work with the required example and share the same instance of the servicies.
using var scope = host.Services.CreateScope();

var categoryApplication = scope.ServiceProvider.GetRequiredService<ICategoryApplication>();

while (true)
{
    Console.WriteLine("\n=== Category Menu ===");
    Console.WriteLine("P - Print category parameters");
    Console.WriteLine("L - Get categories by level");
    Console.WriteLine("E - Exit");
    Console.Write("\nSelect an option: ");

    var option = Console.ReadLine()?.Trim().ToUpper();

    if (option == "E")
    {
        Console.WriteLine("Exiting...");
        break;
    }

    try
    {
        if (option == "P")
        {
            Console.Write("Enter category id: ");
            var input = Console.ReadLine();

            if (!int.TryParse(input, out var categoryId))
            {
                Console.WriteLine("Invalid category id.");
                continue;
            }

            var properties = await categoryApplication.GetProperiesByCategoryId(categoryId);
            Console.WriteLine($"\nCategory Properties:\n{properties}");
        }
        else if (option == "L")
        {
            Console.Write("Enter level: ");
            var input = Console.ReadLine();

            if (!int.TryParse(input, out var level))
            {
                Console.WriteLine("Invalid level.");
                continue;
            }

            var categories = await categoryApplication.GetCategoriesByLevel(level);

            if (categories.Count == 0)
            {
                Console.WriteLine($"\nNo categories found at level {level}.");
            }
            else
            {
                Console.WriteLine($"\nCategories at level {level}:");
                Console.WriteLine(string.Join(", ", categories));
            }
        }
        else
        {
            Console.WriteLine("Invalid option. Please select P, L, or E.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}