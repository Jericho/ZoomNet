using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// This event is triggered when a call history is deleted.
	/// </summary>
	public class PhoneCallHistoryDeletedEvent : PhoneCallLogOperationEvent
	{
		/// <summary>
		/// Gets or sets the deleted call log ids.
		/// Empty if <see cref="DeleteAll"/> is true.
		/// </summary>
		public string[] CallLogIds { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the user has deleted all call logs.
		/// </summary>
		public bool DeleteAll { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the account has enabled call log trash.
		/// </summary>
		public bool MoveToTrash { get; set; }

		/// <summary>
		/// Gets or sets the delete event time. If specified then all call logs prior to this time have been deleted.
		/// </summary>
		public DateTime? ExecuteTime { get; set; }
	}
}
