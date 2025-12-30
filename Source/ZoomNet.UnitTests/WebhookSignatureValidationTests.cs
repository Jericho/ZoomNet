using Shouldly;
using System;
using System.Security;
using Xunit;

namespace ZoomNet.UnitTests
{
	/// <summary>
	/// Unit tests for webhook signature validation.
	/// </summary>
	public class WebhookSignatureValidationTests
	{
		#region Valid Signature Tests

		[Fact]
		public void VerifySignature_WithValidSignature_ReturnsTrue()
		{
			// Arrange
			var requestBody = "{\"payload\":{\"plainToken\":\"xys8n8PGS7mAU0m5-YJjRA\"},\"event_ts\":1720705455858,\"event\":\"endpoint.url_validation\"}";
			var secretToken = "4fv1RkqGQUq5sWbEz6hA5A";
			var signature = "v0=93a1a675965ceb9c5a50c5dfb31f20e50f763be37a54ef74cd2d16a1a8e5c0d6";
			var timestamp = "1720705455";

			var parser = new WebhookParser();

			// Act
			var result = parser.VerifySignature(requestBody, secretToken, signature, timestamp);

			// Assert
			result.ShouldBeTrue();
		}

		[Fact]
		public void VerifySignature_WithValidSignatureAndNonASCIICharacters_ReturnsTrue()
		{
			// Arrange
			var requestBody = "{\"event\":\"meeting.started\",\"payload\":{\"account_id\":\"VjZoEArIT5y-HlWxkV-tVA\",\"object\":{\"duration\":60,\"start_time\":\"2024-07-11T14:12:55Z\",\"timezone\":\"America/New_York\",\"topic\":\"Test \\uD83D\\uDE92\\uD83D\\uDE92 ? - ’ - – \\uD83D\\uDE97 HOLA\",\"id\":\"85393847045\",\"type\":2,\"uuid\":\"jUh5o3dKQIytvcsfTtKBlg==\",\"host_id\":\"8lzIwvZTSOqjndWPbPqzuA\"}},\"event_ts\":1720707175904}";
			var secretToken = "4fv1RkqGQUq5sWbEz6hA5A";
			var signature = "v0=1a14e79349318fa1bead50ebbd3c185ae078e182d3bbd30ab8010fcb7f4357c7";
			var timestamp = "1720707175";

			var parser = new WebhookParser();

			// Act
			var result = parser.VerifySignature(requestBody, secretToken, signature, timestamp);

			// Assert
			result.ShouldBeTrue();
		}

		[Fact]
		public void VerifySignature_WithEmptyRequestBody_ReturnsTrue()
		{
			// Arrange
			var requestBody = "";
			var secretToken = "mySecretToken123";
			var timestamp = "1234567890";

			// Calculate expected signature for empty body
			var message = $"v0:{timestamp}:{requestBody}";
			var hmac = new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(secretToken));
			var hashAsBytes = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(message));
#if NET5_0_OR_GREATER
			var hashAsHex = Convert.ToHexStringLower(hashAsBytes);
#else
			var hashAsHex = BitConverter.ToString(hashAsBytes).Replace("-", "").ToLower();
#endif
			var signature = $"v0={hashAsHex}";

			var parser = new WebhookParser();

			// Act
			var result = parser.VerifySignature(requestBody, secretToken, signature, timestamp);

			// Assert
			result.ShouldBeTrue();
		}

		[Fact]
		public void VerifySignature_WithSpecialCharactersInSecret_ReturnsTrue()
		{
			// Arrange
			var requestBody = "{\"test\":\"data\"}";
			var secretToken = "mySecret!@#$%^&*()Token";
			var timestamp = "1234567890";

			// Calculate expected signature
			var message = $"v0:{timestamp}:{requestBody}";
			var hmac = new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(secretToken));
			var hashAsBytes = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(message));
#if NET5_0_OR_GREATER
			var hashAsHex = Convert.ToHexStringLower(hashAsBytes);
#else
			var hashAsHex = BitConverter.ToString(hashAsBytes).Replace("-", "").ToLower();
