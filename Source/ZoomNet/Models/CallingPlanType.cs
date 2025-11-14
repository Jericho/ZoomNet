namespace ZoomNet.Models;

/// <summary>
/// Represents the type of calling plan.
/// </summary>
public enum CallingPlanType
{
	/// <summary>
	/// No feature package.
	/// </summary>
	NoFeaturePackage = 1,

	/// <summary>
	/// International toll number.
	/// </summary>
	InternationalTollNumber = 3,

	/// <summary>
	/// International toll free number.
	/// </summary>
	InternationalTollFreeNumber = 4,

	/// <summary>
	/// Bring your own carrier number.
	/// </summary>
	ByocNumber = 5,

	/// <summary>
	/// BETA number.
	/// </summary>
	BetaNumber = 6,

	/// <summary>
	/// Metered plan in California.
	/// </summary>
	MeteredPlanUsCa = 100,

	/// <summary>
	/// Metered plan in Australia/New Zealand.
	/// </summary>
	MeteredPlanAuNz = 101,

	/// <summary>
	/// Metered plan in Great Britian/Ireland.
	/// </summary>
	MeteredPlanGbIe = 102,

	/// <summary>
	/// Metered EURA.
	/// </summary>
	MeteredEura = 103,

	/// <summary>
	/// Metered EURB.
	/// </summary>
	MeteredEurb = 104,

	/// <summary>
	/// Metered Japan.
	/// </summary>
	MeteredJp = 107,

	/// <summary>
	/// Unlimited plan US California.
	/// </summary>
	UnlimitedPlanUsCa = 200,

	/// <summary>
	/// Unlimited plan Australia/New Zealand.
	/// </summary>
	UnlimitedPlanAuNz = 201,

	/// <summary>
	/// Unlimited plan Great Britain/Ireland.
	/// </summary>
	UnlimitedPlanGbIe = 202,

	/// <summary>
	/// Unlimited EURA.
	/// </summary>
	UnlimitedEura = 203,

	/// <summary>
	/// Unlimited EURB.
	/// </summary>
	UnlimitedEurb = 204,

	/// <summary>
	/// Unlimited Japan.
	/// </summary>
	UnlimitedJp = 207,

	/// <summary>
	/// US California number.
	/// </summary>
	UsCaNumber = 300,

	/// <summary>
	/// Australia/New Zealand number.
	/// </summary>
	AuNzNumber = 301,

	/// <summary>
	/// Great Britain/Ireland number.
	/// </summary>
	GbIeNumber = 302,

	/// <summary>
	/// EURA number.
	/// </summary>
	EuraNumber = 303,

	/// <summary>
	/// EURB number.
	/// </summary>
	EurbNumber = 304,

	/// <summary>
	/// Japanese number.
	/// </summary>
	JpNumber = 307,

	/// <summary>
	/// US California toll free number.
	/// </summary>
	UsCaTollfreeNumber = 400,

	/// <summary>
	/// Australia toll free number.
	/// </summary>
	AuTollfreeNumber = 401,

	/// <summary>
	/// Great Britain/Ireland toll free number.
	/// </summary>
	GbIeTollfreeNumber = 402,

	/// <summary>
	/// New Zealand toll free number.
	/// </summary>
	NzTollfreeNumber = 403,

	/// <summary>
	/// Global toll free number.
	/// </summary>
	GlobalTollfreeNumber = 404,

	/// <summary>
	/// Beta.
	/// </summary>
	Beta = 600,

	/// <summary>
	/// Unlimited domestic select.
	/// </summary>
	UnlimitedDomesticSelect = 1000,

	/// <summary>
	/// Metered global select.
	/// </summary>
	MeteredGlobalSelect = 1001,

	/// <summary>
	/// Unlimited domestic select number.
	/// </summary>
	UnlimitedDomesticSelectNumber = 2000,

	/// <summary>
	/// Zoom Phone Pro.
	/// </summary>
	ZpPro = 3000,

	/// <summary>
	/// Basic.
	/// </summary>
	Basic = 3010,

	/// <summary>
	/// Zoom Phone common area.
	/// </summary>
	ZpCommonArea = 3040,

	/// <summary>
	/// Reserved plan.
	/// </summary>
	ReservedPlan = 3098,

	/// <summary>
	/// Basic migrated.
	/// </summary>
	BasicMigrated = 3099,

	/// <summary>
	/// International select addon.
	/// </summary>
	InternationalSelectAddon = 4000,

	/// <summary>
	/// Zoom Phone power pack.
	/// </summary>
	ZpPowerPack = 4010,

	/// <summary>
	/// Premium member.
	/// </summary>
	PremiumNumber = 5000,

	/// <summary>
	/// Metered US California number included.
	/// </summary>
	MeteredUsCaNumberIncluded = 30000,

	/// <summary>
	/// Metered Australia/New Zealand number included.
	/// </summary>
	MeteredAuNzNumberIncluded = 30001,

