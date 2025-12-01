using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Call transfer information as provided in webhook events.
	/// </summary>
	public class WebhookCallTransferInfo
	{
		/// <summary>
		/// Gets or sets call's unique id.
		/// </summary>
		[JsonPropertyName("call_id")]
		public string CallId { get; set; }

		/// <summary>
		/// Gets or sets date and time related to call transfer.
		/// </summary>
		/// <remarks>
		/// Depending on the webhook event this can be a timestamp when the call transfer was initiated or completed.
		/// </remarks>
		[JsonPropertyName("date_time")]
		public DateTime Timestamp { get; set; }

		/// <summary>
		/// Gets or sets call transfer destination phone number.
		/// </summary>
		[JsonPropertyName("transfer_phone_number")]
		public string TransferPhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets the transfer account code.
		/// </summary>
		[JsonPropertyName("transfer_account_code")]
		public string TransferAccountCode { get; set; }

		/// <summary>
		/// Gets or sets the call transfer operation failure reason.
		/// </summary>
		/// <remarks>
		/// Empty string indicates successful operation.
		/// </remarks>
		[JsonPropertyName("failure_reason")]
		public string FailureReason { get; set; }

		/// <summary>
		/// Gets or sets the call transfer owner.
		/// </summary>
		[JsonPropertyName("owner")]
		public CallLogOwnerInfo Owner { get; set; }
	}
}
