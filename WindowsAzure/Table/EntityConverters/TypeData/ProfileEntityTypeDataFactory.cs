using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WindowsAzure.Common;
using WindowsAzure.Properties;

namespace WindowsAzure.Table.EntityConverters.TypeData
{
    internal static class ProfileEntityTypeDataFactory
    {
        /// <summary>
        ///     Assemblies to find entity type map profiles.
        /// </summary>
        private static Assembly[] _mappingAssemblies = { };


        /// <summary>
        ///     Entity type data cache.
        /// </summary>
        private static readonly ConcurrentDictionary<Type, object> TypesData =
            new ConcurrentDictionary<Type, object>();
        
        /// <summary>
        ///     Retrieves an entity type data.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <returns>Entity type data.</returns>
        public static IEntityTypeData<T> GetEntityTypeData<T>() where T : class, new()
        {
            var type = typeof(T);
            var assemblies = _mappingAssemblies.Concat(new[] { type.GetTypeInfo().Assembly })
                .Distinct();

            return (IEntityTypeData<T>)TypesData.GetOrAdd(type,
               key => FindsEntityTypeMapFromProfile<T>(assemblies));
        }        

        /// <summary>
        ///     Registers an assembly with profiles.
        /// </summary>
        /// <param name="assemblies"></param>
        public static void RegisterProfileAssembly(params Assembly[] assemblies)
        {
            _mappingAssemblies = assemblies;
        }

        /// <summary>
        ///     Finds an entity type mapping from profile.
        /// </summary>
        /// <param name="assembliesToSearch">Aseemblies to search profile types.</param>
        /// <returns>An instance of entity type mapping.</returns>
        private static IEntityTypeData<T> FindsEntityTypeMapFromProfile<T>(IEnumerable<Assembly> assembliesToSearch) where T : class, new()
        {
            foreach (var assembly in assembliesToSearch)
            {
                var types = assembly.GetTypes()
                    .Where(t => !t.IsAbstractType() && typeof(Profile).IsAssignableFrom(t));

                foreach (var type in types)
                {                   
                    try
                    {
                        var profile = Activator.CreateInstance(type);
                        if (Profile.TypesData.TryGetValue(typeof(EntityTypeMap<T>), out var entityTypeMap))
                        {
                            return entityTypeMap as IEntityTypeData<T>;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new NotSupportedException(string.Format(Resources.ProfileCannotBeCreated, type), ex);
                    }
                }                
            }

            return null;
        }            
    }
}

