using SensorSim.Infrastructure.Repositories;
using SensorSim.Infrastructure.Tests.Fixtures;

namespace SensorSim.Infrastructure.Tests.Repositories;

public class TestEntityRepository : CrudMemoryRepository<TestEntity>
{
}

public class CrudMemoryRepositoryTests
{
    
    private CrudMemoryRepository<TestEntity> _repository = new TestEntityRepository();

    [Fact]
    public void Add_ShouldAddEntityToRepository()
    {
        // Arrange
        var entity = new TestEntity("1", "Entity1");

        // Act
        _repository.Add(entity);

        // Assert
        var result = _repository.Get("1");
        Assert.Equal(entity, result);
    }

    [Fact]
    public void Get_ShouldReturnEntityById()
    {
        // Arrange
        var entity = new TestEntity("1", "Entity1");
        _repository.Add(entity);

        // Act
        var result = _repository.Get("1");

        // Assert
        Assert.Equal(entity, result);
    }

    [Fact]
    public void Get_ShouldReturnNullIfEntityNotFound()
    {
        // Act
        var result = _repository.Get("non-existent-id");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetOrDefault_ShouldReturnExistingEntity()
    {
        // Arrange
        var entity = new TestEntity("1", "Entity1");
        _repository.Add(entity);

        // Act
        var result = _repository.GetOrDefault("1");

        // Assert
        Assert.Equal(entity, result);
    }

    [Fact]
    public void GetOrDefault_ShouldReturnNewEntityIfNotFound()
    {
        // Act
        var result = _repository.GetOrDefault("non-existent-id");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("non-existent-id", result.Id);
    }

    [Fact]
    public void GetOrCreate_ShouldReturnExistingEntity()
    {
        // Arrange
        var entity = new TestEntity("1", "Entity1");
        _repository.Add(entity);

        // Act
        var result = _repository.GetOrCreate("1", () => new TestEntity("1", "NewEntity"));

        // Assert
        Assert.Equal(entity, result);
    }

    [Fact]
    public void GetOrCreate_ShouldAddAndReturnNewEntityIfNotFound()
    {
        // Act
        var result = _repository.GetOrCreate("new-id", () => new TestEntity("new-id", "NewEntity"));

        // Assert
        Assert.NotNull(result);
        Assert.Equal("new-id", result.Id);
        Assert.Equal("NewEntity", result.Name);
    }

    [Fact]
    public void Update_ShouldUpdateEntity()
    {
        // Arrange
        var entity = new TestEntity("1", "Entity1");
        _repository.Add(entity);

        var updatedEntity = new TestEntity("1", "UpdatedEntity");

        // Act
        _repository.Update("1", updatedEntity);

        // Assert
        var result = _repository.Get("1");
        Assert.Equal(updatedEntity, result);
    }

    [Fact]
    public void Delete_ShouldRemoveEntity()
    {
        // Arrange
        var entity = new TestEntity("1", "Entity1");
        _repository.Add(entity);

        // Act
        _repository.Delete("1");

        // Assert
        var result = _repository.Get("1");
        Assert.Null(result);
    }
    
    [Fact]
    public void DeleteAll_ShouldRemoveAllEntities()
    {
        // Arrange
        var entity1 = new TestEntity("1", "Entity1");
        var entity2 = new TestEntity("2", "Entity2");
        _repository.Add(entity1);
        _repository.Add(entity2);

        // Act
        _repository.DeleteAll(new[] { entity1, entity2 });

        // Assert
        var result1 = _repository.Get("1");
        var result2 = _repository.Get("2");
        Assert.Null(result1);
        Assert.Null(result2);
    }

    [Fact]
    public void GetAll_ShouldReturnAllEntities()
    {
        // Arrange
        var entity1 = new TestEntity("1", "Entity1");
        var entity2 = new TestEntity("2", "Entity2");
        _repository.Add(entity1);
        _repository.Add(entity2);

        // Act
        var result = _repository.GetAll();

        // Assert
        Assert.Contains(entity1, result);
        Assert.Contains(entity2, result);
    }
}