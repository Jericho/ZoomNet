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
	public class TrackingFieldsTests
	{
		private const string TRACKING_FIELDS_JSON = @"{
			""tracking_fields"": [
				{
					""id"": ""field1"",
					""field"": ""Department"",
					""required"": true,
					""visible"": true,
					""recommended_values"": [""Sales"", ""Marketing"", ""Engineering""]
				},
				{
					""id"": ""field2"",
					""field"": ""Project Code"",
					""required"": false,
					""visible"": true,
					""recommended_values"": [""PROJ001"", ""PROJ002""]
				}
			]
		}";

		private const string SINGLE_TRACKING_FIELD_JSON = @"{
			""id"": ""field1"",
			""field"": ""Department"",
			""required"": true,
			""visible"": true,
			""recommended_values"": [""Sales"", ""Marketing"", ""Engineering""]
		}";

		private const string CREATED_TRACKING_FIELD_JSON = @"{
			""id"": ""newField123"",
			""field"": ""Cost Center"",
			""required"": false,
			""visible"": true,
			""recommended_values"": [""CC1000"", ""CC2000"", ""CC3000""]
		}";

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
				.Respond("application/json", TRACKING_FIELDS_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act
			var result = await trackingFields.GetAllAsync(TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(2);
			result[0].Id.ShouldBe("field1");
			result[0].Name.ShouldBe("Department");
			result[0].IsRequired.ShouldBeTrue();
			result[0].IsVisible.ShouldBeTrue();
			result[0].RecommendedValues.ShouldNotBeNull();
			result[0].RecommendedValues.Length.ShouldBe(3);
			result[0].RecommendedValues[0].ShouldBe("Sales");
			result[1].Id.ShouldBe("field2");
			result[1].Name.ShouldBe("Project Code");
			result[1].IsRequired.ShouldBeFalse();
		}

		[Fact]
		public async Task GetAllAsync_EmptyFields()
		{
			// Arrange
			var emptyJson = @"{""tracking_fields"": []}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("tracking_fields"))
				.Respond("application/json", emptyJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act
			var result = await trackingFields.GetAllAsync(TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(0);
		}

		[Fact]
		public async Task GetAllAsync_SingleField()
		{
			// Arrange
			var singleFieldJson = @"{
				""tracking_fields"": [
					{
						""id"": ""onlyField"",
						""field"": ""Single Field"",
						""required"": false,
						""visible"": false,
						""recommended_values"": []
					}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("tracking_fields"))
				.Respond("application/json", singleFieldJson);

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
			result[0].Id.ShouldBe("onlyField");
			result[0].Name.ShouldBe("Single Field");
			result[0].IsRequired.ShouldBeFalse();
			result[0].IsVisible.ShouldBeFalse();
			result[0].RecommendedValues.Length.ShouldBe(0);
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
				.Respond("application/json", SINGLE_TRACKING_FIELD_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act
			var result = await trackingFields.GetAsync(trackingFieldId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("field1");
			result.Name.ShouldBe("Department");
			result.IsRequired.ShouldBeTrue();
			result.IsVisible.ShouldBeTrue();
			result.RecommendedValues.ShouldNotBeNull();
			result.RecommendedValues.Length.ShouldBe(3);
			result.RecommendedValues[0].ShouldBe("Sales");
			result.RecommendedValues[1].ShouldBe("Marketing");
			result.RecommendedValues[2].ShouldBe("Engineering");
		}

		[Fact]
		public async Task GetAsync_DifferentField()
		{
			// Arrange
			var trackingFieldId = "field999";
			var customFieldJson = @"{
				""id"": ""field999"",
				""field"": ""Custom Field"",
				""required"": false,
				""visible"": true,
				""recommended_values"": [""Value1"", ""Value2""]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("tracking_fields", trackingFieldId))
				.Respond("application/json", customFieldJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act
			var result = await trackingFields.GetAsync(trackingFieldId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("field999");
			result.Name.ShouldBe("Custom Field");
		}

		[Fact]
		public async Task GetAsync_NoRecommendedValues()
		{
			// Arrange
			var trackingFieldId = "field_empty";
			var emptyValuesJson = @"{
				""id"": ""field_empty"",
				""field"": ""Empty Values"",
				""required"": true,
				""visible"": false,
				""recommended_values"": []
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("tracking_fields", trackingFieldId))
				.Respond("application/json", emptyValuesJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act
			var result = await trackingFields.GetAsync(trackingFieldId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecommendedValues.Length.ShouldBe(0);
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
				.Respond("application/json", CREATED_TRACKING_FIELD_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act
			var result = await trackingFields.CreateAsync(name, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Id.ShouldBe("newField123");
			result.Name.ShouldBe("Cost Center");
		}

		[Fact]
		public async Task CreateAsync_WithRecommendedValues()
		{
			// Arrange
			var name = "Department";
			var recommendedValues = new[] { "Sales", "Marketing", "Engineering", "Support" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("tracking_fields"))
				.Respond("application/json", CREATED_TRACKING_FIELD_JSON);

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
				.Respond("application/json", CREATED_TRACKING_FIELD_JSON);

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
				.Respond("application/json", CREATED_TRACKING_FIELD_JSON);

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
				.Respond("application/json", CREATED_TRACKING_FIELD_JSON);

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
				.Respond("application/json", CREATED_TRACKING_FIELD_JSON);

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
				.Respond("application/json", CREATED_TRACKING_FIELD_JSON);

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
				.Respond("application/json", CREATED_TRACKING_FIELD_JSON);

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
				.Respond("application/json", CREATED_TRACKING_FIELD_JSON);

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

		[Fact]
		public async Task DeleteAsync_DifferentField()
		{
			// Arrange
			var trackingFieldId = "fieldToDelete999";

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

		[Fact]
		public async Task DeleteAsync_WithLongFieldId()
		{
			// Arrange
			var trackingFieldId = "verylongfieldidentifierwithmanycharacters123456789";

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

		#region Edge Cases and Integration Tests

		[Fact]
		public async Task GetAllAsync_WithManyFields()
		{
			// Arrange
			var manyFieldsJson = @"{
				""tracking_fields"": [
					{""id"": ""f1"", ""field"": ""Field 1"", ""required"": true, ""visible"": true, ""recommended_values"": []},
					{""id"": ""f2"", ""field"": ""Field 2"", ""required"": false, ""visible"": true, ""recommended_values"": []},
					{""id"": ""f3"", ""field"": ""Field 3"", ""required"": true, ""visible"": false, ""recommended_values"": []},
					{""id"": ""f4"", ""field"": ""Field 4"", ""required"": false, ""visible"": false, ""recommended_values"": []},
					{""id"": ""f5"", ""field"": ""Field 5"", ""required"": true, ""visible"": true, ""recommended_values"": []}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("tracking_fields"))
				.Respond("application/json", manyFieldsJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act
			var result = await trackingFields.GetAllAsync(TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(5);
		}

		[Fact]
		public async Task CreateAsync_WithSingleRecommendedValue()
		{
			// Arrange
			var name = "Field with One Value";
			var recommendedValues = new[] { "OnlyValue" };

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("tracking_fields"))
				.Respond("application/json", CREATED_TRACKING_FIELD_JSON);

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
		public async Task GetAsync_FieldWithManyRecommendedValues()
		{
			// Arrange
			var trackingFieldId = "field_many";
			var manyValuesJson = @"{
				""id"": ""field_many"",
				""field"": ""Many Values"",
				""required"": true,
				""visible"": true,
				""recommended_values"": [""A"", ""B"", ""C"", ""D"", ""E"", ""F"", ""G"", ""H"", ""I"", ""J""]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("tracking_fields", trackingFieldId))
				.Respond("application/json", manyValuesJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act
			var result = await trackingFields.GetAsync(trackingFieldId, TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.RecommendedValues.Length.ShouldBe(10);
		}

		[Fact]
		public async Task CreateAsync_WithNullName_DoesNotValidate()
		{
			// Arrange
			string name = null;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("tracking_fields"))
				.Respond("application/json", CREATED_TRACKING_FIELD_JSON);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act - Should not throw because null names have length of 0
			var result = await trackingFields.CreateAsync(name, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
		}

		[Fact]
		public async Task UpdateAsync_WithNullName_DoesNotValidate()
		{
			// Arrange
			var trackingFieldId = "field_null_update";
			string name = null;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(new HttpMethod("PATCH"), Utils.GetZoomApiUri("tracking_fields", trackingFieldId))
				.Respond(HttpStatusCode.NoContent);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act - Should not throw because null names have length of 0
			await trackingFields.UpdateAsync(trackingFieldId, name, cancellationToken: TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
		}

		[Fact]
		public async Task CreateAsync_NameBoundaryTest_49Characters()
		{
			// Arrange - Name with 49 characters (1 under limit)
			var name = new string('Z', 49);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Post, Utils.GetZoomApiUri("tracking_fields"))
				.Respond("application/json", CREATED_TRACKING_FIELD_JSON);

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
		public async Task UpdateAsync_NameBoundaryTest_49Characters()
		{
			// Arrange - Name with 49 characters (1 under limit)
			var trackingFieldId = "field_boundary";
			var name = new string('Y', 49);

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
		public async Task GetAllAsync_WithFieldsHavingVariousVisibilityAndRequiredSettings()
		{
			// Arrange
			var variousSettingsJson = @"{
				""tracking_fields"": [
					{""id"": ""f1"", ""field"": ""Required Visible"", ""required"": true, ""visible"": true, ""recommended_values"": []},
					{""id"": ""f2"", ""field"": ""Required Hidden"", ""required"": true, ""visible"": false, ""recommended_values"": []},
					{""id"": ""f3"", ""field"": ""Optional Visible"", ""required"": false, ""visible"": true, ""recommended_values"": []},
					{""id"": ""f4"", ""field"": ""Optional Hidden"", ""required"": false, ""visible"": false, ""recommended_values"": []}
				]
			}";

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("tracking_fields"))
				.Respond("application/json", variousSettingsJson);

			var logger = _outputHelper.ToLogger<IZoomClient>();
			var client = Utils.GetFluentClient(mockHttp, logger: logger);
			var trackingFields = new TrackingFields(client);

			// Act
			var result = await trackingFields.GetAllAsync(TestContext.Current.CancellationToken);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.Length.ShouldBe(4);
			result[0].IsRequired.ShouldBeTrue();
			result[0].IsVisible.ShouldBeTrue();
			result[1].IsRequired.ShouldBeTrue();
			result[1].IsVisible.ShouldBeFalse();
			result[2].IsRequired.ShouldBeFalse();
			result[2].IsVisible.ShouldBeTrue();
			result[3].IsRequired.ShouldBeFalse();
			result[3].IsVisible.ShouldBeFalse();
		}

		#endregion
	}
}
