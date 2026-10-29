using Shouldly;
using System;
using Xunit;
using ZoomNet.Models;
using ZoomNet.Models.Webhooks;
using ZoomNet.UnitTests.Properties;

namespace ZoomNet.UnitTests.WebhookParser
{
	/// <summary>
	/// Unit tests that verify user TSP webhook events parsing.
	/// </summary>
	public partial class WebhookParserTests
	{
		#region tests

		[Fact]
		public void UserTspCreated()
		{
			var parsedEvent = ParseWebhookEvent<UserTspCreatedEvent>(WebhooksResource.user_tsp_created);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.UserTspCreated);

			VerifyUserTspEvent(parsedEvent);
		}

		[Fact]
		public void UserTspDeleted()
		{
			var parsedEvent = ParseWebhookEvent<UserTspDeletedEvent>(WebhooksResource.user_tsp_deleted);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.UserTspDeleted);

			VerifyUserTspEvent(parsedEvent);
		}

		[Fact]
		public void UserTspUpdated()
		{
			var parsedEvent = ParseWebhookEvent<UserTspUpdatedEvent>(WebhooksResource.user_tsp_updated);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.UserTspUpdated);

			VerifyUserTspEvent(parsedEvent, dialInNumbersCount: 1, conferenceCode: "0125", leaderPin: "11189898", accountId: null);

			VerifyTspAccount(parsedEvent.OldAccount, accountId: null, conferenceCode: "0130", leaderPin: "11188124", tspBridge: string.Empty, dialInNumbersCount: 1, singleDialInCode: "2", singleDialInCountryLabel: null, singleDialInNumber: "+355 18899996875", singleDialInType: PhoneNumberType.Toll);
		}

		#endregion

		#region private methods

		/// <summary>
		/// Verify <see cref="UserTspEvent"/> properties.
		/// </summary>
		private static void VerifyUserTspEvent(
			UserTspEvent parsedEvent,
			int dialInNumbersCount = 3,
			string conferenceCode = "123456789",
			string leaderPin = "123456",
			string accountId = "2")
		{
			parsedEvent.Timestamp.ShouldBe(new DateTime(2009, 2, 13, 23, 13, 9, 900, DateTimeKind.Utc));
			parsedEvent.AccountId.ShouldBe("Mtgfdgfgd2lxiA");
			parsedEvent.Operator.ShouldBe(OperatorEmail);
			parsedEvent.OperatorId.ShouldBe("DYdgdg64567g8w");

			VerifyUserTspAccount(parsedEvent.UserTspAccount, dialInNumbersCount, conferenceCode, leaderPin, accountId);
		}

		/// <summary>
		/// Verify <see cref="UserTspAccount"/> properties.
		/// </summary>
		private static void VerifyUserTspAccount(UserTspAccount userTspAccount, int dialInNumbersCount, string conferenceCode, string leaderPin, string accountId)
		{
			userTspAccount.ShouldNotBeNull();
			userTspAccount.Email.ShouldBe(UserEmail);
			userTspAccount.Id.ShouldBe("DYbbbbf7dPkkg8w");

			VerifyTspAccount(userTspAccount.Account, accountId, conferenceCode, leaderPin, tspBridge: dialInNumbersCount == 1 ? "US_TSP_TB" : null, dialInNumbersCount: dialInNumbersCount);
		}

		/// <summary>
		/// Verify <see cref="TspAccount"/> properties.
		/// </summary>
		private static void VerifyTspAccount(
			TspAccount account,
			string accountId = "2",
			string conferenceCode = "123456789",
			string leaderPin = "123456",
			string tspBridge = null,
			int dialInNumbersCount = 3,
			string singleDialInCode = "1",
			string singleDialInCountryLabel = null,
			string singleDialInNumber = "+1 1000200200",
			PhoneNumberType singleDialInType = PhoneNumberType.TollFree)
		{
			account.ShouldNotBeNull();
			account.Id.ShouldBe(accountId);
			account.ConferenceCode.ShouldBe(conferenceCode);
			account.LeaderPin.ShouldBe(leaderPin);
			account.TspBridge.ShouldBe(tspBridge);

			account.DialInNumbers.ShouldNotBeNull();
			account.DialInNumbers.Length.ShouldBe(dialInNumbersCount);

			if (dialInNumbersCount == 3)
			{
				VerifyDialInNumber(account.DialInNumbers[0], "1", null, "123456789", PhoneNumberType.Toll);
				VerifyDialInNumber(account.DialInNumbers[1], "93", null, "980000021", PhoneNumberType.TollFree);
				VerifyDialInNumber(account.DialInNumbers[2], "1", null, "2222456532", PhoneNumberType.MediaLink);
			}

			if (dialInNumbersCount == 1)
			{
				VerifyDialInNumber(account.DialInNumbers[0], singleDialInCode, singleDialInCountryLabel, singleDialInNumber, singleDialInType);
			}
		}

		/// <summary>
		/// Verify <see cref="DialInNumber"/> properties.
		/// </summary>
		private static void VerifyDialInNumber(
			DialInNumber dialInNumber,
			string code,
			string countryLabel,
			string number,
			PhoneNumberType type)
		{
			dialInNumber.ShouldNotBeNull();
			dialInNumber.CountryCode.ShouldBe(code);
			dialInNumber.CountryLabel.ShouldBe(countryLabel);
			dialInNumber.Number.ShouldBe(number);
			dialInNumber.Type.ShouldBe(type);
		}

		#endregion
	}
}
