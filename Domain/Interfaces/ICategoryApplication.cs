namespace Challenge.Domain.Interfaces
{
    public interface ICategoryApplication
    {
        public Task<string> GetProperiesByCategoryId(int categoryId);
    }
}
