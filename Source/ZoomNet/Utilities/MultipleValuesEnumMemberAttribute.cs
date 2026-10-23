using System;

namespace ZoomNet.Utilities
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	internal class MultipleValuesEnumMemberAttribute : Attribute
	{
		public string DefaultValue { get; set; }

		public string[] OtherValues { get; set; }
	}
}
