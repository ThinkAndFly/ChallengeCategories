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
            Category? category = categories.Find(c => c.CategoryId == categoryId);

            if (category == null)
                throw new Exception("Category not found");

            var keyword = FindKeyword(category, categories);

            var builder = new StringBuilder();

            builder.Append(nameof(category.ParentCategoryId)).Append('=').Append(category.ParentCategoryId).Append(", ");
            builder.Append(nameof(category.Name)).Append('=').Append(category.Name ?? string.Empty).Append(", ");
            builder.Append(nameof(category.Keyword)).Append('=').Append(keyword);

            return builder.ToString();
        }

        public async Task<List<int>> GetCategoriesByLevel(int level)
        {
            List<Category> categories = await readRepository.GetListAsync();

            if (level <= 0) 
                throw new ArgumentOutOfRangeException(nameof(level), "Level must be greater than 0");

            // Map the categories
            var map = BuildLevelMap(categories);

            return map.TryGetValue(level, out var ids) ? ids : new List<int>();
        }



        private string FindKeyword(Category category, List<Category> Categories)
        {
            if (category is null)
                return string.Empty;

            // if the category has a keyword, return it.
            if (!string.IsNullOrEmpty(category.Keyword))
                return category.Keyword;

            //if the category does not have a parent id return empty;
            if (category.ParentCategoryId <= 0)
                return string.Empty;

            var parent = Categories.Find(c => c.CategoryId == category.ParentCategoryId);

            //if parent is not found, return empty.
            if (parent == null) 
                return string.Empty;

            return FindKeyword(parent, Categories);
        }


        private static Dictionary<int, List<int>> BuildLevelMap(IList<Category> categories)
        {
            int rootId = -1;

            if (categories == null) 
                throw new ArgumentNullException(nameof(categories));

            if (categories.Count == 0) 
                return new Dictionary<int, List<int>>();

            var categoryIds = categories.ToDictionary(c => c.CategoryId);

            // Cache for storing computed levels, max size is number of categories
            var levelMapCache = new Dictionary<int, int>(categories.Count);

            // Set to track categories currently being processed to prevent loops. This is not necessary for the sample data, but if the data changes in the future it can help prevent infinite recursion.
            var processingCategory = new HashSet<int>();

            int GetLevel(int id)
            {
                // if already mapped, return from cache
                if (levelMapCache.TryGetValue(id, out var mapped)) 
                    return mapped;

                // A category points to a non-existent category
                if (!categoryIds.TryGetValue(id, out var category))
                    throw new KeyNotFoundException($"Category id {id} not found in category list.");

                // Categories with parentId -1 are at 1st level
                if (category.ParentCategoryId == rootId)
                {
                    levelMapCache[id] = 1;
                    return 1;
                }

                // Parent not found
                if (!categoryIds.ContainsKey(category.ParentCategoryId))
                    throw new KeyNotFoundException($"Category id {category.ParentCategoryId} not found in category list.");

                // Loop prevention
                if (!processingCategory.Add(id))
                    throw new InvalidOperationException($"Loop detected while mapping the levels.");

                // Recurse to compute parent's depth
                var parentLevel = GetLevel(category.ParentCategoryId);

                // Remove from processing and cache the level
                processingCategory.Remove(id);
                var level = parentLevel + 1;
                levelMapCache[id] = level;
                return level;
            }

            // Map every category
            foreach (var c in categories)
                GetLevel(c.CategoryId);

            // Group ids by computed level
            var result = levelMapCache
                .GroupBy(map => map.Value) // value holds the mapped level
                .ToDictionary(g => g.Key, g => g.Select(map => map.Key).ToList()); // g.key holds the level. g.Select get all the categories at that level

            return result;
        }

    }
}
