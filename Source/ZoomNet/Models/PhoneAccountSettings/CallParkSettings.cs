using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Settings that allow calls placed on hold to be resumed from another location using a retrieval code.
	/// </summary>
	public class CallParkSettings : SettingsGroupBase
	{
		/// <summary>
		/// Gets or sets the action if a parked call is not picked up.
		/// </summary>
		[JsonPropertyName("call_not_picked_up_action")]
		public CallNotPickedUpAction? CallNotPickedUpAction { get; set; }

		/// <summary>
		/// Gets or sets a time limit for parked calls in minutes.
		/// After the expiration period ends, the retrieval code is no longer valid and a new code will be generated.
		/// </summary>
		[JsonPropertyName("expiration_period")]
		public int? ExpirationPeriod { get; set; }

		/// <summary>
		/// Gets or sets the extension forwarding information.
		/// </summary>
		[JsonPropertyName("forward_to")]
		public ForwardingExtension ForwardTo { get; set; }

		/// <summary>
		/// Gets or sets how the parked calls are assigned to a BLF (Busy Lamp Field) key.
		/// </summary>
		/// <remarks>
		/// This field is only used in the new policy framework.
		/// </remarks>
		[JsonPropertyName("sequence")]
		public ParkedCallsAssignmentSequence? Sequence { get; set; }
	}
}
