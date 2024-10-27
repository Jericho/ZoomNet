using System;

namespace ZoomNet.Utilities
{
	internal class MultipleValuesEnumMemberAttribute : Attribute
	{
		public string DefaultValue { get; set; }

		public string[] OtherValues { get; set; }
	}
}
