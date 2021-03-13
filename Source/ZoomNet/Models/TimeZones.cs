using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration pf the time zones supported by Zoom.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum TimeZones
	{
		/// <summary>Midway Island, Samoa.</summary>
		[EnumMember(Value = "Midway Island, Samoa")]
		Pacific_Midway,

		/// <summary>Pago Pago.</summary>
		[EnumMember(Value = "Pago Pago")]
		Pacific_Pago_Pago,

		/// <summary>Hawaii.</summary>
		[EnumMember(Value = "Hawaii")]
		Pacific_Honolulu,

		/// <summary>Alaska.</summary>
		[EnumMember(Value = "Alaska")]
		America_Anchorage,

		/// <summary>Vancouver.</summary>
		[EnumMember(Value = "Vancouver")]
		America_Vancouver,

		/// <summary>Pacific Time (US and Canada).</summary>
		[EnumMember(Value = "Pacific Time (US and Canada)")]
		America_Los_Angeles,

		/// <summary>Tijuana.</summary>
		[EnumMember(Value = "Tijuana")]
		America_Tijuana,

		/// <summary>Edmonton.</summary>
		[EnumMember(Value = "Edmonton")]
		America_Edmonton,

		/// <summary>Mountain Time (US and Canada).</summary>
		[EnumMember(Value = "Mountain Time (US and Canada)")]
		America_Denver,

		/// <summary>Arizona.</summary>
		[EnumMember(Value = "Arizona")]
		America_Phoenix,

		/// <summary>Mazatlan.</summary>
		[EnumMember(Value = "Mazatlan")]
		America_Mazatlan,

		/// <summary>Winnipeg.</summary>
		[EnumMember(Value = "Winnipeg")]
		America_Winnipeg,

		/// <summary>Saskatchewan.</summary>
		[EnumMember(Value = "Saskatchewan")]
		America_Regina,

		/// <summary>Central Time (US and Canada).</summary>
		[EnumMember(Value = "Central Time (US and Canada)")]
		America_Chicago,

		/// <summary>Mexico City.</summary>
		[EnumMember(Value = "Mexico City")]
		America_Mexico_City,

		/// <summary>Guatemala.</summary>
		[EnumMember(Value = "Guatemala")]
		America_Guatemala,

		/// <summary>El Salvador.</summary>
		[EnumMember(Value = "El Salvador")]
		America_El_Salvador,

		/// <summary>Managua.</summary>
		[EnumMember(Value = "Managua")]
		America_Managua,

		/// <summary>Costa Rica.</summary>
		[EnumMember(Value = "Costa Rica")]
		America_Costa_Rica,

		/// <summary>Montreal.</summary>
		[EnumMember(Value = "Montreal")]
		America_Montreal,

		/// <summary>Eastern Time (US and Canada).</summary>
		[EnumMember(Value = "Eastern Time (US and Canada)")]
		America_New_York,

		/// <summary>Indiana (East).</summary>
		[EnumMember(Value = "Indiana (East)")]
		America_Indianapolis,

		/// <summary>Panama.</summary>
		[EnumMember(Value = "Panama")]
		America_Panama,

		/// <summary>Bogota.</summary>
		[EnumMember(Value = "Bogota")]
		America_Bogota,

		/// <summary>Lima.</summary>
		[EnumMember(Value = "Lima")]
		America_Lima,

		/// <summary>Halifax.</summary>
		[EnumMember(Value = "Halifax")]
		America_Halifax,

		/// <summary>Puerto Rico.</summary>
		[EnumMember(Value = "Puerto Rico")]
		America_Puerto_Rico,

		/// <summary>Caracas.</summary>
		[EnumMember(Value = "Caracas")]
		America_Caracas,

		/// <summary>Santiago.</summary>
		[EnumMember(Value = "Santiago")]
		America_Santiago,

		/// <summary>Newfoundland and Labrador.</summary>
		[EnumMember(Value = "Newfoundland and Labrador")]
		America_St_Johns,

		/// <summary>Montevideo.</summary>
		[EnumMember(Value = "Montevideo")]
		America_Montevideo,

		/// <summary>Brasilia.</summary>
		[EnumMember(Value = "Brasilia")]
		America_Araguaina,

		/// <summary>Buenos Aires, Georgetown.</summary>
		[EnumMember(Value = "Buenos Aires, Georgetown")]
		America_Argentina_Buenos_Aires,

		/// <summary>Greenland.</summary>
		[EnumMember(Value = "Greenland")]
		America_Godthab,

		/// <summary>Sao Paulo.</summary>
		[EnumMember(Value = "Sao Paulo")]
		America_Sao_Paulo,

		/// <summary>Azores.</summary>
		[EnumMember(Value = "Azores")]
		Atlantic_Azores,

		/// <summary>Atlantic Time (Canada).</summary>
		[EnumMember(Value = "Atlantic Time (Canada)")]
		Canada_Atlantic,

		/// <summary>Cape Verde Islands.</summary>
		[EnumMember(Value = "Cape Verde Islands")]
		Atlantic_Cape_Verde,

		/// <summary>Universal Time UTC.</summary>
		[EnumMember(Value = "Universal Time UTC")]
		UTC,

		/// <summary>Greenwich Mean Time.</summary>
		[EnumMember(Value = "Greenwich Mean Time")]
		Etc_Greenwich,

		/// <summary>Belgrade, Bratislava, Ljubljana.</summary>
		[EnumMember(Value = "Belgrade, Bratislava, Ljubljana")]
		Europe_Belgrade,

		/// <summary>Sarajevo, Skopje, Zagreb.</summary>
		[EnumMember(Value = "Sarajevo, Skopje, Zagreb")]
		CET,

		/// <summary>Reykjavik.</summary>
		[EnumMember(Value = "Reykjavik")]
		Atlantic_Reykjavik,

		/// <summary>Dublin.</summary>
		[EnumMember(Value = "Dublin")]
		Europe_Dublin,

		/// <summary>London.</summary>
		[EnumMember(Value = "London")]
		Europe_London,

		/// <summary>Lisbon.</summary>
		[EnumMember(Value = "Lisbon")]
		Europe_Lisbon,

		/// <summary>Casablanca.</summary>
		[EnumMember(Value = "Casablanca")]
		Africa_Casablanca,

		/// <summary>Nouakchott.</summary>
		[EnumMember(Value = "Nouakchott")]
		Africa_Nouakchott,

		/// <summary>Oslo.</summary>
		[EnumMember(Value = "Oslo")]
		Europe_Oslo,

		/// <summary>Copenhagen.</summary>
		[EnumMember(Value = "Copenhagen")]
		Europe_Copenhagen,

		/// <summary>Brussels.</summary>
		[EnumMember(Value = "Brussels")]
		Europe_Brussels,

		/// <summary>Amsterdam, Berlin, Rome, Stockholm, Vienna.</summary>
		[EnumMember(Value = "Amsterdam, Berlin, Rome, Stockholm, Vienna")]
		Europe_Berlin,

		/// <summary>Helsinki.</summary>
		[EnumMember(Value = "Helsinki")]
		Europe_Helsinki,

		/// <summary>Amsterdam.</summary>
		[EnumMember(Value = "Amsterdam")]
		Europe_Amsterdam,

		/// <summary>Rome.</summary>
		[EnumMember(Value = "Rome")]
		Europe_Rome,

		/// <summary>Stockholm.</summary>
		[EnumMember(Value = "Stockholm")]
		Europe_Stockholm,

		/// <summary>Vienna.</summary>
		[EnumMember(Value = "Vienna")]
		Europe_Vienna,

		/// <summary>Luxembourg.</summary>
		[EnumMember(Value = "Luxembourg")]
		Europe_Luxembourg,

		/// <summary>Paris.</summary>
		[EnumMember(Value = "Paris")]
		Europe_Paris,

		/// <summary>Zurich.</summary>
		[EnumMember(Value = "Zurich")]
		Europe_Zurich,

		/// <summary>Madrid.</summary>
		[EnumMember(Value = "Madrid")]
		Europe_Madrid,

		/// <summary>West Central Africa.</summary>
		[EnumMember(Value = "West Central Africa")]
		Africa_Bangui,

		/// <summary>Algiers.</summary>
		[EnumMember(Value = "Algiers")]
		Africa_Algiers,

		/// <summary>Tunis.</summary>
		[EnumMember(Value = "Tunis")]
		Africa_Tunis,

		/// <summary>Harare, Pretoria.</summary>
		[EnumMember(Value = "Harare, Pretoria")]
		Africa_Harare,

		/// <summary>Nairobi.</summary>
		[EnumMember(Value = "Nairobi")]
		Africa_Nairobi,

		/// <summary>Warsaw.</summary>
		[EnumMember(Value = "Warsaw")]
		Europe_Warsaw,

		/// <summary>Prague Bratislava.</summary>
		[EnumMember(Value = "Prague Bratislava")]
		Europe_Prague,

		/// <summary>Budapest.</summary>
		[EnumMember(Value = "Budapest")]
		Europe_Budapest,

		/// <summary>Sofia.</summary>
		[EnumMember(Value = "Sofia")]
		Europe_Sofia,

		/// <summary>Istanbul.</summary>
		[EnumMember(Value = "Istanbul")]
		Europe_Istanbul,

		/// <summary>Athens.</summary>
		[EnumMember(Value = "Athens")]
		Europe_Athens,

		/// <summary>Bucharest.</summary>
		[EnumMember(Value = "Bucharest")]
		Europe_Bucharest,

		/// <summary>Nicosia.</summary>
		[EnumMember(Value = "Nicosia")]
		Asia_Nicosia,

		/// <summary>Beirut.</summary>
		[EnumMember(Value = "Beirut")]
		Asia_Beirut,

		/// <summary>Damascus.</summary>
		[EnumMember(Value = "Damascus")]
		Asia_Damascus,

		/// <summary>Jerusalem.</summary>
		[EnumMember(Value = "Jerusalem")]
		Asia_Jerusalem,

		/// <summary>Amman.</summary>
		[EnumMember(Value = "Amman")]
		Asia_Amman,

		/// <summary>Tripoli.</summary>
		[EnumMember(Value = "Tripoli")]
		Africa_Tripoli,

		/// <summary>Cairo.</summary>
		[EnumMember(Value = "Cairo")]
		Africa_Cairo,

		/// <summary>Johannesburg.</summary>
		[EnumMember(Value = "Johannesburg")]
		Africa_Johannesburg,

		/// <summary>Moscow.</summary>
		[EnumMember(Value = "Moscow")]
		Europe_Moscow,

		/// <summary>Baghdad.</summary>
		[EnumMember(Value = "Baghdad")]
		Asia_Baghdad,

		/// <summary>Kuwait.</summary>
		[EnumMember(Value = "Kuwait")]
		Asia_Kuwait,

		/// <summary>Riyadh.</summary>
		[EnumMember(Value = "Riyadh")]
		Asia_Riyadh,

		/// <summary>Bahrain.</summary>
		[EnumMember(Value = "Bahrain")]
		Asia_Bahrain,

		/// <summary>Qatar.</summary>
		[EnumMember(Value = "Qatar")]
		Asia_Qatar,

		/// <summary>Aden.</summary>
		[EnumMember(Value = "Aden")]
		Asia_Aden,

		/// <summary>Tehran.</summary>
		[EnumMember(Value = "Tehran")]
		Asia_Tehran,

		/// <summary>Khartoum.</summary>
		[EnumMember(Value = "Khartoum")]
		Africa_Khartoum,

		/// <summary>Djibouti.</summary>
		[EnumMember(Value = "Djibouti")]
		Africa_Djibouti,

		/// <summary>Mogadishu.</summary>
		[EnumMember(Value = "Mogadishu")]
		Africa_Mogadishu,

		/// <summary>Dubai.</summary>
		[EnumMember(Value = "Dubai")]
		Asia_Dubai,

		/// <summary>Muscat.</summary>
		[EnumMember(Value = "Muscat")]
		Asia_Muscat,

		/// <summary>Baku, Tbilisi, Yerevan.</summary>
		[EnumMember(Value = "Baku, Tbilisi, Yerevan")]
		Asia_Baku,

		/// <summary>Kabul.</summary>
		[EnumMember(Value = "Kabul")]
		Asia_Kabul,

		/// <summary>Yekaterinburg.</summary>
		[EnumMember(Value = "Yekaterinburg")]
		Asia_Yekaterinburg,

		/// <summary>Islamabad, Karachi, Tashkent.</summary>
		[EnumMember(Value = "Islamabad, Karachi, Tashkent")]
		Asia_Tashkent,

		/// <summary>India.</summary>
		[EnumMember(Value = "India")]
		Asia_Calcutta,

		/// <summary>Kathmandu.</summary>
		[EnumMember(Value = "Kathmandu")]
		Asia_Kathmandu,

		/// <summary>Novosibirsk.</summary>
		[EnumMember(Value = "Novosibirsk")]
		Asia_Novosibirsk,

		/// <summary>Almaty.</summary>
		[EnumMember(Value = "Almaty")]
		Asia_Almaty,

		/// <summary>Dacca.</summary>
		[EnumMember(Value = "Dacca")]
		Asia_Dacca,

		/// <summary>Krasnoyarsk.</summary>
		[EnumMember(Value = "Krasnoyarsk")]
		Asia_Krasnoyarsk,

		/// <summary>Astana, Dhaka.</summary>
		[EnumMember(Value = "Astana, Dhaka")]
		Asia_Dhaka,

		/// <summary>Bangkok.</summary>
		[EnumMember(Value = "Bangkok")]
		Asia_Bangkok,

		/// <summary>Vietnam.</summary>
		[EnumMember(Value = "Vietnam")]
		Asia_Saigon,

		/// <summary>Jakarta.</summary>
		[EnumMember(Value = "Jakarta")]
		Asia_Jakarta,

		/// <summary>Irkutsk, Ulaanbaatar.</summary>
		[EnumMember(Value = "Irkutsk, Ulaanbaatar")]
		Asia_Irkutsk,

		/// <summary>Beijing, Shanghai.</summary>
		[EnumMember(Value = "Beijing, Shanghai")]
		Asia_Shanghai,

		/// <summary>Hong Kong.</summary>
		[EnumMember(Value = "Hong Kong")]
		Asia_Hong_Kong,

		/// <summary>Taipei.</summary>
		[EnumMember(Value = "Taipei")]
		Asia_Taipei,

		/// <summary>Kuala Lumpur.</summary>
		[EnumMember(Value = "Kuala Lumpur")]
		Asia_Kuala_Lumpur,

		/// <summary>Singapore.</summary>
		[EnumMember(Value = "Singapore")]
		Asia_Singapore,

		/// <summary>Perth.</summary>
		[EnumMember(Value = "Perth")]
		Australia_Perth,

		/// <summary>Yakutsk.</summary>
		[EnumMember(Value = "Yakutsk")]
		Asia_Yakutsk,

		/// <summary>Seoul.</summary>
		[EnumMember(Value = "Seoul")]
		Asia_Seoul,

		/// <summary>Osaka, Sapporo, Tokyo.</summary>
		[EnumMember(Value = "Osaka, Sapporo, Tokyo")]
		Asia_Tokyo,

		/// <summary>Darwin.</summary>
		[EnumMember(Value = "Darwin")]
		Australia_Darwin,

		/// <summary>Adelaide.</summary>
		[EnumMember(Value = "Adelaide")]
		Australia_Adelaide,

		/// <summary>Vladivostok.</summary>
		[EnumMember(Value = "Vladivostok")]
		Asia_Vladivostok,

		/// <summary>Guam, Port Moresby.</summary>
		[EnumMember(Value = "Guam, Port Moresby")]
		Pacific_Port_Moresby,

		/// <summary>Brisbane.</summary>
		[EnumMember(Value = "Brisbane")]
		Australia_Brisbane,

		/// <summary>Canberra, Melbourne, Sydney.</summary>
		[EnumMember(Value = "Canberra, Melbourne, Sydney")]
		Australia_Sydney,

		/// <summary>Hobart.</summary>
		[EnumMember(Value = "Hobart")]
		Australia_Hobart,

		/// <summary>Magadan.</summary>
		[EnumMember(Value = "Magadan")]
		Asia_Magadan,

		/// <summary>Solomon Islands.</summary>
		[EnumMember(Value = "Solomon Islands")]
		SST,

		/// <summary>New Caledonia.</summary>
		[EnumMember(Value = "New Caledonia")]
		Pacific_Noumea,

		/// <summary>Kamchatka.</summary>
		[EnumMember(Value = "Kamchatka")]
		Asia_Kamchatka,

		/// <summary>Fiji Islands, Marshall Islands.</summary>
		[EnumMember(Value = "Fiji Islands, Marshall Islands")]
		Pacific_Fiji,

		/// <summary>Auckland, Wellington.</summary>
		[EnumMember(Value = "Auckland, Wellington")]
		Pacific_Auckland,

		/// <summary>Mumbai, Kolkata, New Delhi.</summary>
		[EnumMember(Value = "Mumbai, Kolkata, New Delhi")]
		Asia_Kolkata,

		/// <summary>Kiev.</summary>
		[EnumMember(Value = "Kiev")]
		Europe_Kiev,

		/// <summary>Tegucigalpa.</summary>
		[EnumMember(Value = "Tegucigalpa")]
		America_Tegucigalpa,

		/// <summary>Independent State of Samoa.</summary>
		[EnumMember(Value = "Independent State of Samoa")]
		Pacific_Apia,
	}
}
