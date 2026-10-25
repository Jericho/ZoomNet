using System.Text.Json.Serialization;

namespace ZoomNet.Models;

/// <summary>Host report item.</summary>
public record ReportHost
{
	/// <summary>Gets or sets the custom attributes.</summary>
	[JsonPropertyName("custom_attributes")]
	public CustomAttribute[] CustomAttributes { get; init; }

	/// <summary>Gets or sets the department.</summary>
	[JsonPropertyName("dept")]
	public string Department { get; init; }

	/// <summary>Gets or sets a valid email address.</summary>
	[JsonPropertyName("email")]
	public string Email { get; init; }

	/// <summary>Gets or sets the user id.</summary>
	[JsonPropertyName("id")]
	public string Id { get; init; }

	/// <summary>Gets or sets the number of meeting minutes for user.</summary>
	[JsonPropertyName("meeting_minutes")]
	public int TotalMeetingMinutes { get; init; }

	/// <summary>Gets or sets the number of meetings for user.</summary>
	[JsonPropertyName("meetings")]
	public int TotalMeetings { get; init; }

	/// <summary>Gets or sets the number of participants in meetings for user.</summary>
	[JsonPropertyName("participants")]
	public int TotalParticipants { get; init; }

	/// <summary>Gets or sets the type.</summary>
	[JsonPropertyName("type")]
	public UserType Type { get; init; }

	/// <summary>Gets or sets display name.</summary>
	[JsonPropertyName("user_name")]
	public string DisplayName { get; init; }
}
