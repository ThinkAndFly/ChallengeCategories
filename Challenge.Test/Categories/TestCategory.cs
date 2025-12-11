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
        public async Task GetProperiesByCategoryId()
        {
            var categories = new List<Category>
            {
                new Category
                {
                    CategoryId = 100,
                    ParentCategoryId = -1,
                    Name = "Business",
                    Keyword = "Money"
                }
            };

            var fake = CreateFakeRepository(categories);

            var result = await fake.GetProperiesByCategoryId(100);

            Assert.AreEqual("ParentCaterogyId=-1, Name=Business, Keyword=Money", result);
        }

        [TestMethod]
        public async Task GetProperiesByCategoryId_WithKeywordsFromParent()
        {
            var categories = new List<Category>
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
                    ParentCategoryId = 100,
                    Name = "Taxation",
                    Keyword = string.Empty
                }
            };

            var fake = CreateFakeRepository(categories);

            var result = await fake.GetProperiesByCategoryId(200);

            Assert.AreEqual("ParentCaterogyId=100, Name=Taxation, Keyword=Money", result);
        }

        [TestMethod]
        public async Task GetProperiesByCategoryId_WithUnknownId_Throws()
        {
            var fake = CreateFakeRepository(new List<Category>());

            await Assert.ThrowsAsync<Exception>(() => fake.GetProperiesByCategoryId(999));
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

            public async Task<List<Category>> GetListAsync()
            {
                return _categories;
            }
        }
    }
}
