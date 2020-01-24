using System.Linq;
using WindowsAzure.Table.EntityConverters.TypeData;
using WindowsAzure.Tests.Samples;
using Xunit;

namespace WindowsAzure.Tests.Table.EntityConverters.TypeData
{
    public class ProfileEntityTypeDataFactoryTests
    {
        [Fact]
        public void GetEntityTypeData_ReturnsCorrectEntityTypeMap()
        {
            var map = ProfileEntityTypeDataFactory.GetEntityTypeData<EntityForProfile>();

            Profile.TypesData.TryGetValue(typeof(EntityTypeMap<EntityForProfile>), out EntityTypeMap value);
            var entityTypeMap = value as IEntityTypeData<EntityForProfile>;

            Assert.Equal(map.NameChanges.Count, entityTypeMap.NameChanges.Count);
            Assert.Equal(map.NameChanges.Keys.First(), entityTypeMap.NameChanges.Keys.First());
            Assert.Equal(map.NameChanges.Keys.Last(), entityTypeMap.NameChanges.Keys.Last());
            Assert.Equal(map.NameChanges.Values.First(), entityTypeMap.NameChanges.Values.First());
            Assert.Equal(map.NameChanges.Values.Last(), entityTypeMap.NameChanges.Values.Last());
        }        
    }
}
