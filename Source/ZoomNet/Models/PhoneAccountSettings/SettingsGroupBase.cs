using System.Text.Json.Serialization;

namespace ZoomNet.Models.PhoneAccountSettings
{
	/// <summary>
	/// Represents properties that are applicable to all account or group phone settings.
	/// </summary>
	public abstract class SettingsGroupBase
	{
		/// <summary>
		/// Gets or sets a value indicating whether related settings are enabled.
		/// </summary>
		[JsonPropertyName("enable")]
		public bool Enabled { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the senior administrator allows users to modify the current settings.
		/// </summary>
		[JsonPropertyName("locked")]
		public bool? Locked { get; set; }

		/// <summary>
		/// Gets or sets administrator level which prohibits the modification of the current settings.
		/// </summary>
		/// <remarks>
		/// Value is provided only if <see cref="Locked"/> is true.
		/// </remarks>
		[JsonPropertyName("locked_by")]
		public AdministratorLevel? LockedBy { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the current settings have been modified.
		/// </summary>
		/// <remarks>
		/// Applicable only to group phone settings.
		/// </remarks>
		[JsonPropertyName("modified")]
		public bool? Modified { get; set; }
	}
}
