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

            Assert.AreEqual("ParentCategoryId=-1, Name=Business, Keyword=Money", result);
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

            Assert.AreEqual("ParentCategoryId=100, Name=Taxation, Keyword=Money", result);
        }

        [TestMethod]
        public async Task GetProperiesByCategoryId_WithUnknownId_Throws()
        {
            var fake = CreateFakeRepository(new List<Category>());

            await Assert.ThrowsAsync<Exception>(() => fake.GetProperiesByCategoryId(999));
        }

        [TestMethod]
        public async Task GetCategoriesByLevel_ReturnsCorrectCategoriesAtLevel()
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
                },
                new Category
                {
                    CategoryId = 300,
                    ParentCategoryId = 100,
                    Name = "Finance",
                    Keyword = "Investment"
                },
                new Category
                {
                    CategoryId = 400,
                    ParentCategoryId = 200,
                    Name = "Corporate Tax",
                    Keyword = string.Empty
                }
            };

            var fake = CreateFakeRepository(categories);

            // Level 1: Should return category 100
            var level1 = await fake.GetCategoriesByLevel(1);
            Assert.HasCount(1, level1);
            Assert.Contains(100, level1);

            // Level 2: Should return categories 200 and 300
            var level2 = await fake.GetCategoriesByLevel(2);
            Assert.HasCount(2, level2);
            Assert.Contains(200, level2);
            Assert.Contains(300, level2);

            // Level 3: Should return category 400
            var level3 = await fake.GetCategoriesByLevel(3);
            Assert.HasCount(1, level3);
            Assert.Contains(400, level3);

            // Level 4: Should return empty list
            var level4 = await fake.GetCategoriesByLevel(4);
            Assert.HasCount(0, level4);
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
