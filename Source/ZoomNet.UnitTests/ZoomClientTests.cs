using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ZoomNet.UnitTests
{
	public class ZoomClientTests
	{
		private const string API_KEY = "hFL3X-R0QqSHWqXl5FCwxQ";
		private const string API_SECRET = "bxm1pdUymzSXYOdgNYPb3yCJkFerGljc97KA";

		[Fact]
		public void Version_is_not_empty()
		{
			// Arrange

			// Act
			var result = ZoomClient.Version;

			// Assert
			result.ShouldNotBeNullOrEmpty();
		}

		[Fact]
		public async Task GetRoles()
		{
			// Arrange
			var connectionInfo = new JwtConnectionInfo(API_KEY, API_SECRET);
			var client = new ZoomClient(connectionInfo, (IWebProxy)null);

			// Act
			//var roles = await client.Roles.GetAllAsync(30, 1);
			//var role = await client.Roles.GetAsync("RWO_vaynoSVqb-rnW1hfoNQ");
			//var members = await client.Roles.GetMembersAsync("RWO_vaynoSVqb-rnW1hfoNQ");
			//var newRole = await client.Roles.CreateAsync("Test Role", "Test Role", null);
			//await client.Roles.AssignUsersAsync("RaOPu9fJ3T1SeLnnX5-h2EQ", new List<string>() { "michael.kennedy@oneadvanced.com" });
			//await client.Roles.UnassignUserAsync("RaOPu9fJ3T1SeLnnX5-h2EQ", "mchael.kennedy@oneadvanced.com" );
			//await client.Roles.UpdateRole("RaOPu9fJ3T1SeLnnX5-h2EQ", "Test Role Name", "Test Role Description");
			await client.Roles.DeleteAsync("RaOPu9fJ3T1SeLnnX5-h2EQ");

			//RaOPu9fJ3T1SeLnnX5-h2EQ


			// Assert
			// Nothing to assert. We just want to confirm that 'Dispose' did not throw any exception
			Assert.True(true);
		}

		[Fact]
		public void Dispose()
		{
			// Arrange
			var connectionInfo = new JwtConnectionInfo(API_KEY, API_SECRET);
			var client = new ZoomClient(connectionInfo, (IWebProxy)null);

			// Act
			client.Dispose();

			// Assert
			// Nothing to assert. We just want to confirm that 'Dispose' did not throw any exception
		}

		[Fact]
		public void Throws_if_apikey_is_null()
		{
			Should.Throw<ArgumentNullException>(() => new ZoomClient(new JwtConnectionInfo(null, API_SECRET)));
		}
	}
}
