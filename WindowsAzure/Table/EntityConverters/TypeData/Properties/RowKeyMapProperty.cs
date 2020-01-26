using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq.Expressions;

namespace WindowsAzure.Table.EntityConverters.TypeData.Properties
{
    internal class RowKeyMapProperty<T> : IProperty<T>
    {
        private Expression<Func<T, string>> _propertyAccessor;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="propertyAccessor">Property accessor.</param>
        public RowKeyMapProperty(Expression<Func<T, string>> propertyAccessor)
        {
            _propertyAccessor = propertyAccessor ?? throw new ArgumentNullException(nameof(propertyAccessor));
        }

        /// <summary>
        ///     Gets a POCO member value for table entity.
        /// </summary>
        /// <param name="entity">POCO entity.</param>
        /// <param name="tableEntity">Table entity.</param>
        public void GetMemberValue(T entity, DynamicTableEntity tableEntity)
        {
            tableEntity.RowKey = _propertyAccessor.Compile().Invoke(entity);
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