#endif
			var signature = $"v0={hashAsHex}";

			var parser = new WebhookParser();

			// Act
			var result = parser.VerifySignature(requestBody, secretToken, signature, timestamp);

			// Assert
			result.ShouldBeTrue();
		}

		[Fact]
		public void VerifySignature_WithLongRequestBody_ReturnsTrue()
		{
			// Arrange
			var requestBody = new string('a', 10000);
			var secretToken = "mySecretToken";
			var timestamp = "1234567890";

			// Calculate expected signature
			var message = $"v0:{timestamp}:{requestBody}";
			var hmac = new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(secretToken));
			var hashAsBytes = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(message));
#if NET5_0_OR_GREATER
			var hashAsHex = Convert.ToHexStringLower(hashAsBytes);
#else
			var hashAsHex = BitConverter.ToString(hashAsBytes).Replace("-", "").ToLower();
#endif
			var signature = $"v0={hashAsHex}";

			var parser = new WebhookParser();

			// Act
			var result = parser.VerifySignature(requestBody, secretToken, signature, timestamp);

			// Assert
			result.ShouldBeTrue();
		}

		#endregion

		#region Invalid Signature Tests

		[Fact]
		public void VerifySignature_WithInvalidSignature_ReturnsFalse()
		{
			// Arrange
			var requestBody = "{\"payload\":{\"plainToken\":\"xys8n8PGS7mAU0m5-YJjRA\"},\"event_ts\":1720705455858,\"event\":\"endpoint.url_validation\"}";
			var secretToken = "4fv1RkqGQUq5sWbEz6hA5A";
			var signature = "v0=invalid_signature_hash";
			var timestamp = "1720705455";

			var parser = new WebhookParser();

			// Act
			var result = parser.VerifySignature(requestBody, secretToken, signature, timestamp);

			// Assert
			result.ShouldBeFalse();
		}

		[Fact]
		public void VerifySignature_WithWrongSecretToken_ReturnsFalse()
		{
			// Arrange
			var requestBody = "{\"payload\":{\"plainToken\":\"xys8n8PGS7mAU0m5-YJjRA\"},\"event_ts\":1720705455858,\"event\":\"endpoint.url_validation\"}";
			var secretToken = "wrongSecretToken";
			var signature = "v0=93a1a675965ceb9c5a50c5dfb31f20e50f763be37a54ef74cd2d16a1a8e5c0d6";
			var timestamp = "1720705455";

			var parser = new WebhookParser();

			// Act
			var result = parser.VerifySignature(requestBody, secretToken, signature, timestamp);

			// Assert
			result.ShouldBeFalse();
		}

		[Fact]
		public void VerifySignature_WithModifiedRequestBody_ReturnsFalse()
		{
			// Arrange
			var requestBody = "{\"payload\":{\"plainToken\":\"MODIFIED\"},\"event_ts\":1720705455858,\"event\":\"endpoint.url_validation\"}";
			var secretToken = "4fv1RkqGQUq5sWbEz6hA5A";
			var signature = "v0=93a1a675965ceb9c5a50c5dfb31f20e50f763be37a54ef74cd2d16a1a8e5c0d6";
			var timestamp = "1720705455";

			var parser = new WebhookParser();

			// Act
			var result = parser.VerifySignature(requestBody, secretToken, signature, timestamp);

			// Assert
			result.ShouldBeFalse();
		}

		[Fact]
		public void VerifySignature_WithWrongTimestamp_ReturnsFalse()
		{
			// Arrange
			var requestBody = "{\"payload\":{\"plainToken\":\"xys8n8PGS7mAU0m5-YJjRA\"},\"event_ts\":1720705455858,\"event\":\"endpoint.url_validation\"}";
			var secretToken = "4fv1RkqGQUq5sWbEz6hA5A";
			var signature = "v0=93a1a675965ceb9c5a50c5dfb31f20e50f763be37a54ef74cd2d16a1a8e5c0d6";
			var timestamp = "9999999999";

			var parser = new WebhookParser();

			// Act
			var result = parser.VerifySignature(requestBody, secretToken, signature, timestamp);

			// Assert
			result.ShouldBeFalse();
		}

		[Fact]
		public void VerifySignature_WithMissingV0Prefix_ReturnsFalse()
		{
			// Arrange
			var requestBody = "{\"test\":\"data\"}";
			var secretToken = "mySecret";
			var signature = "93a1a675965ceb9c5a50c5dfb31f20e50f763be37a54ef74cd2d16a1a8e5c0d6";
			var timestamp = "1234567890";

			var parser = new WebhookParser();

			// Act
			var result = parser.VerifySignature(requestBody, secretToken, signature, timestamp);

			// Assert
			result.ShouldBeFalse();
		}

		[Fact]
		public void VerifySignature_WithUppercaseSignature_ReturnsFalse()
		{
			// Arrange
			var requestBody = "{\"payload\":{\"plainToken\":\"xys8n8PGS7mAU0m5-YJjRA\"},\"event_ts\":1720705455858,\"event\":\"endpoint.url_validation\"}";
			var secretToken = "4fv1RkqGQUq5sWbEz6hA5A";
			var signature = "v0=93A1A675965CEB9C5A50C5DFB31F20E50F763BE37A54EF74CD2D16A1A8E5C0D6";
			var timestamp = "1720705455";

			var parser = new WebhookParser();

			// Act
			var result = parser.VerifySignature(requestBody, secretToken, signature, timestamp);

			// Assert
			result.ShouldBeFalse();
		}

		[Fact]
		public void VerifySignature_WithEmptySignature_ReturnsFalse()
		{
			// Arrange
			var requestBody = "{\"test\":\"data\"}";
			var secretToken = "mySecret";
			var signature = "";
			var timestamp = "1234567890";

			var parser = new WebhookParser();

			// Act
			var result = parser.VerifySignature(requestBody, secretToken, signature, timestamp);

			// Assert
			result.ShouldBeFalse();
		}

		[Fact]
		public void VerifySignature_WithNullSecretToken_ReturnsFalse()
		{
			// Arrange
			var requestBody = "{\"test\":\"data\"}";
			string secretToken = null;
			var signature = "v0=93a1a675965ceb9c5a50c5dfb31f20e50f763be37a54ef74cd2d16a1a8e5c0d6";
			var timestamp = "1234567890";

			var parser = new WebhookParser();

			// Act & Assert
			Should.Throw<ArgumentNullException>(() => parser.VerifySignature(requestBody, secretToken, signature, timestamp));
		}

		#endregion

		#region Edge Cases

		[Fact]
		public void VerifySignature_WithWhitespaceInRequestBody_ValidatesCorrectly()
		{
			// Arrange
			var requestBody = "  {\"test\":\"data\"}  ";
			var secretToken = "mySecret";
			var timestamp = "1234567890";

			// Calculate signature with whitespace
			var message = $"v0:{timestamp}:{requestBody}";
			var hmac = new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(secretToken));
			var hashAsBytes = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(message));
