using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Pathoschild.Http.Client;
using Pathoschild.Http.Client.Extensibility;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace ZoomNet.Utilities
{
	/// <summary>
	/// Diagnostic handler for requests dispatched to the Zoom API.
	/// </summary>
	/// <seealso cref="Pathoschild.Http.Client.Extensibility.IHttpFilter" />
	internal class DiagnosticHandler : IHttpFilter
	{
		#region FIELDS

		internal const string DIAGNOSTIC_ID_HEADER_NAME = "ZoomNet-Diagnostic-Id";
		private readonly ILogger _logger;
		private readonly LogLevel _logLevelSuccessfulCalls;
		private readonly LogLevel _logLevelFailedCalls;

		#endregion

		#region PROPERTIES

		internal static ConcurrentDictionary<string, (WeakReference<HttpRequestMessage> RequestReference, string Diagnostic, long RequestTimestamp, long ResponseTimeStamp)> DiagnosticsInfo { get; } = new ConcurrentDictionary<string, (WeakReference<HttpRequestMessage>, string, long, long)>();

		#endregion

		#region CTOR

		public DiagnosticHandler(LogLevel logLevelSuccessfulCalls, LogLevel logLevelFailedCalls, ILogger logger = null)
		{
			_logLevelSuccessfulCalls = logLevelSuccessfulCalls;
			_logLevelFailedCalls = logLevelFailedCalls;
			_logger = logger ?? NullLogger.Instance;
		}

		#endregion

		#region PUBLIC METHODS

		/// <summary>Method invoked just before the HTTP request is submitted. This method can modify the outgoing HTTP request.</summary>
		/// <param name="request">The HTTP request.</param>
		public void OnRequest(IRequest request)
		{
			// Add a unique ID to the request header
			var diagnosticId = Guid.NewGuid().ToString("N");
			request.WithHeader(DIAGNOSTIC_ID_HEADER_NAME, diagnosticId);

			// Log the request
			var httpRequest = request.Message;
			var diagnostic = new StringBuilder();

			diagnostic.AppendLine("REQUEST:");
			diagnostic.AppendLine($"  {httpRequest.Method.Method} {httpRequest.RequestUri}");
			LogHeaders(diagnostic, httpRequest.Headers);
			LogContent(diagnostic, httpRequest.Content);

			// Add the diagnostic info to our cache
			DiagnosticsInfo.TryAdd(diagnosticId, (new WeakReference<HttpRequestMessage>(request.Message), diagnostic.ToString(), Stopwatch.GetTimestamp(), long.MinValue));
		}

		/// <summary>Method invoked just after the HTTP response is received. This method can modify the incoming HTTP response.</summary>
		/// <param name="response">The HTTP response.</param>
		/// <param name="httpErrorAsException">Whether HTTP error responses should be raised as exceptions.</param>
		public void OnResponse(IResponse response, bool httpErrorAsException)
		{
			var responseTimestamp = Stopwatch.GetTimestamp();
			var httpResponse = response.Message;

			var diagnosticId = response.Message.RequestMessage.Headers.GetValue(DIAGNOSTIC_ID_HEADER_NAME);
			if (DiagnosticsInfo.TryGetValue(diagnosticId, out (WeakReference<HttpRequestMessage> RequestReference, string Diagnostic, long RequestTimestamp, long ResponseTimestamp) diagnosticInfo))
			{
				var updatedDiagnostic = new StringBuilder(diagnosticInfo.Diagnostic);
				try
				{
					// Log the response
					updatedDiagnostic.AppendLine();
					updatedDiagnostic.AppendLine("RESPONSE:");
					updatedDiagnostic.AppendLine($"  HTTP/{httpResponse.Version} {(int)httpResponse.StatusCode} {httpResponse.ReasonPhrase}");
					LogHeaders(updatedDiagnostic, httpResponse.Headers);
					LogContent(updatedDiagnostic, httpResponse.Content);

					// Calculate how much time elapsed between request and response
					var elapsed = TimeSpan.FromTicks(responseTimestamp - diagnosticInfo.RequestTimestamp);

					// Log diagnostic
					updatedDiagnostic.AppendLine();
					updatedDiagnostic.AppendLine("DIAGNOSTIC:");
					updatedDiagnostic.AppendLine($"  The request took {elapsed.ToDurationString()}");
				}
				catch (Exception e)
				{
					Debug.WriteLine("{0}\r\nAN EXCEPTION OCCURRED: {1}\r\n{0}", new string('=', 50), e.GetBaseException().Message);
					updatedDiagnostic.AppendLine($"AN EXCEPTION OCCURRED: {e.GetBaseException().Message}");

					if (_logger.IsEnabled(LogLevel.Error))
					{
						_logger.LogError(e, "An exception occurred when inspecting the response from Zoom");
					}
				}
				finally
				{
					var logLevel = response.IsSuccessStatusCode ? _logLevelSuccessfulCalls : _logLevelFailedCalls;
					if (_logger.IsEnabled(logLevel))
					{
						_logger.Log(logLevel, updatedDiagnostic.ToString()
							.Replace("{", "{{")
							.Replace("}", "}}"));
					}

					DiagnosticsInfo.TryUpdate(
						diagnosticId,
						(diagnosticInfo.RequestReference, updatedDiagnostic.ToString(), diagnosticInfo.RequestTimestamp, responseTimestamp),
						(diagnosticInfo.RequestReference, diagnosticInfo.Diagnostic, diagnosticInfo.RequestTimestamp, diagnosticInfo.ResponseTimestamp));
				}
			}

			Cleanup();
		}

		#endregion

		#region PRIVATE METHODS

		private void LogHeaders(StringBuilder diagnostic, HttpHeaders httpHeaders)
		{
			if (httpHeaders != null)
			{
				foreach (var header in httpHeaders)
				{
					if (header.Key.Equals("authorization", StringComparison.OrdinalIgnoreCase))
					{
						diagnostic.AppendLine($"  {header.Key}: ...redacted for security reasons...");
					}
					else
					{
						diagnostic.AppendLine($"  {header.Key}: {string.Join(", ", header.Value)}");
					}
				}
			}
		}

		private void LogContent(StringBuilder diagnostic, HttpContent httpContent)
		{
			if (httpContent == null)
			{
				diagnostic.AppendLine("  Content-Length: 0");
			}
			else
			{
				LogHeaders(diagnostic, httpContent.Headers);

				var contentLength = httpContent.Headers?.ContentLength.GetValueOrDefault(0) ?? 0;
				if (!httpContent.Headers?.Contains("Content-Length") ?? false)
				{
					diagnostic.AppendLine($"  Content-Length: {contentLength}");
				}

				if (contentLength > 0)
				{
					diagnostic.AppendLine();
					diagnostic.AppendLine(httpContent.ReadAsStringAsync(null).ConfigureAwait(false).GetAwaiter().GetResult() ?? "<NULL>");
				}
			}
		}

		private void Cleanup()
		{
			try
			{
				// Remove diagnostic information for requests that have been garbage collected
				foreach (string key in DiagnosticHandler.DiagnosticsInfo.Keys.ToArray())
				{
					if (DiagnosticHandler.DiagnosticsInfo.TryGetValue(key, out (WeakReference<HttpRequestMessage> RequestReference, string Diagnostic, long RequestTimeStamp, long ResponseTimestamp) diagnosticInfo))
					{
						if (!diagnosticInfo.RequestReference.TryGetTarget(out HttpRequestMessage request))
						{
							DiagnosticsInfo.TryRemove(key, out _);
						}
					}
				}
			}
			catch
			{
				// Intentionally left empty
			}
		}

		#endregion
	}
}
