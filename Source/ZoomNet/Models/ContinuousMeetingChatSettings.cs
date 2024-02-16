using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ZoomNet.Models
{
	/// <summary>
	/// Information about the Enable continuous meeting chat feature.
	 /// </summary>
	public class ContinuousMeetingChatSettings
	{
		/// <summary>
		/// Gets or sets whether to enable the Enable continuous meeting chat setting.
		/// </summary>
		[JsonPropertyName("enable")]
		public bool? Enable { get; set; }

		/// <summary>
		/// Gets or sets whether to enable the Automatically add invited external users setting.
		/// </summary>
		[JsonPropertyName("auto_add_invited_external_users")]
		public bool? AutoAddInvitedExternalUsers { get; set; }

		/// <summary>
		/// Gets or sets the channel's ID.
		/// </summary>
		[JsonPropertyName("channel_id")]
		public string ChannelId { get; set; }
	}
}
