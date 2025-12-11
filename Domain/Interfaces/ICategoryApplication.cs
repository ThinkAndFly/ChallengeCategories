namespace Challenge.Domain.Interfaces
{
    public interface ICategoryApplication
    {
        public Task<string> GetProperiesByCategoryId(int categoryId);
        public Task<List<int>> GetCategoriesByLevel(int level);
    }
}
