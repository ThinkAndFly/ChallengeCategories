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

        public List<Category> GetList()
        {
            return _categories;
        }

        private static List<Category> LoadCategories()
        {
            return new List<Category>
            {
                new Category
                {
                    CaterogyId = 100,
                    ParentCaterogyId = -1,
                    Name = "Business",
                    Keyword = "Money"
                },
                new Category
                {
                    CaterogyId = 200,
                    ParentCaterogyId = -1,
                    Name = "Tutoring",
                    Keyword = "Teaching"
                },
                new Category
                {
                    CaterogyId = 101,
                    ParentCaterogyId = 100,
                    Name = "Accounting",
                    Keyword = "Taxes"
                },
                new Category
                {
                    CaterogyId = 102,
                    ParentCaterogyId = 100,
                    Name = "Taxation"
                },
                new Category
                {
                    CaterogyId = 201,
                    ParentCaterogyId = 200,
                    Name = "Computer"
                },
                new Category
                {
                    CaterogyId = 103,
                    ParentCaterogyId = 101,
                    Name = "Corporate Tax"
                },
                new Category
                {
                    CaterogyId = 202,
                    ParentCaterogyId = 201,
                    Name = "Operating System"
                },
                new Category
                {
                    CaterogyId = 109,
                    ParentCaterogyId = 101,
                    Name = "Small Business Tax"
                }
            };
        }
    }
}