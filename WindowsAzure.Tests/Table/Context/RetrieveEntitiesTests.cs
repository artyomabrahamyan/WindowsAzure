using Microsoft.WindowsAzure.Storage.Table;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using WindowsAzure.Table;
using WindowsAzure.Table.RequestExecutor;
using WindowsAzure.Tests.Common;
using WindowsAzure.Tests.Samples;
using Xunit;

namespace WindowsAzure.Tests.Table.Context
{
    public class RetrieveEntitiesTests
    {
        [Fact]
        public void Retrieve_ExecutesTableRetrieveOperation()
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

            // Act
            context.Retrieve(partitionKey, rowKey);

            // Assert
            mock.Verify(executor => 
                executor.Execute(partitionKey, rowKey, TableOperation.Retrieve), Times.Once());
        }

        [Fact]
        public void Retrieve_NullPartitionKey_ThrowsException()
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
            void retrieveOpertaion() => context.Retrieve(partitionKey, rowKey);

            // Assert
            Assert.Throws<ArgumentNullException>(retrieveOpertaion);
        }

        [Fact]
        public void Retrieve_NullRowKey_ThrowsException()
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
            void retrieveOpertaion() => context.Retrieve(partitionKey, rowKey);

            // Assert
            Assert.Throws<ArgumentNullException>(retrieveOpertaion);
        }

        [Fact]
        public async Task RetrieveAsync_ExecutesTableRetrieveOperation()
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

            // Act
            await context.RetrieveAsync(partitionKey, rowKey);

            // Assert
            mock.Verify(executor =>
                executor.ExecuteAsync(partitionKey, rowKey, TableOperation.Retrieve, It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public void RetrieveAsync_NullPartitionKey_ThrowsException()
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
            Task retrieveOpertaion() => context.RetrieveAsync(partitionKey, rowKey);

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(retrieveOpertaion);
        }

        [Fact]
        public void RetrieveAsync_NullRowKey_ThrowsException()
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
            Task retrieveOpertaion() => context.RetrieveAsync(partitionKey, rowKey);

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(retrieveOpertaion);
        }
    }
}
