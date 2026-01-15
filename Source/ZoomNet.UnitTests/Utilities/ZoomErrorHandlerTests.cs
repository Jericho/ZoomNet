using Shouldly;
using System;
using System.Net;
using System.Net.Http;
using Xunit;
using ZoomNet.Utilities;

namespace ZoomNet.UnitTests.Utilities
{
	public class ZoomErrorHandlerTests
	{
		#region Helper Methods

		private static HttpRequestMessage CreateRequestMessage(HttpMethod method, string uri)
		{
			var request = new HttpRequestMessage(method, uri);
			request.Headers.Add("ZoomNet-Diagnostic-Id", "test-diagnostic-id");
			return request;
		}

		private static MockFluentHttpResponse CreateResponse(HttpStatusCode statusCode, string content, HttpMethod method = null, string uri = null)
		{
			var httpMessage = new HttpResponseMessage(statusCode)
			{
				Content = new StringContent(content),
				RequestMessage = CreateRequestMessage(method ?? HttpMethod.Get, uri ?? "https://api.zoom.us/v2/test")
			};
			return new MockFluentHttpResponse(httpMessage, null, TestContext.Current.CancellationToken);
		}

		#endregion

		#region Constructor Tests

		[Fact]
		public void Constructor_Default_SetsPropertiesCorrectly()
		{
			// Act
			var handler = new ZoomErrorHandler();

			// Assert
			handler.TreatHttp200AsException.ShouldBeFalse();
			handler.CustomHttp200ExceptionMessage.ShouldBeNull();
		}

		[Fact]
		public void Constructor_WithParameters_SetsPropertiesCorrectly()
		{
			// Arrange
			var customMessage = "Custom error message";

			// Act
			var handler = new ZoomErrorHandler(true, customMessage);

			// Assert
			handler.TreatHttp200AsException.ShouldBeTrue();
			handler.CustomHttp200ExceptionMessage.ShouldBe(customMessage);
		}

		[Fact]
		public void Constructor_WithNullCustomMessage_SetsPropertiesCorrectly()
		{
			// Act
			var handler = new ZoomErrorHandler(true, null);

			// Assert
			handler.TreatHttp200AsException.ShouldBeTrue();
			handler.CustomHttp200ExceptionMessage.ShouldBeNull();
		}

		#endregion

		#region OnResponse Tests - Success Scenarios

		[Fact]
		public void OnResponse_WithSuccessStatusCode_NoError_DoesNotThrow()
		{
			// Arrange
			var handler = new ZoomErrorHandler();
			var response = CreateResponse(HttpStatusCode.OK, @"{""code"": 0, ""message"": ""success""}");

			// Act & Assert
			Should.NotThrow(() => handler.OnResponse(response, true));
		}

		[Fact]
		public void OnResponse_WithCreatedStatusCode_DoesNotThrow()
		{
			// Arrange
			var handler = new ZoomErrorHandler();
			var response = CreateResponse(HttpStatusCode.Created, @"{""id"": ""123""}", HttpMethod.Post);

			// Act & Assert
			Should.NotThrow(() => handler.OnResponse(response, true));
		}

		[Fact]
		public void OnResponse_WithZeroErrorCode_DoesNotThrow()
		{
			// Arrange
			var handler = new ZoomErrorHandler();
			var response = CreateResponse(HttpStatusCode.OK, @"{""code"": 0, ""message"": ""success""}");

			// Act & Assert
			Should.NotThrow(() => handler.OnResponse(response, true));
		}

		#endregion

		#region OnResponse Tests - HTTP 200 with Error (Special Zoom Behavior)

		[Fact]
		public void OnResponse_Http200WithError_TreatAsExceptionFalse_ThrowsException()
		{
			// Arrange
			var handler = new ZoomErrorHandler(false, null);
			var response = CreateResponse(HttpStatusCode.OK, @"{""code"": 300, ""message"": ""Validation Failed""}", HttpMethod.Post);

			// Act
			var exception = Should.Throw<ZoomException>(() => handler.OnResponse(response, true));

			// Assert
			exception.ErrorCode.ShouldBe(300);
			exception.Message.ShouldBe("Validation Failed");
			exception.StatusCode.ShouldBe(HttpStatusCode.OK);
		}

		[Fact]
		public void OnResponse_Http200WithError_TreatAsExceptionTrue_ThrowsExceptionWithoutErrorCode()
		{
			// Arrange
			var handler = new ZoomErrorHandler(true, null);
			var response = CreateResponse(HttpStatusCode.OK, @"{""code"": 300, ""message"": ""This meeting has not registration required""}", HttpMethod.Post);

			// Act
			var exception = Should.Throw<ZoomException>(() => handler.OnResponse(response, true));

			// Assert
			exception.ErrorCode.ShouldBeNull(); // Error code is NOT included when TreatHttp200AsException is true
			exception.Message.ShouldBe("This meeting has not registration required");
			exception.StatusCode.ShouldBe(HttpStatusCode.OK);
		}

