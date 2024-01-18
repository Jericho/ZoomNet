namespace ZoomNet.Models;

/// <summary>
/// Represents the type of calling plan.
/// </summary>
public enum CallingPlanType
{
	/// <summary>
	/// No feature package.
	/// </summary>
	NO_FEATURE_PACKAGE = 1,

	/// <summary>
	/// International toll number.
	/// </summary>
	INTERNATIONAL_TOLL_NUMBER = 3,

	/// <summary>
	/// International toll free number.
	/// </summary>
	INTERNATIONAL_TOLL_FREE_NUMBER = 4,

	/// <summary>
	/// Bring your own carrier number.
	/// </summary>
	BYOC_NUMBER = 5,

	/// <summary>
	/// BETA number.
	/// </summary>
	BETA_NUMBER = 6,

	/// <summary>
	/// Metered plan in California.
	/// </summary>
	METERED_PLAN_US_CA = 100,

	/// <summary>
	/// Metered plan in Australia/New Zealand.
	/// </summary>
	METERED_PLAN_AU_NZ = 101,

	/// <summary>
	/// Metered plan in Great Britian/Ireland.
	/// </summary>
	METERED_PLAN_GB_IE = 102,

	/// <summary>
	/// Metered EURA.
	/// </summary>
	METERED_EURA = 103,

	/// <summary>
	/// Metered EURB.
	/// </summary>
	METERED_EURB = 104,

	/// <summary>
	/// Metered Japan.
	/// </summary>
	METERED_JP = 107,

	/// <summary>
	/// Unlimited plan US California.
	/// </summary>
	UNLIMITED_PLAN_US_CA = 200,

	/// <summary>
	/// Unlimited plan Australia/New Zealand.
	/// </summary>
	UNLIMITED_PLAN_AU_NZ = 201,

	/// <summary>
	/// Unlimited plan Great Britain/Ireland.
	/// </summary>
	UNLIMITED_PLAN_GB_IE = 202,

	/// <summary>
	/// Unlimited EURA.
	/// </summary>
	UNLIMITED_EURA = 203,

	/// <summary>
	/// Unlimited EURB.
	/// </summary>
	UNLIMITED_EURB = 204,

	/// <summary>
	/// Unlimited Japan.
	/// </summary>
	UNLIMITED_JP = 207,

	/// <summary>
	/// US California number.
	/// </summary>
	US_CA_NUMBER = 300,

	/// <summary>
	/// Australia/New Zealand number.
	/// </summary>
	AU_NZ_NUMBER = 301,

	/// <summary>
	/// Great Britain/Ireland number.
	/// </summary>
	GB_IE_NUMBER = 302,

	/// <summary>
	/// EURA number.
	/// </summary>
	EURA_NUMBER = 303,

	/// <summary>
	/// EURB number.
	/// </summary>
	EURB_NUMBER = 304,

	/// <summary>
	/// Japanese number.
	/// </summary>
	JP_NUMBER = 307,

	/// <summary>
	/// US California toll free number.
	/// </summary>
	US_CA_TOLLFREE_NUMBER = 400,

	/// <summary>
	/// Australia toll free number.
	/// </summary>
	AU_TOLLFREE_NUMBER = 401,

	/// <summary>
	/// Great Britain/Ireland toll free number.
	/// </summary>
	GB_IE_TOLLFREE_NUMBER = 402,

	/// <summary>
	/// New Zealand toll free number.
	/// </summary>
	NZ_TOLLFREE_NUMBER = 403,

	/// <summary>
	/// Global toll free number.
	/// </summary>
	GLOBAL_TOLLFREE_NUMBER = 404,

	/// <summary>
	/// Beta.
	/// </summary>
	BETA = 600,

	/// <summary>
	/// Unlimited domestic select.
	/// </summary>
	UNLIMITED_DOMESTIC_SELECT = 1000,

	/// <summary>
	/// Metered global select.
	/// </summary>
	METERED_GLOBAL_SELECT = 1001,

	/// <summary>
	/// Unlimited domestic select number.
	/// </summary>
	UNLIMITED_DOMESTIC_SELECT_NUMBER = 2000,

	/// <summary>
	/// Zoom Phone Pro.
	/// </summary>
	ZP_PRO = 3000,

	/// <summary>
	/// Basic.
	/// </summary>
	BASIC = 3010,

	/// <summary>
	/// Zoom Phone common area.
	/// </summary>
	ZP_COMMON_AREA = 3040,

	/// <summary>
	/// Reserved plan.
	/// </summary>
	RESERVED_PLAN = 3098,

	/// <summary>
	/// Basic migrated.
	/// </summary>
	BASIC_MIGRATED = 3099,

	/// <summary>
	/// International select addon.
	/// </summary>
	INTERNATIONAL_SELECT_ADDON = 4000,

	/// <summary>
	/// Zoom Phone power pack.
	/// </summary>
	ZP_POWER_PACK = 4010,

	/// <summary>
	/// Premium member.
	/// </summary>
	PREMIUM_NUMBER = 5000,

	/// <summary>
	/// Metered US California number included.
	/// </summary>
	METERED_US_CA_NUMBER_INCLUDED = 30000,

	/// <summary>
	/// Metered Australia/New Zealand number included.
	/// </summary>
	METERED_AU_NZ_NUMBER_INCLUDED = 30001,

	/// <summary>
	/// Metered Great Britain/Ireland number included.
	/// </summary>
	METERED_GB_IE_NUMBER_INCLUDED = 30002,

