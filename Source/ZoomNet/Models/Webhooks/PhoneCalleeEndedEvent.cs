using System;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>This event is triggered when the callee terminates a Zoom Phone call.</summary>
	public record PhoneCalleeEndedEvent : PhoneCallFlowEvent
	{
		/// <summary>Gets or sets the date and time when the ringing started.</summary>
		public DateTime RingingStartedOn { get; init; }

		/// <summary>Gets or sets the date and time when the callee answered the call.</summary>
		public DateTime? AnsweredOn { get; init; }

		/// <summary>Gets or sets the date and time when the call was ended by the callee.</summary>
		public DateTime EndedOn { get; init; }

		/// <summary>
		/// Gets or sets information about the entity that forwarded the call
		/// (if it was routed from a call queue, shared line group, shared lines or auto receptionist).
		/// </summary>
		public PhoneCallParty ForwardedBy { get; init; }

		/// <summary>
		/// Gets or sets information about auto receptionist if the call was indirectly routed from it
		/// (e.g. a routing path example: A -> auto receptionist -> call queue -> B).
		/// </summary>
		public PhoneCallParty RedirectForwardedBy { get; init; }
	}
}
