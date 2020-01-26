using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq.Expressions;

namespace WindowsAzure.Table.EntityConverters.TypeData.Properties
{
    /// <summary>
    ///     Handles accesss to the member property for mapping.
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    internal class MapProperty<T> : IProperty<T>
    {
        private readonly Expression<Func<T, string>> _source;
        private readonly string _propertyName;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="source">Entity type expression that returns string from mapping.</param>
        /// <param name="propertyName">Dynamic table entity property name.</param>
        public MapProperty(Expression<Func<T, string>> source, string propertyName)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _propertyName = string.IsNullOrEmpty(propertyName) ? throw new ArgumentNullException(nameof(propertyName)) : propertyName;
        }


        /// <summary>
        ///     Gets a POCO member value for table entity.
        /// </summary>
        /// <param name="entity">POCO entity.</param>
        /// <param name="tableEntity">Table entity.</param>
        public void GetMemberValue(T entity, DynamicTableEntity tableEntity)
        {
            var stringProperty = _source.Compile().Invoke(entity);
            tableEntity.Properties.Add(_propertyName, new EntityProperty(stringProperty));
        }

        /// <summary>
        ///     Sets a POCO member value from table entity.
        /// </summary>
        /// <param name="tableEntity">Table entity.</param>
        /// <param name="entity">POCO entity.</param>
        public void SetMemberValue(DynamicTableEntity tableEntity, T entity)
        {
            // Nothing.
        }
    }
}
