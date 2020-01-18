using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq.Expressions;

namespace WindowsAzure.Table.EntityConverters.TypeData.Properties
{
    public class PartitionKeyMapProperty<T> : IProperty<T>
    {
        private Expression<Func<T, string>> _propertyAccessor;

        public PartitionKeyMapProperty(Expression<Func<T, string>> propertyAccessor)
        {
            _propertyAccessor = propertyAccessor ?? throw new ArgumentNullException(nameof(propertyAccessor));
        }

        public void GetMemberValue(T entity, DynamicTableEntity tableEntity)
        {
            tableEntity.PartitionKey = _propertyAccessor.Compile().Invoke(entity);
        }

        public void SetMemberValue(DynamicTableEntity tableEntity, T entity)
        {
            // Do nothing if called.
        }
    }
}
