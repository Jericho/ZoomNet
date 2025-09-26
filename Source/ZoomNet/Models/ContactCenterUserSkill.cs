using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A Contect Center userskill.
	/// </summary>
	public class ContactCenterUserSkill
	{
		/// <summary>Gets or sets the maximum proficiency level in a skill.</summary>
		[JsonPropertyName("max_proficiency_level")]
		public int MaxProficiencyLevel { get; set; }

		/// <summary>Gets or sets the category ID.</summary>
		[JsonPropertyName("skill_category_id")]
		public string CategoryId { get; set; }

		/// <summary>Gets or sets the category name.</summary>
		[JsonPropertyName("skill_category_name")]
		public string CategoryName { get; set; }

		/// <summary>Gets or sets the unique identifier.</summary>
		[JsonPropertyName("skill_id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the name of the skill.</summary>
		[JsonPropertyName("skill_name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the skill type.</summary>
		[JsonPropertyName("skill_type")]
		public ContactCenterSkillType Type { get; set; }

		/// <summary>Gets or sets the user proficiency level in a skill.</summary>
		[JsonPropertyName("user_proficiency_level")]
		public int UserProficiencyLevel { get; set; }
	}
}
