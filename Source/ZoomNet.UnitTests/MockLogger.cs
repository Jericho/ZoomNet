using Microsoft.Extensions.Logging;
using System;

namespace ZoomNet.UnitTests
{
	internal class MockLogger : ILogger
	{
		public int LogCount { get; private set; }
		public LogLevel LastLogLevel { get; private set; }
		public bool IsLoggingEnabled { get; set; } = true;

		public IDisposable BeginScope<TState>(TState state) => null;

		public bool IsEnabled(LogLevel logLevel) => IsLoggingEnabled;

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			if (IsLoggingEnabled)
			{
				LogCount++;
				LastLogLevel = logLevel;
			}
		}
	}
}
