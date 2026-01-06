using Microsoft.Extensions.DependencyInjection;
using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Models;
using ZoomNet.Resources;
using ZoomNet.Utilities;

namespace ZoomNet.UnitTests.Extensions
{
	public class PublicTests
	{
		private readonly ITestOutputHelper _outputHelper;

		public PublicTests(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

		#region Dependency Injection Extensions

		[Fact]
		public void AddZoomNet_WithConnectionInfo_RegistersServices()
		{
			// Arrange
			var services = new ServiceCollection();
			services.AddLogging();
			var connectionInfo = OAuthConnectionInfo.ForServerToServer("clientId", "secret", "accountId");

			// Act
			services.AddZoomNet(connectionInfo);

			// Assert
			var serviceProvider = services.BuildServiceProvider();
			var zoomClient = serviceProvider.GetService<IZoomClient>();
			zoomClient.ShouldNotBeNull();

			var retrievedConnectionInfo = serviceProvider.GetService<IConnectionInfo>();
			retrievedConnectionInfo.ShouldNotBeNull();
			retrievedConnectionInfo.ShouldBe(connectionInfo);
		}

		[Fact]
		public void AddZoomNet_WithProxy_RegistersServices()
		{
			// Arrange
			var services = new ServiceCollection();
			services.AddLogging();
			var connectionInfo = OAuthConnectionInfo.ForServerToServer("clientId", "secret", "accountId");
			var proxy = new WebProxy("http://proxy.example.com:8080");

			// Act
			services.AddZoomNet(connectionInfo, proxy);

			// Assert
			var serviceProvider = services.BuildServiceProvider();
			var zoomClient = serviceProvider.GetService<IZoomClient>();
			zoomClient.ShouldNotBeNull();
		}

		[Fact]
		public void AddZoomNet_WithCustomHttpClientName_UsesCustomName()
		{
			// Arrange
			var services = new ServiceCollection();
			services.AddLogging();
			var connectionInfo = OAuthConnectionInfo.ForServerToServer("clientId", "secret", "accountId");
			var customHttpClientName = "MyCustomZoomClient";

			// Act
			services.AddZoomNet(connectionInfo, httpClientName: customHttpClientName);

			// Assert
			var serviceProvider = services.BuildServiceProvider();
			var zoomClient = serviceProvider.GetService<IZoomClient>();
			zoomClient.ShouldNotBeNull();
		}

		[Fact]
		public void AddKeyedZoomNet_WithServiceKey_RegistersKeyedServices()
		{
			// Arrange
			var services = new ServiceCollection();
			services.AddLogging();
			var connectionInfo = OAuthConnectionInfo.ForServerToServer("clientId", "secret", "accountId");
			var serviceKey = "MyZoomClient";

			// Act
			services.AddKeyedZoomNet(connectionInfo, serviceKey);

			// Assert
			var serviceProvider = services.BuildServiceProvider();
			var zoomClient = serviceProvider.GetKeyedService<IZoomClient>(serviceKey);
			zoomClient.ShouldNotBeNull();

			var retrievedConnectionInfo = serviceProvider.GetKeyedService<IConnectionInfo>(serviceKey);
			retrievedConnectionInfo.ShouldNotBeNull();
			retrievedConnectionInfo.ShouldBe(connectionInfo);
		}

		[Fact]
		public void AddKeyedZoomNet_WithProxyAndServiceKey_RegistersKeyedServices()
		{
			// Arrange
			var services = new ServiceCollection();
			services.AddLogging();
			var connectionInfo = OAuthConnectionInfo.ForServerToServer("clientId", "secret", "accountId");
			var proxy = new WebProxy("http://proxy.example.com:8080");
			var serviceKey = "MyZoomClient";

			// Act
			services.AddKeyedZoomNet(connectionInfo, serviceKey, proxy);

			// Assert
			var serviceProvider = services.BuildServiceProvider();
			var zoomClient = serviceProvider.GetKeyedService<IZoomClient>(serviceKey);
			zoomClient.ShouldNotBeNull();
		}

		[Fact]
		public void AddKeyedZoomNet_MultipleKeys_RegistersMultipleClients()
		{
			// Arrange
			var services = new ServiceCollection();
			services.AddLogging();
			var connectionInfo1 = OAuthConnectionInfo.ForServerToServer("clientId1", "secret1", "accountId1");
			var connectionInfo2 = OAuthConnectionInfo.ForServerToServer("clientId2", "secret2", "accountId2");
			var serviceKey1 = "ZoomClient1";
			var serviceKey2 = "ZoomClient2";

			// Act
			services.AddKeyedZoomNet(connectionInfo1, serviceKey1);
			services.AddKeyedZoomNet(connectionInfo2, serviceKey2);

			// Assert
			var serviceProvider = services.BuildServiceProvider();

			var zoomClient1 = serviceProvider.GetKeyedService<IZoomClient>(serviceKey1);
			zoomClient1.ShouldNotBeNull();

			var zoomClient2 = serviceProvider.GetKeyedService<IZoomClient>(serviceKey2);
			zoomClient2.ShouldNotBeNull();
		}

		#endregion

		#region Webhook Parser Extensions

		[Fact]
		public async Task ParseEventWebhookAsync_ParsesStreamContent()
		{
			// Arrange
			var requestBody = @"{""event"":""meeting.started"",""event_ts"":1720705455858,""payload"":{""this_is_a_test"":""hello world""}}";
			var parser = new WebhookParser();
			using var stream = new MemoryStream(Encoding.UTF8.GetBytes(requestBody));

			// Act
			var result = await parser.ParseEventWebhookAsync(stream);

			// Assert
			result.ShouldNotBeNull();
		}

		[Fact]
		public void VerifyAndParseEventWebhook_WithValidSignature_ParsesEvent()
		{
			// Arrange
			var requestBody = @"{""event"":""meeting.started"",""event_ts"":1720705455858,""payload"":{""this_is_a_test"":""hello world""}}";
			var secretToken = "mySecretToken";
			var timestamp = "1609459200000";
			var message = $"v0:{timestamp}:{requestBody}";
			var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretToken));
			var hashAsBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
			var hashAsHex = hashAsBytes.ToHexString();
			var signature = $"v0={hashAsHex}";

