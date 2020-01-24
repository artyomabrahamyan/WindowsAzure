﻿using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Moq;
using WindowsAzure.Table.EntityConverters;
using WindowsAzure.Table.EntityConverters.TypeData.Serializers;
using WindowsAzure.Tests.Samples;
using Xunit;

namespace WindowsAzure.Tests.Table.EntityConverters
{
    public sealed class TableEntityConverterTests
    {
        [Fact]
        public void CreateConverter()
        {
            // Arrange & Act
            var converter = new TableEntityConverter<Country>();

            // Assert
            Assert.NotNull(converter.NameChanges);
            Assert.Equal(2, converter.NameChanges.Count);
            Assert.Equal("PartitionKey", converter.NameChanges["Continent"]);
            Assert.Equal("RowKey", converter.NameChanges["Name"]);
        }

        // ReSharper disable PossibleInvalidOperationException

        [Fact]
        public void ConvertToTableEntity()
        {
            // Arrange
            var converter = new TableEntityConverter<Country>();
            var country = new Country
                {
                    Area = 505992,
                    Continent = "Europe",
                    TopSecretKey = new byte[] {0xaa, 0xbb, 0xcc},
                    Formed = new DateTime(1812, 1, 1),
                    Id = Guid.NewGuid(),
                    IsExists = true,
                    Name = "Spain",
                    Population = 47190493,
                    PresidentsCount = 8
                };

            var context = new OperationContext();

            // Act
            ITableEntity tableEntity = converter.GetEntity(country);
            IDictionary<string, EntityProperty> properties = tableEntity.WriteEntity(context);

            // Assert
            Assert.Equal("*", tableEntity.ETag);
            Assert.Equal(country.Continent, tableEntity.PartitionKey);
            Assert.Equal(country.Name, tableEntity.RowKey);
            Assert.Equal(country.Area, properties["Area"].DoubleValue);
            Assert.Equal(country.TopSecretKey, properties["TopSecretKey"].BinaryValue);
            Assert.True(properties["Formed"].DateTimeOffsetValue.HasValue);
            Assert.Equal(country.Formed, properties["Formed"].DateTimeOffsetValue.Value.DateTime);
            Assert.Equal(country.Id, properties["Id"].GuidValue);
            Assert.Equal(country.IsExists, properties["IsExists"].BooleanValue);
            Assert.Equal(country.Population, properties["Population"].Int64Value);
            Assert.Equal(country.PresidentsCount, properties["PresidentsCount"].Int32Value);
        }

        [Fact]
        public void ConvertToEntity()
        {
            // Arrange
            var converter = new TableEntityConverter<Country>();
            var tableEntity = new DynamicTableEntity
                {
                    PartitionKey = "Europe",
                    RowKey = "Spain",
                    Properties = new Dictionary<string, EntityProperty>
                        {
                            {"Area", new EntityProperty(505992d)},
                            {
                                "TopSecretKey",
                                new EntityProperty(new byte[] {0xaa, 0xbb, 0xcc})
                            },
                            {"Formed", new EntityProperty(new DateTime(1812, 1, 1))},
                            {"Id", new EntityProperty(Guid.NewGuid())},
                            {"IsExists", new EntityProperty(true)},
                            {"Population", new EntityProperty(47190493L)},
                            {"PresidentsCount", new EntityProperty(8)},
                        }
                };

            // Act
            Country country = converter.GetEntity(tableEntity);

            // Assert
            Assert.Equal(country.Continent, tableEntity.PartitionKey);
            Assert.Equal(country.Name, tableEntity.RowKey);
            Assert.Equal(country.Area, tableEntity.Properties["Area"].DoubleValue);
            Assert.Equal(country.TopSecretKey, tableEntity.Properties["TopSecretKey"].BinaryValue);
            Assert.True(tableEntity.Properties["Formed"].DateTimeOffsetValue.HasValue);
            Assert.Equal(country.Formed, tableEntity.Properties["Formed"].DateTimeOffsetValue.Value.DateTime);
            Assert.Equal(country.Id, tableEntity.Properties["Id"].GuidValue);
            Assert.Equal(country.IsExists, tableEntity.Properties["IsExists"].BooleanValue);
            Assert.Equal(country.Population, tableEntity.Properties["Population"].Int64Value);
            Assert.Equal(country.PresidentsCount, tableEntity.Properties["PresidentsCount"].Int32Value);
        }

