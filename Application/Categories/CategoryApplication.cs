using Challenge.Domain.Entities;
using Challenge.Domain.Interfaces;
using System.Text;

namespace Challenge.Application.Categories
{
    public class CategoryApplication(ICategoryReadRepository readRepository) : ICategoryApplication
    {
        public async Task<string> GetProperiesByCategoryId(int categoryId)
        {
            List<Category> categories = await readRepository.GetListAsync();
            Category? category = categories.Find(c => c.CaterogyId == categoryId);

            if (category == null)
                throw new Exception("Category not found");

            var keyword = FindKeyword(category, categories);

            var builder = new StringBuilder();

            builder.Append(nameof(category.ParentCaterogyId)).Append('=').Append(category.ParentCaterogyId).Append(", ");
            builder.Append(nameof(category.Name)).Append('=').Append(category.Name ?? string.Empty).Append(", ");
            builder.Append(nameof(category.Keyword)).Append('=').Append(keyword);

            return builder.ToString();
        }

        public async Task<List<int>> GetCategoriesByLevel(int level)
        {
            List<Category> categories = await readRepository.GetListAsync();
            return [];
        }

        private string FindKeyword(Category category, List<Category> Categories)
        {
            if (category is null)
                return string.Empty;

            // if the category has a keyword, return it.
            if (!string.IsNullOrEmpty(category.Keyword))
                return category.Keyword;

            //if the category does not have a parent id return empty;
            if (category.ParentCaterogyId <= 0)
                return string.Empty;

            var parent = Categories.Find(c => c.CaterogyId == category.ParentCaterogyId);

            //if parent is not found, return empty.
            if (parent == null) 
                return string.Empty;

            return FindKeyword(parent, Categories);
        }

    }
}
