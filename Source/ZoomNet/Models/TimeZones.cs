using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration of the time zones supported by Zoom.
	/// </summary>
	public enum TimeZones
	{
		/// <summary>Midway Island, Samoa.</summary>
		[EnumMember(Value = "Pacific/Midway")]
		Pacific_Midway,

		/// <summary>Pago Pago.</summary>
		[EnumMember(Value = "Pacific/Pago_Pago")]
		Pacific_Pago_Pago,

		/// <summary>Hawaii.</summary>
		[EnumMember(Value = "Pacific/Honolulu")]
		Pacific_Honolulu,

		/// <summary>Alaska.</summary>
		[EnumMember(Value = "America/Anchorage")]
		America_Anchorage,

		/// <summary>Vancouver.</summary>
		[EnumMember(Value = "America/Vancouver")]
		America_Vancouver,

		/// <summary>Pacific Time (US and Canada).</summary>
		[EnumMember(Value = "America/Los_Angeles")]
		America_Los_Angeles,

		/// <summary>Tijuana.</summary>
		[EnumMember(Value = "America/Tijuana")]
		America_Tijuana,

		/// <summary>Edmonton.</summary>
		[EnumMember(Value = "America/Edmonton")]
		America_Edmonton,

		/// <summary>Mountain Time (US and Canada).</summary>
		[EnumMember(Value = "America/Denver")]
		America_Denver,

		/// <summary>Arizona.</summary>
		[EnumMember(Value = "America/Phoenix")]
		America_Phoenix,

		/// <summary>Mazatlan.</summary>
		[EnumMember(Value = "America/Mazatlan")]
		America_Mazatlan,

		/// <summary>Winnipeg.</summary>
		[EnumMember(Value = "America/Winnipeg")]
		America_Winnipeg,

		/// <summary>Saskatchewan.</summary>
		[EnumMember(Value = "America/Regina")]
		America_Regina,

		/// <summary>Central Time (US and Canada).</summary>
		[EnumMember(Value = "America/Chicago")]
		America_Chicago,

		/// <summary>Mexico City.</summary>
		[EnumMember(Value = "America/Mexico_City")]
		America_Mexico_City,

		/// <summary>Guatemala.</summary>
		[EnumMember(Value = "America/Guatemala")]
		America_Guatemala,

		/// <summary>El Salvador.</summary>
		[EnumMember(Value = "America/El_Salvador")]
		America_El_Salvador,

		/// <summary>Managua.</summary>
		[EnumMember(Value = "America/Managua")]
		America_Managua,

		/// <summary>Costa Rica.</summary>
		[EnumMember(Value = "America/Costa_Rica")]
		America_Costa_Rica,

		/// <summary>Montreal.</summary>
		[EnumMember(Value = "America/Montreal")]
		America_Montreal,

		/// <summary>Eastern Time (US and Canada).</summary>
		[EnumMember(Value = "America/New_York")]
		America_New_York,

		/// <summary>Indiana (East).</summary>
		[EnumMember(Value = "America/Indianapolis")]
		America_Indianapolis,

		/// <summary>Panama.</summary>
		[EnumMember(Value = "America/Panama")]
		America_Panama,

		/// <summary>Bogota.</summary>
		[EnumMember(Value = "America/Bogota")]
		America_Bogota,

		/// <summary>Lima.</summary>
		[EnumMember(Value = "America/Lima")]
		America_Lima,

		/// <summary>Halifax.</summary>
		[EnumMember(Value = "America/Halifax")]
		America_Halifax,

		/// <summary>Puerto Rico.</summary>
		[EnumMember(Value = "America/Puerto_Rico")]
		America_Puerto_Rico,

		/// <summary>Caracas.</summary>
		[EnumMember(Value = "America/Caracas")]
		America_Caracas,

		/// <summary>Santiago.</summary>
		[EnumMember(Value = "America/Santiago")]
		America_Santiago,

		/// <summary>Newfoundland and Labrador.</summary>
		[EnumMember(Value = "America/St_Johns")]
		America_St_Johns,

		/// <summary>Montevideo.</summary>
		[EnumMember(Value = "America/Montevideo")]
		America_Montevideo,

		/// <summary>Brasilia.</summary>
		[EnumMember(Value = "America/Araguaina")]
		America_Araguaina,

		/// <summary>Buenos Aires, Georgetown.</summary>
		[EnumMember(Value = "America/Argentina/Buenos_Aires")]
		America_Argentina_Buenos_Aires,

		/// <summary>Greenland.</summary>
		[EnumMember(Value = "America/Godthab")]
		America_Godthab,

		/// <summary>Sao Paulo.</summary>
		[EnumMember(Value = "America/Sao_Paulo")]
		America_Sao_Paulo,

		/// <summary>Azores.</summary>
		[EnumMember(Value = "Atlantic/Azores")]
		Atlantic_Azores,

		/// <summary>Atlantic Time (Canada).</summary>
		[EnumMember(Value = "Canada/Atlantic")]
		Canada_Atlantic,

		/// <summary>Cape Verde Islands.</summary>
		[EnumMember(Value = "Atlantic/Cape_Verde")]
		Atlantic_Cape_Verde,

		/// <summary>Universal Time UTC.</summary>
		[EnumMember(Value = "UTC")]
		UTC,

		/// <summary>Greenwich Mean Time.</summary>
		[EnumMember(Value = "Etc/Greenwich")]
		Etc_Greenwich,

		/// <summary>Belgrade, Bratislava, Ljubljana.</summary>
		[EnumMember(Value = "Europe/Belgrade")]
		Europe_Belgrade,

		/// <summary>Sarajevo, Skopje, Zagreb.</summary>
		[EnumMember(Value = "CET")]
		CET,

		/// <summary>Reykjavik.</summary>
		[EnumMember(Value = "Atlantic/Reykjavik")]
		Atlantic_Reykjavik,

		/// <summary>Dublin.</summary>
		[EnumMember(Value = "Europe/Dublin")]
		Europe_Dublin,

		/// <summary>London.</summary>
		[EnumMember(Value = "Europe/London")]
		Europe_London,

		/// <summary>Lisbon.</summary>
		[EnumMember(Value = "Europe/Lisbon")]
		Europe_Lisbon,

		/// <summary>Casablanca.</summary>
		[EnumMember(Value = "Africa/Casablanca")]
		Africa_Casablanca,

		/// <summary>Nouakchott.</summary>
		[EnumMember(Value = "Africa/Nouakchott")]
		Africa_Nouakchott,

		/// <summary>Oslo.</summary>
		[EnumMember(Value = "Europe/Oslo")]
		Europe_Oslo,

		/// <summary>Copenhagen.</summary>
		[EnumMember(Value = "Europe/Copenhagen")]
		Europe_Copenhagen,

		/// <summary>Brussels.</summary>
		[EnumMember(Value = "Europe/Brussels")]
		Europe_Brussels,

		/// <summary>Amsterdam, Berlin, Rome, Stockholm, Vienna.</summary>
		[EnumMember(Value = "Europe/Berlin")]
		Europe_Berlin,

		/// <summary>Helsinki.</summary>
		[EnumMember(Value = "Europe/Helsinki")]
		Europe_Helsinki,

		/// <summary>Amsterdam.</summary>
		[EnumMember(Value = "Europe/Amsterdam")]
		Europe_Amsterdam,

		/// <summary>Rome.</summary>
		[EnumMember(Value = "Europe/Rome")]
		Europe_Rome,

		/// <summary>Stockholm.</summary>
		[EnumMember(Value = "Europe/Stockholm")]
		Europe_Stockholm,

		/// <summary>Vienna.</summary>
		[EnumMember(Value = "Europe/Vienna")]
		Europe_Vienna,

		/// <summary>Luxembourg.</summary>
		[EnumMember(Value = "Europe/Luxembourg")]
		Europe_Luxembourg,

		/// <summary>Paris.</summary>
		[EnumMember(Value = "Europe/Paris")]
		Europe_Paris,

		/// <summary>Zurich.</summary>
		[EnumMember(Value = "Europe/Zurich")]
		Europe_Zurich,

		/// <summary>Madrid.</summary>
		[EnumMember(Value = "Europe/Madrid")]
		Europe_Madrid,

		/// <summary>West Central Africa.</summary>
		[EnumMember(Value = "Africa/Bangui")]
		Africa_Bangui,

		/// <summary>Algiers.</summary>
		[EnumMember(Value = "Africa/Algiers")]
		Africa_Algiers,

		/// <summary>Tunis.</summary>
		[EnumMember(Value = "Africa/Tunis")]
		Africa_Tunis,

		/// <summary>Harare, Pretoria.</summary>
		[EnumMember(Value = "Africa/Harare")]
		Africa_Harare,

		/// <summary>Nairobi.</summary>
		[EnumMember(Value = "Africa/Nairobi")]
		Africa_Nairobi,

		/// <summary>Warsaw.</summary>
		[EnumMember(Value = "Europe/Warsaw")]
		Europe_Warsaw,

		/// <summary>Prague Bratislava.</summary>
		[EnumMember(Value = "Europe/Prague")]
		Europe_Prague,

		/// <summary>Budapest.</summary>
		[EnumMember(Value = "Europe/Budapest")]
		Europe_Budapest,

		/// <summary>Sofia.</summary>
		[EnumMember(Value = "Europe/Sofia")]
		Europe_Sofia,

		/// <summary>Istanbul.</summary>
		[EnumMember(Value = "Europe/Istanbul")]
		Europe_Istanbul,

		/// <summary>Athens.</summary>
		[EnumMember(Value = "Europe/Athens")]
		Europe_Athens,

		/// <summary>Bucharest.</summary>
		[EnumMember(Value = "Europe/Bucharest")]
		Europe_Bucharest,

		/// <summary>Nicosia.</summary>
		[EnumMember(Value = "Asia/Nicosia")]
		Asia_Nicosia,

		/// <summary>Beirut.</summary>
		[EnumMember(Value = "Asia/Beirut")]
		Asia_Beirut,

		/// <summary>Damascus.</summary>
		[EnumMember(Value = "Asia/Damascus")]
		Asia_Damascus,

		/// <summary>Jerusalem.</summary>
		[EnumMember(Value = "Asia/Jerusalem")]
		Asia_Jerusalem,

		/// <summary>Amman.</summary>
		[EnumMember(Value = "Asia/Amman")]
		Asia_Amman,

		/// <summary>Tripoli.</summary>
		[EnumMember(Value = "Africa/Tripoli")]
		Africa_Tripoli,

		/// <summary>Cairo.</summary>
		[EnumMember(Value = "Africa/Cairo")]
		Africa_Cairo,

		/// <summary>Johannesburg.</summary>
		[EnumMember(Value = "Africa/Johannesburg")]
		Africa_Johannesburg,

		/// <summary>Moscow.</summary>
		[EnumMember(Value = "Europe/Moscow")]
		Europe_Moscow,

		/// <summary>Baghdad.</summary>
		[EnumMember(Value = "Asia/Baghdad")]
		Asia_Baghdad,

		/// <summary>Kuwait.</summary>
		[EnumMember(Value = "Asia/Kuwait")]
		Asia_Kuwait,

		/// <summary>Riyadh.</summary>
		[EnumMember(Value = "Asia/Riyadh")]
		Asia_Riyadh,

		/// <summary>Bahrain.</summary>
		[EnumMember(Value = "Asia/Bahrain")]
		Asia_Bahrain,

		/// <summary>Qatar.</summary>
		[EnumMember(Value = "Asia/Qatar")]
		Asia_Qatar,

		/// <summary>Aden.</summary>
		[EnumMember(Value = "Asia/Aden")]
		Asia_Aden,

		/// <summary>Tehran.</summary>
		[EnumMember(Value = "Asia/Tehran")]
		Asia_Tehran,

		/// <summary>Khartoum.</summary>
		[EnumMember(Value = "Africa/Khartoum")]
		Africa_Khartoum,

		/// <summary>Djibouti.</summary>
		[EnumMember(Value = "Africa/Djibouti")]
		Africa_Djibouti,

		/// <summary>Mogadishu.</summary>
		[EnumMember(Value = "Africa/Mogadishu")]
		Africa_Mogadishu,

		/// <summary>Dubai.</summary>
		[EnumMember(Value = "Asia/Dubai")]
		Asia_Dubai,

		/// <summary>Muscat.</summary>
		[EnumMember(Value = "Asia/Muscat")]
		Asia_Muscat,

		/// <summary>Baku, Tbilisi, Yerevan.</summary>
		[EnumMember(Value = "Asia/Baku")]
		Asia_Baku,

		/// <summary>Kabul.</summary>
		[EnumMember(Value = "Asia/Kabul")]
		Asia_Kabul,

		/// <summary>Yekaterinburg.</summary>
		[EnumMember(Value = "Asia/Yekaterinburg")]
		Asia_Yekaterinburg,

		/// <summary>Islamabad, Karachi, Tashkent.</summary>
		[EnumMember(Value = "Asia/Tashkent")]
		Asia_Tashkent,

		/// <summary>India.</summary>
		[EnumMember(Value = "Asia/Calcutta")]
		Asia_Calcutta,

		/// <summary>Kathmandu.</summary>
		[EnumMember(Value = "Asia/Kathmandu")]
		Asia_Kathmandu,

		/// <summary>Novosibirsk.</summary>
		[EnumMember(Value = "Asia/Novosibirsk")]
		Asia_Novosibirsk,

		/// <summary>Almaty.</summary>
		[EnumMember(Value = "Asia/Almaty")]
		Asia_Almaty,

		/// <summary>Dacca.</summary>
		[EnumMember(Value = "Asia/Dacca")]
		Asia_Dacca,

		/// <summary>Krasnoyarsk.</summary>
		[EnumMember(Value = "Asia/Krasnoyarsk")]
		Asia_Krasnoyarsk,

		/// <summary>Astana, Dhaka.</summary>
		[EnumMember(Value = "Asia/Dhaka")]
		Asia_Dhaka,

		/// <summary>Bangkok.</summary>
		[EnumMember(Value = "Asia/Bangkok")]
		Asia_Bangkok,

		/// <summary>Vietnam.</summary>
		[EnumMember(Value = "Asia/Saigon")]
		Asia_Saigon,

		/// <summary>Jakarta.</summary>
		[EnumMember(Value = "Asia/Jakarta")]
		Asia_Jakarta,

		/// <summary>Irkutsk, Ulaanbaatar.</summary>
		[EnumMember(Value = "Asia/Irkutsk")]
		Asia_Irkutsk,

		/// <summary>Beijing, Shanghai.</summary>
		[EnumMember(Value = "Asia/Shanghai")]
		Asia_Shanghai,

		/// <summary>Hong Kong.</summary>
		[EnumMember(Value = "Asia/Hong_Kong")]
		Asia_Hong_Kong,

		/// <summary>Taipei.</summary>
		[EnumMember(Value = "Asia/Taipei")]
		Asia_Taipei,

		/// <summary>Kuala Lumpur.</summary>
		[EnumMember(Value = "Asia/Kuala_Lumpur")]
		Asia_Kuala_Lumpur,

		/// <summary>Singapore.</summary>
		[EnumMember(Value = "Asia/Singapore")]
		Asia_Singapore,

		/// <summary>Perth.</summary>
		[EnumMember(Value = "Australia/Perth")]
		Australia_Perth,

		/// <summary>Yakutsk.</summary>
		[EnumMember(Value = "Asia/Yakutsk")]
		Asia_Yakutsk,

		/// <summary>Seoul.</summary>
		[EnumMember(Value = "Asia/Seoul")]
		Asia_Seoul,

		/// <summary>Osaka, Sapporo, Tokyo.</summary>
		[EnumMember(Value = "Asia/Tokyo")]
		Asia_Tokyo,

		/// <summary>Darwin.</summary>
		[EnumMember(Value = "Australia/Darwin")]
		Australia_Darwin,

		/// <summary>Adelaide.</summary>
		[EnumMember(Value = "Australia/Adelaide")]
		Australia_Adelaide,

		/// <summary>Vladivostok.</summary>
		[EnumMember(Value = "Asia/Vladivostok")]
		Asia_Vladivostok,

		/// <summary>Guam, Port Moresby.</summary>
		[EnumMember(Value = "Pacific/Port_Moresby")]
		Pacific_Port_Moresby,

		/// <summary>Brisbane.</summary>
		[EnumMember(Value = "Australia/Brisbane")]
		Australia_Brisbane,

		/// <summary>Canberra, Melbourne, Sydney.</summary>
		[EnumMember(Value = "Australia/Sydney")]
		Australia_Sydney,

		/// <summary>Hobart.</summary>
		[EnumMember(Value = "Australia/Hobart")]
		Australia_Hobart,

		/// <summary>Magadan.</summary>
		[EnumMember(Value = "Asia/Magadan")]
		Asia_Magadan,

		/// <summary>Solomon Islands.</summary>
		[EnumMember(Value = "SST")]
		SST,

		/// <summary>New Caledonia.</summary>
		[EnumMember(Value = "Pacific/Noumea")]
		Pacific_Noumea,

		/// <summary>Kamchatka.</summary>
		[EnumMember(Value = "Asia/Kamchatka")]
		Asia_Kamchatka,

		/// <summary>Fiji Islands, Marshall Islands.</summary>
		[EnumMember(Value = "Pacific/Fiji")]
		Pacific_Fiji,

		/// <summary>Auckland, Wellington.</summary>
		[EnumMember(Value = "Pacific/Auckland")]
		Pacific_Auckland,

		/// <summary>Mumbai, Kolkata, New Delhi.</summary>
		[EnumMember(Value = "Asia/Kolkata")]
		Asia_Kolkata,

		/// <summary>Kiev.</summary>
		[EnumMember(Value = "Europe/Kiev")]
		Europe_Kiev,

		/// <summary>Tegucigalpa.</summary>
		[EnumMember(Value = "America/Tegucigalpa")]
		America_Tegucigalpa,

		/// <summary>Independent State of Samoa.</summary>
		[EnumMember(Value = "Pacific/Apia")]
		Pacific_Apia,
	}
}