#if NET5_0_OR_GREATER
			var hashAsHex = Convert.ToHexStringLower(hashAsBytes);
#else
			var hashAsHex = BitConverter.ToString(hashAsBytes).Replace("-", "").ToLower();
#endif
			var signature = $"v0={hashAsHex}";

			var parser = new WebhookParser();

			// Act
			var result = parser.VerifySignature(requestBody, secretToken, signature, timestamp);

			// Assert
			result.ShouldBeTrue();
		}

		[Fact]
		public void VerifySignature_WithNewlinesInRequestBody_ValidatesCorrectly()
		{
			// Arrange
			var requestBody = "{\n\"test\":\"data\"\n}";
			var secretToken = "mySecret";
			var timestamp = "1234567890";

			// Calculate signature
			var message = $"v0:{timestamp}:{requestBody}";
			var hmac = new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(secretToken));
			var hashAsBytes = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(message));
#if NET5_0_OR_GREATER
			var hashAsHex = Convert.ToHexStringLower(hashAsBytes);
#else
			var hashAsHex = BitConverter.ToString(hashAsBytes).Replace("-", "").ToLower();
#endif
			var signature = $"v0={hashAsHex}";

			var parser = new WebhookParser();

			// Act
			var result = parser.VerifySignature(requestBody, secretToken, signature, timestamp);

			// Assert
			result.ShouldBeTrue();
		}

		[Fact]
		public void VerifySignature_WithUnicodeCharacters_ValidatesCorrectly()
		{
			// Arrange
			var requestBody = "{\"message\":\"Hello 世界 🌍\"}";
			var secretToken = "mySecret";
			var timestamp = "1234567890";

			// Calculate signature
			var message = $"v0:{timestamp}:{requestBody}";
			var hmac = new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(secretToken));
			var hashAsBytes = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(message));
