using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration of the time zones supported by Zoom.
	/// </summary>
	public enum TimeZones
	{
		/// <summary>Not specified.</summary>
		[EnumMember(Value = "")]
		NotSpecified,

		/// <summary>Algiers.</summary>
		[EnumMember(Value = "Africa/Algiers")]
		Africa_Algiers,

		/// <summary>West Central Africa.</summary>
		[EnumMember(Value = "Africa/Bangui")]
		Africa_Bangui,

		/// <summary>Cairo.</summary>
		[EnumMember(Value = "Africa/Cairo")]
		Africa_Cairo,

		/// <summary>Casablanca.</summary>
		[EnumMember(Value = "Africa/Casablanca")]
		Africa_Casablanca,

		/// <summary>Djibouti.</summary>
		[EnumMember(Value = "Africa/Djibouti")]
		Africa_Djibouti,

		/// <summary>Harare, Pretoria.</summary>
		[EnumMember(Value = "Africa/Harare")]
		Africa_Harare,

		/// <summary>Johannesburg.</summary>
		[EnumMember(Value = "Africa/Johannesburg")]
		Africa_Johannesburg,

		/// <summary>Khartoum.</summary>
		[EnumMember(Value = "Africa/Khartoum")]
		Africa_Khartoum,

		/// <summary>Mogadishu.</summary>
		[EnumMember(Value = "Africa/Mogadishu")]
		Africa_Mogadishu,

		/// <summary>Nairobi.</summary>
		[EnumMember(Value = "Africa/Nairobi")]
		Africa_Nairobi,

		/// <summary>Nouakchott.</summary>
		[EnumMember(Value = "Africa/Nouakchott")]
		Africa_Nouakchott,

		/// <summary>Tripoli.</summary>
		[EnumMember(Value = "Africa/Tripoli")]
		Africa_Tripoli,

		/// <summary>Tunis.</summary>
		[EnumMember(Value = "Africa/Tunis")]
		Africa_Tunis,

		/// <summary>Alaska.</summary>
		[EnumMember(Value = "America/Anchorage")]
		America_Anchorage,

		/// <summary>Brasilia.</summary>
		[EnumMember(Value = "America/Araguaina")]
		America_Araguaina,

		/// <summary>Buenos Aires, Georgetown.</summary>
		[EnumMember(Value = "America/Argentina/Buenos_Aires")]
		America_Argentina_Buenos_Aires,

		/// <summary>Bogota.</summary>
		[EnumMember(Value = "America/Bogota")]
		America_Bogota,

		/// <summary>Caracas.</summary>
		[EnumMember(Value = "America/Caracas")]
		America_Caracas,

		/// <summary>Central Time (US and Canada).</summary>
		[EnumMember(Value = "America/Chicago")]
		America_Chicago,

		/// <summary>Costa Rica.</summary>
		[EnumMember(Value = "America/Costa_Rica")]
		America_Costa_Rica,

		/// <summary>Mountain Time (US and Canada).</summary>
		[EnumMember(Value = "America/Denver")]
		America_Denver,

		/// <summary>Edmonton.</summary>
		[EnumMember(Value = "America/Edmonton")]
		America_Edmonton,

		/// <summary>El Salvador.</summary>
		[EnumMember(Value = "America/El_Salvador")]
		America_El_Salvador,

		/// <summary>Greenland.</summary>
		[EnumMember(Value = "America/Godthab")]
		America_Godthab,

		/// <summary>Guatemala.</summary>
		[EnumMember(Value = "America/Guatemala")]
		America_Guatemala,

		/// <summary>Halifax.</summary>
		[EnumMember(Value = "America/Halifax")]
		America_Halifax,

		/// <summary>Indiana (East).</summary>
		[EnumMember(Value = "America/Indianapolis")]
		America_Indianapolis,

		/// <summary>Lima.</summary>
		[EnumMember(Value = "America/Lima")]
		America_Lima,

		/// <summary>Pacific Time (US and Canada).</summary>
		[EnumMember(Value = "America/Los_Angeles")]
		America_Los_Angeles,

		/// <summary>Managua.</summary>
		[EnumMember(Value = "America/Managua")]
		America_Managua,

		/// <summary>Mazatlan.</summary>
		[EnumMember(Value = "America/Mazatlan")]
		America_Mazatlan,

		/// <summary>Mexico City.</summary>
		[EnumMember(Value = "America/Mexico_City")]
		America_Mexico_City,

		/// <summary>Montevideo.</summary>
		[EnumMember(Value = "America/Montevideo")]
		America_Montevideo,

		/// <summary>Montreal.</summary>
		[EnumMember(Value = "America/Montreal")]
		America_Montreal,

		/// <summary>Eastern Time (US and Canada).</summary>
		[EnumMember(Value = "America/New_York")]
		America_New_York,

		/// <summary>Panama.</summary>
		[EnumMember(Value = "America/Panama")]
		America_Panama,

		/// <summary>Arizona.</summary>
		[EnumMember(Value = "America/Phoenix")]
		America_Phoenix,

		/// <summary>Puerto Rico.</summary>
		[EnumMember(Value = "America/Puerto_Rico")]
		America_Puerto_Rico,

		/// <summary>Saskatchewan.</summary>
		[EnumMember(Value = "America/Regina")]
		America_Regina,

		/// <summary>Santiago.</summary>
		[EnumMember(Value = "America/Santiago")]
		America_Santiago,

		/// <summary>Sao Paulo.</summary>
		[EnumMember(Value = "America/Sao_Paulo")]
		America_Sao_Paulo,

		/// <summary>Newfoundland and Labrador.</summary>
		[EnumMember(Value = "America/St_Johns")]
		America_St_Johns,

		/// <summary>Tegucigalpa.</summary>
		[EnumMember(Value = "America/Tegucigalpa")]
		America_Tegucigalpa,

		/// <summary>Tijuana.</summary>
		[EnumMember(Value = "America/Tijuana")]
		America_Tijuana,

		/// <summary>Vancouver.</summary>
		[EnumMember(Value = "America/Vancouver")]
		America_Vancouver,

		/// <summary>Winnipeg.</summary>
		[EnumMember(Value = "America/Winnipeg")]
		America_Winnipeg,

		/// <summary>Aden.</summary>
		[EnumMember(Value = "Asia/Aden")]
		Asia_Aden,

		/// <summary>Almaty.</summary>
		[EnumMember(Value = "Asia/Almaty")]
		Asia_Almaty,

		/// <summary>Amman.</summary>
		[EnumMember(Value = "Asia/Amman")]
		Asia_Amman,

		/// <summary>Baghdad.</summary>
		[EnumMember(Value = "Asia/Baghdad")]
		Asia_Baghdad,

		/// <summary>Bahrain.</summary>
		[EnumMember(Value = "Asia/Bahrain")]
		Asia_Bahrain,

		/// <summary>Baku, Tbilisi, Yerevan.</summary>
		[EnumMember(Value = "Asia/Baku")]
		Asia_Baku,

		/// <summary>Bangkok.</summary>
		[EnumMember(Value = "Asia/Bangkok")]
		Asia_Bangkok,

		/// <summary>Beirut.</summary>
		[EnumMember(Value = "Asia/Beirut")]
		Asia_Beirut,

		/// <summary>India.</summary>
		[EnumMember(Value = "Asia/Calcutta")]
		Asia_Calcutta,

		/// <summary>Dacca.</summary>
		[EnumMember(Value = "Asia/Dacca")]
		Asia_Dacca,

		/// <summary>Damascus.</summary>
		[EnumMember(Value = "Asia/Damascus")]
		Asia_Damascus,

		/// <summary>Astana, Dhaka.</summary>
		[EnumMember(Value = "Asia/Dhaka")]
		Asia_Dhaka,

		/// <summary>Dubai.</summary>
		[EnumMember(Value = "Asia/Dubai")]
		Asia_Dubai,

		/// <summary>Hong Kong.</summary>
		[EnumMember(Value = "Asia/Hong_Kong")]
		Asia_Hong_Kong,

		/// <summary>Irkutsk, Ulaanbaatar.</summary>
		[EnumMember(Value = "Asia/Irkutsk")]
		Asia_Irkutsk,

		/// <summary>Jakarta.</summary>
		[EnumMember(Value = "Asia/Jakarta")]
		Asia_Jakarta,

		/// <summary>Jerusalem.</summary>
		[EnumMember(Value = "Asia/Jerusalem")]
		Asia_Jerusalem,

		/// <summary>Kabul.</summary>
		[EnumMember(Value = "Asia/Kabul")]
		Asia_Kabul,

		/// <summary>Kamchatka.</summary>
		[EnumMember(Value = "Asia/Kamchatka")]
		Asia_Kamchatka,

		/// <summary>Kathmandu.</summary>
		[EnumMember(Value = "Asia/Kathmandu")]
		Asia_Kathmandu,

		/// <summary>Mumbai, Kolkata, New Delhi.</summary>
		[EnumMember(Value = "Asia/Kolkata")]
		Asia_Kolkata,

		/// <summary>Krasnoyarsk.</summary>
		[EnumMember(Value = "Asia/Krasnoyarsk")]
		Asia_Krasnoyarsk,

		/// <summary>Kuala Lumpur.</summary>
		[EnumMember(Value = "Asia/Kuala_Lumpur")]
		Asia_Kuala_Lumpur,

		/// <summary>Kuwait.</summary>
		[EnumMember(Value = "Asia/Kuwait")]
		Asia_Kuwait,

		/// <summary>Magadan.</summary>
		[EnumMember(Value = "Asia/Magadan")]
		Asia_Magadan,

		/// <summary>Manila.</summary>
		/// <remarks>
		/// This timezone is undocumented.
		/// See <a href="https://github.com/Jericho/ZoomNet/issues/443">GH-443</a> for details.
		/// </remarks>
		[EnumMember(Value = "Asia/Manila")]
		Asia_Manila,
		
		/// <summary>Muscat.</summary>
		[EnumMember(Value = "Asia/Muscat")]
		Asia_Muscat,

		/// <summary>Nicosia.</summary>
		[EnumMember(Value = "Asia/Nicosia")]
		Asia_Nicosia,

		/// <summary>Novosibirsk.</summary>
		[EnumMember(Value = "Asia/Novosibirsk")]
		Asia_Novosibirsk,

		/// <summary>Qatar.</summary>
		[EnumMember(Value = "Asia/Qatar")]
		Asia_Qatar,

		/// <summary>Riyadh.</summary>
		[EnumMember(Value = "Asia/Riyadh")]
		Asia_Riyadh,

		/// <summary>Vietnam.</summary>
		[EnumMember(Value = "Asia/Saigon")]
		Asia_Saigon,

		/// <summary>Seoul.</summary>
		[EnumMember(Value = "Asia/Seoul")]
		Asia_Seoul,

		/// <summary>Beijing, Shanghai.</summary>
		[EnumMember(Value = "Asia/Shanghai")]
		Asia_Shanghai,

		/// <summary>Singapore.</summary>
		[EnumMember(Value = "Asia/Singapore")]
		Asia_Singapore,

		/// <summary>Taipei.</summary>
		[EnumMember(Value = "Asia/Taipei")]
		Asia_Taipei,

		/// <summary>Islamabad, Karachi, Tashkent.</summary>
		[EnumMember(Value = "Asia/Tashkent")]
		Asia_Tashkent,

		/// <summary>Tehran.</summary>
		[EnumMember(Value = "Asia/Tehran")]
		Asia_Tehran,

		/// <summary>Osaka, Sapporo, Tokyo.</summary>
		[EnumMember(Value = "Asia/Tokyo")]
		Asia_Tokyo,

		/// <summary>Vladivostok.</summary>
		[EnumMember(Value = "Asia/Vladivostok")]
		Asia_Vladivostok,

		/// <summary>Yakutsk.</summary>
		[EnumMember(Value = "Asia/Yakutsk")]
		Asia_Yakutsk,

		/// <summary>Yekaterinburg.</summary>
		[EnumMember(Value = "Asia/Yekaterinburg")]
		Asia_Yekaterinburg,

		/// <summary>Azores.</summary>
		[EnumMember(Value = "Atlantic/Azores")]
		Atlantic_Azores,

		/// <summary>Cape Verde Islands.</summary>
		[EnumMember(Value = "Atlantic/Cape_Verde")]
		Atlantic_Cape_Verde,

		/// <summary>Reykjavik.</summary>
		[EnumMember(Value = "Atlantic/Reykjavik")]
		Atlantic_Reykjavik,

		/// <summary>Adelaide.</summary>
		[EnumMember(Value = "Australia/Adelaide")]
		Australia_Adelaide,

		/// <summary>Brisbane.</summary>
		[EnumMember(Value = "Australia/Brisbane")]
		Australia_Brisbane,

		/// <summary>Darwin.</summary>
		[EnumMember(Value = "Australia/Darwin")]
		Australia_Darwin,

		/// <summary>Hobart.</summary>
		[EnumMember(Value = "Australia/Hobart")]
		Australia_Hobart,

		/// <summary>Perth.</summary>
		[EnumMember(Value = "Australia/Perth")]
		Australia_Perth,

		/// <summary>Canberra, Melbourne, Sydney.</summary>
		[EnumMember(Value = "Australia/Sydney")]
		Australia_Sydney,

		/// <summary>Atlantic Time (Canada).</summary>
		[EnumMember(Value = "Canada/Atlantic")]
		Canada_Atlantic,

		/// <summary>Sarajevo, Skopje, Zagreb.</summary>
		[EnumMember(Value = "CET")]
		CET,

		/// <summary>Greenwich Mean Time.</summary>
		[EnumMember(Value = "Etc/Greenwich")]
		Etc_Greenwich,

		/// <summary>Amsterdam.</summary>
		[EnumMember(Value = "Europe/Amsterdam")]
		Europe_Amsterdam,

		/// <summary>Athens.</summary>
		[EnumMember(Value = "Europe/Athens")]
		Europe_Athens,

		/// <summary>Belgrade, Bratislava, Ljubljana.</summary>
		[EnumMember(Value = "Europe/Belgrade")]
		Europe_Belgrade,

		/// <summary>Amsterdam, Berlin, Rome, Stockholm, Vienna.</summary>
		[EnumMember(Value = "Europe/Berlin")]
		Europe_Berlin,

		/// <summary>Brussels.</summary>
		[EnumMember(Value = "Europe/Brussels")]
		Europe_Brussels,

		/// <summary>Bucharest.</summary>
		[EnumMember(Value = "Europe/Bucharest")]
		Europe_Bucharest,

		/// <summary>Budapest.</summary>
		[EnumMember(Value = "Europe/Budapest")]
		Europe_Budapest,

		/// <summary>Copenhagen.</summary>
		[EnumMember(Value = "Europe/Copenhagen")]
		Europe_Copenhagen,

		/// <summary>Dublin.</summary>
		[EnumMember(Value = "Europe/Dublin")]
		Europe_Dublin,

		/// <summary>Helsinki.</summary>
		[EnumMember(Value = "Europe/Helsinki")]
		Europe_Helsinki,

		/// <summary>Istanbul.</summary>
		[EnumMember(Value = "Europe/Istanbul")]
		Europe_Istanbul,

		/// <summary>Kiev.</summary>
		[EnumMember(Value = "Europe/Kiev")]
		Europe_Kiev,

		/// <summary>Lisbon.</summary>
		[EnumMember(Value = "Europe/Lisbon")]
		Europe_Lisbon,

		/// <summary>London.</summary>
		[EnumMember(Value = "Europe/London")]
		Europe_London,

		/// <summary>Luxembourg.</summary>
		[EnumMember(Value = "Europe/Luxembourg")]
		Europe_Luxembourg,

		/// <summary>Madrid.</summary>
		[EnumMember(Value = "Europe/Madrid")]
		Europe_Madrid,

		/// <summary>Moscow.</summary>
		[EnumMember(Value = "Europe/Moscow")]
		Europe_Moscow,

		/// <summary>Oslo.</summary>
		[EnumMember(Value = "Europe/Oslo")]
		Europe_Oslo,

		/// <summary>Paris.</summary>
		[EnumMember(Value = "Europe/Paris")]
		Europe_Paris,

		/// <summary>Prague Bratislava.</summary>
		[EnumMember(Value = "Europe/Prague")]
		Europe_Prague,

		/// <summary>Rome.</summary>
		[EnumMember(Value = "Europe/Rome")]
		Europe_Rome,

		/// <summary>Sofia.</summary>
		[EnumMember(Value = "Europe/Sofia")]
		Europe_Sofia,

		/// <summary>Stockholm.</summary>
		[EnumMember(Value = "Europe/Stockholm")]
		Europe_Stockholm,

		/// <summary>Vienna.</summary>
		[EnumMember(Value = "Europe/Vienna")]
		Europe_Vienna,

		/// <summary>Warsaw.</summary>
		[EnumMember(Value = "Europe/Warsaw")]
		Europe_Warsaw,

		/// <summary>Zurich.</summary>
		[EnumMember(Value = "Europe/Zurich")]
		Europe_Zurich,

		/// <summary>Independent State of Samoa.</summary>
		[EnumMember(Value = "Pacific/Apia")]
		Pacific_Apia,

		/// <summary>Auckland, Wellington.</summary>
		[EnumMember(Value = "Pacific/Auckland")]
		Pacific_Auckland,

		/// <summary>Fiji Islands, Marshall Islands.</summary>
		[EnumMember(Value = "Pacific/Fiji")]
		Pacific_Fiji,

		/// <summary>Hawaii.</summary>
		[EnumMember(Value = "Pacific/Honolulu")]
		Pacific_Honolulu,

		/// <summary>Midway Island, Samoa.</summary>
		[EnumMember(Value = "Pacific/Midway")]
		Pacific_Midway,

		/// <summary>New Caledonia.</summary>
		[EnumMember(Value = "Pacific/Noumea")]
		Pacific_Noumea,

		/// <summary>Pago Pago.</summary>
		[EnumMember(Value = "Pacific/Pago_Pago")]
		Pacific_Pago_Pago,

		/// <summary>Guam, Port Moresby.</summary>
		[EnumMember(Value = "Pacific/Port_Moresby")]
		Pacific_Port_Moresby,

		/// <summary>Solomon Islands.</summary>
		[EnumMember(Value = "SST")]
		SST,

		/// <summary>Universal Time UTC.</summary>
		[EnumMember(Value = "UTC")]
		UTC,
	}
}
