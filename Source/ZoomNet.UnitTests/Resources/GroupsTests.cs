using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Models;
using ZoomNet.Resources;

namespace ZoomNet.UnitTests.Resources
{
	public class GroupsTests
	{
		private const string GROUPS_JSON = @"{
			""groups"": [
				{
					""id"": ""group1"",
					""name"": ""Engineering Team"",
					""total_members"": 25
				},
				{
					""id"": ""group2"",
					""name"": ""Sales Team"",
					""total_members"": 15
				}
			]
		}";

		private const string SINGLE_GROUP_JSON = @"{
			""id"": ""group1"",
			""name"": ""Engineering Team"",
			""total_members"": 25
		}";

		private const string CREATED_GROUP_JSON = @"{
			""id"": ""newGroup123"",
			""name"": ""New Group"",
			""total_members"": 0
		}";

		private const string ADD_MEMBERS_RESPONSE_JSON = @"{
			""ids"": ""user1,user2,user3""
		}";

		private const string ADD_SINGLE_MEMBER_RESPONSE_JSON = @"{
			""ids"": ""user1""
		}";

		private const string ADD_ADMINS_RESPONSE_JSON = @"{
			""ids"": ""admin1,admin2""
		}";

		private const string ADD_SINGLE_ADMIN_RESPONSE_JSON = @"{
			""ids"": ""admin1""
		}";

		private const string GROUP_MEMBERS_JSON = @"{
			""page_size"": 30,
			""next_page_token"": ""memberToken123"",
			""total_records"": 2,
			""members"": [
				{
					""id"": ""member1"",
					""email"": ""member1@example.com"",
					""first_name"": ""John"",
					""last_name"": ""Doe"",
					""type"": 2
				},
				{
					""id"": ""member2"",
					""email"": ""member2@example.com"",
					""first_name"": ""Jane"",
					""last_name"": ""Smith"",
					""type"": 1
				}
			]
		}";

		private const string GROUP_ADMINS_JSON = @"{
			""page_size"": 30,
			""next_page_token"": ""adminToken123"",
			""total_records"": 2,
			""admins"": [
				{
					""email"": ""admin1@example.com"",
					""name"": ""Admin One""
				},
				{
					""email"": ""admin2@example.com"",
					""name"": ""Admin Two""
				}
			]
		}";

		private const string VIRTUAL_BACKGROUND_FILE_JSON = @"{
			""id"": ""bgFile123"",
			""is_default"": false,
			""name"": ""background.jpg"",
			""size"": 1024000,
			""type"": ""image""
		}";

		private readonly ITestOutputHelper _outputHelper;

		public GroupsTests(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

		#region GetAllAsync Tests

		[Fact]
		public async Task GetAllAsync()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("groups"))
				.Respond("application/json", GROUPS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			var result = await groups.GetAllAsync(TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(2);
			result[0].Id.ShouldBe("group1");
			result[0].Name.ShouldBe("Engineering Team");
			result[0].NumberOfMembers.ShouldBe(25);
			result[1].Id.ShouldBe("group2");
			result[1].Name.ShouldBe("Sales Team");
			result[1].NumberOfMembers.ShouldBe(15);
		}

		[Fact]
		public async Task GetAllAsync_EmptyGroups()
		{
			// Arrange
			var emptyGroupsJson = @"{""groups"": []}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("groups"))
				.Respond("application/json", emptyGroupsJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			var result = await groups.GetAllAsync(TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(0);
		}

		#endregion

		#region GetAsync Tests

		[Fact]
		public async Task GetAsync()
		{
			// Arrange
			var groupId = "group1";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("groups", groupId))
				.Respond("application/json", SINGLE_GROUP_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			var result = await groups.GetAsync(groupId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("group1");
			result.Name.ShouldBe("Engineering Team");
			result.NumberOfMembers.ShouldBe(25);
		}

		[Fact]
		public async Task GetAsync_DifferentGroup()
		{
			// Arrange
			var groupId = "group999";
			var customGroupJson = @"{
				""id"": ""group999"",
				""name"": ""Custom Group"",
				""total_members"": 50
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("groups", groupId))
				.Respond("application/json", customGroupJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			var result = await groups.GetAsync(groupId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("group999");
			result.Name.ShouldBe("Custom Group");
		}

		#endregion

		#region CreateAsync Tests

		[Fact]
		public async Task CreateAsync()
		{
			// Arrange
			var groupName = "New Group";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("groups"))
				.Respond("application/json", CREATED_GROUP_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			var result = await groups.CreateAsync(groupName, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("newGroup123");
			result.Name.ShouldBe("New Group");
			result.NumberOfMembers.ShouldBe(0);
		}

		[Fact]
		public async Task CreateAsync_WithLongName()
		{
			// Arrange
			var groupName = "This is a very long group name with special characters !@#$%";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("groups"))
				.Respond("application/json", CREATED_GROUP_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			var result = await groups.CreateAsync(groupName, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion

		#region UpdateAsync Tests

		[Fact]
		public async Task UpdateAsync()
		{
			// Arrange
			var groupId = "group1";
			var newName = "Updated Group Name";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("groups", groupId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			await groups.UpdateAsync(groupId, newName, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateAsync_WithSpecialCharacters()
		{
			// Arrange
			var groupId = "group2";
			var newName = "Updated Name with Special Chars: !@#$%^&*()";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("groups", groupId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			await groups.UpdateAsync(groupId, newName, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region DeleteAsync Tests

		[Fact]
		public async Task DeleteAsync()
		{
			// Arrange
			var groupId = "group1";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("groups", groupId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			await groups.DeleteAsync(groupId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeleteAsync_DifferentGroup()
		{
			// Arrange
			var groupId = "groupToDelete456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("groups", groupId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			await groups.DeleteAsync(groupId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region AddMembersByEmailAsync Tests

		[Fact]
		public async Task AddMembersByEmailAsync()
		{
			// Arrange
			var groupId = "group1";
			var emails = new[] { "user1@example.com", "user2@example.com", "user3@example.com" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("groups", groupId, "members"))
				.Respond("application/json", ADD_MEMBERS_RESPONSE_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			var result = await groups.AddMembersByEmailAsync(groupId, emails, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(3);
			result[0].ShouldBe("user1");
			result[1].ShouldBe("user2");
			result[2].ShouldBe("user3");
		}

		[Fact]
		public async Task AddMembersByEmailAsync_SingleMember()
		{
			// Arrange
			var groupId = "group1";
			var emails = new[] { "single@example.com" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("groups", groupId, "members"))
				.Respond("application/json", ADD_SINGLE_MEMBER_RESPONSE_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			var result = await groups.AddMembersByEmailAsync(groupId, emails, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
			result[0].ShouldBe("user1");
		}

		#endregion

		#region AddMembersByIdAsync Tests

		[Fact]
		public async Task AddMembersByIdAsync()
		{
			// Arrange
			var groupId = "group1";
			var userIds = new[] { "userId1", "userId2", "userId3" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("groups", groupId, "members"))
				.Respond("application/json", ADD_MEMBERS_RESPONSE_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			var result = await groups.AddMembersByIdAsync(groupId, userIds, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(3);
		}

		[Fact]
		public async Task AddMembersByIdAsync_SingleMember()
		{
			// Arrange
			var groupId = "group2";
			var userIds = new[] { "singleUserId" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("groups", groupId, "members"))
				.Respond("application/json", ADD_SINGLE_MEMBER_RESPONSE_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			var result = await groups.AddMembersByIdAsync(groupId, userIds, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
		}

		#endregion

		#region AddAdministratorsByEmailAsync Tests

		[Fact]
		public async Task AddAdministratorsByEmailAsync()
		{
			// Arrange
			var groupId = "group1";
			var emails = new[] { "admin1@example.com", "admin2@example.com" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("groups", groupId, "admins"))
				.Respond("application/json", ADD_ADMINS_RESPONSE_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			var result = await groups.AddAdministratorsByEmailAsync(groupId, emails, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(2);
			result[0].ShouldBe("admin1");
			result[1].ShouldBe("admin2");
		}

		[Fact]
		public async Task AddAdministratorsByEmailAsync_SingleAdmin()
		{
			// Arrange
			var groupId = "group1";
			var emails = new[] { "singleadmin@example.com" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("groups", groupId, "admins"))
				.Respond("application/json", ADD_SINGLE_ADMIN_RESPONSE_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			var result = await groups.AddAdministratorsByEmailAsync(groupId, emails, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
			result[0].ShouldBe("admin1");
		}

		#endregion

		#region AddAdministratorsByIdAsync Tests

		[Fact]
		public async Task AddAdministratorsByIdAsync()
		{
			// Arrange
			var groupId = "group1";
			var userIds = new[] { "adminId1", "adminId2" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("groups", groupId, "admins"))
				.Respond("application/json", ADD_ADMINS_RESPONSE_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			var result = await groups.AddAdministratorsByIdAsync(groupId, userIds, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(2);
		}

		[Fact]
		public async Task AddAdministratorsByIdAsync_SingleAdmin()
		{
			// Arrange
			var groupId = "group2";
			var userIds = new[] { "singleAdminId" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("groups", groupId, "admins"))
				.Respond("application/json", ADD_SINGLE_ADMIN_RESPONSE_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			var result = await groups.AddAdministratorsByIdAsync(groupId, userIds, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
		}

		#endregion

		#region RemoveMemberAsync Tests

		[Fact]
		public async Task RemoveMemberAsync()
		{
			// Arrange
			var groupId = "group1";
			var memberId = "member1";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("groups", groupId, "members", memberId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			await groups.RemoveMemberAsync(groupId, memberId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task RemoveMemberAsync_DifferentMember()
		{
			// Arrange
			var groupId = "group2";
			var memberId = "memberToRemove999";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("groups", groupId, "members", memberId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			await groups.RemoveMemberAsync(groupId, memberId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region RemoveAdministratorAsync Tests

		[Fact]
		public async Task RemoveAdministratorAsync()
		{
			// Arrange
			var groupId = "group1";
			var adminId = "admin1";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("groups", groupId, "admins", adminId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			await groups.RemoveAdministratorAsync(groupId, adminId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task RemoveAdministratorAsync_DifferentAdmin()
		{
			// Arrange
			var groupId = "group2";
			var adminId = "adminToRemove999";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("groups", groupId, "admins", adminId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			await groups.RemoveAdministratorAsync(groupId, adminId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region GetMembersAsync Tests

		[Fact]
		public async Task GetMembersAsync_DefaultParameters()
		{
			// Arrange
			var groupId = "group1";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("groups", groupId, "members"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", GROUP_MEMBERS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			var result = await groups.GetMembersAsync(groupId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecordsPerPage.ShouldBe(30);
			result.NextPageToken.ShouldBe("memberToken123");
			result.TotalRecords.ShouldBe(2);
			result.Records.Length.ShouldBe(2);
			result.Records[0].Id.ShouldBe("member1");
			result.Records[0].EmailAddress.ShouldBe("member1@example.com");
			result.Records[0].FirstName.ShouldBe("John");
			result.Records[0].LastName.ShouldBe("Doe");
			result.Records[0].Type.ShouldBe(GroupMemberType.Licensed);
		}

		[Fact]
		public async Task GetMembersAsync_WithPagination()
		{
			// Arrange
			var groupId = "group1";
			var recordsPerPage = 10;
			var pagingToken = "customToken";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("groups", groupId, "members"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("next_page_token", pagingToken)
				.Respond("application/json", GROUP_MEMBERS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			var result = await groups.GetMembersAsync(groupId, recordsPerPage, pagingToken, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public void GetMembersAsync_InvalidRecordsPerPage_ThrowsException()
		{
			// Arrange
			var groupId = "group1";
			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act & Assert
			Should.Throw<ArgumentOutOfRangeException>(async () =>
			{
				await groups.GetMembersAsync(groupId, recordsPerPage: 0, cancellationToken: TestContext.Current.CancellationToken);
			});
		}

		[Fact]
		public async Task GetMembersAsync_EmptyMembers()
		{
			// Arrange
			var groupId = "group1";
			var emptyMembersJson = @"{
				""page_size"": 30,
				""next_page_token"": """",
				""total_records"": 0,
				""members"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("groups", groupId, "members"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", emptyMembersJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			var result = await groups.GetMembersAsync(groupId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(0);
			result.TotalRecords.ShouldBe(0);
		}

		#endregion

		#region GetAdministratorsAsync Tests

		[Fact]
		public async Task GetAdministratorsAsync_DefaultParameters()
		{
			// Arrange
			var groupId = "group1";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("groups", groupId, "admins"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", GROUP_ADMINS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			var result = await groups.GetAdministratorsAsync(groupId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecordsPerPage.ShouldBe(30);
			result.NextPageToken.ShouldBe("adminToken123");
			result.TotalRecords.ShouldBe(2);
			result.Records.Length.ShouldBe(2);
			result.Records[0].EmailAddress.ShouldBe("admin1@example.com");
			result.Records[0].Name.ShouldBe("Admin One");
			result.Records[1].EmailAddress.ShouldBe("admin2@example.com");
			result.Records[1].Name.ShouldBe("Admin Two");
		}

		[Fact]
		public async Task GetAdministratorsAsync_WithPagination()
		{
			// Arrange
			var groupId = "group1";
			var recordsPerPage = 15;
			var pagingToken = "adminPageToken";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("groups", groupId, "admins"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("next_page_token", pagingToken)
				.Respond("application/json", GROUP_ADMINS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			var result = await groups.GetAdministratorsAsync(groupId, recordsPerPage, pagingToken, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public void GetAdministratorsAsync_InvalidRecordsPerPage_ThrowsException()
		{
			// Arrange
			var groupId = "group1";
			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act & Assert
			Should.Throw<ArgumentOutOfRangeException>(async () =>
			{
				await groups.GetAdministratorsAsync(groupId, recordsPerPage: 0, cancellationToken: TestContext.Current.CancellationToken);
			});
		}

		[Fact]
		public async Task GetAdministratorsAsync_EmptyAdmins()
		{
			// Arrange
			var groupId = "group1";
			var emptyAdminsJson = @"{
				""page_size"": 30,
				""next_page_token"": """",
				""total_records"": 0,
				""admins"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("groups", groupId, "admins"))
				.WithQueryString("page_size", "30")
				.Respond("application/json", emptyAdminsJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			var result = await groups.GetAdministratorsAsync(groupId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(0);
			result.TotalRecords.ShouldBe(0);
		}

		#endregion

		#region UploadVirtualBackgroundAsync Tests

		[Fact]
		public async Task UploadVirtualBackgroundAsync()
		{
			// Arrange
			var groupId = "group1";
			var fileName = "background.jpg";
			var pictureData = new MemoryStream(Encoding.UTF8.GetBytes("fake image data"));

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("groups", groupId, "settings", "virtual_backgrounds"))
				.Respond("application/json", VIRTUAL_BACKGROUND_FILE_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			var result = await groups.UploadVirtualBackgroundAsync(groupId, fileName, pictureData, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("bgFile123");
			result.IsDefault.ShouldBeFalse();
			result.Name.ShouldBe("background.jpg");
			result.Size.ShouldBe(1024000);
			result.Type.ShouldBe(VirtualBackgroundType.Image);
		}

		[Fact]
		public async Task UploadVirtualBackgroundAsync_DifferentFile()
		{
			// Arrange
			var groupId = "group2";
			var fileName = "custom_background.png";
			var pictureData = new MemoryStream(Encoding.UTF8.GetBytes("different image data"));

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("groups", groupId, "settings", "virtual_backgrounds"))
				.Respond("application/json", VIRTUAL_BACKGROUND_FILE_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			var result = await groups.UploadVirtualBackgroundAsync(groupId, fileName, pictureData, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task UploadVirtualBackgroundAsync_LargeFile()
		{
			// Arrange
			var groupId = "group3";
			var fileName = "large_background.jpg";
			var largeData = new byte[1024 * 1024]; // 1 MB
			var pictureData = new MemoryStream(largeData);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("groups", groupId, "settings", "virtual_backgrounds"))
				.Respond("application/json", VIRTUAL_BACKGROUND_FILE_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			var result = await groups.UploadVirtualBackgroundAsync(groupId, fileName, pictureData, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion

		#region DeleteVirtualBackgroundsAsync Tests

		[Fact]
		public async Task DeleteVirtualBackgroundsAsync_SingleFile()
		{
			// Arrange
			var groupId = "group1";
			var fileIds = new[] { "file1" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("groups", groupId, "settings", "virtual_backgrounds"))
				.WithQueryString("file_ids", "file1")
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			await groups.DeleteVirtualBackgroundsAsync(groupId, fileIds, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeleteVirtualBackgroundsAsync_MultipleFiles()
		{
			// Arrange
			var groupId = "group1";
			var fileIds = new[] { "file1", "file2", "file3" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("groups", groupId, "settings", "virtual_backgrounds"))
				.WithQueryString("file_ids", "file1,file2,file3")
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			await groups.DeleteVirtualBackgroundsAsync(groupId, fileIds, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeleteVirtualBackgroundsAsync_DifferentGroup()
		{
			// Arrange
			var groupId = "group999";
			var fileIds = new[] { "fileToDelete" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("groups", groupId, "settings", "virtual_backgrounds"))
				.WithQueryString("file_ids", "fileToDelete")
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			await groups.DeleteVirtualBackgroundsAsync(groupId, fileIds, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Edge Case Tests

		[Fact]
		public async Task GetMembersAsync_MaxRecordsPerPage()
		{
			// Arrange
			var groupId = "group1";
			var recordsPerPage = 300;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("groups", groupId, "members"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.Respond("application/json", GROUP_MEMBERS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			var result = await groups.GetMembersAsync(groupId, recordsPerPage: recordsPerPage, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAdministratorsAsync_MaxRecordsPerPage()
		{
			// Arrange
			var groupId = "group1";
			var recordsPerPage = 300;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("groups", groupId, "admins"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.Respond("application/json", GROUP_ADMINS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			var result = await groups.GetAdministratorsAsync(groupId, recordsPerPage: recordsPerPage, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task CreateAsync_EmptyName()
		{
			// Arrange
			var groupName = "";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("groups"))
				.Respond("application/json", CREATED_GROUP_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			var result = await groups.CreateAsync(groupName, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetAllAsync_SingleGroup()
		{
			// Arrange
			var singleGroupJson = @"{
				""groups"": [
					{
						""id"": ""onlyGroup"",
						""name"": ""Only Group"",
						""total_members"": 1
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("groups"))
				.Respond("application/json", singleGroupJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var groups = new Groups(client);

			// Act
			var result = await groups.GetAllAsync(TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
			result[0].Id.ShouldBe("onlyGroup");
		}

		#endregion
	}
}
