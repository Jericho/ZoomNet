using Newtonsoft.Json.Linq;
using Shouldly;
using System;
using Xunit;

namespace ZoomNet.UnitTests.Utilities
{
	public class InternalTests
	{
		[Fact]
		public void GetProperty_when_property_is_present_and_throwIfMissing_is_true()
		{
			// Arrange
			var item = new JObject()
			{
				{ "aaa", "123" }
			};

			// Act
			var property = item.GetProperty("aaa", true);

			// Assert
			property.ShouldNotBeNull();
			property.Value<string>().ShouldBe("123");
		}

		[Fact]
		public void GetProperty_when_property_is_present_and_throwIfMissing_is_false()
		{
			// Arrange
			var item = new JObject()
			{
				{ "aaa", "123" }
			};

			// Act
			var property = item.GetProperty("aaa", false);

			// Assert
			property.ShouldNotBeNull();
			property.Value<string>().ShouldBe("123");
		}

		[Fact]
		public void GetProperty_returns_null_when_property_is_missing_and_throwIfMissing_is_false()
		{
			// Arrange
			var item = new JObject()
			{
				{ "aaa", "123" }
			};

			// Act
			var property = item.GetProperty("zzz", false);

			// Assert
			property.ShouldBeNull();
		}

		[Fact]
		public void GetProperty_throws_when_property_is_missing_and_throwIfMissing_is_true()
		{
			// Arrange
			var item = new JObject()
			{
				{ "aaa", "123" }
			};

			// Act
			Should.Throw<ArgumentException>(() => item.GetProperty("zzz", true));
		}

		[Fact]
		public void GetPropertyValue_when_property_is_present_and_default_value_is_provided()
		{
			// Arrange
			var item = new JObject()
			{
				{ "aaa", "123" }
			};

			// Act
			var property = item.GetPropertyValue("aaa", "Default value");

			// Assert
			property.ShouldBe("123");
		}

		[Fact]
		public void GetPropertyValue_when_property_is_present_and_default_value_is_omitted()
		{
			// Arrange
			var item = new JObject()
			{
				{ "aaa", "123" }
			};

			// Act
			var property = item.GetPropertyValue<string>("aaa");

			// Assert
			property.ShouldBe("123");
		}

		[Fact]
		public void GetPropertyValue_returns_default_value_when_property_is_missing()
		{
			// Arrange
			var item = new JObject()
			{
				{ "aaa", "123" }
			};

			// Act
			var property = item.GetPropertyValue("zzz", "Default value");

			// Assert
			property.ShouldBe("Default value");
		}

		[Fact]
		public void GetPropertyValue_throws_when_property_is_missing()
		{
			// Arrange
			var item = new JObject()
			{
				{ "aaa", "123" }
			};

			// Act
			Should.Throw<ArgumentException>(() => item.GetPropertyValue<string>("zzz"));
		}
	}
}
