using System.Collections.Generic;

namespace ZoomNet.Json
{
	/// <summary>
	/// Converts an array of <see cref="KeyValuePair{String, String}"/> to or from JSON.
	/// </summary>
	/// <seealso cref="KeyValuePairConverter"/>
	internal class CustomQuestionsAnswersConverter : KeyValuePairConverter
	{
		public CustomQuestionsAnswersConverter()
			: base("title", "value")
		{
		}
	}
}
