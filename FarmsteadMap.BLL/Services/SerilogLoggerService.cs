namespace FarmsteadMap.BLL.Services
{
    using System;
    using Serilog;
    using Serilog.Core;

    /// <summary>
    /// Serilog-based implementation of <see cref="ILoggerService"/>.
    /// </summary>
    public class SerilogLoggerService : ILoggerService, IDisposable
    {
        /// <summary>
        /// The Serilog logger instance.
        /// </summary>
        private readonly Logger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerilogLoggerService"/> class.
        /// </summary>
        /// <param name="logFilePath">The path to the log file.</param>
        public SerilogLoggerService(string logFilePath)
        {
            this.logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(
                    logFilePath,
                    rollingInterval: RollingInterval.Infinite,
                    flushToDiskInterval: TimeSpan.FromMilliseconds(50))
                .CreateLogger();
        }

        /// <inheritdoc/>
        public void LogEvent(string message)
        {
            this.logger.Information(message);
        }

        /// <inheritdoc/>
        public void LogWarning(string message)
        {
            this.logger.Warning(message);
        }

        /// <inheritdoc/>
        public void LogError(string message, Exception? ex = null)
        {
            if (ex != null)
            {
                this.logger.Error(ex, message);
            }
            else
            {
                this.logger.Error(message);
            }
        }

        /// <summary>
        /// Disposes the logger and releases all resources.
        /// </summary>
        public void Dispose()
        {
            this.logger.Dispose();
        }
    }
}