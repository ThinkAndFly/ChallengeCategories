using System.Collections.Generic;
using Challenge.Domain.Entities;

namespace Challenge.Domain.Interfaces
{
    public interface ICategoryReadRepository
    {
        public List<Category> GetList();
    }
}