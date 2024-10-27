using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.Json.Serialization;
using ZoomNet.Json;

namespace ZoomNet.Models.ChatbotMessage;

/// <summary>
/// A message section that groups together multiple messages with a common sidebar.
/// </summary>
public class ChatbotSection : IChatbotBody, IChatbotValidate
{
	/// <summary>
	/// Gets or sets the color of the sidebar for this section.
	/// </summary>
	[JsonPropertyName("sidebar_color")]
	[JsonConverter(typeof(NullableHexColorConverter))]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public Color? SidebarColor { get; set; }

	/// <summary>
	/// Gets or sets the associated messages for this section.
	/// </summary>
	[JsonPropertyName("sections")]
	public ICollection<IChatbotSection> Sections { get; set; }

	/// <summary>
	/// Gets or sets the footer for this section.
	/// </summary>
	[JsonPropertyName("footer")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string Footer { get; set; }

	/// <summary>
	/// Gets or sets the URL of the footer icon for this section.
	/// </summary>
	[JsonPropertyName("footer_icon")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string FooterIcon { get; set; }

	/// <summary>
	/// Gets or sets a timestamp to display in the footer. Unix timestamp format or use <see cref="TimeStampFromDateTime"/>.
	/// </summary>
	/// <remarks>
	/// There may be a timezone conversion issue on Zoom's side.
	/// The value here is one hour off the time displayed on the Zoom message.
	/// </remarks>
	[JsonPropertyName("ts")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public long? TimeStamp { get; set; }

	/// <summary>
	/// Sets the timestamp from a <see cref="DateTime"/> value.
	/// </summary>
	/// <remarks>
	/// There may be a timezone conversion issue on Zoom's side.
	/// The value here is one hour off the time displayed on the Zoom message.
	/// </remarks>
	[JsonIgnore]
	public DateTime TimeStampFromDateTime
	{
		set => TimeStamp = value.ToUnixTime(Internal.UnixTimePrecision.Milliseconds);
	}

	/// <inheritdoc/>
	public void Validate(bool enableMarkdownSupport)
	{
		foreach (var section in Sections.OfType<IChatbotValidate>())
		{
			section.Validate(enableMarkdownSupport);
		}
	}
}
