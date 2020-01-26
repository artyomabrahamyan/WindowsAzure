using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq.Expressions;
using WindowsAzure.Table.EntityConverters.TypeData.Properties;
using WindowsAzure.Tests.Samples;
using Xunit;

namespace WindowsAzure.Tests.Table.EntityConverters.Properties
{
    public class PartitionKeyMapPropertyTests
    {
        private readonly PartitionKeyMapProperty<EntityWithMappedKey> _objectUnderTest;

        public PartitionKeyMapPropertyTests()
        {
            Expression<Func<EntityWithMappedKey, string>> propertyTypeMapping = x => x.IntVal + "|";
            _objectUnderTest = new PartitionKeyMapProperty<EntityWithMappedKey>(propertyTypeMapping);
        }


        [Fact]
        public void CreatePartitionKeyMap()
        {
            // Assert
            Assert.NotNull(_objectUnderTest);
        }

        [Fact]
        public void SetPartitionKeyMapPropertyShouldDoNothing()
        {
            // Arrange
            var tableEntity = new DynamicTableEntity { PartitionKey = "Key" };
            var entity = new EntityWithMappedKey { IntVal = 26 };

            // Act
            _objectUnderTest.SetMemberValue(tableEntity, entity);

            // Assert
            Assert.NotNull(tableEntity.PartitionKey);
            Assert.NotEqual($"{entity.IntVal}|", tableEntity.PartitionKey);
            Assert.Equal("Key", tableEntity.PartitionKey);
        }

        [Fact]
        public void GetPartitionKeyMapPropertyValue()
        {
            // Arrange
            var tableEntity = new DynamicTableEntity { PartitionKey = "Key" };
            var entity = new EntityWithMappedKey { IntVal = 26 };

            // Act
            _objectUnderTest.GetMemberValue(entity, tableEntity);

            // Assert
            Assert.Equal($"{entity.IntVal}|", tableEntity.PartitionKey);
        }

        [Fact]
        public void CreatePartitionKeyPropertyMapWithNullArgument()
        {
            // Act
            Assert.Throws<ArgumentNullException>(() => new PartitionKeyMapProperty<EntityWithFields>(null));
        }
    }
}

