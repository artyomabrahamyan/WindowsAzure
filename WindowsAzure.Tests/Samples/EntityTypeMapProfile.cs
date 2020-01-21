using WindowsAzure.Table.EntityConverters.TypeData;

namespace WindowsAzure.Tests.Samples
{
    public class EntityTypeMapProfile : Profile
    {
        public EntityTypeMapProfile()
        {
            CreateMap<EntityForProfile>(e => e.PartitionKey(x => x.Pk).RowKey(x => x.Rk));
        }
    }

    public class EntityForProfile
    {
        public string Pk { get; set; }

        public string Rk { get; set; }

        public int IntVal { get; set; }
    }         
}
