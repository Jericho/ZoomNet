using System;
using System.Text.Json;
using ZoomNet.Models;

namespace ZoomNet.Json
{
	/// <summary>
	/// Converts a <see cref="ContactCenterSystemStatusConverter"/> to or from JSON.
	/// </summary>
	/// <seealso cref="ZoomNetJsonConverter{T}" />
	internal class ContactCenterSystemStatusConverter : ZoomNetJsonConverter<ContactCenterSystemStatus>
	{
		public override ContactCenterSystemStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var doc = JsonDocument.ParseValue(ref reader);
			var rootElement = doc.RootElement;

			var statusCategoryName = rootElement.GetPropertyValue("status_category", string.Empty);

			if (!statusCategoryName.TryToEnum(out ContactCenterAgentStatusCategory statusCategory))
			{
				throw new ArgumentException($"{statusCategoryName} is not a recognized status category.", nameof(statusCategoryName));
			}

			switch (statusCategory)
			{
				case ContactCenterAgentStatusCategory.SystemStatus:
					return rootElement.ToObject<ContactCenterAgentStatus>(options);
				case ContactCenterAgentStatusCategory.NotReadyReason:
					return rootElement.ToObject<ContactCenterAgentNotReadyReason>(options);
				default:
					throw new JsonException($"{statusCategory} is an unknown status category");
			}
		}
	}
}
