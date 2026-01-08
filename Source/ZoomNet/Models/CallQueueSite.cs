using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ZoomNet.Models
{
	/// <summary>
	/// Represents a site where a Call Queue is assigned.
	/// </summary>
	public class CallQueueSite
	{
		/// <summary>
		/// Gets or sets the Unique identifier of the site where the Call Queue is assigned.
		/// </summary>
		[JsonPropertyName("id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the site.
		/// </summary>
		[JsonPropertyName("name")]
		public string Name { get; set; }
	}
}
