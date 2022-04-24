using Shouldly;
using System.Text.Json;
using Xunit;
using ZoomNet.Json;
using ZoomNet.Models;

namespace ZoomNet.UnitTests.Resources
{
	public class RegistrantTests
	{
		#region FIELDS

		internal const string SINGLE_REGISTRANT_JSON = @"{
			""address"": ""1800 Amphibious Blvd."",
			""city"": ""Mountain View"",
			""comments"": ""Looking forward to the Webinar"",
			""country"": ""US"",
			""create_time"": ""2019-02-26T23:01:16.899Z"",
			""custom_questions"": [
				{
					""title"": ""What do you hope to learn from this Webinar?"",
					""value"": ""Look forward to learning how you come up with new recipes and what other services you offer.""
				}
			],
			""email"": ""example@example.com"",
			""first_name"": ""Jill"",
			""id"": ""24000002122"",
			""industry"": ""Food"",
			""job_title"": ""Chef"",
			""join_url"": ""https://zoom.us/webinar/mywebinarissocooldighdghodghodg"",
			""last_name"": ""Chill"",
			""no_of_employees"": """",
			""org"": ""Cooking Org"",
			""phone"": ""5550100"",
			""purchasing_time_frame"": ""1-3 months"",
			""role_in_purchase_process"": ""Influencer"",
			""state"": ""CA"",
			""status"": ""approved"",
			""zip"": ""94045""
		}";

		#endregion

		[Fact]
		public void Parse_json()
		{
			// Arrange

			// Act
			var result = JsonSerializer.Deserialize<Registrant>(SINGLE_REGISTRANT_JSON, ZoomNetJsonFormatter.SerializerOptions);

			// Assert
			result.ShouldNotBeNull();
			result.City.ShouldBe("Mountain View");
			result.NumberOfEmployees.ShouldBe(NumberOfEmployees.Unknown);
			result.PurchasingTimeFrame.ShouldBe(PurchasingTimeFrame.Between_1_and_3_months);
			result.RoleInPurchaseProcess.ShouldBe(RoleInPurchaseProcess.Influencer);
			result.State.ShouldBe("CA");
			result.Status.ShouldBe(RegistrantStatus.Approved);
		}
	}
}
