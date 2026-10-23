using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>Pre assigned breakout room.</summary>
	public class PreAssignedBreakoutRoom
	{
		/// <summary>Gets or sets the name of the breakout room.</summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the email addresses of the participants who are to be assigned to the breakout room.</summary>
		[JsonPropertyName("participants")]
		public string[] Participants { get; set; }
	}
}
