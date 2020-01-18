using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq.Expressions;

namespace WindowsAzure.Table.EntityConverters.TypeData.Properties
{
    public class RowKeyMapProperty<T> : IProperty<T>
    {
        private Expression<Func<T, string>> _propertyAccessor;

        public RowKeyMapProperty(Expression<Func<T, string>> propertyAccessor)
        {
            _propertyAccessor = propertyAccessor ?? throw new ArgumentNullException(nameof(propertyAccessor));
        }

        public void GetMemberValue(T entity, DynamicTableEntity tableEntity)
        {
            tableEntity.RowKey = _propertyAccessor.Compile().Invoke(entity);
        }

        public void SetMemberValue(DynamicTableEntity tableEntity, T entity)
        {
            // Do nothing if called.
        }
    }
}
