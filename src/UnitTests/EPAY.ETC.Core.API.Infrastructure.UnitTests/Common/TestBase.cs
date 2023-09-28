using Microsoft.Extensions.Logging;
using Moq;

namespace EPAY.ETC.Core.API.Infrastructure.UnitTests.Common
{
    public class TestBase<T> : AutoMapperTestBase
    {
        public Mock<ILogger<T>> _loggerMock = new();
        public Exception _nullException = null!;
    }
}
