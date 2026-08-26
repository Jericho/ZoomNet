using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>A user call log item.</summary>
	/// <seealso cref="ZoomNet.Models.CallLog" />
	public class UserCallLog : CallLog
	{
		/// <summary>Gets or sets the "accepted by" information.</summary>
		[JsonPropertyName("accepted_by")]
		public CallLogTransferInfo AcceptedBy { get; set; }

		/// <summary>Gets or sets the "forwarded from" information.</summary>
		[JsonPropertyName("forwarded_by")]
		public CallLogTransferInfo ForwardedBy { get; set; }

		/// <summary>Gets or sets the "forwarded to" information.</summary>
		[JsonPropertyName("forwarded_to")]
		public CallLogTransferInfo ForwardedTo { get; set; }

		/// <summary>Gets or sets the "outgoing by" information.</summary>
		[JsonPropertyName("outgoing_by")]
		public CallLogTransferInfo OutgoingBy { get; set; }
	}
}
