using Microsoft.WindowsAzure.Storage.Table;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsAzure.Table;
using WindowsAzure.Table.RequestExecutor;
using WindowsAzure.Tests.Common;
using WindowsAzure.Tests.Samples;
using Xunit;

namespace WindowsAzure.Tests.Table.Context
{
    public class DeleteByPartitionKeyAndRowKeyTests
    {
        [Fact]
        public void Remove_ExecutesTableDeleteOperaion()
        {
            // Arrange
            var partitionKey = "PartitionKey";
            var rowKey = "RowKey";

            Mock<ITableRequestExecutor<Country>> mock = MocksFactory.GetQueryExecutorMock<Country>();
            CloudTableClient tableClient = ObjectsFactory.GetCloudTableClient();
            var context = new TableSet<Country>(tableClient)
            {
                RequestExecutor = mock.Object
            };

            var tableEntity = new DynamicTableEntity 
            { 
                PartitionKey = partitionKey,
                RowKey = rowKey,
                ETag = "*",
            };

            // Act
            context.Remove(partitionKey, rowKey);

            // Assert
            mock.Verify(executor => executor.ExecuteWithoutResult(
                It.Is<DynamicTableEntity>(
                    d => d.PartitionKey == partitionKey 
                    && d.RowKey == rowKey
                    && d.ETag == "*"), TableOperation.Delete), Times.Once());
        }

        [Fact]
        public void Remove_NullPartitionKey_ThrowsException()
        {
            // Arrange
            string partitionKey = null;
            var rowKey = "RowKey";

            Mock<ITableRequestExecutor<Country>> mock = MocksFactory.GetQueryExecutorMock<Country>();
            CloudTableClient tableClient = ObjectsFactory.GetCloudTableClient();
            var context = new TableSet<Country>(tableClient)
            {
                RequestExecutor = mock.Object
            };

            // Act 
            void removeOperation() => context.Remove(partitionKey, rowKey);

            // Assert
            Assert.Throws<ArgumentNullException>(removeOperation);
        }

        [Fact]
        public void Remove_NullRowKey_ThrowsException()
        {
            // Arrange
            string partitionKey = null;
            var rowKey = "RowKey";

            Mock<ITableRequestExecutor<Country>> mock = MocksFactory.GetQueryExecutorMock<Country>();
            CloudTableClient tableClient = ObjectsFactory.GetCloudTableClient();
            var context = new TableSet<Country>(tableClient)
            {
                RequestExecutor = mock.Object
            };

            // Act 
            void removeOperation() => context.Remove(partitionKey, rowKey);

            // Assert
            Assert.Throws<ArgumentNullException>(removeOperation);
        }

        [Fact]
        public void RemoveAsync_ExecutesTableDeleteOperaion()
        {
            // Arrange
            var partitionKey = "PartitionKey";
            var rowKey = "RowKey";

            Mock<ITableRequestExecutor<Country>> mock = MocksFactory.GetQueryExecutorMock<Country>();
            CloudTableClient tableClient = ObjectsFactory.GetCloudTableClient();
            var context = new TableSet<Country>(tableClient)
            {
                RequestExecutor = mock.Object
            };

            var tableEntity = new DynamicTableEntity
            {
                PartitionKey = partitionKey,
                RowKey = rowKey,
                ETag = "*",
            };

            // Act
            context.RemoveAsync(partitionKey, rowKey);

            // Assert
            mock.Verify(executor => executor.ExecuteWithoutResultAsync(
                It.Is<DynamicTableEntity>(d => 
                d.PartitionKey == partitionKey 
                && d.RowKey == rowKey 
                && d.ETag == "*"), 
                TableOperation.Delete, 
                It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public void RemoveAsync_NullPartitionKey_ThrowsException()
        {
            // Arrange
            string partitionKey = null;
            var rowKey = "RowKey";

            Mock<ITableRequestExecutor<Country>> mock = MocksFactory.GetQueryExecutorMock<Country>();
            CloudTableClient tableClient = ObjectsFactory.GetCloudTableClient();
            var context = new TableSet<Country>(tableClient)
            {
                RequestExecutor = mock.Object
            };

            // Act 
            Task removeOperation() => context.RemoveAsync(partitionKey, rowKey);

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(removeOperation);
        }

        [Fact]
        public void RemoveAsync_NullRowKey_ThrowsException()
        {
            // Arrange
            string partitionKey = null;
            var rowKey = "RowKey";

            Mock<ITableRequestExecutor<Country>> mock = MocksFactory.GetQueryExecutorMock<Country>();
            CloudTableClient tableClient = ObjectsFactory.GetCloudTableClient();
            var context = new TableSet<Country>(tableClient)
            {
                RequestExecutor = mock.Object
            };

            // Act 
            Task removeOperation() => context.RemoveAsync(partitionKey, rowKey);

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(removeOperation);
        }
    }
}
