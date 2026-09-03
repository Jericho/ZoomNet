using System;

namespace ZoomNet.Utilities
{
	[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	internal class MultipleValuesEnumMemberAttribute : Attribute
	{
		public string DefaultValue { get; set; }

		public string[] OtherValues { get; set; }
	}
}
