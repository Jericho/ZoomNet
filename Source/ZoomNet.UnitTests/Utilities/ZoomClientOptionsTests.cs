using Shouldly;
using System;
using Xunit;
using ZoomNet.Utilities;

namespace ZoomNet.UnitTests.Utilities
{
	public class ZoomClientOptionsTests
	{
		[Fact]
		public void Uses_global_url_by_default()
		{
			// Arrange
			var options = new ZoomClientOptions();

			// Assert
			options.ApiBaseUrl.Host.ShouldBe("api.zoom.us");
		}

		[Fact]
		public void Can_use_australia_url()
		{
			// Arrange
			var options = new ZoomClientOptions().WithAustraliaBaseUrl();

			// Assert
			options.ApiBaseUrl.Host.ShouldBe("api-au.zoom.us");
		}

		[Fact]
		public void Can_use_canada_url()
		{
			// Arrange
			var options = new ZoomClientOptions().WithCanadaBaseUrl();

			// Assert
			options.ApiBaseUrl.Host.ShouldBe("api-ca.zoom.us");
		}

		[Fact]
		public void Can_use_europe_url()
		{
			// Arrange
			var options = new ZoomClientOptions().WithEuropeanUnionBaseUrl();

			// Assert
			options.ApiBaseUrl.Host.ShouldBe("api-eu.zoom.us");
		}

		[Fact]
		public void Can_use_india_url()
		{
			// Arrange
			var options = new ZoomClientOptions().WithIndiaBaseUrl();

			// Assert
			options.ApiBaseUrl.Host.ShouldBe("api-in.zoom.us");
		}

		[Fact]
		public void Can_use_saudi_arabia_url()
		{
			// Arrange
			var options = new ZoomClientOptions().WithSaudiArabiaBaseUrl();

			// Assert
			options.ApiBaseUrl.Host.ShouldBe("api-sa.zoom.us");
		}

		[Fact]
		public void Can_use_singapore_url()
		{
			// Arrange
			var options = new ZoomClientOptions().WithSingaporeBaseUrl();

			// Assert
			options.ApiBaseUrl.Host.ShouldBe("api-sg.zoom.us");
		}

		[Fact]
		public void Can_use_united_states_url()
		{
			// Arrange
			var options = new ZoomClientOptions().WithUnitedStatesBaseUrl();

			// Assert
			options.ApiBaseUrl.Host.ShouldBe("api-us.zoom.us");
		}

		[Fact]
		public void Can_use_vanity_url()
		{
			// Arrange
			var options = new ZoomClientOptions().WithCustomBaseUrl("https://my-vanity.zoom.us");

			// Assert
			options.ApiBaseUrl.Host.ShouldBe("my-vanity.zoom.us");
		}

		[Fact]
		public void Throws_when_base_url_is_null()
		{
			// Arrange
			Should.Throw<ArgumentNullException>(() =>
			{
				var options = new ZoomClientOptions().WithCustomBaseUrl(null);
			});
		}

		[Fact]
		public void Throws_when_base_url_is_blank()
		{
			// Arrange
			Should.Throw<ArgumentNullException>(() =>
			{
				var options = new ZoomClientOptions().WithCustomBaseUrl(string.Empty);
			});
		}
	}
}
