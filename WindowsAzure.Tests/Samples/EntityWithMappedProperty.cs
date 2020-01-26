using WindowsAzure.Table.EntityConverters.TypeData;

namespace WindowsAzure.Tests.Samples
{
    public class EntityWithMappedProperty
    {
        public string Pk { get; set; }

        public string Rk { get; set; }

        public Id EntityId { get; set; }

        public class Id
        {
            private readonly int _numValue;
            private readonly decimal _decimalValue;

            public Id(int numValue, decimal decimalValue)
            {
                _numValue = numValue;
                _decimalValue = decimalValue;
            }

            public override string ToString()
            {
                return $"{_numValue}.{_decimalValue}";
            }

            public static Id Parse(string rawId)
            {
                var data = rawId.Split('.');
                return data.Length == 2 ? new Id(int.Parse(data[0]), decimal.Parse(data[1])) : null;               
            }
        }
    }

    public class EntityWithCustomPropertyMap : EntityTypeMap<EntityWithMappedProperty>
    {
        public EntityWithCustomPropertyMap()
        {
            this.PartitionKey(x => x.Pk)
                .RowKey(x => x.Rk)
                .Map(x => x.EntityId, x => x.EntityId.ToString(), $"{nameof(EntityWithMappedProperty.EntityId)}Raw")
                .ReverseMap(x => x.EntityId, x => EntityWithMappedProperty.Id.Parse(x.Properties[$"{nameof(EntityWithMappedProperty.EntityId)}Raw"].StringValue));
        }
    }
}
