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
using ZoomNet.UnitTests.Properties;

namespace ZoomNet.UnitTests.Resources
{
	public class GroupsTests
	{
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
				.Respond("application/json", EndpointsResource.groups_GET);

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

		#endregion

		#region GetAsync Tests

		[Fact]
		public async Task GetAsync()
		{
			// Arrange
			var groupId = "group1";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("groups", groupId))
				.Respond("application/json", EndpointsResource.groups__groupId__GET);

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

		#endregion

		#region CreateAsync Tests

		[Fact]
		public async Task CreateAsync()
		{
			// Arrange
			var groupName = "New Group";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("groups"))
				.Respond("application/json", EndpointsResource.groups_POST);

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
				.Respond("application/json", EndpointsResource.groups__groupId__members_POST);

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
				.Respond("application/json", EndpointsResource.groups__groupId__members_POST);

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
				.Respond("application/json", EndpointsResource.groups__groupId__admins_POST);

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
				.Respond("application/json", EndpointsResource.groups__groupId__admins_POST);

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
				.Respond("application/json", EndpointsResource.groups__groupId__members_GET);

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
				.Respond("application/json", EndpointsResource.groups__groupId__admins_GET);

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
				.Respond("application/json", EndpointsResource.groups__groupId__settings_virtual_backgrounds_POST);

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

		#endregion
	}
}
