using System;
using System.Collections.Generic;
using WindowsAzure.Table.EntityConverters.TypeData;
using WindowsAzure.Tests.Samples;
using Xunit;

namespace WindowsAzure.Tests.Table.EntityConverters.TypeData
{
    public class ProfileTests
    {
        [Fact]
        public void CreateProfileShouldAddEntityTypeMapToTypesData()
        {
            //Arrange & Acts
            new EntityTypeMapProfile();

            //Assert
            Assert.Contains(
                typeof(EntityTypeMap<EntityForProfile>), (IDictionary<Type, EntityTypeMap>)Profile.TypesData);
        }        
    }
}
