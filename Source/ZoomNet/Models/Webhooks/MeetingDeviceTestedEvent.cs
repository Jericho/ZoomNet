using System.Text.Json.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered every time a user completes a connected device test.
	/// When a user joins a meeting, they choose to test their connected camera, microphone, and speaker devices.
	/// The Zoom client performs a test for the selected device and prompts a user to confirm whether the device works.
	/// </summary>
	public class MeetingDeviceTestedEvent : Event
	{
		/// <summary>
		/// Gets or sets the account id of the user who created the meeting.
		/// </summary>
		[JsonPropertyName("account_id")]
		public string AccountId { get; set; }

		/// <summary>
		/// Gets or sets the meeting information.
		/// </summary>
		[JsonPropertyName("object")]
		public MeetingBasicInfo Meeting { get; set; }

		/// <summary>
		/// Gets or sets information about the meeting's devices test results.
		/// </summary>
		public DeviceTestResult TestResult { get; set; }
	}
}
