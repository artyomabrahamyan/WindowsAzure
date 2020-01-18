using System;
using WindowsAzure.Table.EntityConverters.TypeData;

namespace WindowsAzure.Tests.Samples
{
    public class EntityWithMappedKey
    {
        public int IntVal { get; set; }

        public string StringVal { get; set; }
    }

    public class EntityWithCustomPartitionKeyMap : EntityTypeMap<EntityWithMappedKey>
    {
        public EntityWithCustomPartitionKeyMap()
        {
            this.PartitionKeyMap(x => x.IntVal + "|")
            .ReverseMap(z => z.IntVal, x => int.Parse(x.PartitionKey.Split('|', StringSplitOptions.RemoveEmptyEntries)[0]))
            .RowKey(x => x.StringVal);
        }
    }
}
