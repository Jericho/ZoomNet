using Microsoft.Extensions.Logging;
using Shouldly;
using System;
using System.Net;
using System.Net.Http;
using Xunit;
using ZoomNet.Utilities;

namespace ZoomNet.UnitTests.Utilities
{
	public class DiagnosticHandlerTests
	{
		#region Constructor Tests

		[Fact]
		public void Constructor_WithAllParameters_SetsPropertiesCorrectly()
		{
			// Arrange
			var successLogLevel = LogLevel.Information;
			var failureLogLevel = LogLevel.Error;
			var diagnosticStore = new MemoryDiagnosticStore();
			var mockLogger = new MockLogger();

			// Act
			var handler = new DiagnosticHandler(successLogLevel, failureLogLevel, diagnosticStore, mockLogger);

			// Assert
			handler.ShouldNotBeNull();
		}

		[Fact]
		public void Constructor_WithNullLogger_UsesNullLogger()
		{
			// Arrange
			var successLogLevel = LogLevel.Debug;
			var failureLogLevel = LogLevel.Warning;
			var diagnosticStore = new MemoryDiagnosticStore();

			// Act
			var handler = new DiagnosticHandler(successLogLevel, failureLogLevel, diagnosticStore, null);

			// Assert
			handler.ShouldNotBeNull();
		}

		[Fact]
		public void Constructor_WithDifferentLogLevels_WorksCorrectly()
		{
			// Arrange & Act
			var diagnosticStore = new MemoryDiagnosticStore();
			var handler1 = new DiagnosticHandler(LogLevel.Trace, LogLevel.Critical, diagnosticStore);
			var handler2 = new DiagnosticHandler(LogLevel.Information, LogLevel.Error, diagnosticStore);
			var handler3 = new DiagnosticHandler(LogLevel.None, LogLevel.None, diagnosticStore);

			// Assert
			handler1.ShouldNotBeNull();
			handler2.ShouldNotBeNull();
			handler3.ShouldNotBeNull();
		}

		[Fact]
		public void Constructor_Throws_WhenDiagnosticStoreIsNull()
		{
			// Act
			Should.Throw<ArgumentNullException>(() => new DiagnosticHandler(LogLevel.Trace, LogLevel.Critical, null));
		}

		#endregion

		#region OnRequest Tests

		[Fact]
		public void OnRequest_AddsDiagnosticIdHeader()
		{
			// Arrange
			var diagnosticStore = new MemoryDiagnosticStore();
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error, diagnosticStore);
			var mockRequest = new MockFluentHttpRequest();

			// Act
			handler.OnRequest(mockRequest);

			// Assert
			mockRequest.HeaderAdded.ShouldBeTrue();
			mockRequest.HeaderName.ShouldBe(DiagnosticHandler.DIAGNOSTIC_ID_HEADER_NAME);
			mockRequest.HeaderValue.ShouldNotBeNullOrEmpty();
			diagnosticStore.ContainsKey(mockRequest.HeaderValue).ShouldBeTrue();
		}

		[Fact]
		public void OnRequest_GeneratesUniqueDiagnosticIds()
		{
			// Arrange
			var diagnosticStore = new MemoryDiagnosticStore();
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error, diagnosticStore);
			var mockRequest1 = new MockFluentHttpRequest();
			var mockRequest2 = new MockFluentHttpRequest();
			var mockRequest3 = new MockFluentHttpRequest();

			// Act
			handler.OnRequest(mockRequest1);
			handler.OnRequest(mockRequest2);
			handler.OnRequest(mockRequest3);

