using System;
using WindowsAzure.Tests.Samples;
using WindowsAzure.Table.EntityConverters.TypeData.Properties;
using System.Linq.Expressions;
using Microsoft.WindowsAzure.Storage.Table;
using Xunit;

namespace WindowsAzure.Tests.Table.EntityConverters.Properties
{
    public class ReverseMapPropertyTests
    {
        private readonly ReverseMapProperty<EntityForReverseMap, NestedEntity> _objectUnderTest;

        public ReverseMapPropertyTests()
        {
            Expression<Func<DynamicTableEntity, NestedEntity>> propertyTypeMapping = y =>
                    NestedEntity.Create(y.PartitionKey.Split('|', StringSplitOptions.RemoveEmptyEntries)[0]);

            var propertyInfo = typeof(EntityForReverseMap).GetProperty(nameof(EntityForReverseMap.NestedEntity));

            _objectUnderTest = new ReverseMapProperty<EntityForReverseMap, NestedEntity>(propertyTypeMapping, propertyInfo);
        }

        [Fact]
        public void CreatePartitionKeyMap()
        {
            // Assert
            Assert.NotNull(_objectUnderTest);
        }

        [Fact]
        public void SetReverseMapPropertyValue()
        {
            // Arrange
            var tableEntity = new DynamicTableEntity { PartitionKey = "26|" };
            var entity = new EntityForReverseMap();

            // Act
            _objectUnderTest.SetMemberValue(tableEntity, entity);

            // Assert
            Assert.Equal(26, entity.NestedEntity.DecimalValue);
        }

        [Fact]
        public void GetReverseMapPropertyShouldDoNothing()
        {
            // Arrange
            var tableEntity = new DynamicTableEntity { PartitionKey = "26|" };
            var entity = new EntityForReverseMap { NestedEntity = NestedEntity.Create("1") };

            // Act
            _objectUnderTest.GetMemberValue(entity, tableEntity);

            // Assert
            Assert.NotNull(entity);
            Assert.NotNull(entity.NestedEntity);

            Assert.NotEqual(26 , entity.NestedEntity.DecimalValue);
            Assert.Equal(1, entity.NestedEntity.DecimalValue);
        }

        [Fact]
        public void CreateReverseMapPropertyWithNullArguments()
        {
            // Arrange
            var propertyInfo = typeof(EntityForReverseMap).GetProperty(nameof(EntityForReverseMap.NestedEntity));

            // Act
            Assert.Throws<ArgumentNullException>(() => new ReverseMapProperty<EntityForReverseMap, NestedEntity>(null, propertyInfo));

            // Arrange
            Expression<Func<DynamicTableEntity, NestedEntity>> propertyTypeMapping = y =>
                    NestedEntity.Create(y.PartitionKey.Split('|', StringSplitOptions.RemoveEmptyEntries)[0]);

            // Act
            Assert.Throws<ArgumentNullException>(() => new ReverseMapProperty<EntityForReverseMap, NestedEntity>(propertyTypeMapping, null));
        }
    }
}
