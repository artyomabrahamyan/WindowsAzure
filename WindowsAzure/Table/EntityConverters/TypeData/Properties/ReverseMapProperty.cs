using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace WindowsAzure.Table.EntityConverters.TypeData.Properties
{
    /// <summary>
    ///     Handles accesss to the member property from mapping.
    /// </summary>
    /// <typeparam name="T">Entity type.</typeparam>
    /// <typeparam name="TMember">Member type.</typeparam>
    public class ReverseMapProperty<T, TMember> : IProperty<T>
    {
        private readonly Expression<Func<DynamicTableEntity, TMember>> _source;
        private readonly MemberInfo _member;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="source">Dynamic table entity member.</param>
        /// <param name="member">Entity member.</param>
        public ReverseMapProperty(Expression<Func<DynamicTableEntity, TMember>> source, MemberInfo member)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _member = member ?? throw new ArgumentNullException(nameof(member));
        }

        /// <summary>
        ///     Gets a POCO member value for table entity.
        /// </summary>
        /// <param name="entity">POCO entity.</param>
        /// <param name="tableEntity">Table entity.</param>
        public void GetMemberValue(T entity, DynamicTableEntity tableEntity)
        {
            // Do nothing if called.
        }

        /// <summary>
        ///     Sets a POCO member value from table entity.
        /// </summary>
        /// <param name="tableEntity">Table entity.</param>
        /// <param name="entity">POCO entity.</param>
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
