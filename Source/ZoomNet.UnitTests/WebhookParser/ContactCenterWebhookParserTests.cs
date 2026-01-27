using Shouldly;
using System;
using Xunit;
using ZoomNet.Models;
using ZoomNet.Models.Webhooks;
using ZoomNet.UnitTests.Properties;

namespace ZoomNet.UnitTests.WebhookParser
{
	/// <summary>
	/// Unit tests that verify contact center webhook events parsing.
	/// </summary>
	public partial class WebhookParserTests
	{
		#region private fields

		private const string AssetId = "WkdWelkzSnBjSFJwYjI0PQ11";
		private const string AssetName = "My Asset";
		private const string AssetDescription = "This is an example asset.";
		private const string AssetModifiedBy = "IGTRVt3gQ2i-WjoUIjeZxw";
		private const string AssetCategoryId = "bnGEj1yrRb6qlNMvyMK08g";
		private const string AssetItemId = "beYjXoDOS_eV1QmTpj63PQ";
		private const string AssetItemName = "My Asset Item";
		private static readonly DateTime assetTimestamp = new DateTime(2022, 2, 15, 9, 27, 15, 987);

		#endregion

		#region tests

		[Fact]
		public void ContactCenterAssetCreated()
		{
			var parsedEvent = ParseWebhookEvent<ContactCenterAssetCreatedEvent>(WebhooksDataResource.contact_center_asset_created);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.ContactCenterAssetCreated);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);

			VerifyWebhookAssetCreated(parsedEvent.Asset);
		}

		[Fact]
		public void ContactCenterAssetDeleted()
		{
			var parsedEvent = ParseWebhookEvent<ContactCenterAssetDeletedEvent>(WebhooksDataResource.contact_center_asset_deleted);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.ContactCenterAssetDeleted);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);

			VerifyWebhookAssetDeleted(parsedEvent.Asset);
		}

		[Fact]
		public void ContactCenterAssetUpdated()
		{
			var parsedEvent = ParseWebhookEvent<ContactCenterAssetUpdatedEvent>(WebhooksDataResource.contact_center_asset_updated);

			parsedEvent.EventType.ShouldBe(ZoomNet.Models.Webhooks.EventType.ContactCenterAssetUpdated);
			parsedEvent.Timestamp.ShouldBe(eventTimestamp);
			parsedEvent.AccountId.ShouldBe(AccountId);

			VerifyWebhookAssetUpdated(parsedEvent.Asset);
		}

		#endregion

		#region private methods

		/// <summary>
		/// Verify <see cref="WebhookAssetCreated"/> properties.
		/// </summary>
		private void VerifyWebhookAssetCreated(WebhookAssetCreated info)
		{
			info.ShouldNotBeNull();
			info.Id.ShouldBe(AssetId);
			info.Name.ShouldBe(AssetName);
			info.Description.ShouldBe(AssetDescription);
			info.Type.ShouldBe(AssetType.Audio);
			info.LanguageCode.ShouldBe(Language.English_US);
			info.CategoryId.ShouldBe(AssetCategoryId);
			info.CategoryName.ShouldBe("My Category");
			info.CreatedOn.ShouldBe(assetTimestamp);
			info.ModifiedBy.ShouldBe(AssetModifiedBy);

			info.Items.ShouldNotBeNull();
			info.Items.ShouldHaveSingleItem();

			VerifyAssetItem(info.Items[0]);
		}

		/// <summary>
		/// Verify <see cref="WebhookAssetDeleted"/> properties.
		/// </summary>
		private void VerifyWebhookAssetDeleted(WebhookAssetDeleted info)
		{
			info.ShouldNotBeNull();
			info.Id.ShouldBe(AssetId);
			info.Name.ShouldBe(AssetName);
			info.DeletedOn.ShouldBe(assetTimestamp);
			info.ModifiedBy.ShouldBe(AssetModifiedBy);
			info.Archived.ShouldBeTrue();
			info.ArchivedTime.ShouldBe(new DateTime(2025, 6, 26, 9, 27, 15));
		}

		/// <summary>
		/// Verify <see cref="WebhookAssetUpdated"/> properties.
		/// </summary>
		private void VerifyWebhookAssetUpdated(WebhookAssetUpdated info)
		{
			info.ShouldNotBeNull();
			info.Id.ShouldBe(AssetId);
			info.UpdatedOn.ShouldBe(assetTimestamp);
			info.ModifiedBy.ShouldBe(AssetModifiedBy);

			info.Updates.ShouldNotBeNull();
			info.Updates.Name.ShouldBe(AssetName);
			info.Updates.Description.ShouldBe(AssetDescription);
			info.Updates.CategoryId.ShouldBe(AssetCategoryId);
			info.Updates.IsRestored.ShouldBe(true);

			info.Updates.AddedItems.ShouldNotBeNull();
			info.Updates.AddedItems.ShouldHaveSingleItem();

			VerifyAssetItem(info.Updates.AddedItems[0]);

			info.Updates.UpdatedItems.ShouldNotBeNull();
			info.Updates.UpdatedItems.ShouldHaveSingleItem();

			VerifyAssetItem(info.Updates.UpdatedItems[0]);

			info.Updates.DeletedItems.ShouldNotBeNull();
			info.Updates.DeletedItems.ShouldHaveSingleItem();

			info.Updates.DeletedItems[0].Id.ShouldBe(AssetItemId);
			info.Updates.DeletedItems[0].Name.ShouldBe(AssetItemName);
		}

		/// <summary>
		/// Verify <see cref="AssetItem"/> properties.
		/// </summary>
		private void VerifyAssetItem(AssetItem item)
		{
			item.Id.ShouldBe(AssetItemId);
			item.Name.ShouldBe(AssetItemName);
			item.Language.ShouldBe(Language.English_US);
			item.FileUrl.ShouldBe("https://file.zoom.us/example.mp3");
			item.Content.ShouldBe("My Asset Item Content");
			item.Voice.ShouldBe("Joanna");
			item.IsDefault.ShouldBeTrue();
		}

		#endregion
	}
}
