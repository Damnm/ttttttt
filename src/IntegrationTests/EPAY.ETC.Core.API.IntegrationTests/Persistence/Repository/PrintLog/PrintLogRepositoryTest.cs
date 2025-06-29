﻿using EPAY.ETC.Core.API.Core.Models.PrintLog;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.PrintLog;
using EPAY.ETC.Core.API.IntegrationTests.Common;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using XUnitPriorityOrderer;

namespace EPAY.ETC.Core.API.IntegrationTests.Persistence.Repository.PrintLog
{
    [TestCaseOrderer(CasePriorityOrderer.TypeName, CasePriorityOrderer.AssembyName)]
    public class PrintLogRepositoryTest : IntegrationTestBase
    {
        private IPrintLogRepository? _repository;
        #region Init test data
        private static Guid printLogId = Guid.NewGuid();
        private PrintLogModel printLog = new PrintLogModel()
        {
            Id = printLogId,
            RFID = "123asdas48v6aaswd",
            PlateNumber = "12A123456",
          
        };
        #endregion
        #region AddAsync
        [Fact, Order(1)]
        public async void GivenValidEntity_WhenAddAsyncIsCalled_ThenRecordAddedSuccessfully()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _repository = scope.ServiceProvider.GetService<IPrintLogRepository>()!;

            // Arrange

            // Act
            var result = await _repository!.AddAsync(printLog);
            var expected = await _repository.GetByIdAsync(printLog.Id);

            // Assert
            result.Should().NotBeNull();
            expected.Should().NotBeNull();
            result!.Id.Should().Be(expected!.Id);
            result!.RFID.Should().Be(expected!.RFID);
            result!.PlateNumber.Should().Be(expected!.PlateNumber);
        }
        #endregion
        #region GetByIdAsync
        [Fact, Order(2)]
        public async Task GivenValidEntity_WhenGetByIdAsyncIsCalled_ThenReturnCorrectResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _repository = scope.ServiceProvider.GetService<IPrintLogRepository>();
            Guid temp = new Guid("40336aa7-fafa-4028-9a31-c9f6d77938ba");
            // Arrange
            
            // Act
            var oldData = await _repository!.GetByIdAsync(temp);
            if(oldData == null)
            {
                printLog.Id = temp;
                await _repository!.AddAsync(printLog);
            }
            var result = await _repository!.GetByIdAsync(temp);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(temp);
          
        }
        [Fact, Order(2)]
        public async Task GivenRequestIsValidAndIdIsNotExists_WhenGetByIdAsyncIsCalled_ThenReturnNull()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _repository = scope.ServiceProvider.GetRequiredService<IPrintLogRepository>();

            // Arrange
            Guid guid = Guid.NewGuid();

            // Act
            var result = await _repository.GetByIdAsync(guid);

            // Assert
            result.Should().BeNull();
        }
        #endregion
        #region UpdateAsync
        [Fact, Order(3)]
        public async Task GivenValidEntity_WhenUpdateAsyncIsCalled_ThenRecordIsUpdatedAndNotReturnResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _repository = scope.ServiceProvider.GetService<IPrintLogRepository>();
            // Arrange
            printLog.PlateNumber = "1111111";
            printLog.Id = new Guid("40336aa7-fafa-4028-9a31-c9f6d77938ba");

            // Arrange

            // Act
            var oldData = await _repository!.GetByIdAsync(printLog.Id);
            if (oldData == null)
            {
                await _repository!.AddAsync(printLog);
            }

            // Act
            await _repository!.UpdateAsync(printLog);
            var expected = await _repository.GetByIdAsync(printLog.Id);

            // Assert
            expected.Should().NotBeNull();
            expected?.Id.Should().Be(expected!.Id);
            expected?.PlateNumber.Should().Be(expected!.PlateNumber);
        }
        #endregion
        #region RemoveAsync
        [Fact, Order(4)]
        public async Task GivenValidEntity_WhenRemoveAsyncIsCalled_ThenDeletedAndNotReturnResult()
        {
            using var scope = WebApplicationFactory.Services.CreateScope();
            _repository = scope.ServiceProvider.GetService<IPrintLogRepository>();
            // Arrange
            printLog.PlateNumber = "1111111qqq";
            printLog.Id = new Guid("40336aa7-fafa-4028-9a31-c9f6d77948ba");

            // Arrange

            // Act
            var oldData = await _repository!.GetByIdAsync(printLog.Id);
            if (oldData == null)
            {
                await _repository!.AddAsync(printLog);
            }
            // Act
            await _repository!.RemoveAsync(printLog);
            var expected = await _repository.GetByIdAsync(printLog.Id);

            // Assert
            expected.Should().BeNull();
        }
        #endregion
    }
}
