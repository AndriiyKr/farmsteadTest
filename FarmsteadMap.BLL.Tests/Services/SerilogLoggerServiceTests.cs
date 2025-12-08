namespace FarmsteadMap.BLL.Tests.Services
{
    using System;
    using FarmsteadMap.BLL.Services;
    using Moq;
    using Serilog;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="SerilogLoggerService"/> class.
    /// </summary>
    public class SerilogLoggerServiceTests
    {
        /// <summary>
        /// Asserts that LogEvent calls Information on the logger.
        /// </summary>
        [Fact]
        public void LogEvent_CallsInformation()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();
            var service = new SerilogLoggerService(loggerMock.Object);

            // Act
            service.LogEvent("Test event message");

            // Assert
            loggerMock.Verify(l => l.Information("Test event message"), Times.Once);
        }

        /// <summary>
        /// Asserts that LogWarning calls Warning on the logger.
        /// </summary>
        [Fact]
        public void LogWarning_CallsWarning()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();
            var service = new SerilogLoggerService(loggerMock.Object);

            // Act
            service.LogWarning("Test warning message");

            // Assert
            loggerMock.Verify(l => l.Warning("Test warning message"), Times.Once);
        }

        /// <summary>
        /// Asserts that LogError calls Error with an exception on the logger.
        /// </summary>
        [Fact]
        public void LogError_CallsError_WithException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();
            var service = new SerilogLoggerService(loggerMock.Object);
            var ex = new InvalidOperationException("Test exception");

            // Act
            service.LogError("Test error message", ex);

            // Assert
            loggerMock.Verify(l => l.Error(ex, "Test error message"), Times.Once);
        }

        /// <summary>
        /// Asserts that LogError calls Error without an exception on the logger.
        /// </summary>
        [Fact]
        public void LogError_CallsError_WithoutException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();
            var service = new SerilogLoggerService(loggerMock.Object);

            // Act
            service.LogError("Test error message");

            // Assert
            loggerMock.Verify(l => l.Error("Test error message"), Times.Once);
        }
    }
}