#if NET5_0_OR_GREATER
			var hashAsHex = Convert.ToHexStringLower(hashAsBytes);
#else
			var hashAsHex = BitConverter.ToString(hashAsBytes).Replace("-", "").ToLower();
#endif
			var signature = $"v0={hashAsHex}";

			var parser = new WebhookParser();

			// Act
			var result = parser.VerifySignature(requestBody, secretToken, signature, timestamp);

			// Assert
			result.ShouldBeTrue();
		}

		[Fact]
		public void VerifySignature_WithVeryLongTimestamp_ValidatesCorrectly()
		{
			// Arrange
			var requestBody = "{\"test\":\"data\"}";
			var secretToken = "mySecret";
			var timestamp = "99999999999999999999";

			// Calculate signature
			var message = $"v0:{timestamp}:{requestBody}";
			var hmac = new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(secretToken));
			var hashAsBytes = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(message));
#if NET5_0_OR_GREATER
			var hashAsHex = Convert.ToHexStringLower(hashAsBytes);
#else
			var hashAsHex = BitConverter.ToString(hashAsBytes).Replace("-", "").ToLower();
#endif
			var signature = $"v0={hashAsHex}";

			var parser = new WebhookParser();

			// Act
			var result = parser.VerifySignature(requestBody, secretToken, signature, timestamp);

			// Assert
			result.ShouldBeTrue();
		}

		[Fact]
		public void VerifySignature_WithZeroTimestamp_ValidatesCorrectly()
		{
			// Arrange
			var requestBody = "{\"test\":\"data\"}";
			var secretToken = "mySecret";
			var timestamp = "0";

			// Calculate signature
			var message = $"v0:{timestamp}:{requestBody}";
			var hmac = new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(secretToken));
			var hashAsBytes = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(message));
#if NET5_0_OR_GREATER
			var hashAsHex = Convert.ToHexStringLower(hashAsBytes);
#else
			var hashAsHex = BitConverter.ToString(hashAsBytes).Replace("-", "").ToLower();
#endif
			var signature = $"v0={hashAsHex}";

			var parser = new WebhookParser();

			// Act
			var result = parser.VerifySignature(requestBody, secretToken, signature, timestamp);

			// Assert
			result.ShouldBeTrue();
		}

		#endregion

		#region Integration with VerifyAndParseEventWebhook

		[Fact]
		public void VerifyAndParseEventWebhook_WithValidSignature_ParsesSuccessfully()
		{
			// Arrange
			var requestBody = "{\"payload\":{\"plainToken\":\"xys8n8PGS7mAU0m5-YJjRA\"},\"event_ts\":1720705455858,\"event\":\"endpoint.url_validation\"}";
			var secretToken = "4fv1RkqGQUq5sWbEz6hA5A";
			var signature = "v0=93a1a675965ceb9c5a50c5dfb31f20e50f763be37a54ef74cd2d16a1a8e5c0d6";
			var timestamp = "1720705455";

			var parser = new WebhookParser();

			// Act
			var parsedEvent = parser.VerifyAndParseEventWebhook(requestBody, secretToken, signature, timestamp);

			// Assert
			parsedEvent.ShouldNotBeNull();
			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.EndpointUrlValidation);
		}

		[Fact]
		public void VerifyAndParseEventWebhook_WithInvalidSignature_ThrowsSecurityException()
		{
			// Arrange
			var requestBody = "{\"payload\":{\"plainToken\":\"xys8n8PGS7mAU0m5-YJjRA\"},\"event_ts\":1720705455858,\"event\":\"endpoint.url_validation\"}";
			var secretToken = "wrongSecret";
			var signature = "v0=93a1a675965ceb9c5a50c5dfb31f20e50f763be37a54ef74cd2d16a1a8e5c0d6";
			var timestamp = "1720705455";

			var parser = new WebhookParser();

			// Act & Assert
			Should.Throw<SecurityException>(() => parser.VerifyAndParseEventWebhook(requestBody, secretToken, signature, timestamp))
				.Message.ShouldBe("Webhook signature validation failed.");
		}

		[Fact]
		public void VerifyAndParseEventWebhook_WithModifiedBody_ThrowsSecurityException()
		{
			// Arrange
			var requestBody = "{\"payload\":{\"plainToken\":\"TAMPERED\"},\"event_ts\":1720705455858,\"event\":\"endpoint.url_validation\"}";
			var secretToken = "4fv1RkqGQUq5sWbEz6hA5A";
			var signature = "v0=93a1a675965ceb9c5a50c5dfb31f20e50f763be37a54ef74cd2d16a1a8e5c0d6";
			var timestamp = "1720705455";

			var parser = new WebhookParser();

			// Act & Assert
			Should.Throw<SecurityException>(() => parser.VerifyAndParseEventWebhook(requestBody, secretToken, signature, timestamp))
				.Message.ShouldBe("Webhook signature validation failed.");
		}

		#endregion

		#region Real-World Scenarios

		[Fact]
		public void VerifySignature_MeetingStartedWebhook_ValidatesCorrectly()
		{
			// Arrange - Real meeting.started webhook example
			var requestBody = "{\"event\":\"meeting.started\",\"payload\":{\"account_id\":\"ABC123\",\"object\":{\"id\":\"123456789\",\"uuid\":\"abc+def==\",\"host_id\":\"host123\",\"topic\":\"Test Meeting\",\"type\":2,\"start_time\":\"2024-01-15T10:00:00Z\",\"duration\":60,\"timezone\":\"UTC\"}},\"event_ts\":1705315200000}";
			var secretToken = "testSecret123";
			var timestamp = "1705315200";

			// Calculate signature
			var message = $"v0:{timestamp}:{requestBody}";
			var hmac = new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(secretToken));
			var hashAsBytes = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(message));