        // ReSharper restore PossibleInvalidOperationException

        [Fact]
        public void ConvertToTableEntityWithFewKeys()
        {
            // Arrange
            var converter = new TableEntityConverter<LogEntry>();
            var entry = new LogEntry
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ETag = "MyETag",
                    Timestamp = DateTime.UtcNow,
                    Message = "My message",
                    PrivateData = new byte[] {0xaa, 0xbb, 0xcc}
                };

            var context = new OperationContext();

            // Act
            ITableEntity tableEntity = converter.GetEntity(entry);
            IDictionary<string, EntityProperty> properties = tableEntity.WriteEntity(context);

            // Assert
            Assert.Equal(entry.ETag, tableEntity.ETag);
            Assert.Equal(entry.Id, tableEntity.PartitionKey);
            Assert.Equal(string.Empty, tableEntity.RowKey);
            Assert.Equal(default(DateTimeOffset), tableEntity.Timestamp);
            Assert.Equal(entry.Message, properties["OldMessage"].StringValue);
            Assert.DoesNotContain("PrivateData", properties.Keys);
        }

        [Fact]
        public void ConvertToTableEntityWithNameMapping()
        {
            // Arrange
            var converter = new TableEntityConverter<LogEntry>();
            var entity = new LogEntry
                {
                    Id = "My id",
                    Message = "My message"
                };

            var context = new OperationContext();

            // Act
            ITableEntity tableEntity = converter.GetEntity(entity);
            IDictionary<string, EntityProperty> properties = tableEntity.WriteEntity(context);

            // Assert
            Assert.Equal(null, tableEntity.ETag);
            Assert.Equal(entity.Id, tableEntity.PartitionKey);
            Assert.Equal(string.Empty, tableEntity.RowKey);
            Assert.Equal(entity.Message, properties["OldMessage"].StringValue);
        }

        [Fact]
        public void ConvertToEntityWithNameMapping()
        {
            // Arrange
            var converter = new TableEntityConverter<LogEntry>();
            var tableEntity = new DynamicTableEntity
                {
                    PartitionKey = "My partiton key",
                    Properties = new Dictionary<string, EntityProperty>
                        {
                            {"OldMessage", new EntityProperty("My message")}
                        }
                };

            // Act
            LogEntry entity = converter.GetEntity(tableEntity);

            // Assert
            Assert.Equal(entity.Id, tableEntity.PartitionKey);
            Assert.Null(tableEntity.RowKey);
            Assert.Equal(entity.Message, tableEntity.Properties["OldMessage"].StringValue);
        }

        [Fact]
        public void ConvertToTableEntityWithPropertyMapping()
        {
            // Arrange
            var converter = new TableEntityConverter<EntityWithMappedKey>();
            var entity = new EntityWithMappedKey
            {
                StringVal = "26",
                IntVal = 26,
            };

            // Act
            var tableEntity = converter.GetEntity(entity) as DynamicTableEntity;

            //Assert
            Assert.Equal(tableEntity.PartitionKey, $"{entity.IntVal}|");
            Assert.Equal(tableEntity.RowKey, entity.StringVal);
            Assert.Empty(tableEntity.WriteEntity(new OperationContext()));            
        }

        [Fact]
        public void ConvertToEntityWithPropertyMapping()
        {
            // Arrange
            var converter = new TableEntityConverter<EntityWithMappedKey>();
            var entity = new EntityWithMappedKey
            {
                StringVal = "26",
                IntVal = 26,
            };

            // Act
            var tableEntity = converter.GetEntity(entity) as DynamicTableEntity;
            var convertedEntity = converter.GetEntity(tableEntity);

            //Assert
            Assert.NotNull(convertedEntity);
            Assert.Equal(convertedEntity.IntVal, entity.IntVal);
            Assert.Equal(convertedEntity.StringVal, entity.StringVal);
        }

