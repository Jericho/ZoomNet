using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Resources;

namespace ZoomNet.UnitTests.Resources
{
	public class RolesTests
	{
		private const string ROLES_JSON = @"{
			""page_size"": 300,
			""next_page_token"": ""token123"",
			""roles"": [
				{
					""id"": ""role123"",
					""name"": ""Admin"",
					""description"": ""Administrator role"",
					""total_members"": 5
				},
				{
					""id"": ""role456"",
					""name"": ""Member"",
					""description"": ""Standard member role"",
					""total_members"": 25
				}
			]
		}";

		private const string SINGLE_ROLE_JSON = @"{
			""id"": ""role123"",
			""name"": ""Admin"",
			""description"": ""Administrator role with full access"",
			""total_members"": 5,
			""privileges"": [
				""User:Edit"",
				""User:View"",
				""Account:Edit""
			]
		}";

		private const string CREATED_ROLE_JSON = @"{
			""id"": ""newRole789"",
			""name"": ""Custom Role"",
			""description"": ""Custom role description"",
			""total_members"": 0
		}";

		private const string ROLE_MEMBERS_JSON = @"{
			""page_size"": 300,
			""next_page_token"": ""memberToken456"",
			""total_records"": 3,
			""members"": [
				{
					""id"": ""user123"",
					""email"": ""user1@example.com"",
					""first_name"": ""John"",
					""last_name"": ""Doe"",
					""type"": 2
				},
				{
					""id"": ""user456"",
					""email"": ""user2@example.com"",
					""first_name"": ""Jane"",
					""last_name"": ""Smith"",
					""type"": 1
				},
				{
					""id"": ""user789"",
					""email"": ""user3@example.com"",
					""first_name"": ""Bob"",
					""last_name"": ""Johnson"",
					""type"": 2
				}
			]
		}";

		private readonly ITestOutputHelper _outputHelper;

		public RolesTests(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

		#region GetAllAsync Tests

		[Fact]
		public async Task GetAllAsync_DefaultParameters()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("roles"))
				.WithQueryString("page_size", "300")
				.Respond("application/json", ROLES_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act
			var result = await roles.GetAllAsync(cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecordsPerPage.ShouldBe(300);
			result.NextPageToken.ShouldBe("token123");
			result.Records.ShouldNotBeNull();
			result.Records.Length.ShouldBe(2);
			result.Records[0].Id.ShouldBe("role123");
			result.Records[0].Name.ShouldBe("Admin");
			result.Records[0].Description.ShouldBe("Administrator role");
			result.Records[0].MembersCount.ShouldBe(5);
			result.Records[1].Id.ShouldBe("role456");
			result.Records[1].Name.ShouldBe("Member");
		}

		[Fact]
		public async Task GetAllAsync_WithPagination()
		{
			// Arrange
			var recordsPerPage = 100;
			var pagingToken = "customToken123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("roles"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("next_page_token", pagingToken)
				.Respond("application/json", ROLES_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act
			var result = await roles.GetAllAsync(recordsPerPage, pagingToken, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(2);
		}

		[Fact]
		public async Task GetAllAsync_EmptyRoles()
		{
			// Arrange
			var emptyRolesJson = @"{
				""page_size"": 300,
				""next_page_token"": """",
				""roles"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("roles"))
				.WithQueryString("page_size", "300")
				.Respond("application/json", emptyRolesJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act
			var result = await roles.GetAllAsync(cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(0);
		}

		[Fact]
		public async Task GetAllAsync_MaxRecordsPerPage()
		{
			// Arrange
			var recordsPerPage = 300;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("roles"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.Respond("application/json", ROLES_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act
			var result = await roles.GetAllAsync(recordsPerPage, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion

		#region CreateAsync Tests

		[Fact]
		public async Task CreateAsync_MinimalParameters()
		{
			// Arrange
			var name = "New Role";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("roles"))
				.Respond(HttpStatusCode.Created, "application/json", CREATED_ROLE_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act
			var result = await roles.CreateAsync(name, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("newRole789");
			result.Name.ShouldBe("Custom Role");
			result.Description.ShouldBe("Custom role description");
			result.MembersCount.ShouldBe(0);
		}

		[Fact]
		public async Task CreateAsync_WithDescription()
		{
			// Arrange
			var name = "Manager Role";
			var description = "Role for managers with limited admin access";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("roles"))
				.Respond(HttpStatusCode.Created, "application/json", CREATED_ROLE_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act
			var result = await roles.CreateAsync(name, description, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("newRole789");
		}

		[Fact]
		public async Task CreateAsync_WithPrivileges()
		{
			// Arrange
			var name = "Custom Admin";
			var description = "Custom administrator role";
			var privileges = new[] { "User:Edit", "User:View", "Account:View" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("roles"))
				.Respond(HttpStatusCode.Created, "application/json", CREATED_ROLE_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act
			var result = await roles.CreateAsync(name, description, privileges, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task CreateAsync_WithAllParameters()
		{
			// Arrange
			var name = "Super Admin";
			var description = "Super administrator with all privileges";
			var privileges = new[] { "User:Edit", "User:View", "Account:Edit", "Account:View", "Billing:Edit" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("roles"))
				.Respond(HttpStatusCode.Created, "application/json", CREATED_ROLE_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act
			var result = await roles.CreateAsync(name, description, privileges, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("newRole789");
		}

		#endregion

		#region GetMembersAsync Tests

		[Fact]
		public async Task GetMembersAsync_DefaultParameters()
		{
			// Arrange
			var roleId = "role123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("roles", roleId, "members"))
				.WithQueryString("page_size", "300")
				.Respond("application/json", ROLE_MEMBERS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act
			var result = await roles.GetMembersAsync(roleId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecordsPerPage.ShouldBe(300);
			result.NextPageToken.ShouldBe("memberToken456");
			result.TotalRecords.ShouldBe(3);
			result.Records.Length.ShouldBe(3);
			result.Records[0].Id.ShouldBe("user123");
			result.Records[0].Email.ShouldBe("user1@example.com");
			result.Records[0].FirstName.ShouldBe("John");
			result.Records[0].LastName.ShouldBe("Doe");
			result.Records[1].Id.ShouldBe("user456");
			result.Records[2].Id.ShouldBe("user789");
		}

		[Fact]
		public async Task GetMembersAsync_WithPagination()
		{
			// Arrange
			var roleId = "role456";
			var recordsPerPage = 50;
			var pagingToken = "customMemberToken";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("roles", roleId, "members"))
				.WithQueryString("page_size", recordsPerPage.ToString())
				.WithQueryString("next_page_token", pagingToken)
				.Respond("application/json", ROLE_MEMBERS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act
			var result = await roles.GetMembersAsync(roleId, recordsPerPage, pagingToken, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(3);
		}

		[Fact]
		public async Task GetMembersAsync_EmptyMembers()
		{
			// Arrange
			var roleId = "role789";
			var emptyMembersJson = @"{
				""page_size"": 300,
				""next_page_token"": """",
				""total_records"": 0,
				""members"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("roles", roleId, "members"))
				.WithQueryString("page_size", "300")
				.Respond("application/json", emptyMembersJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act
			var result = await roles.GetMembersAsync(roleId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Records.Length.ShouldBe(0);
			result.TotalRecords.ShouldBe(0);
		}

		#endregion

		#region AssignUsersAsync Tests

		[Fact]
		public async Task AssignUsersAsync_WithUserIds()
		{
			// Arrange
			var roleId = "role123";
			var userIds = new[] { "user123", "user456", "user789" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("roles", roleId, "members"))
				.Respond(HttpStatusCode.Created);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act
			await roles.AssignUsersAsync(roleId, userIds, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task AssignUsersAsync_WithEmailAddresses()
		{
			// Arrange
			var roleId = "role456";
			var emailAddresses = new[] { "user1@example.com", "user2@example.com", "user3@example.com" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("roles", roleId, "members"))
				.Respond(HttpStatusCode.Created);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act
			await roles.AssignUsersAsync(roleId, emailAddresses, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task AssignUsersAsync_MixedIdsAndEmails()
		{
			// Arrange
			var roleId = "role789";
			var mixedIds = new[] { "userId123", "admin@example.com", "userId456", "user@domain.com" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("roles", roleId, "members"))
				.Respond(HttpStatusCode.Created);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act
			await roles.AssignUsersAsync(roleId, mixedIds, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task AssignUsersAsync_SingleUser()
		{
			// Arrange
			var roleId = "role123";
			var userIds = new[] { "singleUser123" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("roles", roleId, "members"))
				.Respond(HttpStatusCode.Created);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act
			await roles.AssignUsersAsync(roleId, userIds, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task AssignUsersAsync_NullUserIds_ThrowsException()
		{
			// Arrange
			var roleId = "role123";
			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentNullException>(() => roles.AssignUsersAsync(roleId, null, TestContext.Current.CancellationToken));
		}

		[Fact]
		public async Task AssignUsersAsync_EmptyUserIds_ThrowsException()
		{
			// Arrange
			var roleId = "role123";
			var emptyUserIds = Array.Empty<string>();
			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act & Assert
			await Should.ThrowAsync<ArgumentException>(() => roles.AssignUsersAsync(roleId, emptyUserIds, TestContext.Current.CancellationToken));
		}

		#endregion

		#region UnassignUserAsync Tests

		[Fact]
		public async Task UnassignUserAsync_WithUserId()
		{
			// Arrange
			var roleId = "role123";
			var userId = "user456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("roles", roleId, "members", userId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act
			await roles.UnassignUserAsync(roleId, userId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UnassignUserAsync_WithEmailAddress()
		{
			// Arrange
			var roleId = "role789";
			var userEmail = "user@example.com";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("roles", roleId, "members", userEmail))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act
			await roles.UnassignUserAsync(roleId, userEmail, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UnassignUserAsync_DifferentRole()
		{
			// Arrange
			var roleId = "customRole999";
			var userId = "userToRemove123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("roles", roleId, "members", userId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act
			await roles.UnassignUserAsync(roleId, userId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region GetAsync Tests

		[Fact]
		public async Task GetAsync()
		{
			// Arrange
			var roleId = "role123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("roles", roleId))
				.Respond("application/json", SINGLE_ROLE_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act
			var result = await roles.GetAsync(roleId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("role123");
			result.Name.ShouldBe("Admin");
			result.Description.ShouldBe("Administrator role with full access");
			result.MembersCount.ShouldBe(5);
		}

		[Fact]
		public async Task GetAsync_DifferentRole()
		{
			// Arrange
			var roleId = "role999";
			var customRoleJson = @"{
				""id"": ""role999"",
				""name"": ""Viewer"",
				""description"": ""Read-only access"",
				""total_members"": 50,
				""privileges"": [
					""User:View"",
					""Account:View""
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("roles", roleId))
				.Respond("application/json", customRoleJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act
			var result = await roles.GetAsync(roleId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("role999");
			result.Name.ShouldBe("Viewer");
			result.Description.ShouldBe("Read-only access");
			result.MembersCount.ShouldBe(50);
		}

		#endregion

		#region UpdateRole Tests

		[Fact]
		public async Task UpdateRole_NameOnly()
		{
			// Arrange
			var roleId = "role123";
			var newName = "Updated Role Name";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("roles", roleId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act
			await roles.UpdateRole(roleId, newName, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateRole_WithDescription()
		{
			// Arrange
			var roleId = "role456";
			var newName = "Manager";
			var newDescription = "Updated description for manager role";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("roles", roleId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act
			await roles.UpdateRole(roleId, newName, newDescription, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateRole_WithPrivileges()
		{
			// Arrange
			var roleId = "role789";
			var newName = "Custom Admin";
			var newDescription = "Custom administrator";
			var newPrivileges = new[] { "User:Edit", "User:View", "Account:View", "Billing:View" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("roles", roleId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act
			await roles.UpdateRole(roleId, newName, newDescription, newPrivileges, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateRole_WithAllParameters()
		{
			// Arrange
			var roleId = "role999";
			var newName = "Super Admin Updated";
			var newDescription = "Updated super administrator role";
			var newPrivileges = new[] { "User:Edit", "User:View", "User:Delete", "Account:Edit", "Account:View", "Billing:Edit", "Billing:View" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("roles", roleId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act
			await roles.UpdateRole(roleId, newName, newDescription, newPrivileges, TestContext.Current.CancellationToken);

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
			var roleId = "role123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("roles", roleId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act
			await roles.DeleteAsync(roleId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeleteAsync_DifferentRole()
		{
			// Arrange
			var roleId = "roleToDelete999";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("roles", roleId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act
			await roles.DeleteAsync(roleId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task DeleteAsync_CustomRole()
		{
			// Arrange
			var roleId = "customRole456";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("roles", roleId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act
			await roles.DeleteAsync(roleId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion

		#region Edge Case and Integration Tests

		[Fact]
		public async Task GetAllAsync_MultiplePages()
		{
			// Arrange
			var firstPageJson = @"{
				""page_size"": 300,
				""next_page_token"": ""page2Token"",
				""roles"": [
					{
						""id"": ""role1"",
						""name"": ""Role 1"",
						""total_members"": 10
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("roles"))
				.WithQueryString("page_size", "300")
				.Respond("application/json", firstPageJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act
			var result = await roles.GetAllAsync(cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.NextPageToken.ShouldBe("page2Token");
			result.MoreRecordsAvailable.ShouldBeTrue();
		}

		[Fact]
		public async Task AssignUsersAsync_HandleEmailWithAtSymbol()
		{
			// Arrange
			var roleId = "role123";
			var emailsWithSpecialChars = new[] { "user+test@example.com", "user.name@sub.domain.com" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("roles", roleId, "members"))
				.Respond(HttpStatusCode.Created);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act
			await roles.AssignUsersAsync(roleId, emailsWithSpecialChars, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task CreateAsync_EmptyPrivilegesArray()
		{
			// Arrange
			var name = "Limited Role";
			var description = "Role with no privileges";
			var emptyPrivileges = Array.Empty<string>();

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("roles"))
				.Respond(HttpStatusCode.Created, "application/json", CREATED_ROLE_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act
			var result = await roles.CreateAsync(name, description, emptyPrivileges, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task GetMembersAsync_LargeResultSet()
		{
			// Arrange
			var roleId = "popularRole123";
			var largeMembersJson = @"{
				""page_size"": 300,
				""next_page_token"": ""moreMembers"",
				""total_records"": 1000,
				""members"": [
					{
						""id"": ""user1"",
						""email"": ""user1@example.com"",
						""first_name"": ""User"",
						""last_name"": ""One"",
						""type"": 2
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("roles", roleId, "members"))
				.WithQueryString("page_size", "300")
				.Respond("application/json", largeMembersJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var roles = new Roles(client);

			// Act
			var result = await roles.GetMembersAsync(roleId, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.TotalRecords.ShouldBe(1000);
			result.MoreRecordsAvailable.ShouldBeTrue();
		}

		#endregion
	}
}
