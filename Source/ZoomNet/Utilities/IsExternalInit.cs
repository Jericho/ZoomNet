#if NET48 || NETSTANDARD2_0
namespace System.Runtime.CompilerServices
{
	/// <summary>
	/// Provides compiler support for init-only setters on target frameworks that don't define IsExternalInit in the BCL.
	/// </summary>
	internal static class IsExternalInit
	{
	}
}
#endif
