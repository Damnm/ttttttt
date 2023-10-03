using Microsoft.Extensions.Logging;
using Moq;

namespace EPAY.ETC.Core.API.UnitTests.Common
{
    public class TestBase<T> : AutoMapperTestBase
    {
        public Mock<ILogger<T>> _loggerMock = new();
        public Exception _nullException = null!;
    }
}
