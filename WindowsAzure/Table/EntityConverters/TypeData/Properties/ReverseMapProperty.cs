using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace WindowsAzure.Table.EntityConverters.TypeData.Properties
{
    public class ReverseMapProperty<T, TMember> : IProperty<T>
    {
        private readonly Expression<Func<DynamicTableEntity, TMember>> _source;
        private readonly MemberInfo _member;

        public ReverseMapProperty(Expression<Func<DynamicTableEntity, TMember>> source, MemberInfo member)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _member = member ?? throw new ArgumentNullException(nameof(member));
        }

        public void GetMemberValue(T entity, DynamicTableEntity tableEntity)
        {
            // Do nothing if called.
        }

        public void SetMemberValue(DynamicTableEntity tableEntity, T entity)
        {
            ParameterExpression instanceExpression = Expression.Parameter(typeof(T), "instance");
            MemberExpression memberExpression = Expression.Property(instanceExpression, (PropertyInfo)_member);

            ParameterExpression entityPropertyParameter = Expression.Parameter(typeof(DynamicTableEntity));
            var invokeExpresssion = Expression.Invoke(_source, entityPropertyParameter);

            Expression.Lambda<Action<T, DynamicTableEntity>>(
                Expression.Assign(memberExpression, invokeExpresssion),
                instanceExpression, entityPropertyParameter)
                .Compile()
                .Invoke(entity, tableEntity);
        }
    }
}
