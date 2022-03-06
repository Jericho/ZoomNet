using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Answer to a custom registration question.
	/// </summary>
	public class RegistrationAnswer
	{
		/// <summary>
		/// Gets or sets the title of the custom question.
		/// </summary>
		/// <value>
		/// The title.
		/// </value>
		[JsonPropertyName("title")]
		public string Title { get; set; }

		/// <summary>Gets or sets the response.</summary>
		/// <remarks>This has a limit of 128 characters.</remarks>
		/// <value>The response.</value>
		[JsonPropertyName("value")]
		public string Answer { get; set; }
	}
}
