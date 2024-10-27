using System.Text.Json.Serialization;

namespace ZoomNet.Models;

/// <summary>
/// Phone call user profile information.
/// </summary>
public class PhoneCallUserProfile
{
	/// <summary>
	/// Gets or sets the calling plans.
	/// </summary>
	[JsonPropertyName("calling_plans")]
	public CallingPlan[] CallingPlans { get; set; }

	/// <summary>
	/// Gets or sets the cost center.
	/// </summary>
	[JsonPropertyName("cost_center")]
	public string CostCenter { get; set; }

	/// <summary>
	/// Gets or sets the department of the object.
	/// </summary>
	[JsonPropertyName("department")]
	public string Department { get; set; }

	/// <summary>
	/// Gets or sets the email address.
	/// </summary>
	[JsonPropertyName("email")]
	public string Email { get; set; }

	/// <summary>
	/// Gets or sets the emergency address.
	/// </summary>
	[JsonPropertyName("emergency_address")]
	public EmergencyAddress EmergencyAddress { get; set; }

	/// <summary>
	/// Gets or sets the extension ID.
	/// </summary>
	[JsonPropertyName("extension_id")]
	public string ExtensionId { get; set; }

	/// <summary>
	/// Gets or sets the extension number.
	/// </summary>
	[JsonPropertyName("extension_number")]
	public int ExtensionNumber { get; set; }

	/// <summary>
	/// Gets or sets the Zoom user ID.
	/// </summary>
	[JsonPropertyName("id")]
	public string Id { get; set; }

	/// <summary>
	/// Gets or sets the phone numbers.
	/// </summary>
	[JsonPropertyName("phone_numbers")]
	public PhoneCallPhoneNumber[] PhoneNumbers { get; set; }

	/// <summary>
	/// Gets or sets the Zoom phone user id.
	/// </summary>
	[JsonPropertyName("phone_user_id")]
	public string PhoneUserId { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether the user is a site admin.
	/// </summary>
	[JsonPropertyName("site_admin")]
	public bool SiteAdmin { get; set; }

	/// <summary>
	/// Gets or sets the site id.
	/// </summary>
	[JsonPropertyName("site_id")]
	public string SiteId { get; set; }

	/// <summary>
	/// Gets or sets the status of the user.
	/// </summary>
	[JsonPropertyName("status")]
	public PhoneCallUserStatus Status { get; set; }
}