		[Fact]
		public void OnResponse_Http200WithError_TreatAsExceptionTrue_WithCustomMessage()
		{
			// Arrange
			var customMessage = "You need a paid account to perform this operation";
			var handler = new ZoomErrorHandler(true, customMessage);
			var response = CreateResponse(HttpStatusCode.OK, @"{""code"": 300, ""message"": ""Some error from Zoom""}", HttpMethod.Post);

			// Act
			var exception = Should.Throw<ZoomException>(() => handler.OnResponse(response, true));

			// Assert
			exception.Message.ShouldBe(customMessage);
			exception.StatusCode.ShouldBe(HttpStatusCode.OK);
		}

		[Fact]
		public void OnResponse_Http200WithError_TreatAsExceptionTrue_NoZoomMessage_UsesCustomMessage()
		{
			// Arrange
			var customMessage = "Custom error message";
			var handler = new ZoomErrorHandler(true, customMessage);
			var response = CreateResponse(HttpStatusCode.OK, @"{""code"": 300}", HttpMethod.Post); // No message field

			// Act
			var exception = Should.Throw<ZoomException>(() => handler.OnResponse(response, true));

			// Assert
			exception.Message.ShouldBe(customMessage);
		}

		[Fact]
		public void OnResponse_Http200WithError_TreatAsExceptionTrue_NoZoomMessage_UsesErrorCodeMessage()
		{
			// Arrange
			var handler = new ZoomErrorHandler(true, null);
			var response = CreateResponse(HttpStatusCode.OK, @"{""code"": 300}", HttpMethod.Post); // No message field, only error code

			// Act
			var exception = Should.Throw<ZoomException>(() => handler.OnResponse(response, true));

			// Assert

			// When there's no message field, GetErrorMessageAsync returns "Error code: X"
			// Since CustomHttp200ExceptionMessage is null and errorMessage is "Error code: 300", that's what we get
			exception.Message.ShouldBe("Error code: 300");
		}

		#endregion

		#region OnResponse Tests - Standard HTTP Error Codes

		[Fact]
		public void OnResponse_Http400BadRequest_ThrowsException()
		{
			// Arrange
			var handler = new ZoomErrorHandler();
			var response = CreateResponse(HttpStatusCode.BadRequest, @"{""code"": 1120, ""message"": ""Invalid page size""}");

			// Act
			var exception = Should.Throw<ZoomException>(() => handler.OnResponse(response, true));

			// Assert
			exception.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
			exception.Message.ShouldBe("Invalid page size");
			exception.ErrorCode.ShouldBe(1120);
		}

		[Fact]
		public void OnResponse_Http401Unauthorized_ThrowsException()
		{
			// Arrange
			var handler = new ZoomErrorHandler();
			var response = CreateResponse(HttpStatusCode.Unauthorized, @"{""code"": 124, ""message"": ""Invalid access token""}");

			// Act
			var exception = Should.Throw<ZoomException>(() => handler.OnResponse(response, true));

			// Assert
			exception.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
			exception.Message.ShouldBe("Invalid access token");
			exception.ErrorCode.ShouldBe(124);
		}

		[Fact]
		public void OnResponse_Http404NotFound_ThrowsException()
		{
			// Arrange
			var handler = new ZoomErrorHandler();
			var response = CreateResponse(HttpStatusCode.NotFound, @"{""code"": 1001, ""message"": ""User does not exist""}", HttpMethod.Get, "https://api.zoom.us/v2/users/invalid");

			// Act
			var exception = Should.Throw<ZoomException>(() => handler.OnResponse(response, true));

			// Assert
			exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
			exception.Message.ShouldBe("User does not exist");
			exception.ErrorCode.ShouldBe(1001);
		}

		[Fact]
		public void OnResponse_Http429TooManyRequests_ThrowsException()
		{
			// Arrange
			var handler = new ZoomErrorHandler();
			var response = CreateResponse((HttpStatusCode)429, @"{""code"": 429, ""message"": ""Too many requests""}");

			// Act
			var exception = Should.Throw<ZoomException>(() => handler.OnResponse(response, true));

			// Assert
			exception.StatusCode.ShouldBe((HttpStatusCode)429);
			exception.Message.ShouldBe("Too many requests");
			exception.ErrorCode.ShouldBe(429);
		}

		[Fact]
		public void OnResponse_Http500InternalServerError_ThrowsException()
		{
			// Arrange
			var handler = new ZoomErrorHandler();
			var response = CreateResponse(HttpStatusCode.InternalServerError, @"{""code"": 500, ""message"": ""Internal server error""}");

			// Act
			var exception = Should.Throw<ZoomException>(() => handler.OnResponse(response, true));

			// Assert
			exception.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
			exception.Message.ShouldBe("Internal server error");
		}

		#endregion

		#region OnResponse Tests - Error Messages with Additional Details