			var parser = new WebhookParser();

			// Act
			var result = parser.VerifyAndParseEventWebhook(requestBody, secretToken, signature, timestamp);

			// Assert
			result.ShouldNotBeNull();
		}

		[Fact]
		public void VerifyAndParseEventWebhook_WithInvalidSignature_ThrowsSecurityException()
		{
			// Arrange
			var requestBody = @"{""event"":""meeting.started"",""event_ts"":1720705455858,""payload"":{""this_is_a_test"":""hello world""}}";
			var secretToken = "mySecretToken";
			var signature = "invalid_signature";
			var timestamp = "1609459200000";

			var parser = new WebhookParser();

			// Act & Assert
			Should.Throw<SecurityException>(() => parser.VerifyAndParseEventWebhook(requestBody, secretToken, signature, timestamp));
		}

		[Fact]
		public async Task VerifyAndParseEventWebhookAsync_WithStream_ParsesEvent()
		{
			// Arrange
			var requestBody = @"{""event"":""meeting.started"",""event_ts"":1720705455858,""payload"":{""this_is_a_test"":""hello world""}}";
			var secretToken = "mySecretToken";
			var timestamp = "1609459200000";
			var message = $"v0:{timestamp}:{requestBody}";
			var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretToken));
			var hashAsBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
			var hashAsHex = hashAsBytes.ToHexString();
			var signature = $"v0={hashAsHex}";

			var parser = new WebhookParser();
			using var stream = new MemoryStream(Encoding.UTF8.GetBytes(requestBody));

			// Act
			var result = await parser.VerifyAndParseEventWebhookAsync(stream, secretToken, signature, timestamp);

			// Assert
			result.ShouldNotBeNull();
		}

		#endregion

		#region CloudRecordings Extensions

		[Fact]
		public async Task DownloadFileAsync_UsesRecordingFileDownloadUrl()
		{
			// Arrange
			var downloadUrl = "http://dummywebsite.com/dummyfile.txt";
			var recordingFile = new RecordingFile
			{
				DownloadUrl = downloadUrl,
				Id = "file123",
				FileType = RecordingFileType.Video
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp
				.Expect(HttpMethod.Get, downloadUrl)
				.Respond(HttpStatusCode.OK, new StringContent("This is the content of the file"));

			var connectionInfo = OAuthConnectionInfo.ForServerToServer(
				"MyClientId",
				"MyClientSecret",
				"MyAccountId",
				accessToken: "MyToken");

			var client = new ZoomClient(connectionInfo, mockHttp.ToHttpClient(), null, null);

			// Act
			var result = await client.CloudRecordings.DownloadFileAsync(recordingFile, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion

		#region HasPermission Extension

		[Fact]
		public void HasPermission_WithMatchingScope_ReturnsTrue()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			var tokenResponse = JsonSerializer.Serialize(new
			{
				access_token = "test_access_token",
				token_type = "bearer",
				expires_in = 3600,
				scope = "meeting:write user:read recording:read"
			});

			mockHttp.When(HttpMethod.Post, "https://api.zoom.us/oauth/token")
				.Respond("application/json", tokenResponse);

			var connectionInfo = OAuthConnectionInfo.ForServerToServer("clientId", "clientSecret", "accountId");
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = new ZoomClient(connectionInfo, mockHttp.ToHttpClient(), null, logger);

			// Act
			var result = client.HasPermission("meeting:write");

			// Assert
			result.ShouldBeTrue();
		}

		[Fact]
		public void HasPermission_WithMissingScope_ReturnsFalse()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			var tokenResponse = JsonSerializer.Serialize(new
			{
				access_token = "test_access_token",
				token_type = "bearer",
				expires_in = 3600,
				scope = "meeting:write user:read"
			});

			mockHttp.When(HttpMethod.Post, "https://api.zoom.us/oauth/token")
				.Respond("application/json", tokenResponse);

			var connectionInfo = OAuthConnectionInfo.ForServerToServer("clientId", "clientSecret", "accountId");
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = new ZoomClient(connectionInfo, mockHttp.ToHttpClient(), null, logger);

			// Act
			var result = client.HasPermission("dashboard:read");

			// Assert
			result.ShouldBeFalse();
		}

		[Fact]
		public void HasPermission_WithEmptyScope_ReturnsFalse()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			var tokenResponse = JsonSerializer.Serialize(new
			{
				access_token = "test_access_token",
				token_type = "bearer",
				expires_in = 3600,
				scope = "meeting:write user:read"
			});

			mockHttp.When(HttpMethod.Post, "https://api.zoom.us/oauth/token")
				.Respond("application/json", tokenResponse);

			var connectionInfo = OAuthConnectionInfo.ForServerToServer("clientId", "clientSecret", "accountId");
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = new ZoomClient(connectionInfo, mockHttp.ToHttpClient(), null, logger);

			// Act
			var result = client.HasPermission("");

			// Assert
			result.ShouldBeFalse();
		}

		[Fact]
		public void HasPermission_WithCaseSensitiveScope_IsCaseInsensitive()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			var tokenResponse = JsonSerializer.Serialize(new
			{
				access_token = "test_access_token",
				token_type = "bearer",
				expires_in = 3600,
				scope = "meeting:write user:read"
			});

			mockHttp.When(HttpMethod.Post, "https://api.zoom.us/oauth/token")
				.Respond("application/json", tokenResponse);

			var connectionInfo = OAuthConnectionInfo.ForServerToServer("clientId", "clientSecret", "accountId");
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = new ZoomClient(connectionInfo, mockHttp.ToHttpClient(), null, logger);

			// Act - Try with different casing
			var lowerCaseResult = client.HasPermission("meeting:write");
			var upperCaseResult = client.HasPermission("MEETING:WRITE");

			// Assert
			lowerCaseResult.ShouldBe(upperCaseResult);
		}

		[Fact]
		public void HasPermission_WithSpecialCharacters_HandlesCorrectly()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			var tokenResponse = JsonSerializer.Serialize(new
			{
				access_token = "test_access_token",
				token_type = "bearer",
				expires_in = 3600,
				scope = "meeting:write:admin user:read:admin"
			});

			mockHttp.When(HttpMethod.Post, "https://api.zoom.us/oauth/token")
				.Respond("application/json", tokenResponse);

			var connectionInfo = OAuthConnectionInfo.ForServerToServer("clientId", "clientSecret", "accountId");
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = new ZoomClient(connectionInfo, mockHttp.ToHttpClient(), null, logger);

			// Act
			var exactMatchResult = client.HasPermission("meeting:write:admin");
			var partialMatchResult = client.HasPermission("meeting:write");

			// Assert
			exactMatchResult.ShouldBeTrue();
			partialMatchResult.ShouldBeFalse(); // Partial match should not work
		}

		#endregion

		#region Simple Assertions for Convenience Methods

		[Fact]
		public void ExtensionMethods_AreAccessible()
		{
			// This test ensures that all the extension methods are properly defined
			// and accessible. It doesn't test functionality but ensures the API surface is correct.

			// Users Extensions
			typeof(Public).GetMethod("GetCurrentAsync").ShouldNotBeNull();
			typeof(Public).GetMethod("GetCurrentPermissionsAsync").ShouldNotBeNull();
			typeof(Public).GetMethod("GetCurrentSettingsAsync").ShouldNotBeNull();
			typeof(Public).GetMethod("AddAssistantByIdAsync").ShouldNotBeNull();
			typeof(Public).GetMethod("AddAssistantByEmailAsync").ShouldNotBeNull();

			// Chat Extensions
			typeof(Public).GetMethod("LeaveChannelAsync").ShouldNotBeNull();
			typeof(Public).GetMethod("SendMessageToContactAsync", new[] { typeof(IChat), typeof(string), typeof(string), typeof(string), typeof(IEnumerable<string>), typeof(IEnumerable<ChatMention>), typeof(CancellationToken) }).ShouldNotBeNull();
			typeof(Public).GetMethod("SendMessageToChannelAsync", new[] { typeof(IChat), typeof(string), typeof(string), typeof(string), typeof(IEnumerable<string>), typeof(IEnumerable<ChatMention>), typeof(CancellationToken) }).ShouldNotBeNull();
			typeof(Public).GetMethod("GetMessagesToContactAsync").ShouldNotBeNull();
			typeof(Public).GetMethod("GetMessagesToChannelAsync").ShouldNotBeNull();
			typeof(Public).GetMethod("UpdateMessageToContactAsync").ShouldNotBeNull();
			typeof(Public).GetMethod("UpdateMessageToChannelAsync").ShouldNotBeNull();
			typeof(Public).GetMethod("DeleteMessageToContactAsync").ShouldNotBeNull();
			typeof(Public).GetMethod("DeleteMessageToChannelAsync").ShouldNotBeNull();

			// Webhook Parser Extensions
			typeof(Public).GetMethod("ParseEventWebhookAsync").ShouldNotBeNull();
			typeof(Public).GetMethod("VerifyAndParseEventWebhookAsync").ShouldNotBeNull();
			typeof(Public).GetMethod("VerifyAndParseEventWebhook", new[] { typeof(IWebhookParser), typeof(string), typeof(string), typeof(string), typeof(string) }).ShouldNotBeNull();

			// CloudRecordings Extensions
			typeof(Public).GetMethod("DownloadFileAsync", new[] { typeof(ICloudRecordings), typeof(RecordingFile), typeof(string), typeof(CancellationToken) }).ShouldNotBeNull();

			// Meetings Extensions
			typeof(Public).GetMethod("InviteParticipantAsync").ShouldNotBeNull();
			typeof(Public).GetMethod("InviteParticipantByEmailAsync").ShouldNotBeNull();
			typeof(Public).GetMethod("InviteParticipantByIdAsync").ShouldNotBeNull();
			typeof(Public).GetMethod("GetAsync", new[] { typeof(IMeetings), typeof(long), typeof(string), typeof(CancellationToken) }).ShouldNotBeNull();

			// Webinars Extensions
			typeof(Public).GetMethod("GetAsync", new[] { typeof(IWebinars), typeof(long), typeof(string), typeof(CancellationToken) }).ShouldNotBeNull();

			// Groups Extensions
			typeof(Public).GetMethod("AddMemberByEmailAsync").ShouldNotBeNull();
			typeof(Public).GetMethod("AddMemberByIdAsync").ShouldNotBeNull();
			typeof(Public).GetMethod("AddAdministratorByEmailAsync").ShouldNotBeNull();
			typeof(Public).GetMethod("AddAdministratorByIdAsync").ShouldNotBeNull();
			typeof(Public).GetMethod("DeleteVirtualBackgroundAsync").ShouldNotBeNull();

			// ZoomClient Extensions
			typeof(Public).GetMethod("HasPermission").ShouldNotBeNull();

			// Chatbot Extensions
			typeof(Public).GetMethod("SendMessageAsync", new[] { typeof(IChatbot), typeof(string), typeof(string), typeof(string), typeof(string), typeof(bool), typeof(CancellationToken) }).ShouldNotBeNull();
			typeof(Public).GetMethod("EditMessageAsync").ShouldNotBeNull();

			// Rooms Extensions
			typeof(Public).GetMethod("AssignTagToRoom").ShouldNotBeNull();
			typeof(Public).GetMethod("AssignTagToRoomsInLocation").ShouldNotBeNull();
			typeof(Public).GetMethod("DisplayEmergencyContentToAccountAsync").ShouldNotBeNull();
			typeof(Public).GetMethod("DisplayEmergencyContentToLocationAsync").ShouldNotBeNull();
			typeof(Public).GetMethod("DisplayEmergencyContentToRoomAsync").ShouldNotBeNull();
			typeof(Public).GetMethod("RemoveEmergencyContentFromAccountAsync").ShouldNotBeNull();
			typeof(Public).GetMethod("RemoveEmergencyContentFromLocationAsync").ShouldNotBeNull();
			typeof(Public).GetMethod("RemoveEmergencyContentFromRoomAsync").ShouldNotBeNull();

			// Events Extensions
			typeof(Public).GetMethod("CreateTicketAsync").ShouldNotBeNull();

			// Dependency Injection Extensions
			typeof(Public).GetMethod("AddZoomNet", new[] { typeof(IServiceCollection), typeof(IConnectionInfo), typeof(ZoomClientOptions), typeof(string) }).ShouldNotBeNull();
			typeof(Public).GetMethod("AddZoomNet", new[] { typeof(IServiceCollection), typeof(IConnectionInfo), typeof(IWebProxy), typeof(ZoomClientOptions), typeof(string) }).ShouldNotBeNull();
			typeof(Public).GetMethod("AddKeyedZoomNet", new[] { typeof(IServiceCollection), typeof(IConnectionInfo), typeof(string), typeof(ZoomClientOptions), typeof(string) }).ShouldNotBeNull();
			typeof(Public).GetMethod("AddKeyedZoomNet", new[] { typeof(IServiceCollection), typeof(IConnectionInfo), typeof(string), typeof(IWebProxy), typeof(ZoomClientOptions), typeof(string) }).ShouldNotBeNull();
		}

		#endregion
	}
}
