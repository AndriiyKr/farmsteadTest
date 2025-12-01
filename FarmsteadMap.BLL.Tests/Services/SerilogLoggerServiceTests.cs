namespace FarmsteadMap.BLL.Tests.Services
{
    using System;
    using System.IO;
    using FarmsteadMap.BLL.Services;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="SerilogLoggerService"/>.
    /// </summary>
    public class SerilogLoggerServiceTests
    {
        /// <summary>
        /// Tests that LogEvent writes an informational message to the log file.
        /// </summary>
        [Fact]
        public void LogEvent_WritesToFile()
        {
            var logFile = Path.GetTempFileName();

            try
            {
                using (var logger = new SerilogLoggerService(logFile))
                {
                    logger.LogEvent("Test event message");
                }

                var content = File.ReadAllText(logFile);
                Assert.Contains("Test event message", content);
            }
            finally
            {
                File.Delete(logFile);
            }
        }

        /// <summary>
        /// Tests that LogWarning writes a warning message to the log file.
        /// </summary>
        [Fact]
        public void LogWarning_WritesToFile()
        {
            var logFile = Path.GetTempFileName();

            try
            {
                using (var logger = new SerilogLoggerService(logFile))
                {
                    logger.LogWarning("Test warning message");
                }

                var content = File.ReadAllText(logFile);
                Assert.Contains("Test warning message", content);
            }
            finally
            {
                File.Delete(logFile);
            }
        }

        /// <summary>
        /// Tests that LogError writes an error message and exception to the log file.
        /// </summary>
        [Fact]
        public void LogError_WritesToFile()
        {
            var logFile = Path.GetTempFileName();

            try
            {
                using (var logger = new SerilogLoggerService(logFile))
                {
                    logger.LogError("Test error message", new InvalidOperationException("Test exception"));
                }

                var content = File.ReadAllText(logFile);
                Assert.Contains("Test error message", content);
                Assert.Contains("Test exception", content);
            }
            finally
            {
                File.Delete(logFile);
            }
        }
    }
}