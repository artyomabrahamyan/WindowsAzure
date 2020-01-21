using WindowsAzure.Table.EntityConverters.TypeData;

namespace WindowsAzure.Tests.Samples
{
    public class EntityForInvalidProfileMap
    {
        public string Pk { get; set; }

        public string Rk { get; set; }

        public int IntVal { get; set; }
    }

    public class EntityForInvalidProfileMapMapping : EntityTypeMap<EntityForInvalidProfileMap>
    {
        public EntityForInvalidProfileMapMapping()
        {
            PartitionKey(e => e.Pk)
                .RowKey(e => e.Rk);
        }
    }
}
