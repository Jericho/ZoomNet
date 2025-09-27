using System;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// A Contact Center skill category.
	/// </summary>
	public class ContactCenterSkillCategory
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

		/// <summary>Gets or sets the description.</summary>
		[JsonPropertyName("skill_category_description")]
		public string Description { get; set; }

		/// <summary>Gets or sets the unique identifier.</summary>
		[JsonPropertyName("skill_category_id")]
		public string Id { get; set; }

		/// <summary>Gets or sets the name.</summary>
		[JsonPropertyName("skill_category_name")]
		public string Name { get; set; }

		/// <summary>Gets or sets the skill type.</summary>
		[JsonPropertyName("skill_type")]
		public ContactCenterSkillType Type { get; set; }
	}
}