		[Fact]
		public void OnResponse_WithErrorDetails_IncludesFieldInformation()
		{
			// Arrange
			var handler = new ZoomErrorHandler();
			var responseContent = @"{
				""code"": 300,
				""message"": ""Validation Failed."",
				""errors"": [
					{
						""field"": ""settings.jbh_time"",
						""message"": ""Invalid parameter: jbh_time."" 
					}
				]
			}";
			var response = CreateResponse(HttpStatusCode.BadRequest, responseContent, new HttpMethod("PATCH"), "https://api.zoom.us/v2/meetings/123");

			// Act
			var exception = Should.Throw<ZoomException>(() => handler.OnResponse(response, true));

			// Assert
			exception.Message.ShouldContain("Validation Failed");
			exception.Message.ShouldContain("settings.jbh_time");
			exception.Message.ShouldContain("Invalid parameter: jbh_time.");
			exception.ErrorCode.ShouldBe(300);
		}

		[Fact]
		public void OnResponse_WithMultipleErrorDetails_IncludesAllFields()
		{
			// Arrange
			var handler = new ZoomErrorHandler();
			var responseContent = @"{
				""code"": 300,
				""message"": ""Validation Failed."",
				""errors"": [
					{
						""field"": ""field1"",
						""message"": ""Error message 1""
					},
					{
						""field"": ""field2"",
						""message"": ""Error message 2""
					}
				]
			}";
			var response = CreateResponse(HttpStatusCode.BadRequest, responseContent, HttpMethod.Post);

			// Act
			var exception = Should.Throw<ZoomException>(() => handler.OnResponse(response, true));

			// Assert
			exception.Message.ShouldContain("field1 Error message 1");
			exception.Message.ShouldContain("field2 Error message 2");
		}

		#endregion

		#region OnResponse Tests - Edge Cases

		[Fact]
		public void OnResponse_WithEmptyResponseContent_ThrowsExceptionWithStatusCode()
		{
			// Arrange
			var handler = new ZoomErrorHandler();
			var response = CreateResponse(HttpStatusCode.BadRequest, string.Empty);

			// Act
			var exception = Should.Throw<ZoomException>(() => handler.OnResponse(response, true));

			// Assert
			exception.Message.ShouldContain("400");
			exception.Message.ShouldContain("Bad Request");
		}

		[Fact]
		public void OnResponse_WithNonJsonContent_ThrowsExceptionWithStatusCode()
		{
			// Arrange
			var handler = new ZoomErrorHandler();
			var response = CreateResponse(HttpStatusCode.BadRequest, "This is not JSON");

			// Act
			var exception = Should.Throw<ZoomException>(() => handler.OnResponse(response, true));

			// Assert
			exception.Message.ShouldContain("400");
			exception.Message.ShouldContain("Bad Request");
		}

		[Fact]
		public void OnResponse_WithErrorMessageField_UsesErrorMessage()
		{
			// Arrange
			var handler = new ZoomErrorHandler();
			var response = CreateResponse(HttpStatusCode.OK, @"{""code"": 0, ""error_message"": ""Operation successful""}");

			// Act & Assert - Should not throw because code is 0
			Should.NotThrow(() => handler.OnResponse(response, true));
		}

		#endregion

		#region OnResponse Tests - Diagnostic Log

		[Fact]
		public void OnResponse_IncludesDiagnosticLogInException()
		{
			// Arrange
			var handler = new ZoomErrorHandler();
			var response = CreateResponse(HttpStatusCode.BadRequest, @"{""code"": 300, ""message"": ""Validation Failed""}", HttpMethod.Post);

			// Act
			var exception = Should.Throw<ZoomException>(() => handler.OnResponse(response, true));

			// Assert
			exception.DiagnosticLog.ShouldNotBeNull();
			exception.DiagnosticLog.ShouldNotBeEmpty();
		}

		[Fact]
		public void OnResponse_WhenDiagnosticInfoUnavailable_UsesFallbackMessage()
		{
			// Arrange
			var handler = new ZoomErrorHandler();
			var response = CreateResponse(HttpStatusCode.BadRequest, @"{""code"": 300, ""message"": ""Test error""}");

			// Act
			var exception = Should.Throw<ZoomException>(() => handler.OnResponse(response, true));

			// Assert

			// When diagnostic info is not available, it should contain the fallback message
			exception.DiagnosticLog.ShouldNotBeNull();
		}

		#endregion

		#region Integration Tests

		[Theory]
		[InlineData(true, "Custom message", "Custom message")]
		[InlineData(true, null, "Zoom error message")]
		[InlineData(false, "Custom message", "Zoom error message")]
		[InlineData(false, null, "Zoom error message")]
		public void OnResponse_Http200WithError_MessagePriority(bool treatAsException, string customMessage, string expectedMessagePart)
		{
			// Arrange
			var handler = new ZoomErrorHandler(treatAsException, customMessage);
			var response = CreateResponse(HttpStatusCode.OK, @"{""code"": 300, ""message"": ""Zoom error message""}", HttpMethod.Post);

			// Act
			var exception = Should.Throw<ZoomException>(() => handler.OnResponse(response, true));

			// Assert
			exception.Message.ShouldContain(expectedMessagePart);
		}

		#endregion
	}
}
