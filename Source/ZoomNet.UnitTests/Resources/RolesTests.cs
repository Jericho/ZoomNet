using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Resources;
using ZoomNet.UnitTests.Properties;

namespace ZoomNet.UnitTests.Resources
{
	public class RolesTests
	{
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
				.Respond("application/json", EndpointsResource.roles_GET);

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

		#endregion

		#region CreateAsync Tests

		[Fact]
		public async Task CreateAsync_MinimalParameters()
		{
			// Arrange
			var name = "New Role";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("roles"))
				.Respond(HttpStatusCode.Created, "application/json", EndpointsResource.roles_POST);

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
				.Respond(HttpStatusCode.Created, "application/json", EndpointsResource.roles_POST);

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
				.Respond(HttpStatusCode.Created, "application/json", EndpointsResource.roles_POST);

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
				.Respond(HttpStatusCode.Created, "application/json", EndpointsResource.roles_POST);

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
				.Respond("application/json", EndpointsResource.roles__roleId__members_GET);

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

		#endregion

		#region GetAsync Tests

		[Fact]
		public async Task GetAsync()
		{
			// Arrange
			var roleId = "role123";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("roles", roleId))
			.Respond("application/json", EndpointsResource.roles__roleId__GET);

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

		#endregion

		#region Edge Case and Integration Tests

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
				.Respond(HttpStatusCode.Created, "application/json", EndpointsResource.roles_POST);

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

		#endregion
	}
}
