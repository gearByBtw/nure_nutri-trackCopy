using Microsoft.EntityFrameworkCore;
using NutritionalRecipeBook.Domain.Entities;
using NutritionalRecipeBook.Infrastructure.Repositories;

namespace NutritionalRecipeBook.Infrastructure.Tests
{
    public class GenericRepositoryUnitTests
    {
        private DbContextOptions<RecipeBookContext> _options;
        private GenericRepository<Category> _repository;

        [SetUp]
        public void SetUp()
        {
            _options = new DbContextOptionsBuilder<RecipeBookContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        
        public async Task GetAll_ShouldReturnAllEntitiesAsync()
        {
            var entities = GetFakeEntities();

            using (var context = new RecipeBookContext(_options))
            {
                context.AddRange(entities);
                await context.SaveChangesAsync();

                _repository = new GenericRepository<Category>(context);

                var result = await _repository.GetAllAsync();

                Assert.AreEqual(entities.Count(), result.Count());
                CollectionAssert.AreEquivalent(entities, result);
            }
        }

        
        public async Task Create_ShouldCreateNewEntityAsync()
        {
            var entityToCreate = new Category("NewEntity");

            using (var context = new RecipeBookContext(_options))
            {
                _repository = new GenericRepository<Category>(context);

                await _repository.CreateAsync(entityToCreate);
                await context.SaveChangesAsync();

                var createdEntity = await context.Categories.FindAsync(entityToCreate.Id);
                Assert.NotNull(createdEntity);
                Assert.AreEqual(entityToCreate.Name, createdEntity.Name);
            }
        }

        
        public async Task GetById_ShouldReturnEntityByIdAsync()
        {
            var entities = GetFakeEntities();
            var entityIdToGet = entities.First().Id;

            using (var context = new RecipeBookContext(_options))
            {
                context.AddRange(entities);
                await context.SaveChangesAsync();

                _repository = new GenericRepository<Category>(context);
                var entity = await _repository.GetByIdAsync(entityIdToGet);

                Assert.NotNull(entity);
                Assert.AreEqual(entities.First().Name, entity.Name);
            }
        }

        
        public async Task Remove_ShouldRemoveEntityAsync()
        {
            var entities = GetFakeEntities();
            var entityIdToRemove = entities.First().Id;

            using (var context = new RecipeBookContext(_options))
            {
                _repository = new GenericRepository<Category>(context);

                context.AddRange(entities);
                await context.SaveChangesAsync();

                _repository.RemoveByIdAsync(entityIdToRemove);
                await context.SaveChangesAsync();
                var remainingEntities = await _repository.GetAllAsync();

                Assert.AreEqual(entities.Count() - 1, remainingEntities.Count());
                Assert.IsFalse(remainingEntities.Any(e => e.Id == entityIdToRemove));
            }
        }

        private List<Category> GetFakeEntities()
        {
            return new List<Category>
            {
                new Category("Entity1"),
                new Category("Entity2"),
                new Category("Entity3")
            };
        }
    }
}
