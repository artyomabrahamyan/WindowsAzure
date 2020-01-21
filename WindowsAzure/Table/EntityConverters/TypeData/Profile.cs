using System;
using System.Collections.Concurrent;

namespace WindowsAzure.Table.EntityConverters.TypeData
{
    public class Profile
    {
        public static ConcurrentDictionary<Type, EntityTypeMap> TypesData { get; private set; } = 
            new ConcurrentDictionary<Type, EntityTypeMap>();

        protected void CreateMap<T>(Action<EntityTypeMap<T>> classMapInitializer) where T : class, new()
        {
            if (classMapInitializer == null)
            {
                throw new ArgumentNullException(nameof(classMapInitializer));
            }

            if (!TypesData.ContainsKey(typeof(EntityTypeMap<T>)))
            {
                var entityTypeMap = new EntityTypeMap<T>(classMapInitializer);
                TypesData.TryAdd(typeof(EntityTypeMap<T>), entityTypeMap);
            }
        }
    }
}
