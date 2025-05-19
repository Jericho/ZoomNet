using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{

	public class PhoneDetail
	{
		[JsonPropertyName("assignee")]
		public PhoneDetailAssignee Assignee { get; set; }

		[JsonPropertyName("capability")]
		public List<string> Capability { get; set; }

		[JsonPropertyName("carrier")]
		public PhoneDetailCarrier Carrier { get; set; }

		[JsonPropertyName("display_name")]
		public string DisplayName { get; set; }

		[JsonPropertyName("emergency_address")]
		public EmergencyAddress EmergencyAddress { get; set; }

		[JsonPropertyName("emergency_address_status")]
		public int? EmergencyAddressStatus { get; set; }

		[JsonPropertyName("emergency_address_update_time")]
		public string EmergencyAddressUpdateTime { get; set; }

		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("location")]
		public string Location { get; set; }

		[JsonPropertyName("number")]
		public string Number { get; set; }

		[JsonPropertyName("number_type")]
		public string NumberType { get; set; }

		[JsonPropertyName("sip_group")]
		public PhoneDetailSipGroup SipGroup { get; set; }

		[JsonPropertyName("site")]
		public Site Site { get; set; }

		[JsonPropertyName("source")]
		public string Source { get; set; }

		[JsonPropertyName("status")]
		public string Status { get; set; }
	}

	public class PhoneDetailAssignee
	{
		[JsonPropertyName("extension_number")]
		public long? ExtensionNumber { get; set; }

		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("type")]
		public string Type { get; set; }
	}

	public class PhoneDetailCarrier
	{
		[JsonPropertyName("code")]
		public int? Code { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }
	}

	public class PhoneDetailSipGroup
	{
		[JsonPropertyName("display_name")]
		public string DisplayName { get; set; }

		[JsonPropertyName("id")]
		public string Id { get; set; }
	}

}
