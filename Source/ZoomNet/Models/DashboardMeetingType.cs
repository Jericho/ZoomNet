using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of meeting metrics are being returned for.
	/// </summary>
	public enum DashboardMeetingType
	{
		/// <summary>
		/// Live.
		/// </summary>
		[EnumMember(Value = "live")]
		Live,

		/// <summary>
		/// Past
		/// </summary>
		[EnumMember(Value = "past")]
		Past,

		/// <summary>
		/// PastOne
		/// </summary>
		[EnumMember(Value = "pastOne")]
		PastOne
	}
}
