using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A user call log item.
	/// </summary>
	/// <seealso cref="ZoomNet.Models.CallLog" />
	public class UserCallLog : CallLog
	{
		/// <summary>
		/// Gets or sets the "accepted by" information.
		/// </summary>
		/// <value>Indicates who accepted the call.</value>
		[JsonPropertyName("accepted_by")]
		public CallLogTransferInfo AcceptedBy { get; set; }

		/// <summary>
		/// Gets or sets the "forwarded from" information.
		/// </summary>
		/// <value>Indicates where the call was forwarded from.</value>
		[JsonPropertyName("forwarded_by")]
		public CallLogTransferInfo ForwardedBy { get; set; }

		/// <summary>
		/// Gets or sets the "forwarded to" information.
		/// </summary>
		/// <value>Indicates who the call was forwarded to.</value>
		[JsonPropertyName("forwarded_to")]
		public CallLogTransferInfo ForwardedTo { get; set; }

		/// <summary>
		/// Gets or sets the "outgoing by" information.
		/// </summary>
		/// <value>Call "outgoing by" information.</value>
		[JsonPropertyName("outgoing_by")]
		public CallLogTransferInfo OutgoingBy { get; set; }
	}
}
