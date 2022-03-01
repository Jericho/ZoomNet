using Newtonsoft.Json;

namespace ZoomNet.Models
{
	/// <summary>
	/// Password requirements.
	/// </summary>
	public class PasswordRequirements
	{
		/// <summary>
		/// Gets or sets the minimum required length.
		/// </summary>
		[JsonProperty(PropertyName = "length")]
		public int MinimumLength { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether password must contain at least one alphabetical letter.
		/// </summary>
		[JsonProperty(PropertyName = "have_letter")]
		public bool AtLeastOneLetter { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether password must contain at least one number.
		/// </summary>
		[JsonProperty(PropertyName = "have_number")]
		public bool AtLeastOneNumber { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether password must contain at least one special character.
		/// </summary>
		[JsonProperty(PropertyName = "have_special_character")]
		public bool AtLeastOneSpecialCharacter { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether password must contain only numeric characters.
		/// </summary>
		[JsonProperty(PropertyName = "only_allow_numeric")]
		public bool MustOnlyContainNumbers { get; set; }
	}
}
