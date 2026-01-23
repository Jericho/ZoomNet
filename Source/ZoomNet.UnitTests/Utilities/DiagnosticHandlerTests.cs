using Microsoft.Extensions.Logging;
using Pathoschild.Http.Client;
using Shouldly;
using System;
using System.Net;
using System.Net.Http;
using Xunit;
using ZoomNet.Utilities;

namespace ZoomNet.UnitTests.Utilities
{
	public class DiagnosticHandlerTests : IDisposable
	{
		public DiagnosticHandlerTests()
		{
			// Clear shared static state before each test to ensure test isolation
			DiagnosticHandler.DiagnosticsInfo.Clear();
		}

		public void Dispose()
		{
			// Clean up shared state after each test
			DiagnosticHandler.DiagnosticsInfo.Clear();
		}

		#region Constructor Tests

		[Fact]
		public void Constructor_WithAllParameters_SetsPropertiesCorrectly()
		{
			// Arrange
			var successLogLevel = LogLevel.Information;
			var failureLogLevel = LogLevel.Error;
			var mockLogger = new MockLogger();

			// Act
			var handler = new DiagnosticHandler(successLogLevel, failureLogLevel, mockLogger);

			// Assert
			handler.ShouldNotBeNull();
			DiagnosticHandler.DiagnosticsInfo.ShouldNotBeNull();
		}

		[Fact]
		public void Constructor_WithNullLogger_UsesNullLogger()
		{
			// Arrange
			var successLogLevel = LogLevel.Debug;
			var failureLogLevel = LogLevel.Warning;

			// Act
			var handler = new DiagnosticHandler(successLogLevel, failureLogLevel, null);

			// Assert
			handler.ShouldNotBeNull();
		}

		[Fact]
		public void Constructor_WithDifferentLogLevels_WorksCorrectly()
		{
			// Arrange & Act
			var handler1 = new DiagnosticHandler(LogLevel.Trace, LogLevel.Critical);
			var handler2 = new DiagnosticHandler(LogLevel.Information, LogLevel.Error);
			var handler3 = new DiagnosticHandler(LogLevel.None, LogLevel.None);

			// Assert
			handler1.ShouldNotBeNull();
			handler2.ShouldNotBeNull();
			handler3.ShouldNotBeNull();
		}

		#endregion

		#region OnRequest Tests

		[Fact]
		public void OnRequest_AddsDiagnosticIdHeader()
		{
			// Arrange
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error);
			var MockFluentHttpRequest = new MockFluentHttpRequest();

			// Act
			handler.OnRequest(MockFluentHttpRequest);

			// Assert
			MockFluentHttpRequest.HeaderAdded.ShouldBeTrue();
			MockFluentHttpRequest.HeaderName.ShouldBe(DiagnosticHandler.DIAGNOSTIC_ID_HEADER_NAME);
			MockFluentHttpRequest.HeaderValue.ShouldNotBeNullOrEmpty();
		}

		[Fact]
		public void OnRequest_GeneratesUniqueDiagnosticIds()
		{
			// Arrange
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error);
			var MockFluentHttpRequest1 = new MockFluentHttpRequest();
			var MockFluentHttpRequest2 = new MockFluentHttpRequest();
			var MockFluentHttpRequest3 = new MockFluentHttpRequest();

			// Act
			handler.OnRequest(MockFluentHttpRequest1);
			handler.OnRequest(MockFluentHttpRequest2);
			handler.OnRequest(MockFluentHttpRequest3);