#if NET5_0_OR_GREATER
			var hashAsHex = Convert.ToHexStringLower(hashAsBytes);
#else
			var hashAsHex = BitConverter.ToString(hashAsBytes).Replace("-", "").ToLower();
#endif
			var signature = $"v0={hashAsHex}";

			var parser = new WebhookParser();

			// Act
			var result = parser.VerifySignature(requestBody, secretToken, signature, timestamp);

			// Assert
			result.ShouldBeTrue();
		}

		[Fact]
		public void VerifySignature_WithReplayAttack_SameSignatureDifferentBody_ReturnsFalse()
		{
			// Arrange
			var originalBody = "{\"event\":\"meeting.started\",\"payload\":{\"id\":1}}";
			var attackBody = "{\"event\":\"meeting.started\",\"payload\":{\"id\":2}}";
			var secretToken = "mySecret";
			var timestamp = "1234567890";

			// Calculate signature for original body
			var message = $"v0:{timestamp}:{originalBody}";
			var hmac = new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(secretToken));
			var hashAsBytes = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(message));
#if NET5_0_OR_GREATER
			var hashAsHex = Convert.ToHexStringLower(hashAsBytes);
#else
			var hashAsHex = BitConverter.ToString(hashAsBytes).Replace("-", "").ToLower();
#endif
			var signature = $"v0={hashAsHex}";

			var parser = new WebhookParser();

			// Act - Try to use the signature with a different body
			var result = parser.VerifySignature(attackBody, secretToken, signature, timestamp);

			// Assert
			result.ShouldBeFalse();
		}

		[Fact]
		public void VerifySignature_CaseSensitivity_BodyCaseMatters()
		{
			// Arrange
			var requestBody1 = "{\"Event\":\"test\"}";
			var requestBody2 = "{\"event\":\"test\"}";
			var secretToken = "mySecret";
			var timestamp = "1234567890";

			// Calculate signature for first body
			var message = $"v0:{timestamp}:{requestBody1}";
			var hmac = new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(secretToken));
			var hashAsBytes = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(message));
#if NET5_0_OR_GREATER
			var hashAsHex = Convert.ToHexStringLower(hashAsBytes);
#else
			var hashAsHex = BitConverter.ToString(hashAsBytes).Replace("-", "").ToLower();
#endif
			var signature = $"v0={hashAsHex}";

			var parser = new WebhookParser();

			// Act - Try to use signature from first body with second body
			var result = parser.VerifySignature(requestBody2, secretToken, signature, timestamp);

			// Assert - Should fail because JSON keys have different case
			result.ShouldBeFalse();
		}

		#endregion
	}
}
