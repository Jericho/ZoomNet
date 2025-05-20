using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
    /// <summary>
    /// Represents the details of a phone in the Zoom system.
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <item><term>assignee</term><description>The assignee information for the phone.</description></item>
    /// <item><term>capability</term><description>The list of capabilities for the phone (e.g., SMS, voice).</description></item>
    /// <item><term>carrier</term><description>The carrier information for the phone.</description></item>
    /// <item><term>display_name</term><description>The display name of the phone.</description></item>
    /// <item><term>emergency_address</term><description>The emergency address associated with the phone.</description></item>
    /// <item><term>emergency_address_status</term><description>The status of the emergency address.</description></item>
    /// <item><term>emergency_address_update_time</term><description>The last update time of the emergency address.</description></item>
    /// <item><term>id</term><description>The unique identifier of the phone.</description></item>
    /// <item><term>location</term><description>The location of the phone.</description></item>
    /// <item><term>number</term><description>The phone number.</description></item>
    /// <item><term>number_type</term><description>The type of the phone number.</description></item>
    /// <item><term>sip_group</term><description>The SIP group information for the phone.</description></item>
    /// <item><term>site</term><description>The site information associated with the phone.</description></item>
    /// <item><term>source</term><description>The source of the phone number.</description></item>
    /// <item><term>status</term><description>The status of the phone.</description></item>
    /// </list>
    /// </remarks>
    public class PhoneDetail
    {
        /// <summary>
        /// Gets or sets the assignee information for the phone.
        /// </summary>
        [JsonPropertyName("assignee")]
        public PhoneDetailAssignee Assignee { get; set; }

        /// <summary>
        /// Gets or sets the list of capabilities for the phone (e.g., SMS, voice).
        /// </summary>
        [JsonPropertyName("capability")]
        public List<string> Capability { get; set; }

        /// <summary>
        /// Gets or sets the carrier information for the phone.
        /// </summary>
        [JsonPropertyName("carrier")]
        public PhoneDetailCarrier Carrier { get; set; }

        /// <summary>
        /// Gets or sets the display name of the phone.
        /// </summary>
        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the emergency address associated with the phone.
        /// </summary>
        [JsonPropertyName("emergency_address")]
        public EmergencyAddress EmergencyAddress { get; set; }

        /// <summary>
        /// Gets or sets the status of the emergency address.
        /// </summary>
        [JsonPropertyName("emergency_address_status")]
        public int? EmergencyAddressStatus { get; set; }

        /// <summary>
        /// Gets or sets the last update time of the emergency address.
        /// </summary>
        [JsonPropertyName("emergency_address_update_time")]
        public string EmergencyAddressUpdateTime { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the phone.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the location of the phone.
        /// </summary>
        [JsonPropertyName("location")]
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        [JsonPropertyName("number")]
        public string Number { get; set; }

        /// <summary>
        /// Gets or sets the type of the phone number.
        /// </summary>
        [JsonPropertyName("number_type")]
        public string NumberType { get; set; }

        /// <summary>
        /// Gets or sets the SIP group information for the phone.
        /// </summary>
        [JsonPropertyName("sip_group")]
        public PhoneDetailSipGroup SipGroup { get; set; }

        /// <summary>
        /// Gets or sets the site information associated with the phone.
        /// </summary>
        [JsonPropertyName("site")]
        public Site Site { get; set; }

        /// <summary>
        /// Gets or sets the source of the phone number.
        /// </summary>
        [JsonPropertyName("source")]
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the status of the phone.
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}