			// Assert
			MockFluentHttpRequest1.HeaderValue.ShouldNotBe(MockFluentHttpRequest2.HeaderValue);
			MockFluentHttpRequest2.HeaderValue.ShouldNotBe(MockFluentHttpRequest3.HeaderValue);
			MockFluentHttpRequest1.HeaderValue.ShouldNotBe(MockFluentHttpRequest3.HeaderValue);
		}

		[Fact]
		public void OnRequest_AddsDiagnosticInfoToCache()
		{
			// Arrange
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error);
			var MockFluentHttpRequest = new MockFluentHttpRequest();
			// Act
			handler.OnRequest(MockFluentHttpRequest);

			// Assert
			DiagnosticHandler.DiagnosticsInfo.Count.ShouldBeGreaterThan(0);
			DiagnosticHandler.DiagnosticsInfo.ContainsKey(MockFluentHttpRequest.HeaderValue).ShouldBeTrue();
		}

		[Fact]
		public void OnRequest_DiagnosticInfoContainsRequestReference()
		{
			// Arrange
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error);
			var MockFluentHttpRequest = new MockFluentHttpRequest();

			// Act
			handler.OnRequest(MockFluentHttpRequest);

			// Assert
			var diagnosticInfo = DiagnosticHandler.DiagnosticsInfo[MockFluentHttpRequest.HeaderValue];
			diagnosticInfo.RequestReference.ShouldNotBeNull();
			diagnosticInfo.RequestReference.TryGetTarget(out HttpRequestMessage request).ShouldBeTrue();
			request.ShouldBe(MockFluentHttpRequest.Message);
		}

		[Fact]
		public void OnRequest_DiagnosticInfoContainsTimestamp()
		{
			// Arrange
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error);
			var MockFluentHttpRequest = new MockFluentHttpRequest();

			// Act
			handler.OnRequest(MockFluentHttpRequest);

			// Assert
			var diagnosticInfo = DiagnosticHandler.DiagnosticsInfo[MockFluentHttpRequest.HeaderValue];
			diagnosticInfo.RequestTimestamp.ShouldBeGreaterThan(0);
		}

		[Fact]
		public void OnRequest_DiagnosticInfoContainsOptions()
		{
			// Arrange
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error);
			var MockFluentHttpRequest = new MockFluentHttpRequest();

			// Act
			handler.OnRequest(MockFluentHttpRequest);

			// Assert
			var diagnosticInfo = DiagnosticHandler.DiagnosticsInfo[MockFluentHttpRequest.HeaderValue];
			diagnosticInfo.Options.ShouldNotBeNull();
			diagnosticInfo.Options.ShouldBe(MockFluentHttpRequest.Options);
		}

		[Fact]
		public void OnRequest_MultipleRequests_AddsMultipleDiagnosticInfos()
		{
			// Arrange
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error);
			var MockFluentHttpRequest1 = new MockFluentHttpRequest();
			var MockFluentHttpRequest2 = new MockFluentHttpRequest();
			var MockFluentHttpRequest3 = new MockFluentHttpRequest();
			// Act
			handler.OnRequest(MockFluentHttpRequest1);
			handler.OnRequest(MockFluentHttpRequest2);
			handler.OnRequest(MockFluentHttpRequest3);

			// Assert
			DiagnosticHandler.DiagnosticsInfo.Count.ShouldBe(3);
			DiagnosticHandler.DiagnosticsInfo.ContainsKey(MockFluentHttpRequest1.HeaderValue).ShouldBeTrue();
			DiagnosticHandler.DiagnosticsInfo.ContainsKey(MockFluentHttpRequest2.HeaderValue).ShouldBeTrue();
			DiagnosticHandler.DiagnosticsInfo.ContainsKey(MockFluentHttpRequest3.HeaderValue).ShouldBeTrue();
		}

		#endregion

		#region OnResponse Tests - Success Scenarios

		[Fact]
		public void OnResponse_WithSuccessStatusCode_UpdatesDiagnosticInfo()
		{
			// Arrange
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error);
			var MockFluentHttpRequest = new MockFluentHttpRequest();
			handler.OnRequest(MockFluentHttpRequest);

			var response = Utils.CreateResponse(HttpStatusCode.OK, @"{""success"": true}");
			response.Message.RequestMessage.Headers.Add(DiagnosticHandler.DIAGNOSTIC_ID_HEADER_NAME, MockFluentHttpRequest.HeaderValue);

			// Act
			handler.OnResponse(response, true);

			// Assert
			var diagnosticInfo = DiagnosticHandler.DiagnosticsInfo[MockFluentHttpRequest.HeaderValue];
			diagnosticInfo.ResponseReference.ShouldNotBeNull();
			diagnosticInfo.ResponseReference.TryGetTarget(out HttpResponseMessage responseMessage).ShouldBeTrue();
			responseMessage.ShouldBe(response.Message);
		}

		[Fact]
		public void OnResponse_WithSuccessStatusCode_UpdatesTimestamp()
		{
			// Arrange
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error);
			var MockFluentHttpRequest = new MockFluentHttpRequest();
			handler.OnRequest(MockFluentHttpRequest);

			var initialTimestamp = DiagnosticHandler.DiagnosticsInfo[MockFluentHttpRequest.HeaderValue].ResponseTimestamp;
			var response = Utils.CreateResponse(HttpStatusCode.OK, @"{""success"": true}");
			response.Message.RequestMessage.Headers.Add(DiagnosticHandler.DIAGNOSTIC_ID_HEADER_NAME, MockFluentHttpRequest.HeaderValue);

			// Act
			handler.OnResponse(response, true);

			// Assert
			var diagnosticInfo = DiagnosticHandler.DiagnosticsInfo[MockFluentHttpRequest.HeaderValue];
			diagnosticInfo.ResponseTimestamp.ShouldNotBe(initialTimestamp);
			diagnosticInfo.ResponseTimestamp.ShouldNotBe(long.MinValue);
		}

		[Fact]
		public void OnResponse_WithUnknownDiagnosticId_DoesNotThrow()
		{
			// Arrange
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error);
			var response = Utils.CreateResponse(HttpStatusCode.OK, @"{""success"": true}");
			response.Message.RequestMessage.Headers.Add(DiagnosticHandler.DIAGNOSTIC_ID_HEADER_NAME, "unknown-diagnostic-id");

			// Act & Assert
			Should.NotThrow(() => handler.OnResponse(response, true));
		}

		#endregion

		#region OnResponse Tests - Logging

		[Fact]
		public void OnResponse_WithSuccessStatusCode_LogsAtSuccessLevel()
		{
			// Arrange
			var mockLogger = new MockLogger();
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error, mockLogger);
			var MockFluentHttpRequest = new MockFluentHttpRequest();
			handler.OnRequest(MockFluentHttpRequest);

			var response = Utils.CreateResponse(HttpStatusCode.OK, @"{""success"": true}");
			response.Message.RequestMessage.Headers.Add(DiagnosticHandler.DIAGNOSTIC_ID_HEADER_NAME, MockFluentHttpRequest.HeaderValue);

			// Act
			handler.OnResponse(response, true);

			// Assert
			mockLogger.LogCount.ShouldBeGreaterThanOrEqualTo(1);
			mockLogger.LastLogLevel.ShouldBe(LogLevel.Information);
		}

		[Fact]
		public void OnResponse_WithFailureStatusCode_LogsAtFailureLevel()
		{
			// Arrange
			var mockLogger = new MockLogger();
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error, mockLogger);
			var MockFluentHttpRequest = new MockFluentHttpRequest();
			handler.OnRequest(MockFluentHttpRequest);

			var response = Utils.CreateResponse(HttpStatusCode.BadRequest, @"{""error"": ""Bad request""}");
			response.Message.RequestMessage.Headers.Add(DiagnosticHandler.DIAGNOSTIC_ID_HEADER_NAME, MockFluentHttpRequest.HeaderValue);

			// Act
			handler.OnResponse(response, true);

			// Assert
			mockLogger.LogCount.ShouldBeGreaterThanOrEqualTo(1);
			mockLogger.LastLogLevel.ShouldBe(LogLevel.Error);
		}

		[Fact]
		public void OnResponse_WhenLoggerDisabled_DoesNotLog()
		{
			// Arrange
			var mockLogger = new MockLogger { IsLoggingEnabled = false };
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error, mockLogger);
			var MockFluentHttpRequest = new MockFluentHttpRequest();
			handler.OnRequest(MockFluentHttpRequest);

			var response = Utils.CreateResponse(HttpStatusCode.OK, @"{""success"": true}");
			response.Message.RequestMessage.Headers.Add(DiagnosticHandler.DIAGNOSTIC_ID_HEADER_NAME, MockFluentHttpRequest.HeaderValue);

			// Act
			handler.OnResponse(response, true);

			// Assert
			mockLogger.LogCount.ShouldBe(0);
		}

		[Fact]
		public void OnResponse_WithNullLogger_DoesNotThrow()
		{
			// Arrange
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error, null);
			var MockFluentHttpRequest = new MockFluentHttpRequest();
			handler.OnRequest(MockFluentHttpRequest);

			var response = Utils.CreateResponse(HttpStatusCode.OK, @"{""success"": true}");
			response.Message.RequestMessage.Headers.Add(DiagnosticHandler.DIAGNOSTIC_ID_HEADER_NAME, MockFluentHttpRequest.HeaderValue);

			// Act & Assert
			Should.NotThrow(() => handler.OnResponse(response, true));
		}

		#endregion

		#region OnResponse Tests - Different Status Codes

		[Fact]
		public void OnResponse_WithCreatedStatusCode_LogsAtSuccessLevel()
		{
			// Arrange
			var mockLogger = new MockLogger();
			var handler = new DiagnosticHandler(LogLevel.Debug, LogLevel.Warning, mockLogger);
			var MockFluentHttpRequest = new MockFluentHttpRequest();
			handler.OnRequest(MockFluentHttpRequest);

			var response = Utils.CreateResponse(HttpStatusCode.Created, @"{""id"": ""123""}");
			response.Message.RequestMessage.Headers.Add(DiagnosticHandler.DIAGNOSTIC_ID_HEADER_NAME, MockFluentHttpRequest.HeaderValue);

			// Act
			handler.OnResponse(response, true);

			// Assert
			mockLogger.LastLogLevel.ShouldBe(LogLevel.Debug);
		}

		[Fact]
		public void OnResponse_WithNotFoundStatusCode_LogsAtFailureLevel()
		{
			// Arrange
			var mockLogger = new MockLogger();
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Critical, mockLogger);
			var MockFluentHttpRequest = new MockFluentHttpRequest();
			handler.OnRequest(MockFluentHttpRequest);

			var response = Utils.CreateResponse(HttpStatusCode.NotFound, @"{""error"": ""Not found""}");
			response.Message.RequestMessage.Headers.Add(DiagnosticHandler.DIAGNOSTIC_ID_HEADER_NAME, MockFluentHttpRequest.HeaderValue);

			// Act
			handler.OnResponse(response, true);

			// Assert
			mockLogger.LastLogLevel.ShouldBe(LogLevel.Critical);
		}

		[Fact]
		public void OnResponse_WithUnauthorizedStatusCode_LogsAtFailureLevel()
		{
			// Arrange
			var mockLogger = new MockLogger();
			var handler = new DiagnosticHandler(LogLevel.Trace, LogLevel.Error, mockLogger);
			var MockFluentHttpRequest = new MockFluentHttpRequest();
			handler.OnRequest(MockFluentHttpRequest);

			var response = Utils.CreateResponse(HttpStatusCode.Unauthorized, @"{""error"": ""Unauthorized""}");
			response.Message.RequestMessage.Headers.Add(DiagnosticHandler.DIAGNOSTIC_ID_HEADER_NAME, MockFluentHttpRequest.HeaderValue);

			// Act
			handler.OnResponse(response, true);

			// Assert
			mockLogger.LastLogLevel.ShouldBe(LogLevel.Error);
		}

		#endregion

		#region Cleanup Tests

		[Fact]
		public void OnResponse_TriggersCleanup()
		{
			// Arrange
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error);
			var MockFluentHttpRequest = new MockFluentHttpRequest();
			handler.OnRequest(MockFluentHttpRequest);

			var response = Utils.CreateResponse(HttpStatusCode.OK, @"{""success"": true}");
			response.Message.RequestMessage.Headers.Add(DiagnosticHandler.DIAGNOSTIC_ID_HEADER_NAME, MockFluentHttpRequest.HeaderValue);

			// Act
			handler.OnResponse(response, true);

			// Assert
			// Cleanup should have been called but may not have removed anything if references are still alive
			// This just verifies that cleanup doesn't throw
			DiagnosticHandler.DiagnosticsInfo.ShouldNotBeNull();
		}

		[Fact]
		public void Cleanup_RemovesGarbageCollectedRequests()
		{
			// Arrange
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error);
			var diagnosticId = Guid.NewGuid().ToString("N");

			// Add a diagnostic info with a weak reference that will be garbage collected
			var weakRef = new WeakReference<HttpRequestMessage>(null);
			DiagnosticHandler.DiagnosticsInfo.TryAdd(diagnosticId, new DiagnosticInfo(
				weakRef,
				0,
				null,
				long.MinValue,
				new RequestOptions()));

			var MockFluentHttpRequest = new MockFluentHttpRequest();
			handler.OnRequest(MockFluentHttpRequest);

			var response = Utils.CreateResponse(HttpStatusCode.OK, @"{""success"": true}");
			response.Message.RequestMessage.Headers.Add(DiagnosticHandler.DIAGNOSTIC_ID_HEADER_NAME, MockFluentHttpRequest.HeaderValue);

			// Act
			handler.OnResponse(response, true);

			// Assert
			// The cleanup method should have removed the entry with null target
			DiagnosticHandler.DiagnosticsInfo.ContainsKey(diagnosticId).ShouldBeFalse();
		}

		#endregion

		#region Integration Tests

		[Fact]
		public void FullRequestResponseCycle()
		{
			// Arrange
			var mockLogger = new MockLogger();
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error, mockLogger);
			var MockFluentHttpRequest = new MockFluentHttpRequest();

			// Act - Request
			handler.OnRequest(MockFluentHttpRequest);

			// Assert - After Request
			DiagnosticHandler.DiagnosticsInfo.ContainsKey(MockFluentHttpRequest.HeaderValue).ShouldBeTrue();
			var diagnosticInfo = DiagnosticHandler.DiagnosticsInfo[MockFluentHttpRequest.HeaderValue];
			diagnosticInfo.RequestReference.TryGetTarget(out HttpRequestMessage _).ShouldBeTrue();
			diagnosticInfo.ResponseReference.ShouldBeNull();

			// Act - Response
			var response = Utils.CreateResponse(HttpStatusCode.OK, @"{""data"": ""test""}");
			response.Message.RequestMessage.Headers.Add(DiagnosticHandler.DIAGNOSTIC_ID_HEADER_NAME, MockFluentHttpRequest.HeaderValue);
			handler.OnResponse(response, true);

			// Assert - After Response
			diagnosticInfo = DiagnosticHandler.DiagnosticsInfo[MockFluentHttpRequest.HeaderValue];
			diagnosticInfo.ResponseReference.ShouldNotBeNull();
			diagnosticInfo.ResponseReference.TryGetTarget(out HttpResponseMessage _).ShouldBeTrue();
			diagnosticInfo.ResponseTimestamp.ShouldNotBe(long.MinValue);

			// Verify logging occurred
			mockLogger.LogCount.ShouldBeGreaterThanOrEqualTo(1);
		}

		[Fact]
		public void MultipleRequestResponseCycles()
		{
			// Arrange
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error);
			var MockFluentHttpRequest1 = new MockFluentHttpRequest();
			var MockFluentHttpRequest2 = new MockFluentHttpRequest();
			var MockFluentHttpRequest3 = new MockFluentHttpRequest();

			// Act - Multiple Requests
			handler.OnRequest(MockFluentHttpRequest1);
			handler.OnRequest(MockFluentHttpRequest2);
			handler.OnRequest(MockFluentHttpRequest3);

			// Create responses
			var response1 = Utils.CreateResponse(HttpStatusCode.OK, @"{""data"": ""test1""}");
			response1.Message.RequestMessage.Headers.Add(DiagnosticHandler.DIAGNOSTIC_ID_HEADER_NAME, MockFluentHttpRequest1.HeaderValue);

			var response2 = Utils.CreateResponse(HttpStatusCode.Created, @"{""data"": ""test2""}");
			response2.Message.RequestMessage.Headers.Add(DiagnosticHandler.DIAGNOSTIC_ID_HEADER_NAME, MockFluentHttpRequest2.HeaderValue);

			var response3 = Utils.CreateResponse(HttpStatusCode.BadRequest, @"{""error"": ""test3""}");
			response3.Message.RequestMessage.Headers.Add(DiagnosticHandler.DIAGNOSTIC_ID_HEADER_NAME, MockFluentHttpRequest3.HeaderValue);

			// Act - Multiple Responses
			handler.OnResponse(response1, true);
			handler.OnResponse(response2, true);
			handler.OnResponse(response3, true);

			// Assert
			DiagnosticHandler.DiagnosticsInfo.ContainsKey(MockFluentHttpRequest1.HeaderValue).ShouldBeTrue();
			DiagnosticHandler.DiagnosticsInfo.ContainsKey(MockFluentHttpRequest2.HeaderValue).ShouldBeTrue();
			DiagnosticHandler.DiagnosticsInfo.ContainsKey(MockFluentHttpRequest3.HeaderValue).ShouldBeTrue();

			var info1 = DiagnosticHandler.DiagnosticsInfo[MockFluentHttpRequest1.HeaderValue];
			var info2 = DiagnosticHandler.DiagnosticsInfo[MockFluentHttpRequest2.HeaderValue];
			var info3 = DiagnosticHandler.DiagnosticsInfo[MockFluentHttpRequest3.HeaderValue];

			info1.ResponseReference.TryGetTarget(out HttpResponseMessage r1).ShouldBeTrue();
			info2.ResponseReference.TryGetTarget(out HttpResponseMessage r2).ShouldBeTrue();
			info3.ResponseReference.TryGetTarget(out HttpResponseMessage r3).ShouldBeTrue();

			r1.StatusCode.ShouldBe(HttpStatusCode.OK);
			r2.StatusCode.ShouldBe(HttpStatusCode.Created);
			r3.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
		}

		[Theory]
		[InlineData(LogLevel.Trace, LogLevel.Critical)]
		[InlineData(LogLevel.Debug, LogLevel.Error)]
		[InlineData(LogLevel.Information, LogLevel.Warning)]
		[InlineData(LogLevel.Warning, LogLevel.Critical)]
		[InlineData(LogLevel.Error, LogLevel.Critical)]
		public void OnResponse_WithVariousLogLevels_LogsCorrectly(LogLevel successLevel, LogLevel failureLevel)
		{
			// Arrange
			var mockLogger = new MockLogger();
			var handler = new DiagnosticHandler(successLevel, failureLevel, mockLogger);
			var MockFluentHttpRequest = new MockFluentHttpRequest();
			handler.OnRequest(MockFluentHttpRequest);

			var response = Utils.CreateResponse(HttpStatusCode.OK, @"{""success"": true}");
			response.Message.RequestMessage.Headers.Add(DiagnosticHandler.DIAGNOSTIC_ID_HEADER_NAME, MockFluentHttpRequest.HeaderValue);

			// Act
			handler.OnResponse(response, true);

			// Assert
			mockLogger.LastLogLevel.ShouldBe(successLevel);
		}

		#endregion
	}
}
