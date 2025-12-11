using Challenge.Domain.Entities;
using Challenge.Domain.Interfaces;

namespace Challenge.Infraestructure.DataAccess.Categories
{
    public sealed class SampleCategoryReadRepository : ICategoryReadRepository
    {
        private readonly List<Category> _categories;

        public SampleCategoryReadRepository()
        {
            _categories = LoadCategories();
        }

        public async Task<List<Category>> GetListAsync()
        {
            // normally this would be an async call to a database
            return _categories;
        }

        private static List<Category> LoadCategories()
        {
            return new List<Category>
            {
                new Category
                {
                    CategoryId = 100,
                    ParentCategoryId = -1,
                    Name = "Business",
                    Keyword = "Money"
                },
                new Category
                {
                    CategoryId = 200,
                    ParentCategoryId = -1,
                    Name = "Tutoring",
                    Keyword = "Teaching"
                },
                new Category
                {
                    CategoryId = 101,
                    ParentCategoryId = 100,
                    Name = "Accounting",
                    Keyword = "Taxes"
                },
                new Category
                {
                    CategoryId = 102,
                    ParentCategoryId = 100,
                    Name = "Taxation"
                },
                new Category
                {
                    CategoryId = 201,
                    ParentCategoryId = 200,
                    Name = "Computer"
                },
                new Category
                {
                    CategoryId = 103,
                    ParentCategoryId = 101,
                    Name = "Corporate Tax"
                },
                new Category
                {
                    CategoryId = 202,
                    ParentCategoryId = 201,
                    Name = "Operating System"
                },
                new Category
                {
                    CategoryId = 109,
                    ParentCategoryId = 101,
                    Name = "Small Business Tax"
                }
            };
        }
    }
}