        [Fact]
        public void ConvertToTableEntityFromEntityWithReverseMapWithPropertyMapping()
        {
            // Arrange
            var converter = new TableEntityConverter<EntityForReverseMap>();
            var entity = new EntityForReverseMap
            {
                NestedEntity = NestedEntity.Create("10"),
                RowKey = "25",
                DateTimeOffset = DateTimeOffset.UtcNow
            };

            // Act
            var tableEntity = converter.GetEntity(entity) as DynamicTableEntity;

            //Assert
            Assert.Equal(tableEntity.PartitionKey,
                $"{entity.NestedEntity.DecimalValue}|{entity.DateTimeOffset.UtcDateTime.ToString("yyyyMMdd")}");
            Assert.Equal(tableEntity.RowKey, entity.RowKey);
            Assert.Empty(tableEntity.WriteEntity(new OperationContext()));           
        }

        [Fact]
        public void ConvertToEntityWithReverseMapWithPropertyMapping()
        {
            // Arrange
            var converter = new TableEntityConverter<EntityForReverseMap>();
            var entity = new EntityForReverseMap
            {
                NestedEntity = NestedEntity.Create("10"),
                RowKey = "25",
                DateTimeOffset = DateTimeOffset.UtcNow
            };

            // Act
            var tableEntity = converter.GetEntity(entity) as DynamicTableEntity;
            var convertedEntity = converter.GetEntity(tableEntity);

            //Assert
            Assert.NotNull(convertedEntity);
            Assert.Equal(convertedEntity.RowKey, entity.RowKey);
            Assert.Equal(convertedEntity.NestedEntity.DecimalValue, entity.NestedEntity.DecimalValue);
            Assert.Equal(convertedEntity.DateTimeOffset.Date, entity.DateTimeOffset.Date);
        }               

        [Fact]
        public void ConvertFromEntityWithSerializeAttribute()
        {
            // Arrange
            var serializer = SerializationSettings.Instance.DefaultSerializer;

            var converter = new TableEntityConverter<EntityWithSerializeAttribute>();
            var entity = new EntityWithSerializeAttribute
            {
                Nested = new EntityWithSerializeAttribute.NestedEntity
                {
                    DecimalValue = 10,
                },
            };

            // Act
            var tableEntity = converter.GetEntity(entity);
            var properties = tableEntity.WriteEntity(new OperationContext());
            var deserialized = serializer.Deserialize<EntityWithSerializeAttribute.NestedEntity>(properties["NestedSerialized"].StringValue);

            // Assert
            Assert.NotNull(properties["NestedSerialized"].StringValue);
            Assert.Equal(entity.Nested.DecimalValue, deserialized.DecimalValue);
        }

        [Fact]
        public void ConvertToEntityWithSerializeAttribute()
        {
            // Arrange
            var serializer = SerializationSettings.Instance.DefaultSerializer;

            var converter = new TableEntityConverter<EntityWithSerializeAttribute>();
            var tableEntity = new DynamicTableEntity
            {
                PartitionKey = "Pk",
                Properties = new Dictionary<string, EntityProperty>
                        {
                            {"NestedSerialized", new EntityProperty("{DecimalValue: 10}")}
                        }
            };            

            // Act
            var entity = converter.GetEntity(tableEntity);

            // Assert
            Assert.Equal(entity.Pk, tableEntity.PartitionKey);
            Assert.Null(tableEntity.RowKey);
            Assert.Equal(
                entity.Nested.DecimalValue, 
                serializer.Deserialize<EntityWithSerializeAttribute.NestedEntity>(
                    tableEntity.Properties["NestedSerialized"].StringValue).DecimalValue);
        }       
    }
}