using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Possible issue types in a Zoom room.
	/// </summary>
	public enum IssueType
	{
		/// <summary>
		/// Room Controller disconnected.
		/// </summary>
		[EnumMember(Value = "Room Controller disconnected")]
		RoomControllerDisconnected,

		/// <summary>
		/// Room Controller connected.
		/// </summary>
		[EnumMember(Value = "Room Controller connected")]
		RoomControllerConnected,

		/// <summary>
		/// Selected camera has disconnected.
		/// </summary>
		[EnumMember(Value = "Selected camera has disconnected")]
		SelectedCameraHasDisconnected,

		/// <summary>
		/// Selected camera is reconnected.
		/// </summary>
		[EnumMember(Value = "Selected camera is reconnected")]
		SelectedCameraIsReconnected,

		/// <summary>
		/// Selected microphone has disconnected.
		/// </summary>
		[EnumMember(Value = "Selected microphone has disconnected")]
		SelectedMicrophoneHasDisconnected,

		/// <summary>
		/// Selected microphone is reconnected.
		/// </summary>
		[EnumMember(Value = "Selected microphone is reconnected")]
		SelectedMicrophoneIsReconnected,

		/// <summary>
		/// Selected speaker has disconnected.
		/// </summary>
		[EnumMember(Value = "Selected speaker has disconnected")]
		SelectedSpeakerHasDisconnected,

		/// <summary>
		/// Selected speaker is reconnected.
		/// </summary>
		[EnumMember(Value = "Selected speaker is reconnected")]
		SelectedSpeakerIsReconnected,

		/// <summary>
		/// Zoom room is offline.
		/// </summary>
		[EnumMember(Value = "Zoom room is offline")]
		ZoomRoomIsOffline,

		/// <summary>
		/// Zoom room is online.
		/// </summary>
		[EnumMember(Value = "Zoom room is online")]
		ZoomRoomIsOnline,

		/// <summary>
		/// High CPU usage is detected.
		/// </summary>
		[EnumMember(Value = "High CPU usage is detected")]
		HighCpuUsageIsDetected,

		/// <summary>
		/// Low bandwidth network is detected.
		/// </summary>
		[EnumMember(Value = "Low bandwidth network is detected")]
		LowBandwidthNetworkIsDetected,

		/// <summary>
		/// Zoom Rooms Computer battery is low.
		/// </summary>
		[EnumMember(Value = "Zoom Rooms Computer battery is low")]
		ZoomRoomsComputerBatteryIsLow,

		/// <summary>
		/// Zoom Rooms Computer battery is normal.
		/// </summary>
		[EnumMember(Value = "Zoom Rooms Computer battery is normal")]
		ZoomRoomsComputerBatteryIsNormal,

		/// <summary>
		/// Zoom Rooms Computer disconnected.
		/// </summary>
		[EnumMember(Value = "Zoom Rooms Computer disconnected")]
		ZoomRoomsComputerDisconnected,

		/// <summary>
		/// Zoom Rooms Computer connected.
		/// </summary>
		[EnumMember(Value = "Zoom Rooms Computer connected")]
		ZoomRoomsComputerConnected,

		/// <summary>
		/// Zoom Rooms Computer is not charging.
		/// </summary>
		[EnumMember(Value = "Zoom Rooms Computer is not charging")]
		ZoomRoomsComputerIsNotCharging,

		/// <summary>
		/// Controller battery is low.
		/// </summary>
		[EnumMember(Value = "Controller battery is low")]
		ControllerBatteryIsLow,

		/// <summary>
		/// Controller battery is normal.
		/// </summary>
		[EnumMember(Value = "Controller battery is normal")]
		ControllerBatteryIsNormal,

		/// <summary>
		/// Controller disconnected.
		/// </summary>
		[EnumMember(Value = "Controller disconnected")]
		ControllerDisconnected,

		/// <summary>
		/// Controller connected.
		/// </summary>
		[EnumMember(Value = "Controller connected")]
		ControllerConnected,

		/// <summary>
		/// Controller is not charging.
		/// </summary>
		[EnumMember(Value = "Controller is not charging")]
		ControllerIsNotCharging,

		/// <summary>
		/// Scheduling Display battery is low.
		/// </summary>
		[EnumMember(Value = "Scheduling Display battery is low")]
		SchedulingDisplayBatteryIsLow,

		/// <summary>
		/// Scheduling Display battery is normal.
		/// </summary>
		[EnumMember(Value = "Scheduling Display battery is normal")]
		SchedulingDisplayBatteryIsNormal,

		/// <summary>
		/// Scheduling Display disconnected.
		/// </summary>
		[EnumMember(Value = "Scheduling Display disconnected")]
		SchedulingDisplayDisconnected,

		/// <summary>
		/// Scheduling Display connected.
		/// </summary>
		[EnumMember(Value = "Scheduling Display connected")]
		SchedulingDisplayConnected,

		/// <summary>
		/// Scheduling Display is not charging.
		/// </summary>
		[EnumMember(Value = "Scheduling Display is not charging")]
		SchedulingDisplayIsNotCharging,

		/// <summary>
		/// No camera.
		/// </summary>
		[EnumMember(Value = "No camera")]
		NoCamera,

		/// <summary>
		/// No microphone.
		/// </summary>
		[EnumMember(Value = "No microphone")]
		NoMicrophone
	}
}