	/// <summary>
	/// Metered EURA number included.
	/// </summary>
	METERED_EURA_NUMBER_INCLUDED = 30003,

	/// <summary>
	/// Metered EURB number included.
	/// </summary>
	METERED_EURB_NUMBER_INCLUDED = 30004,

	/// <summary>
	/// Metered Japan number included.
	/// </summary>
	METERED_JP_NUMBER_INCLUDED = 30007,

	/// <summary>
	/// Unlimited US California number included.
	/// </summary>
	UNLIMITED_US_CA_NUMBER_INCLUDED = 31000,

	/// <summary>
	/// Unlimited Australia/New Zealand number included.
	/// </summary>
	UNLIMITED_AU_NZ_NUMBER_INCLUDED = 31001,

	/// <summary>
	/// Unlimited Great Britain/Ireland number included.
	/// </summary>
	UNLIMITED_GB_IE_NUMBER_INCLUDED = 31002,

	/// <summary>
	/// Unlimited EURA number included.
	/// </summary>
	UNLIMITED_EURA_NUMBER_INCLUDED = 31003,

	/// <summary>
	/// Unlimited EURB number included.
	/// </summary>
	UNLIMITED_EURB_NUMBER_INCLUDED = 31004,

	/// <summary>
	/// Unlimited Japan number included.
	/// </summary>
	UNLIMITED_JP_NUMBER_INCLUDED = 31007,

	/// <summary>
	/// Unlimited domestic select number included.
	/// </summary>
	UNLIMITED_DOMESTIC_SELECT_NUMBER_INCLUDED = 31005,

	/// <summary>
	/// Metered global select number included.
	/// </summary>
	METERED_GLOBAL_SELECT_NUMBER_INCLUDED = 31006,

	/// <summary>
	/// Meetings pro unlimited US California.
	/// </summary>
	MEETINGS_PRO_UNLIMITED_US_CA = 40200,

	/// <summary>
	/// Meetings pro unlimited Australia/New Zealand.
	/// </summary>
	MEETINGS_PRO_UNLIMITED_AU_NZ = 40201,

	/// <summary>
	/// Meetings pro unlimited Great Britain/Ireland.
	/// </summary>
	MEETINGS_PRO_UNLIMITED_GB_IE = 40202,

	/// <summary>
	/// Meetings pro unlimited Japan.
	/// </summary>
	MEETINGS_PRO_UNLIMITED_JP = 40207,

	/// <summary>
	/// Meetings pro global select.
	/// </summary>
	MEETINGS_PRO_GLOBAL_SELECT = 41000,

	/// <summary>
	/// Meetings pro PN Pro.
	/// </summary>
	MEETINGS_PRO_PN_PRO = 43000,

	/// <summary>
	/// Meetings business unlimited US California.
	/// </summary>
	MEETINGS_BUS_UNLIMITED_US_CA = 50200,

	/// <summary>
	/// Meetings business unlimited Australia/New Zealand.
	/// </summary>
	MEETINGS_BUS_UNLIMITED_AU_NZ = 50201,

	/// <summary>
	/// Meetings business unlimited Great Britain/Ireland.
	/// </summary>
	MEETINGS_BUS_UNLIMITED_GB_IE = 50202,

	/// <summary>
	/// Meetings business unlimited Japan.
	/// </summary>
	MEETINGS_BUS_UNLIMITED_JP = 50207,

	/// <summary>
	/// Meetings business global select.
	/// </summary>
	MEETINGS_BUS_GLOBAL_SELECT = 51000,

	/// <summary>
	/// Meetings business PN Pro.
	/// </summary>
	MEETINGS_BUS_PN_PRO = 53000,

	/// <summary>
	/// Meetings enterprise unlimited US California.
	/// </summary>
	MEETINGS_ENT_UNLIMITED_US_CA = 60200,

	/// <summary>
	/// Meetings enterprise unlimited Australia/New Zealand.
	/// </summary>
	MEETINGS_ENT_UNLIMITED_AU_NZ = 60201,

	/// <summary>
	/// Meetings enterprise unlimited Great Britain/Ireland.
	/// </summary>
	MEETINGS_ENT_UNLIMITED_GB_IE = 60202,

	/// <summary>
	/// Meetings enterprise unlimited Japan.
	/// </summary>
	MEETINGS_ENT_UNLIMITED_JP = 60207,

	/// <summary>
	/// Meetings enterprise global select.
	/// </summary>
	MEETINGS_ENT_GLOBAL_SELECT = 61000,

	/// <summary>
	/// Meetings enterprise PN Pro.
	/// </summary>
	MEETINGS_ENT_PN_PRO = 63000,

	/// <summary>
	/// Meetings US California number included.
	/// </summary>
	MEETINGS_US_CA_NUMBER_INCLUDED = 70200,

	/// <summary>
	/// Meetings Australia/New Zealand number included.
	/// </summary>
	MEETINGS_AU_NZ_NUMBER_INCLUDED = 70201,

	/// <summary>
	/// Meetings Great Britain/Ireland number included.
	/// </summary>
	MEETINGS_GB_IE_NUMBER_INCLUDED = 70202,

	/// <summary>
	/// Meetings Japanese number included.
	/// </summary>
	MEETINGS_JP_NUMBER_INCLUDED = 70207,

	/// <summary>
	/// Meetings global select number included.
	/// </summary>
	MEETINGS_GLOBAL_SELECT_NUMBER_INCLUDED = 71000
}
