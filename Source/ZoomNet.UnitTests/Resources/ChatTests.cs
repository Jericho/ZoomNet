using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Models;
using ZoomNet.Resources;
using ZoomNet.UnitTests.Properties;

namespace ZoomNet.UnitTests.Resources
{
	public class ChatTests
	{
		private const string FILE_ID_JSON = @"{
			""id"": ""file123""
		}";

		private readonly ITestOutputHelper _outputHelper;

		public ChatTests(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

		#region Account Channel Tests

		[Fact]
		public async Task GetAccountChannelsForUserAsync()
		{
			// Arrange
			var userId = "user123";
			var recordsPerPage = 30;
			var pagingToken = "token123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("chat", "users", userId, "channels"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("next_page_token", pagingToken)
				.Respond("application/json", EndpointsResource.chat_users__userId__channels_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chat = new Chat(client);

			// Act
			var result = await chat.GetAccountChannelsForUserAsync(userId, recordsPerPage, pagingToken, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecordsPerPage.ShouldBe(10);
			result.NextPageToken.ShouldBe("qUEQqB1V0HVhJmwKFQrGOD");
			result.Records.ShouldNotBeNull();
			result.Records.Length.ShouldBe(1);
			result.Records[0].Id.ShouldBe("cabc1234567defghijkl01234");
			result.Records[0].Name.ShouldBe("Developers");
		}

		[Fact]
		public async Task GetAccountChannelAsync()
		{
			// Arrange
			var userId = "user123";
			var channelId = "channel1";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("chat", "users", userId, "channels", channelId))
				.Respond("application/json", EndpointsResource.chat_users__userId__channels__channelId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chat = new Chat(client);

			// Act
			var result = await chat.GetAccountChannelAsync(userId, channelId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("cabc1234567defghijkl01234");
			result.Name.ShouldBe("Developers");
		}

		[Fact]
		public async Task CreateAccountChannelAsync()
		{
			// Arrange
			var userId = "user123";
			var name = "New Channel";
			var type = ChatChannelType.Private;
			var emails = new[] { "user1@example.com", "user2@example.com" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("chat", "users", userId, "channels"))
				.Respond("application/json", EndpointsResource.chat_users__userId__channels_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chat = new Chat(client);

			// Act
			var result = await chat.CreateAccountChannelAsync(userId, name, type, emails, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("825c9e31f1064c73b394c5e4557d3447");
		}

		[Fact]
		public async Task CreateAccountChannelAsync_TooManyEmails_ThrowsException()
		{
			// Arrange
			var userId = "user123";
			var name = "New Channel";
			var type = ChatChannelType.Private;
			var emails = new[] { "user1@example.com", "user2@example.com", "user3@example.com", "user4@example.com", "user5@example.com", "user6@example.com" };

			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chat = new Chat(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentOutOfRangeException>(() => chat.CreateAccountChannelAsync(userId, name, type, emails, TestContext.Current.CancellationToken));
		}

		[Fact]
		public async Task UpdateAccountChannelAsync()
		{
			// Arrange
			var userId = "user123";
			var channelId = "channel1";
			var name = "Updated Channel";
			var settings = new ChatChannelSettings
			{
				AddMemberPermissions = ChatChannelAddMemberPermissions.Everyone,
				NewMembersCanSeePreviousMessageFiles = true,
				PostingPermissions = ChatChannelPostingPermissions.Everyone,
				MentionAllPermissions = ChatChannelMentionAllPermissions.Everyone
			};

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("chat", "users", userId, "channels", channelId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chat = new Chat(client);

			// Act
			await chat.UpdateAccountChannelAsync(userId, channelId, name, settings, ChatChannelType.Private, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeleteAccountChannelAsync()
		{
			// Arrange
			var userId = "user123";
			var channelId = "channel1";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("chat", "users", userId, "channels", channelId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chat = new Chat(client);

			// Act
			await chat.DeleteAccountChannelAsync(userId, channelId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Account Channel Members Tests

		[Fact]
		public async Task GetAccountChannelMembersAsync()
		{
			// Arrange
			var userId = "user123";
			var channelId = "channel1";
			var recordsPerPage = 30;
			var pagingToken = "membertoken";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("chat", "users", userId, "channels", channelId, "members"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("next_page_token", pagingToken)
				.Respond("application/json", EndpointsResource.chat_users__userId__channels__channelId__members_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chat = new Chat(client);

			// Act
			var result = await chat.GetAccountChannelMembersAsync(userId, channelId, recordsPerPage, pagingToken, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(1);
			result.Records[0].Id.ShouldBe("v4iyWT1LTfy8QvPG4GTvdg");
			result.Records[0].Email.ShouldBe("jchill@example.com");
		}

		[Fact]
		public async Task InviteMembersToAccountChannelAsync()
		{
			// Arrange
			var userId = "user123";
			var channelId = "channel1";
			var emails = new[] { "newmember1@example.com", "newmember2@example.com" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("chat", "users", userId, "channels", channelId, "members"))
				.Respond("application/json", EndpointsResource.chat_users__userId__channels__channelId__members_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chat = new Chat(client);

			// Act
			var result = await chat.InviteMembersToAccountChannelAsync(userId, channelId, emails, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Ids.Length.ShouldBe(2);
		}

		[Fact]
		public async Task InviteMembersToAccountChannelAsync_NoEmails_ThrowsException()
		{
			// Arrange
			var userId = "user123";
			var channelId = "channel1";
			var emails = Array.Empty<string>();

			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chat = new Chat(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentException>(() => chat.InviteMembersToAccountChannelAsync(userId, channelId, emails, TestContext.Current.CancellationToken));
		}

		[Fact]
		public async Task InviteMembersToAccountChannelAsync_TooManyEmails_ThrowsException()
		{
			// Arrange
			var userId = "user123";
			var channelId = "channel1";
			var emails = Enumerable.Range(1, 21).Select(i => $"user{i}@example.com").ToArray();

			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chat = new Chat(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentOutOfRangeException>(() => chat.InviteMembersToAccountChannelAsync(userId, channelId, emails, TestContext.Current.CancellationToken));
		}

		[Fact]
		public async Task RemoveMemberFromAccountChannelAsync()
		{
			// Arrange
			var userId = "user123";
			var channelId = "channel1";
			var memberId = "member1";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("chat", "users", userId, "channels", channelId, "members", memberId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chat = new Chat(client);

			// Act
			await chat.RemoveMemberFromAccountChannelAsync(userId, channelId, memberId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task PromoteMembersInAccountChannelByEmailAsync()
		{
			// Arrange
			var userId = "user123";
			var channelId = "channel1";
			var emails = new[] { "member1@example.com", "member2@example.com" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("chat", "users", userId, "channels", channelId, "admins"))
				.Respond("application/json", EndpointsResource.chat_users__userId__channels__channelId__admins_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chat = new Chat(client);

			// Act
			var result = await chat.PromoteMembersInAccountChannelByEmailAsync(userId, channelId, emails, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task DemoteAdminsInAccountChannelByIdAsync()
		{
			// Arrange
			var userId = "user123";
			var channelId = "channel1";
			var adminIds = new[] { "admin1", "admin2" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("chat", "users", userId, "channels", channelId, "admins"))
				.WithQueryString("admin_ids", "admin1,admin2")
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chat = new Chat(client);

			// Act
			await chat.DemoteAdminsInAccountChannelByIdAsync(userId, channelId, adminIds, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DemoteAdminsInAccountChannelByIdAsync_NoAdminIds_ThrowsException()
		{
			// Arrange
			var userId = "user123";
			var channelId = "channel1";
			var adminIds = Array.Empty<string>();

			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chat = new Chat(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentException>(() => chat.DemoteAdminsInAccountChannelByIdAsync(userId, channelId, adminIds, TestContext.Current.CancellationToken));
		}

		[Fact]
		public async Task DemoteAdminsInAccountChannelByIdAsync_TooManyAdminIds_ThrowsException()
		{
			// Arrange
			var userId = "user123";
			var channelId = "channel1";
			var adminIds = Enumerable.Range(1, 11).Select(i => $"admin{i}").ToArray();

			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chat = new Chat(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentOutOfRangeException>(() => chat.DemoteAdminsInAccountChannelByIdAsync(userId, channelId, adminIds, TestContext.Current.CancellationToken));
		}

		[Fact]
		public async Task DemoteAdminsInAccountChannelByUserIdAsync()
		{
			// Arrange
			var userId = "user123";
			var channelId = "channel1";
			var userIds = new[] { "user1", "user2" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("chat", "users", userId, "channels", channelId, "admins"))
				.WithQueryString("user_ids", "user1,user2")
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chat = new Chat(client);

			// Act
			await chat.DemoteAdminsInAccountChannelByUserIdAsync(userId, channelId, userIds, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Channel (non-Account) Tests

		[Fact]
		public async Task GetChannelAsync()
		{
			// Arrange
			var channelId = "channel1";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("chat", "channels", channelId))
				.Respond("application/json", EndpointsResource.chat_channels__channelId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chat = new Chat(client);

			// Act
			var result = await chat.GetChannelAsync(channelId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("cabc1234567defghijkl01234");
		}

		[Fact]
		public async Task UpdateChannelAsync()
		{
			// Arrange
			var channelId = "channel1";
			var name = "Updated Channel Name";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("chat", "channels", channelId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chat = new Chat(client);

			// Act
			await chat.UpdateChannelAsync(channelId, name, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeleteChannelAsync()
		{
			// Arrange
			var channelId = "channel1";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("chat", "channels", channelId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chat = new Chat(client);

			// Act
			await chat.DeleteChannelAsync(channelId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task RemoveMemberFromChannelAsync()
		{
			// Arrange
			var channelId = "channel1";
			var memberId = "member1";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("chat", "channels", channelId, "members", memberId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chat = new Chat(client);

			// Act
			await chat.RemoveMemberFromChannelAsync(channelId, memberId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task JoinChannelAsync()
		{
			// Arrange
			var channelId = "channel1";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("chat", "channels", channelId, "members", "me"))
				.Respond("application/json", EndpointsResource.chat_channels__channelId__members_me_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chat = new Chat(client);

			// Act
			var result = await chat.JoinChannelAsync(channelId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldBe("v4iyWT1LTfy8QvPG4GTvdg");
		}

		#endregion

		#region Message Tests

		[Fact]
		public async Task SendMessageToContactAsync()
		{
			// Arrange
			var userId = "user123";
			var recipientEmail = "recipient@example.com";
			var message = "Hello!";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("chat", "users", userId, "messages"))
				.Respond("application/json", EndpointsResource.chat_users__userId__messages_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chat = new Chat(client);

			// Act
			var result = await chat.SendMessageToContactAsync(userId, recipientEmail, message, null, null, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldBe("8cfaf567-bf5a-4acc-b4f2-88b3d371aca5");
		}

		[Fact]
		public async Task SendMessageToChannelAsync()
		{
			// Arrange
			var userId = "user123";
			var channelId = "channel1";
			var message = "Hello channel!";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("chat", "users", userId, "messages"))
				.Respond("application/json", EndpointsResource.chat_users__userId__messages_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chat = new Chat(client);

			// Act
			var result = await chat.SendMessageToChannelAsync(userId, channelId, message, null, null, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldBe("8cfaf567-bf5a-4acc-b4f2-88b3d371aca5");
		}

		[Fact]
		public async Task GetMessagesToContactAsync()
		{
			// Arrange
			var userId = "user123";
			var recipientEmail = "recipient@example.com";
			var recordsPerPage = 30;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("chat", "users", userId, "messages"))
				.WithQueryString("to_contact", recipientEmail)
				.WithQueryString("page_size", recordsPerPage.ToString())
				.Respond("application/json", EndpointsResource.chat_users__userId__messages_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chat = new Chat(client);

			// Act
			var result = await chat.GetMessagesToContactAsync(userId, recipientEmail, recordsPerPage, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(1);
			result.Records[0].Id.ShouldBe("EAB58B01-B35F-4F97-BA69-F9650F54679A");
		}

		[Fact]
		public async Task GetMessagesToChannelAsync()
		{
			// Arrange
			var userId = "user123";
			var channelId = "channel1";
			var recordsPerPage = 30;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("chat", "users", userId, "messages"))
				.WithQueryString("to_channel", channelId)
				.WithQueryString("page_size", recordsPerPage.ToString())
				.Respond("application/json", EndpointsResource.chat_users__userId__messages_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chat = new Chat(client);

			// Act
			var result = await chat.GetMessagesToChannelAsync(userId, channelId, recordsPerPage, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(1);
		}

		[Fact]
		public async Task UpdateMessageToContactAsync()
		{
			// Arrange
			var messageId = "msg123";
			var userId = "user123";
			var recipientEmail = "recipient@example.com";
			var message = "Updated message";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("chat", "users", userId, "messages", messageId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chat = new Chat(client);

			// Act
			await chat.UpdateMessageToContactAsync(messageId, userId, recipientEmail, message, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateMessageToChannelAsync()
		{
			// Arrange
			var messageId = "msg123";
			var userId = "user123";
			var channelId = "channel1";
			var message = "Updated channel message";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Put, Utils.GetZoomApiUri("chat", "users", userId, "messages", messageId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chat = new Chat(client);

			// Act
			await chat.UpdateMessageToChannelAsync(messageId, userId, channelId, message, null, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeleteMessageToContactAsync()
		{
			// Arrange
			var messageId = "msg123";
			var userId = "user123";
			var recipientEmail = "recipient@example.com";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("chat", "users", userId, "messages", messageId))
				.WithQueryString("to_contact", recipientEmail)
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chat = new Chat(client);

			// Act
			await chat.DeleteMessageToContactAsync(messageId, userId, recipientEmail, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeleteMessageToChannelAsync()
		{
			// Arrange
			var messageId = "msg123";
			var userId = "user123";
			var channelId = "channel1";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("chat", "users", userId, "messages", messageId))
				.WithQueryString("to_channel", channelId)
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chat = new Chat(client);

			// Act
			await chat.DeleteMessageToChannelAsync(messageId, userId, channelId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region File Tests

		[Fact]
		public async Task SendFileAsync()
		{
			// Arrange
			var messageId = "msg123";
			var userId = "user123";
			var recipientId = "recipient123";
			var channelId = "channel1";
			var fileName = "test.txt";
			var fileContent = "Test file content";
			using var fileData = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, $"https://file.zoom.us/v2/chat/users/{userId}/messages/files")
				.Respond("application/json", FILE_ID_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chat = new Chat(client);

			// Act
			var result = await chat.SendFileAsync(messageId, userId, recipientId, channelId, fileName, fileData, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldBe("file123");
		}

		[Fact]
		public async Task UploadFileAsync()
		{
			// Arrange
			var userId = "user123";
			var fileName = "document.pdf";
			var fileContent = "PDF file content";
			using var fileData = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, $"https://file.zoom.us/v2/chat/users/{userId}/files")
				.Respond("application/json", FILE_ID_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var chat = new Chat(client);

			// Act
			var result = await chat.UploadFileAsync(userId, fileName, fileData, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldBe("file123");
		}

		#endregion
	}
}
