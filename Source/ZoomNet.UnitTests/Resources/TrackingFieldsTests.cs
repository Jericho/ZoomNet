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
	public class TrackingFieldsTests
	{
		private readonly ITestOutputHelper _outputHelper;

		public TrackingFieldsTests(ITestOutputHelper outputHelper)
		{
			_outputHelper = outputHelper;
		}

		#region GetAllAsync Tests

		[Fact]
		public async Task GetAllAsync()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("tracking_fields"))
				.Respond("application/json", EndpointsResource.tracking_fields_GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act
			var result = await trackingFields.GetAllAsync(TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(1);
			result[0].Id.ShouldBe("a32CJji-weJ92");
			result[0].Name.ShouldBe("field1");
			result[0].IsRequired.ShouldBeFalse();
			result[0].IsVisible.ShouldBeTrue();
			result[0].RecommendedValues.ShouldNotBeNull();
			result[0].RecommendedValues.Length.ShouldBe(1);
			result[0].RecommendedValues[0].ShouldBe("value1");
		}

		#endregion

		#region GetAsync Tests

		[Fact]
		public async Task GetAsync()
		{
			// Arrange
			var trackingFieldId = "field1";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("tracking_fields", trackingFieldId))
				.Respond("application/json", EndpointsResource.tracking_fields__fieldId__GET);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act
			var result = await trackingFields.GetAsync(trackingFieldId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("a32CJji-weJ92");
			result.Name.ShouldBe("field1");
			result.IsRequired.ShouldBeFalse();
			result.IsVisible.ShouldBeTrue();
			result.RecommendedValues.ShouldNotBeNull();
			result.RecommendedValues.Length.ShouldBe(1);
			result.RecommendedValues[0].ShouldBe("value1");
		}

		#endregion

		#region CreateAsync Tests

		[Fact]
		public async Task CreateAsync_MinimalParameters()
		{
			// Arrange
			var name = "New Field";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("tracking_fields"))
				.Respond("application/json", EndpointsResource.tracking_fields_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act
			var result = await trackingFields.CreateAsync(name, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("a32CJji-weJ92");
			result.Name.ShouldBe("field1");
		}

		[Fact]
		public async Task CreateAsync_WithRecommendedValues()
		{
			// Arrange
			var name = "Department";
			var recommendedValues = new[] { "Sales", "Marketing", "Engineering", "Support" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("tracking_fields"))
				.Respond("application/json", EndpointsResource.tracking_fields_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act
			var result = await trackingFields.CreateAsync(name, recommendedValues, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task CreateAsync_WithAllParameters()
		{
			// Arrange
			var name = "Project Code";
			var recommendedValues = new[] { "PROJ001", "PROJ002", "PROJ003" };
			var isRequired = true;
			var isVisible = false;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("tracking_fields"))
				.Respond("application/json", EndpointsResource.tracking_fields_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act
			var result = await trackingFields.CreateAsync(name, recommendedValues, isRequired, isVisible, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task CreateAsync_ExactlyMaxNameLength()
		{
			// Arrange - Name with exactly 50 characters
			var name = new string('A', 50);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("tracking_fields"))
				.Respond("application/json", EndpointsResource.tracking_fields_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act
			var result = await trackingFields.CreateAsync(name, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task CreateAsync_NameTooLong_ThrowsException()
		{
			// Arrange - Name with 51 characters (exceeds limit)
			var name = new string('A', 51);

			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act & Assert
			var exception = await Should.ThrowAsync<ArgumentOutOfRangeException>(() => trackingFields.CreateAsync(name, cancellationToken: TestContext.Current.CancellationToken));
			exception.ParamName.ShouldBe("name");
			exception.Message.ShouldContain("50 character limit");
		}

		[Fact]
		public async Task CreateAsync_NameExceedsLimit_ThrowsException()
		{
			// Arrange - Name with 100 characters (way over limit)
			var name = new string('X', 100);

			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act & Assert
			var exception = await Should.ThrowAsync<ArgumentOutOfRangeException>(() => trackingFields.CreateAsync(name, cancellationToken: TestContext.Current.CancellationToken));
			exception.ParamName.ShouldBe("name");
			exception.Message.ShouldContain("50 character limit");
		}

		[Fact]
		public async Task CreateAsync_EmptyRecommendedValues()
		{
			// Arrange
			var name = "No Values Field";
			var recommendedValues = Array.Empty<string>();

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("tracking_fields"))
				.Respond("application/json", EndpointsResource.tracking_fields_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act
			var result = await trackingFields.CreateAsync(name, recommendedValues, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task CreateAsync_RequiredAndVisible()
		{
			// Arrange
			var name = "Mandatory Visible Field";
			var isRequired = true;
			var isVisible = true;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("tracking_fields"))
				.Respond("application/json", EndpointsResource.tracking_fields_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act
			var result = await trackingFields.CreateAsync(name, isRequired: isRequired, isVisible: isVisible, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task CreateAsync_NotRequiredAndNotVisible()
		{
			// Arrange
			var name = "Optional Hidden Field";
			var isRequired = false;
			var isVisible = false;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("tracking_fields"))
				.Respond("application/json", EndpointsResource.tracking_fields_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act
			var result = await trackingFields.CreateAsync(name, isRequired: isRequired, isVisible: isVisible, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task CreateAsync_WithSpecialCharactersInName()
		{
			// Arrange
			var name = "Field with Special Chars: @#$%";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("tracking_fields"))
				.Respond("application/json", EndpointsResource.tracking_fields_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act
			var result = await trackingFields.CreateAsync(name, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task CreateAsync_WithUnicodeCharactersInName()
		{
			// Arrange
			var name = "部門 (Department in Japanese)"; // Unicode characters

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("tracking_fields"))
				.Respond("application/json", EndpointsResource.tracking_fields_POST);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act
			var result = await trackingFields.CreateAsync(name, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		#endregion

		#region UpdateAsync Tests

		[Fact]
		public async Task UpdateAsync_NameOnly()
		{
			// Arrange
			var trackingFieldId = "field1";
			var name = "Updated Field Name";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("tracking_fields", trackingFieldId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act
			await trackingFields.UpdateAsync(trackingFieldId, name, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateAsync_WithRecommendedValues()
		{
			// Arrange
			var trackingFieldId = "field2";
			var name = "Updated Department";
			var recommendedValues = new[] { "Sales", "Marketing", "HR", "IT" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("tracking_fields", trackingFieldId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act
			await trackingFields.UpdateAsync(trackingFieldId, name, recommendedValues, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateAsync_WithAllParameters()
		{
			// Arrange
			var trackingFieldId = "field3";
			var name = "Completely Updated Field";
			var recommendedValues = new[] { "Value1", "Value2", "Value3" };
			var isRequired = true;
			var isVisible = true;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("tracking_fields", trackingFieldId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act
			await trackingFields.UpdateAsync(trackingFieldId, name, recommendedValues, isRequired, isVisible, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateAsync_OnlyRequiredFlag()
		{
			// Arrange
			var trackingFieldId = "field4";
			var isRequired = false;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("tracking_fields", trackingFieldId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act
			await trackingFields.UpdateAsync(trackingFieldId, isRequired: isRequired, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateAsync_OnlyVisibleFlag()
		{
			// Arrange
			var trackingFieldId = "field5";
			var isVisible = false;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("tracking_fields", trackingFieldId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act
			await trackingFields.UpdateAsync(trackingFieldId, isVisible: isVisible, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateAsync_ExactlyMaxNameLength()
		{
			// Arrange - Name with exactly 50 characters
			var trackingFieldId = "field6";
			var name = new string('B', 50);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("tracking_fields", trackingFieldId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act
			await trackingFields.UpdateAsync(trackingFieldId, name, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateAsync_NameTooLong_ThrowsException()
		{
			// Arrange - Name with 51 characters (exceeds limit)
			var trackingFieldId = "field7";
			var name = new string('C', 51);

			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act & Assert
			var exception = await Should.ThrowAsync<ArgumentOutOfRangeException>(() => trackingFields.UpdateAsync(trackingFieldId, name, cancellationToken: TestContext.Current.CancellationToken));
			exception.ParamName.ShouldBe("name");
			exception.Message.ShouldContain("50 character limit");
		}

		[Fact]
		public async Task UpdateAsync_NameExceedsLimit_ThrowsException()
		{
			// Arrange - Name with 100 characters (way over limit)
			var trackingFieldId = "field8";
			var name = new string('D', 100);

			var mockHttp = new MockHttpMessageHandler();
			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act & Assert
			var exception = await Should.ThrowAsync<ArgumentOutOfRangeException>(() => trackingFields.UpdateAsync(trackingFieldId, name, cancellationToken: TestContext.Current.CancellationToken));
			exception.ParamName.ShouldBe("name");
			exception.Message.ShouldContain("50 character limit");
		}

		[Fact]
		public async Task UpdateAsync_EmptyRecommendedValues()
		{
			// Arrange
			var trackingFieldId = "field9";
			var name = "No Values Field";
			var recommendedValues = Array.Empty<string>();

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("tracking_fields", trackingFieldId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act
			await trackingFields.UpdateAsync(trackingFieldId, name, recommendedValues, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateAsync_ToggleBothFlags()
		{
			// Arrange
			var trackingFieldId = "field10";
			var isRequired = true;
			var isVisible = false;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("tracking_fields", trackingFieldId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act
			await trackingFields.UpdateAsync(trackingFieldId, isRequired: isRequired, isVisible: isVisible, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateAsync_WithSpecialCharactersInName()
		{
			// Arrange
			var trackingFieldId = "field11";
			var name = "Updated Field with Special Chars: !@#$%";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("tracking_fields", trackingFieldId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act
			await trackingFields.UpdateAsync(trackingFieldId, name, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateAsync_WithUnicodeCharactersInName()
		{
			// Arrange
			var trackingFieldId = "field12";
			var name = "更新されたフィールド"; // Japanese characters

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("tracking_fields", trackingFieldId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act
			await trackingFields.UpdateAsync(trackingFieldId, name, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task UpdateAsync_WithManyRecommendedValues()
		{
			// Arrange
			var trackingFieldId = "field13";
			var name = "Field with Many Values";
			var recommendedValues = new[] { "Val1", "Val2", "Val3", "Val4", "Val5", "Val6", "Val7", "Val8", "Val9", "Val10" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("tracking_fields", trackingFieldId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act
			await trackingFields.UpdateAsync(trackingFieldId, name, recommendedValues, cancellationToken: TestContext.Current.CancellationToken);

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
			var trackingFieldId = "field1";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Delete, Utils.GetZoomApiUri("tracking_fields", trackingFieldId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act
			await trackingFields.DeleteAsync(trackingFieldId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		#endregion
	}
}
