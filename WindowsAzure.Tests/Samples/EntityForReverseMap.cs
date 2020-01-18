using System;
using System.Globalization;
using WindowsAzure.Table.EntityConverters.TypeData;

namespace WindowsAzure.Tests.Samples
{
    public class EntityForReverseMap
    {
        public NestedEntity NestedEntity { get; set; }

        public string RowKey { get; set; }

        public DateTimeOffset DateTimeOffset { get; set; }
    }

    public class NestedEntity
    {
        private NestedEntity(decimal decimalValue)
        {
            DecimalValue = decimalValue;
        }

        public decimal DecimalValue { get; }        

        public static NestedEntity Create(string decimalValue) 
        {
            return new NestedEntity(decimal.Parse(decimalValue));
        }
    }

    public class EntityForComplexMapMapping : EntityTypeMap<EntityForReverseMap>
    {
        public EntityForComplexMapMapping()
        {
            this.PartitionKeyMap(p => $"{p.NestedEntity.DecimalValue}|{p.DateTimeOffset.UtcDateTime.ToString("yyyyMMdd")}")                
                .ReverseMap(x => x.NestedEntity, y =>
                    NestedEntity.Create(y.PartitionKey.Split('|', StringSplitOptions.RemoveEmptyEntries)[0]))
                .ReverseMap(x => x.DateTimeOffset, 
                y => DateTimeOffset.ParseExact(
                    y.PartitionKey.Split('|', StringSplitOptions.RemoveEmptyEntries)[1], "yyyyMMdd", null, DateTimeStyles.AssumeUniversal)
                )
                .RowKey(x => x.RowKey);
        }
    }
}