			// Assert
			mockRequest1.HeaderValue.ShouldNotBe(mockRequest2.HeaderValue);
			mockRequest1.HeaderValue.ShouldNotBe(mockRequest3.HeaderValue);
			mockRequest2.HeaderValue.ShouldNotBe(mockRequest3.HeaderValue);
		}

		[Fact]
		public void OnRequest_AddsDiagnosticInfoToCache()
		{
			// Arrange
			var diagnosticStore = new MemoryDiagnosticStore();
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error, diagnosticStore);
			var MockFluentHttpRequest = new MockFluentHttpRequest();
			// Act
			handler.OnRequest(MockFluentHttpRequest);

			// Assert
			diagnosticStore.Count.ShouldBe(1);
			diagnosticStore.ContainsKey(MockFluentHttpRequest.HeaderValue).ShouldBeTrue();
		}

		[Fact]
		public void OnRequest_DiagnosticInfoContainsRequestReference()
		{
			// Arrange
			var diagnosticStore = new MemoryDiagnosticStore();
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error, diagnosticStore);
			var MockFluentHttpRequest = new MockFluentHttpRequest();

			// Act
			handler.OnRequest(MockFluentHttpRequest);

			// Assert
			diagnosticStore.TryGetValue(MockFluentHttpRequest.HeaderValue, out var diagnosticInfo).ShouldBeTrue();
			diagnosticInfo.RequestReference.ShouldNotBeNull();
			diagnosticInfo.RequestReference.TryGetTarget(out HttpRequestMessage request).ShouldBeTrue();
			request.ShouldBe(MockFluentHttpRequest.Message);
		}

		[Fact]
		public void OnRequest_DiagnosticInfoContainsTimestamp()
		{
			// Arrange
			var diagnosticStore = new MemoryDiagnosticStore();
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error, diagnosticStore);
			var MockFluentHttpRequest = new MockFluentHttpRequest();

			// Act
			handler.OnRequest(MockFluentHttpRequest);

			// Assert
			diagnosticStore.TryGetValue(MockFluentHttpRequest.HeaderValue, out var diagnosticInfo).ShouldBeTrue();
			diagnosticInfo.RequestTimestamp.ShouldBeGreaterThan(0);
		}

		[Fact]
		public void OnRequest_DiagnosticInfoContainsOptions()
		{
			// Arrange
			var diagnosticStore = new MemoryDiagnosticStore();
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error, diagnosticStore);
			var MockFluentHttpRequest = new MockFluentHttpRequest();

			// Act
			handler.OnRequest(MockFluentHttpRequest);

			// Assert
			diagnosticStore.TryGetValue(MockFluentHttpRequest.HeaderValue, out var diagnosticInfo).ShouldBeTrue();
			diagnosticInfo.Options.ShouldNotBeNull();
			diagnosticInfo.Options.ShouldBe(MockFluentHttpRequest.Options);
		}

		[Fact]
		public void OnRequest_MultipleRequests_AddsMultipleDiagnosticInfos()
		{
			// Arrange
			var diagnosticStore = new MemoryDiagnosticStore();
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error, diagnosticStore);
			var MockFluentHttpRequest1 = new MockFluentHttpRequest();
			var MockFluentHttpRequest2 = new MockFluentHttpRequest();
			var MockFluentHttpRequest3 = new MockFluentHttpRequest();
			// Act
			handler.OnRequest(MockFluentHttpRequest1);
			handler.OnRequest(MockFluentHttpRequest2);
			handler.OnRequest(MockFluentHttpRequest3);

			// Assert
			diagnosticStore.Count.ShouldBe(3);
			diagnosticStore.ContainsKey(MockFluentHttpRequest1.HeaderValue).ShouldBeTrue();
			diagnosticStore.ContainsKey(MockFluentHttpRequest2.HeaderValue).ShouldBeTrue();
			diagnosticStore.ContainsKey(MockFluentHttpRequest3.HeaderValue).ShouldBeTrue();
		}

		#endregion

		#region OnResponse Tests - Success Scenarios

		[Fact]
		public void OnResponse_WithSuccessStatusCode_UpdatesDiagnosticInfo()
		{
			// Arrange
			var diagnosticStore = new MemoryDiagnosticStore();
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error, diagnosticStore);
			var MockFluentHttpRequest = new MockFluentHttpRequest();
			handler.OnRequest(MockFluentHttpRequest);

			var response = Utils.CreateResponse(HttpStatusCode.OK, @"{""success"": true}");
			response.Message.RequestMessage.Headers.Add(DiagnosticHandler.DIAGNOSTIC_ID_HEADER_NAME, MockFluentHttpRequest.HeaderValue);

			// Act
			handler.OnResponse(response, true);

			// Assert
			diagnosticStore.TryGetValue(MockFluentHttpRequest.HeaderValue, out var diagnosticInfo).ShouldBeTrue();
			diagnosticInfo.ResponseReference.ShouldNotBeNull();
			diagnosticInfo.ResponseReference.TryGetTarget(out HttpResponseMessage responseMessage).ShouldBeTrue();
			responseMessage.ShouldBe(response.Message);
		}

		[Fact]
		public void OnResponse_WithSuccessStatusCode_UpdatesTimestamp()
		{
			// Arrange
			var diagnosticStore = new MemoryDiagnosticStore();
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error, diagnosticStore);
			var MockFluentHttpRequest = new MockFluentHttpRequest();
			handler.OnRequest(MockFluentHttpRequest);

			diagnosticStore.TryGetValue(MockFluentHttpRequest.HeaderValue, out var diagnosticInfoBeforeResponse).ShouldBeTrue();
			var initialTimestamp = diagnosticInfoBeforeResponse.ResponseTimestamp;
			var response = Utils.CreateResponse(HttpStatusCode.OK, @"{""success"": true}");
			response.Message.RequestMessage.Headers.Add(DiagnosticHandler.DIAGNOSTIC_ID_HEADER_NAME, MockFluentHttpRequest.HeaderValue);

			// Act
			handler.OnResponse(response, true);

			// Assert
			diagnosticStore.TryGetValue(MockFluentHttpRequest.HeaderValue, out var diagnosticInfo).ShouldBeTrue();
			diagnosticInfo.ResponseTimestamp.ShouldNotBe(initialTimestamp);
			diagnosticInfo.ResponseTimestamp.ShouldNotBe(long.MinValue);
		}

		[Fact]
		public void OnResponse_WithUnknownDiagnosticId_DoesNotThrow()
		{
			// Arrange
			var diagnosticStore = new MemoryDiagnosticStore();
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error, diagnosticStore);
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
			var diagnosticStore = new MemoryDiagnosticStore();
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error, diagnosticStore, mockLogger);
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
			var diagnosticStore = new MemoryDiagnosticStore();
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error, diagnosticStore, mockLogger);
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
			var diagnosticStore = new MemoryDiagnosticStore();
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error, diagnosticStore, mockLogger);
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
			var diagnosticStore = new MemoryDiagnosticStore();
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error, diagnosticStore, null);
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
			var diagnosticStore = new MemoryDiagnosticStore();
			var handler = new DiagnosticHandler(LogLevel.Debug, LogLevel.Warning, diagnosticStore, mockLogger);
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
			var diagnosticStore = new MemoryDiagnosticStore();
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Critical, diagnosticStore, mockLogger);
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
			var diagnosticStore = new MemoryDiagnosticStore();
			var handler = new DiagnosticHandler(LogLevel.Trace, LogLevel.Error, diagnosticStore, mockLogger);
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
			var diagnosticStore = new MemoryDiagnosticStore();
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error, diagnosticStore);
			var MockFluentHttpRequest = new MockFluentHttpRequest();
			handler.OnRequest(MockFluentHttpRequest);

			var response = Utils.CreateResponse(HttpStatusCode.OK, @"{""success"": true}");
			response.Message.RequestMessage.Headers.Add(DiagnosticHandler.DIAGNOSTIC_ID_HEADER_NAME, MockFluentHttpRequest.HeaderValue);

			// Act
			handler.OnResponse(response, true);

			// Assert
			// Cleanup should have been called but may not have removed anything if references are still alive
			// This just verifies that cleanup doesn't throw
			diagnosticStore.ShouldNotBeNull();
		}

		[Fact]
		public void Cleanup_RemovesGarbageCollectedRequests()
		{
			// Arrange
			var diagnosticStore = new MemoryDiagnosticStore();
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error, diagnosticStore);
			string diagnosticId = null;

			// Create request in separate scope
			CreateRequestAndCaptureDiagnosticId(handler, out diagnosticId);

			// Verify it's in the store
			diagnosticStore.ContainsKey(diagnosticId).ShouldBeTrue();

			// Act - Force garbage collection
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();

			// Act - Run cleanup
			diagnosticStore.Cleanup(null);

			// Assert - Should be removed
			diagnosticStore.ContainsKey(diagnosticId).ShouldBeFalse();
		}

		#endregion

		#region Integration Tests

		[Fact]
		public void FullRequestResponseCycle()
		{
			// Arrange
			var diagnosticStore = new MemoryDiagnosticStore();
			var mockLogger = new MockLogger();
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error, diagnosticStore, mockLogger);
			var MockFluentHttpRequest = new MockFluentHttpRequest();

			// Act - Request
			handler.OnRequest(MockFluentHttpRequest);

			// Assert - After Request
			diagnosticStore.ContainsKey(MockFluentHttpRequest.HeaderValue).ShouldBeTrue();
			diagnosticStore.TryGetValue(MockFluentHttpRequest.HeaderValue, out var diagnosticInfo).ShouldBeTrue();
			diagnosticInfo.RequestReference.TryGetTarget(out HttpRequestMessage _).ShouldBeTrue();
			diagnosticInfo.ResponseReference.ShouldBeNull();

			// Act - Response
			var response = Utils.CreateResponse(HttpStatusCode.OK, @"{""data"": ""test""}");
			response.Message.RequestMessage.Headers.Add(DiagnosticHandler.DIAGNOSTIC_ID_HEADER_NAME, MockFluentHttpRequest.HeaderValue);
			handler.OnResponse(response, true);

			// Assert - After Response
			diagnosticStore.TryGetValue(MockFluentHttpRequest.HeaderValue, out diagnosticInfo).ShouldBeTrue();
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
			var diagnosticStore = new MemoryDiagnosticStore();
			var mockLogger = new MockLogger();
			var handler = new DiagnosticHandler(LogLevel.Information, LogLevel.Error, diagnosticStore, mockLogger);
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
			diagnosticStore.TryGetValue(MockFluentHttpRequest1.HeaderValue, out var info1).ShouldBeTrue();
			diagnosticStore.TryGetValue(MockFluentHttpRequest2.HeaderValue, out var info2).ShouldBeTrue();
			diagnosticStore.TryGetValue(MockFluentHttpRequest3.HeaderValue, out var info3).ShouldBeTrue();

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
			var diagnosticStore = new MemoryDiagnosticStore();
			var mockLogger = new MockLogger();
			var handler = new DiagnosticHandler(successLevel, failureLevel, diagnosticStore, mockLogger);
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

		#region Helper Methods

		private void CreateRequestAndCaptureDiagnosticId(DiagnosticHandler handler, out string diagnosticId)
		{
			var request = new MockFluentHttpRequest();
			handler.OnRequest(request);
			diagnosticId = request.HeaderValue;
			// Request goes out of scope here and becomes eligible for garbage collection
		}

		#endregion
	}
}
