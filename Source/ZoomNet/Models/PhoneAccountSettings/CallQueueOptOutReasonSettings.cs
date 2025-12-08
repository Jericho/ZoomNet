using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Settings that define opt-out reasons for call queues.
	/// When enabled, call queue members will need to select an opt-out reason when they turn off the receive queue call feature.
	/// </summary>
	public class CallQueueOptOutReasonSettings : SettingsGroupBase
	{
		/// <summary>
		/// Gets or sets the opt-out reasons for call queues.
		/// </summary>
		[JsonPropertyName("call_queue_opt_out_reasons_list")]
		public CallQueueOptOutReason[] OptOutReasons { get; set; }
	}
}
