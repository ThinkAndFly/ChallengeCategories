using System;
using System.Collections.Generic;
using Challenge.Application.Categories;
using Challenge.Domain.Entities;
using Challenge.Domain.Interfaces;

namespace Challenge.Test.Categories
{
    [TestClass]
    public sealed class TestCategory
    {
        [TestMethod]
        public void GetProperiesByCategoryId()
        {
            var categories = new List<Category>
            {
                new Category
                {
                    CaterogyId = 100,
                    ParentCaterogyId = -1,
                    Name = "Business",
                    Keyword = "Money"
                }
            };

            var fake = CreateFakeRepository(categories);

            var result = fake.GetProperiesByCategoryId(100);

            Assert.AreEqual("ParentCaterogyId=-1, Name=Business, Keyword=Money", result);
        }

        [TestMethod]
        public void GetProperiesByCategoryId_WithKeywordsFromParent()
        {
            var categories = new List<Category>
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
                    ParentCaterogyId = 100,
                    Name = "Taxation",
                    Keyword = string.Empty
                }
            };

            var fake = CreateFakeRepository(categories);

            var result = fake.GetProperiesByCategoryId(200);

            Assert.AreEqual("ParentCaterogyId=100, Name=Taxation, Keyword=Money", result);
        }

        [TestMethod]
        public void GetProperiesByCategoryId_WithUnknownId_Throws()
        {
            var fake = CreateFakeRepository(new List<Category>());

            Assert.Throws<Exception>(() => fake.GetProperiesByCategoryId(999));
        }

        private static CategoryApplication CreateFakeRepository(IEnumerable<Category> categories)
        {
            return new CategoryApplication(new FakeCategoryReadRepository(categories));
        }

        private class FakeCategoryReadRepository : ICategoryReadRepository
        {
            private readonly List<Category> _categories;

            public FakeCategoryReadRepository(IEnumerable<Category> categories)
            {
                _categories = new List<Category>(categories);
            }

            public List<Category> GetList()
            {
                return _categories;
            }
        }
    }
}
