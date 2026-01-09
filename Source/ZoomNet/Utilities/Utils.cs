using Microsoft.IO;
using System;
using System.Web;

namespace ZoomNet.Utilities
{
	/// <summary>
	/// Utils.
	/// </summary>
	internal static class Utils
	{
		public static RecyclableMemoryStreamManager MemoryStreamManager { get; } = new RecyclableMemoryStreamManager();

		public static void ValidateRecordPerPage(int recordsPerPage, int min = 1, int max = 300)
		{
			if (recordsPerPage < min || recordsPerPage > max)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), $"Records per page must be between {min} and {max}");
			}
		}

		/// <summary>
		/// Encodes a UUID string for safe use in URLs by applying double URL encoding if the string contains a forward slash
		/// ('/').
		/// </summary>
		/// <remarks>Double URL encoding is applied to ensure that forward slashes in the UUID are preserved when used
		/// as part of a URL path segment. This is useful when UUIDs may contain characters that could be interpreted as path
		/// separators.</remarks>
		/// <param name="uuid">The UUID string to encode. If the string is null, empty, or does not contain a forward slash ('/'), it is returned
		/// unchanged.</param>
		/// <returns>A double URL-encoded version of the input string if it contains a forward slash ('/'); otherwise, the original
		/// string.</returns>
		public static string EncodeUUID(string uuid)
		{
			if (string.IsNullOrEmpty(uuid)) return uuid;
			if (!uuid.Contains("/")) return uuid;
			return HttpUtility.UrlEncode(HttpUtility.UrlEncode(uuid));
		}
	}
}
