namespace FarmsteadMap.BLL.Services
{
    /// <summary>
    /// Provides methods for logging events, warnings, and errors.
    /// </summary>
    public interface ILoggerService
    {
        /// <summary>
        /// Logs an informational event.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void LogEvent(string message);

        /// <summary>
        /// Logs a warning event.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        void LogWarning(string message);

        /// <summary>
        /// Logs an error event.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        /// <param name="ex">The exception associated with the error (optional).</param>
        void LogError(string message, Exception? ex = null);
    }
}