	/// <summary>
	/// Metered Great Britain/Ireland number included.
	/// </summary>
	MeteredGbIeNumberIncluded = 30002,

	/// <summary>
	/// Metered EURA number included.
	/// </summary>
	MeteredEuraNumberIncluded = 30003,

	/// <summary>
	/// Metered EURB number included.
	/// </summary>
	MeteredEurbNumberIncluded = 30004,

	/// <summary>
	/// Metered Japan number included.
	/// </summary>
	MeteredJpNumberIncluded = 30007,

	/// <summary>
	/// Unlimited Australia/New Zealand number included.
	/// </summary>
	UnlimitedAuNzNumberIncluded = 31001,

	/// <summary>
	/// Unlimited Great Britain/Ireland number included.
	/// </summary>
	UnlimitedGbIeNumberIncluded = 31002,

	/// <summary>
	/// Unlimited EURA number included.
	/// </summary>
	UnlimitedEuraNumberIncluded = 31003,

	/// <summary>
	/// Unlimited EURB number included.
	/// </summary>
	UnlimitedEurbNumberIncluded = 31004,

	/// <summary>
	/// Unlimited Japan number included.
	/// </summary>
	UnlimitedJpNumberIncluded = 31007,

	/// <summary>
	/// Unlimited domestic select number included.
	/// </summary>
	UnlimitedDomesticSelectNumberIncluded = 31005,

	/// <summary>
	/// Metered global select number included.
	/// </summary>
	MeteredGlobalSelectNumberIncluded = 31006,

	/// <summary>
	/// Meetings pro unlimited US California.
	/// </summary>
	MeetingsProUnlimitedUsCa = 40200,

	/// <summary>
	/// Meetings pro unlimited Australia/New Zealand.
	/// </summary>
	MeetingsProUnlimitedAuNz = 40201,

	/// <summary>
	/// Meetings pro unlimited Great Britain/Ireland.
	/// </summary>
	MeetingsProUnlimitedGbIe = 40202,

	/// <summary>
	/// Meetings pro unlimited Japan.
	/// </summary>
	MeetingsProUnlimitedJp = 40207,

	/// <summary>
	/// Meetings pro global select.
	/// </summary>
	MeetingsProGlobalSelect = 41000,

	/// <summary>
	/// Meetings pro PN Pro.
	/// </summary>
	MeetingsProPnPro = 43000,

	/// <summary>
	/// Meetings business unlimited US California.
	/// </summary>
	MeetingsBusUnlimitedUsCa = 50200,

	/// <summary>
	/// Meetings business unlimited Australia/New Zealand.
	/// </summary>
	MeetingsBusUnlimitedAuNz = 50201,

	/// <summary>
	/// Meetings business unlimited Great Britain/Ireland.
	/// </summary>
	MeetingsBusUnlimitedGbIe = 50202,

	/// <summary>
	/// Meetings business unlimited Japan.
	/// </summary>
	MeetingsBusUnlimitedJp = 50207,

	/// <summary>
	/// Meetings business global select.
	/// </summary>
	MeetingsBusGlobalSelect = 51000,

	/// <summary>
	/// Meetings business PN Pro.
	/// </summary>
	MeetingsBusPnPro = 53000,

	/// <summary>
	/// Meetings enterprise unlimited US California.
	/// </summary>
	MeetingsEntUnlimitedUsCa = 60200,

	/// <summary>
	/// Meetings enterprise unlimited Australia/New Zealand.
	/// </summary>
	MeetingsEntUnlimitedAuNz = 60201,

	/// <summary>
	/// Meetings enterprise unlimited Great Britain/Ireland.
	/// </summary>
	MeetingsEntUnlimitedGbIe = 60202,

	/// <summary>
	/// Meetings enterprise unlimited Japan.
	/// </summary>
	MeetingsEntUnlimitedJp = 60207,

	/// <summary>
	/// Meetings enterprise global select.
	/// </summary>
	MeetingsEntGlobalSelect = 61000,

	/// <summary>
	/// Meetings enterprise PN Pro.
	/// </summary>
	MeetingsEntPnPro = 63000,

	/// <summary>
	/// Meetings US California number included.
	/// </summary>
	MeetingsUsCaNumberIncluded = 70200,

	/// <summary>
	/// Meetings Australia/New Zealand number included.
	/// </summary>
	MeetingsAuNzNumberIncluded = 70201,

	/// <summary>
	/// Meetings Great Britain/Ireland number included.
	/// </summary>
	MeetingsGbIeNumberIncluded = 70202,

	/// <summary>
	/// Meetings Japanese number included.
	/// </summary>
	MeetingsJpNumberIncluded = 70207,

	/// <summary>
	/// Meetings global select number included.
	/// </summary>
	MeetingsGlobalSelectNumberIncluded = 71000
}
