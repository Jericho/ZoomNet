using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A Contect Center userskill.
	/// </summary>
	public class ContactCenterUserSkill : ContactCenterSkill
	{
		/// <summary>Gets or sets the user proficiency level in a skill.</summary>
		[JsonPropertyName("user_proficiency_level")]
		public int UserProficiencyLevel { get; set; }
	}
}
