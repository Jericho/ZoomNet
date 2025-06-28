using System.Collections.Generic;

namespace ZoomNet.Json
{
	/// <summary>
	/// Converts an array of <see cref="KeyValuePair{String, String}"/> to or from JSON.
	/// </summary>
	/// <seealso cref="KeyValuePairConverter"/>
	internal class TicketCustomQuestionsAnswersConverter : KeyValuePairConverter
	{
		public TicketCustomQuestionsAnswersConverter()
			: base("title", "answer")
		{
		}
	}
}
