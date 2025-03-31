using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Usage information.
	/// </summary>
	public class BillingPlanUsageInfo
	{
		/// <summary>
		/// Gets or sets the Base plan usage information.
		/// </summary>
		[JsonPropertyName("plan_base")]
		public BillingBasePlanUsage BasePlan { get; set; }

		/// <summary>
		/// Gets or sets the Large Meetings plan usage information.
		/// </summary>
		[JsonPropertyName("plan_large_meeting")]
		public BillingPlanUsage[] LargeMeetingsPlan { get; set; }

		/// <summary>
		/// Gets or sets the Recording plan usage information.
		/// </summary>
		[JsonPropertyName("plan_recording")]
		public BillingRecordingPlanUsage RecordingPlan { get; set; }

		/// <summary>
		/// Gets or sets the United plan usage information.
		/// </summary>
		[JsonPropertyName("plan_united")]
		public BillingPlanUsage UnitedPlan { get; set; }

		/// <summary>
		/// Gets or sets the Webinar plan usage information.
		/// </summary>
		[JsonPropertyName("plan_webinar")]
		public BillingPlanUsage[] WebinarPlan { get; set; }

		/// <summary>
		/// Gets or sets the Events plan usage information.
		/// </summary>
		[JsonPropertyName("plan_zoom_events")]
		public BillingPlanUsage[] EventsPlan { get; set; }

		/// <summary>
		/// Gets or sets the Workplace plan usage information.
		/// </summary>
		[JsonPropertyName("plan_zoom_one")]
		public BillingPlanUsage[] WorkplacePlan { get; set; }

		/// <summary>
		/// Gets or sets the Rooms plan usage information.
		/// </summary>
		[JsonPropertyName("plan_zoom_rooms")]
		public BillingPlanUsage RoomsPlan { get; set; }

		/// <summary>
		/// Gets or sets the H.323/SIP Device Room Connector plan usage information.
		/// </summary>
		[JsonPropertyName("plan_room_connector")]
		public BillingPlanUsage[] DeviceRoomConnectorPlan { get; set; }

		/// <summary>
		/// Gets or sets the Whiteboard usage information.
		/// </summary>
		[JsonPropertyName("plan_whiteboard")]
		public BillingPlanUsage WhiteboardPlan { get; set; }

		/// <summary>
		/// Gets or sets the Whiteboard Plus plan usage information.
		/// </summary>
		[JsonPropertyName("plan_whiteboard_plus")]
		public BillingPlanUsage WhiteboardPlusPlan { get; set; }

		/// <summary>
		/// Gets or sets the Revenue Accelerator plan usage information.
		/// </summary>
		[JsonPropertyName("plan_zoom_iq")]
		public BillingPlanUsage RevenueAcceleratorPlan { get; set; }

		/// <summary>
		/// Gets or sets the Translation plan usage information.
		/// </summary>
		[JsonPropertyName("plan_ztransl")]
		public BillingPlanUsage TranslationPlan { get; set; }

		/// <summary>
		/// Gets or sets the Workspace Reservation plan usage information.
		/// </summary>
		[JsonPropertyName("plan_zwr")]
		public BillingPlanUsage[] WorkspaceReservationPlan { get; set; }

		/// <summary>
		/// Gets or sets the Virtual Agents plan usage information.
		/// </summary>
		[JsonPropertyName("plan_zva")]
		public BillingPlanUsage[] VirtualAgentsPlan { get; set; }

		/// <summary>
		/// Gets or sets the Node base plan usage information.
		/// </summary>
		[JsonPropertyName("plan_znode_base")]
		public BillingPlanUsage NodeBasePlan { get; set; }

		/// <summary>
		/// Gets or sets the Node Phone Hybrid plan usage information.
		/// </summary>
		[JsonPropertyName("plan_zphybrid")]
		public BillingPlanUsage NodePhoneHybridPlan { get; set; }

		/// <summary>
		/// Gets or sets the Scheduler plan usage information.
		/// </summary>
		[JsonPropertyName("plan_zsched")]
		public BillingPlanUsage SchedulerPlan { get; set; }

		/// <summary>
		/// Gets or sets the Meeting Hybrid plan usage information.
		/// </summary>
		[JsonPropertyName("plan_zmhybrid")]
		public BillingPlanUsage MeetingHybridPlan { get; set; }

		/// <summary>
		/// Gets or sets the Meeting Connector plan usage information.
		/// </summary>
		[JsonPropertyName("plan_mchybrid")]
		public BillingPlanUsage MeetingConnectorPlan { get; set; }

		/// <summary>
		/// Gets or sets the Recording Connector plan usage information.
		/// </summary>
		[JsonPropertyName("plan_rchybrid")]
		public BillingPlanUsage RecordingConnectorPlan { get; set; }

		/// <summary>
		/// Gets or sets the Recording Hybrid plan usage information.
		/// </summary>
		[JsonPropertyName("plan_cmrhybrid")]
		public BillingPlanUsage RecordingHybridPlan { get; set; }

		/// <summary>
		/// Gets or sets the Node Team Chat Hybrid plan usage information.
		/// </summary>
		[JsonPropertyName("plan_tchybrid")]
		public BillingPlanUsage NodeTeamChatHybridPlan { get; set; }

		/// <summary>
		/// Gets or sets the Workforce Management plan usage information.
		/// </summary>
		[JsonPropertyName("plan_workforce_management")]
		public BillingPlanUsage WorkforceManagementPlan { get; set; }

		/// <summary>
		/// Gets or sets the Workplace Enterprise Premier plan usage information.
		/// </summary>
		[JsonPropertyName("plan_zoom_one_premier")]
		public BillingPlanUsage[] WorkplaceEnterprisePremierPlan { get; set; }

		/// <summary>
		/// Gets or sets the Education School and Campus Plus plan usage information.
		/// </summary>
		[JsonPropertyName("plan_zoom_one_edu_school_campus_plus")]
		public BillingPlanUsage[] EducationSchoolAndCampusPlusPlan { get; set; }

		/// <summary>
		/// Gets or sets the Education Enterprise Premier plan usage information.
		/// </summary>
		[JsonPropertyName("plan_zoom_one_edu_premier")]
		public BillingPlanUsage[] EnterprisePremierPlan { get; set; }

		/// <summary>
		/// Gets or sets the Workplace for Education: Enterprise Higher Ed Student plan usage information.
		/// </summary>
		[JsonPropertyName("plan_zoom_one_edu_student")]
		public BillingPlanUsage WorkplaceForEducationEnterpriseHigherEdStudentPlan { get; set; }

		/// <summary>
		/// Gets or sets the Visitor Management plan usage information.
		/// </summary>
		[JsonPropertyName("plan_visitor_management")]
		public BillingPlanUsage VisitorManagementPlan { get; set; }

		/// <summary>
		/// Gets or sets the Partner Premier Support plan usage information.
		/// </summary>
		[JsonPropertyName("plan_partner_premier_support")]
		public BillingPlanUsage PartnerPremierSupportPlan { get; set; }
	}
}
