using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A Contact Center skill.
	/// </summary>
	public class ContactCenterSkill
	{
		/// <summary>Gets or sets the date and time when the queue was last modified.</summary>
		[JsonPropertyName("last_modified_time")]
		public DateTime ModifiedOn { get; set; }

		/// <summary>Gets or sets the maximum proficiency level in a skill.</summary>
		[JsonPropertyName("max_proficiency_level")]
		public int MaxProficiencyLevel { get; set; }

		/// <summary>Gets or sets the ID of the user that last modified the queue.</summary>
		[JsonPropertyName("modified_by")]
		public string ModifiedBy { get; set; }

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

		/// <summary>Gets or sets the total number of users of the skill.</summary>
		[JsonPropertyName("total_users")]
		public int UserCount { get; set; }
	}
}
