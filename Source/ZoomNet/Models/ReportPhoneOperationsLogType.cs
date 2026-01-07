using System.Runtime.Serialization;

namespace ZoomNet.Models;

/// <summary>
/// Phone operation categories and their descriptions from Zoom Phone API.
/// </summary>
public enum ReportPhoneOperationsLogType
{
	/// <summary>All Operation Types.</summary>
	[EnumMember(Value = "all")]
	All,

	/// <summary>Approved account.</summary>
	[EnumMember(Value = "account_allow_list")]
	AccountAllowList,

	/// <summary>Blocked account.</summary>
	[EnumMember(Value = "account_blocked")]
	AccountBlocked,

	/// <summary>Auto receptionist.</summary>
	[EnumMember(Value = "auto_receptionist")]
	AutoReceptionist,

	/// <summary>Directory backup routing rules.</summary>
	[EnumMember(Value = "backup_routing_rule")]
	BackupRoutingRule,

	/// <summary>Call monitoring (barge/monitor/whisper).</summary>
	[EnumMember(Value = "barge_monitor_whisper")]
	BargeMonitorWhisper,

	/// <summary>Call forwarding.</summary>
	[EnumMember(Value = "call_forward")]
	CallForward,

	/// <summary>Call logs.</summary>
	[EnumMember(Value = "call_log")]
	CallLog,

	/// <summary>Call queues.</summary>
	[EnumMember(Value = "call_queue")]
	CallQueue,

	/// <summary>Call setting.</summary>
	[EnumMember(Value = "call_setting")]
	CallSetting,

	/// <summary>Calling plan.</summary>
	[EnumMember(Value = "calling_plan")]
	CallingPlan,

	/// <summary>Company information.</summary>
	[EnumMember(Value = "company_info")]
	CompanyInfo,

	/// <summary>Contact center.</summary>
	[EnumMember(Value = "contact_center")]
	ContactCenter,

	/// <summary>Domain of the contact center.</summary>
	[EnumMember(Value = "contact_center_domain")]
	ContactCenterDomain,

	/// <summary>CSV file imports.</summary>
	[EnumMember(Value = "csv_import")]
	CsvImport,

	/// <summary>Custom voicemail greeting.</summary>
	[EnumMember(Value = "custom_vm_greeting")]
	CustomVoicemailGreeting,

	/// <summary>Call delegation.</summary>
	[EnumMember(Value = "delegation")]
	Delegation,

	/// <summary>Zoom Phone device.</summary>
	[EnumMember(Value = "device")]
	Device,

	/// <summary>Dial-by-name directory.</summary>
	[EnumMember(Value = "dial_by_name")]
	DialByName,

	/// <summary>Emergency address.</summary>
	[EnumMember(Value = "emergency_address")]
	EmergencyAddress,

	/// <summary>Emergency calling.</summary>
	[EnumMember(Value = "emergency_calling")]
	EmergencyCalling,

	/// <summary>Emergency service.</summary>
	[EnumMember(Value = "emergency_service")]
	EmergencyService,

	/// <summary>Extension number.</summary>
	[EnumMember(Value = "extension")]
	Extension,

	/// <summary>Extension line keys.</summary>
	[EnumMember(Value = "extension_line_keys")]
	ExtensionLineKeys,

	/// <summary>Extension template.</summary>
	[EnumMember(Value = "extension_template")]
	ExtensionTemplate,

	/// <summary>External contacts.</summary>
	[EnumMember(Value = "external_contacts")]
	ExternalContacts,

	/// <summary>Holiday hours.</summary>
	[EnumMember(Value = "holiday_hours")]
	HolidayHours,

	/// <summary>Location.</summary>
	[EnumMember(Value = "location")]
	Location,

	/// <summary>Multiple sites.</summary>
	[EnumMember(Value = "multiple_site")]
	MultipleSite,

	/// <summary>Outbound number.</summary>
	[EnumMember(Value = "outbound_number")]
	OutboundNumber,

	/// <summary>Phone number.</summary>
	[EnumMember(Value = "phone_number")]
	PhoneNumber,

	/// <summary>Policy.</summary>
	[EnumMember(Value = "policy")]
	Policy,

	/// <summary>Provision templates.</summary>
	[EnumMember(Value = "provision_template")]
	ProvisionTemplate,

	/// <summary>Call recordings.</summary>
	[EnumMember(Value = "recording")]
	Recording,

	/// <summary>Shared line groups.</summary>
	[EnumMember(Value = "shared_line_group")]
	SharedLineGroup,

	/// <summary>SIP group.</summary>
	[EnumMember(Value = "sip_group")]
	SipGroup,

	/// <summary>SIP trunk group.</summary>
	[EnumMember(Value = "sip_trunk_group")]
	SipTrunkGroup,

	/// <summary>Spam.</summary>
	[EnumMember(Value = "spam")]
	Spam,

	/// <summary>User.</summary>
	[EnumMember(Value = "user")]
	User,

	/// <summary>Blocked user.</summary>
	[EnumMember(Value = "user_blocked")]
	UserBlocked,

	/// <summary>Voicemail.</summary>
	[EnumMember(Value = "voicemail")]
	Voicemail,

	/// <summary>Emergency number routing rule.</summary>
	[EnumMember(Value = "emergency_number_routing_rule")]
	EmergencyNumberRoutingRule,

	/// <summary>Account options.</summary>
	[EnumMember(Value = "account_options")]
	AccountOptions,

	/// <summary>Zoom Phone roles.</summary>
	[EnumMember(Value = "zoom_phone_role")]
	ZoomPhoneRole,

	/// <summary>Zoom Rooms PBX Support.</summary>
	[EnumMember(Value = "zoom_room")]
	ZoomRoom
}
