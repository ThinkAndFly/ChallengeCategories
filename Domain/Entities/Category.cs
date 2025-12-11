namespace Challenge.Domain.Entities
{
    public class Category
    {
        public int CaterogyId { get; set; }
        public int ParentCaterogyId { get; set; }
        public string Name { get; set; }
        public string Keyword { get; set; }
    }
}
