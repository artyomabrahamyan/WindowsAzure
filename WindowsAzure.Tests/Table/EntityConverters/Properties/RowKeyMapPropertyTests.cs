using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq.Expressions;
using WindowsAzure.Table.EntityConverters.TypeData.Properties;
using WindowsAzure.Tests.Samples;
using Xunit;

namespace WindowsAzure.Tests.Table.EntityConverters.Properties
{

    public sealed class RowKeyMapPropertyTests
    {

        private readonly RowKeyMapProperty<EntityWithMappedKey> _objectUnderTest;

        public RowKeyMapPropertyTests()
        {
            Expression<Func<EntityWithMappedKey, string>> propertyTypeMapping = x => x.StringVal + 26;
            _objectUnderTest = new RowKeyMapProperty<EntityWithMappedKey>(propertyTypeMapping);
        }


        [Fact]
        public void CreatRowKeyMap()
        {
            // Assert
            Assert.NotNull(_objectUnderTest);
        }

        [Fact]
        public void SetRowKeyMapPropertyShouldDoNothing()
        {
            // Arrange
            var tableEntity = new DynamicTableEntity { RowKey = "RKey" };
            var entity = new EntityWithMappedKey { StringVal = "26" };

            // Act
            _objectUnderTest.SetMemberValue(tableEntity, entity);

            // Assert
            Assert.NotNull(tableEntity.RowKey);
            Assert.NotEqual($"2626", tableEntity.RowKey);
            Assert.Equal("RKey", tableEntity.RowKey);
        }

        [Fact]
        public void GetRowKeyMapPropertyValue()
        {
            // Arrange
            var tableEntity = new DynamicTableEntity { RowKey = "RKey" };
            var entity = new EntityWithMappedKey { StringVal = "26" };

            // Act
            _objectUnderTest.GetMemberValue(entity, tableEntity);

            // Assert
            Assert.Equal($"2626", tableEntity.RowKey);
        }

        [Fact]
        public void CreateRowKeyMapPropertyMapWithNullArgument()
        {
            // Act
            Assert.Throws<ArgumentNullException>(() => new RowKeyMapProperty<EntityWithMappedKey>(null));
        }
    }
}

