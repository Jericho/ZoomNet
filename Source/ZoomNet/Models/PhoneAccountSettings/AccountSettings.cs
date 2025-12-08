using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Zoom Account phone settings.
	/// </summary>
	public class AccountSettings : CommonAccountAndGroupSettings
	{
		/// <summary>
		/// Gets or sets settings that allow users to perform call control actions from authorized Zoom Marketplace apps.
		/// </summary>
		[JsonPropertyName("auto_call_from_third_party_apps")]
		public AutoCallFromThirdPartyAppsSettings AutoCallFromThirdPartyApps { get; set; }

		/// <summary>
		/// Gets or sets settings that allow Zoom to automatically delete data after the retention duration has elapsed.
		/// </summary>
		[JsonPropertyName("auto_delete_data_after_retention_duration")]
		public AutoDeleteDataSettings AutoDeleteDataAfterRetentionDuration { get; set; }

		/// <summary>
		/// Gets or sets settings that allow users to block and classify calls as threat.
		/// </summary>
		[JsonPropertyName("block_calls_as_threat")]
		public BlockCallsAsThreatSettings BlockCallsAsThreat { get; set; }

		/// <summary>
		/// Gets or sets settings that define opt-out reasons for call queues.
		/// </summary>
		[JsonPropertyName("call_queue_opt_out_reason")]
		public CallQueueOptOutReasonSettings CallQueueOptOutReason { get; set; }

		/// <summary>
		/// Gets or sets settings that allow Zoom Rooms to call external phone numbers based on the calling plans and other Zoom Phone policies.
		/// </summary>
		[JsonPropertyName("external_calling_on_zoom_room_common_area")]
		public ExternalCallingOnZoomRoomSettings ExternalCallingOnZoomRoomCommonArea { get; set; }

		/// <summary>
		/// Gets or sets settings that allow to set a port assignment range.
		/// </summary>
		[JsonPropertyName("override_default_port")]
		public OverrideDefaultPortSettings OverrideDefaultPort { get; set; }
	}
}
