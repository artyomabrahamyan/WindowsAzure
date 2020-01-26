using Microsoft.WindowsAzure.Storage.Table;
using System;
using WindowsAzure.Table.EntityConverters.TypeData.Properties;
using WindowsAzure.Tests.Samples;
using Xunit;

namespace WindowsAzure.Tests.Table.EntityConverters.Properties
{
    public class MapPropertyTests
    {
        private readonly MapProperty<EntityWithMappedProperty> _objectUnderTest;

        public MapPropertyTests()
        {
            _objectUnderTest = new MapProperty<EntityWithMappedProperty>(y => y.EntityId.ToString(), nameof(EntityWithMappedProperty.EntityId));
        }

        [Fact]
        public void CreatePartitionKeyMap()
        {
            // Act & Assert
            Assert.NotNull(_objectUnderTest);
        }

        [Fact]
        public void SetMapPropertyShouldDoNothing()
        {
            // Arrange
            var tableEntity = new DynamicTableEntity { PartitionKey = "Key" };
            var entity = new EntityWithMappedProperty { Pk = "Pk", Rk = "Rk", EntityId = new EntityWithMappedProperty.Id(25, 99) };

            // Act
            _objectUnderTest.SetMemberValue(tableEntity, entity);

            // Assert
            Assert.NotNull(tableEntity.PartitionKey);
            Assert.Empty(tableEntity.Properties);
            Assert.Equal("Key", tableEntity.PartitionKey);
        }

        [Fact]
        public void GetMapPropertyValueShouldAddProperty()
        {
            // Arrange
            var tableEntity = new DynamicTableEntity { PartitionKey = "Key" };
            var entity = new EntityWithMappedProperty { Pk = "Pk", Rk = "Rk", EntityId = new EntityWithMappedProperty.Id(25, 99) };

            // Act
            _objectUnderTest.GetMemberValue(entity, tableEntity);

            //Assert
            Assert.NotNull(tableEntity.PartitionKey);
            Assert.NotEmpty(tableEntity.Properties);
            Assert.Equal(entity.EntityId.ToString(), tableEntity.Properties[nameof(EntityWithMappedProperty.EntityId)].StringValue);
        }

        [Fact]
        public void CreateMapProperty_NullSourceArgument_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new MapProperty<EntityWithMappedProperty>(null, "name"));
        }

        [Fact]
        public void CreateMapProperty_NullOrEmptyPropertyNameArgument_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new MapProperty<EntityWithMappedProperty>(y => y.EntityId.ToString(), null));                
            Assert.Throws<ArgumentNullException>(() => new MapProperty<EntityWithMappedProperty>(y => y.EntityId.ToString(), string.Empty));
        }
    }
}
