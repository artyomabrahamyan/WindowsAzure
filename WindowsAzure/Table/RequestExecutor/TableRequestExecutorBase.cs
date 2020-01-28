﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using WindowsAzure.Table.EntityConverters;
using WindowsAzure.Table.Wrappers;

namespace WindowsAzure.Table.RequestExecutor
{
    internal abstract class TableRequestExecutorBase<T> : ITableRequestExecutor<T> where T : class, new()
    {
        private readonly ICloudTable _cloudTable;
        private readonly ITableEntityConverter<T> _entityConverter;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="cloudTable">Cloud table.</param>
        /// <param name="entityConverter">Entity converter.</param>
        internal TableRequestExecutorBase(ICloudTable cloudTable, ITableEntityConverter<T> entityConverter)
        {
            if (cloudTable == null)
            {
                throw new ArgumentNullException(nameof(cloudTable));
            }

            if (entityConverter == null)
            {
                throw new ArgumentNullException(nameof(entityConverter));
            }

            _cloudTable = cloudTable;
            _entityConverter = entityConverter;
        }

        /// <summary>
        ///     Executes operation.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <param name="operation">Operation.</param>
        /// <returns>Result entity.</returns>
        public T Execute(T entity, Func<ITableEntity, TableOperation> operation)
        {
            ITableEntity tableEntity = _entityConverter.GetEntity(entity);
            TableResult result = _cloudTable.Execute(operation(tableEntity));

            return _entityConverter.GetEntity((DynamicTableEntity) result.Result);
        }

        /// <summary>
        ///     Executes operation without returning result.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <param name="operation">Operation.</param>
        public void ExecuteWithoutResult(T entity, Func<ITableEntity, TableOperation> operation)
        {
            ITableEntity tableEntity = _entityConverter.GetEntity(entity);
            _cloudTable.Execute(operation(tableEntity));
        }

        /// <summary>
        ///     Executes operation without returning result.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <param name="operation">Operation.</param>
        public void ExecuteWithoutResult(ITableEntity entity, Func<ITableEntity, TableOperation> operation)
        {
            _cloudTable.Execute(operation(entity));
        }

        /// <summary>
        ///     Executes operation without returning result asyncronously.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <param name="operation">Operation.</param>
        /// <param name="cancellationToken">Operation.</param>
        public Task ExecuteWithoutResultAsync(ITableEntity entity, Func<ITableEntity, TableOperation> operation, CancellationToken cancellationToken)
        {
            return _cloudTable.ExecuteAsync(operation(entity), cancellationToken);
        }       

        /// <summary>
        ///     Executes table query asynchronously.
        /// </summary>
        /// <param name="query">Table query.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Result entity.</returns>
        public async Task<IEnumerable<T>> ExecuteAsync(ITableQuery query, CancellationToken cancellationToken)
        {         
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var results = await _cloudTable
                .ExecuteQueryAsync(query, cancellationToken);

            return results?.Select(e => _entityConverter.GetEntity(e));
        }        

        /// <summary>
        ///     Executes operation asynchronously.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <param name="operation">Operation.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Result entity.</returns>
        public Task<T> ExecuteAsync(T entity, Func<ITableEntity, TableOperation> operation, CancellationToken cancellationToken)
        {
            ITableEntity tableEntity = _entityConverter.GetEntity(entity);

            return _cloudTable
                .ExecuteAsync(operation(tableEntity), cancellationToken)
                .Then(result =>
                    {
                        var value = (DynamicTableEntity) result.Result;
                        return _entityConverter.GetEntity(value);
                    }, cancellationToken);
        }

        /// <summary>
        ///     Executes operation without returning result asynchronously.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <param name="operation">Operation.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public Task ExecuteWithoutResultAsync(T entity, Func<ITableEntity, TableOperation> operation, CancellationToken cancellationToken)
        {
            ITableEntity tableEntity = _entityConverter.GetEntity(entity);
            return _cloudTable.ExecuteAsync(operation(tableEntity), cancellationToken);
        }

        public abstract IEnumerable<T> ExecuteBatches(IEnumerable<T> entities, Func<ITableEntity, TableOperation> operation);

        public abstract void ExecuteBatchesWithoutResult(IEnumerable<T> entities, Func<ITableEntity, TableOperation> operation);

        public abstract Task<IEnumerable<T>> ExecuteBatchesAsync(IEnumerable<T> entities, Func<ITableEntity, TableOperation> operation, CancellationToken cancellationToken);

        public abstract Task ExecuteBatchesWithoutResultAsync(IEnumerable<T> entities, Func<ITableEntity, TableOperation> operation, CancellationToken cancellationToken);
    }
}