using System.Text.Json;
using Shouldly;
using Xunit;
using ZoomNet.Json;
using ZoomNet.Models;

namespace ZoomNet.UnitTests.Models
{
	public class PhoneCallUserProfileTests
	{
		#region constants

		internal const string PHONE_CALL_USER_PROFILE = @"{
		  ""calling_plans"": [
		    {
		      ""type"": 600,
		      ""billing_account_id"": ""3WWAEiEjTj2IQuyDiKMd_A"",
		      ""billing_account_name"": ""Delhi billing""
		    }
		  ],
		  ""cost_center"": ""testCostCenter"",
		  ""department"": ""testDepartment"",
		  ""email"": ""suesu_test_delete3@testapi.com"",
		  ""emergency_address"": {
		    ""address_line1"": ""55 Almaden Boulevard"",
		    ""address_line2"": ""1002 Airport Way S"",
		    ""city"": ""SAN JOSE"",
		    ""country"": ""US"",
		    ""id"": ""CCc8zYT1SN60i7uDMzDbXA"",
		    ""state_code"": ""CA"",
		    ""zip"": ""95113""
		  },
		  ""extension_id"": ""nNGsNx2zRDyiIXWVI23FCQ"",
		  ""extension_number"": 100012347,
		  ""id"": ""NL3cEpSdRc-c2t8aLoZqiw"",
		  ""phone_numbers"": [
		    {
		      ""id"": ""---M1padRvSUtw7YihN7sA"",
		      ""number"": ""14232058798""
		    }
		  ],
		  ""phone_user_id"": ""u7pnC468TaS46OuNoEw6GA"",
		  ""policy"": {
		    ""ad_hoc_call_recording"": {
		      ""enable"": true,
		      ""recording_start_prompt"": true,
		      ""recording_transcription"": true,
		      ""play_recording_beep_tone"": {
		        ""enable"": true,
		        ""play_beep_volume"": 60,
		        ""play_beep_time_interval"": 15,
		        ""play_beep_member"": ""allMember""
		      },
		      ""locked"": true,
		      ""locked_by"": ""account""
		    },
		    ""ad_hoc_call_recording_access_members"": [
		      {
		        ""access_user_id"": ""w0RChiauQeqRlv5fgxYULQ"",
		        ""allow_delete"": false,
		        ""allow_download"": false,
		        ""shared_id"": ""--e8ugg0SeS-9clgrDkn2w""
		      }
		    ],
		    ""auto_call_recording"": {
		      ""allow_stop_resume_recording"": true,
		      ""disconnect_on_recording_failure"": true,
		      ""enable"": true,
		      ""locked"": true,
		      ""locked_by"": ""account"",
		      ""recording_calls"": ""inbound"",
		      ""recording_explicit_consent"": true,
		      ""recording_start_prompt"": true,
		      ""recording_transcription"": true,
		      ""play_recording_beep_tone"": {
		        ""enable"": true,
		        ""play_beep_volume"": 60,
		        ""play_beep_time_interval"": 15,
		        ""play_beep_member"": ""allMember""
		      }
		    },
		    ""auto_call_recording_access_members"": [
		      {
		        ""access_user_id"": ""w0RChiauQeqRlv5fgxYULQ"",
		        ""allow_delete"": false,
		        ""allow_download"": false,
		        ""shared_id"": ""--e8ugg0SeS-9clgrDkn2w""
		      }
		    ],
		    ""call_overflow"": {
		      ""call_overflow_type"": 1,
		      ""enable"": true,
		      ""locked"": true,
		      ""locked_by"": ""site"",
		      ""modified"": true
		    },
		    ""call_park"": {
		      ""call_not_picked_up_action"": 9,
		      ""enable"": true,
		      ""expiration_period"": 10,
		      ""forward_to"": {
		        ""display_name"": ""test extension name"",
		        ""extension_id"": ""CcrEGgmeQem1uyJsuIRKwA"",
		        ""extension_number"": 1000123477,
		        ""extension_type"": ""user"",
		        ""id"": ""fWOgOALdT1ei4vjXK-QYsA""
		      },
		      ""locked"": true,
		      ""locked_by"": ""account""
		    },
		    ""call_transferring"": {
		      ""call_transferring_type"": 2,
		      ""enable"": true,
		      ""locked"": true,
		      ""locked_by"": ""account""
		    },
		    ""delegation"": true,
		    ""elevate_to_meeting"": true,
		    ""emergency_address_management"": {
		      ""enable"": true,
		      ""prompt_default_address"": true
		    },
		    ""emergency_calls_to_psap"": true,
		    ""call_handling_forwarding_to_other_users"": {
		      ""enable"": true,
		      ""call_forwarding_type"": 1,
		      ""locked"": true,
		      ""locked_by"": ""site"",
		      ""modified"": true
		    },
		    ""hand_off_to_room"": {
		      ""enable"": true,
		      ""locked"": true,
		      ""locked_by"": ""account""
		    },
		    ""international_calling"": true,
		    ""mobile_switch_to_carrier"": {
		      ""enable"": true,
		      ""locked"": true,
		      ""locked_by"": ""account""
		    },
		    ""select_outbound_caller_id"": {
		      ""enable"": true,
		      ""allow_hide_outbound_caller_id"": true,
		      ""locked"": true,
		      ""locked_by"": ""account""
		    },
		    ""sms"": {
		      ""enable"": true,
		      ""international_sms"": true,
		      ""international_sms_countries"": [
		        ""US""
		      ],
		      ""locked"": true,
		      ""locked_by"": ""account""
		    },
		    ""voicemail"": {
		      ""allow_delete"": true,
		      ""allow_download"": true,
		      ""allow_transcription"": true,
		      ""allow_videomail"": true,
		      ""enable"": true
		    },
		    ""voicemail_access_members"": [
		      {
		        ""access_user_id"": ""w0RChiauQeqRlv5fgxYULQ"",
		        ""allow_delete"": false,
		        ""allow_download"": false,
		        ""allow_sharing"": false,
		        ""shared_id"": ""--e8ugg0SeS-9clgrDkn2w""
		      }
		    ],
		    ""zoom_phone_on_mobile"": {
		      ""allow_calling_sms_mms"": true,
		      ""enable"": true,
		      ""locked"": true,
		      ""locked_by"": ""account""
		    },
		    ""personal_audio_library"": {
		      ""enable"": true,
		      ""locked"": true,
		      ""locked_by"": ""account"",
		      ""modified"": true,
		      ""allow_music_on_hold_customization"": true,
		      ""allow_voicemail_and_message_greeting_customization"": true
		    },
		    ""voicemail_transcription"": {
		      ""enable"": true,
		      ""locked"": true,
		      ""locked_by"": ""account"",
		      ""modified"": true
		    },
		    ""voicemail_notification_by_email"": {
		      ""include_voicemail_file"": true,
		      ""include_voicemail_transcription"": false,
		      ""enable"": true,
		      ""locked"": true,
		      ""locked_by"": ""account"",
		      ""modified"": true
		    },
		    ""shared_voicemail_notification_by_email"": {
		      ""enable"": true,
		      ""locked"": true,
		      ""locked_by"": ""account"",
		      ""modified"": true
		    },
		    ""check_voicemails_over_phone"": {
		      ""enable"": true,
		      ""locked"": true,
		      ""locked_by"": ""account"",
		      ""modified"": true
		    },
		    ""audio_intercom"": {
		      ""enable"": true,
		      ""locked"": true,
		      ""locked_by"": ""account"",
		      ""modified"": true
		    },
		    ""peer_to_peer_media"": {
		      ""enable"": true,
		      ""locked"": true,
		      ""locked_by"": ""account"",
		      ""modified"": true
		    },
		    ""e2e_encryption"": {
		      ""enable"": true,
		      ""locked"": true,
		      ""locked_by"": ""site"",
		      ""modified"": true
		    },
		    ""outbound_calling"": {
		      ""enable"": true,
		      ""locked"": true,
		      ""modified"": true
		    },
		    ""outbound_sms"": {
		      ""enable"": true,
		      ""locked"": true,
		      ""modified"": true
		    }
		  },
		  ""site_admin"": true,
		  ""site_id"": ""8f71O6rWT8KFUGQmJIFAdQ"",
		  ""status"": ""activate""
				}";

		#endregion

		#region tests

		[Fact]
		public void Parse_Json_PhoneCallUserProfile()
		{
			// Arrange

			// Act
			var result = JsonSerializer.Deserialize<PhoneCallUserProfile>(
				PHONE_CALL_USER_PROFILE, JsonFormatter.SerializerOptions);

			// Assert
			result.CallingPlans.ShouldNotBeNull();
			result.CallingPlans.ShouldNotBeEmpty();
			result.CallingPlans[0].Type.ShouldBe(CallingPlanType.Beta);
			result.CallingPlans[0].BillingAccountId.ShouldBe("3WWAEiEjTj2IQuyDiKMd_A");
			result.CallingPlans[0].BillingAccountName.ShouldBe("Delhi billing");
			result.CostCenter.ShouldBe("testCostCenter");
			result.Department.ShouldBe("testDepartment");
			result.Email.ShouldBe("suesu_test_delete3@testapi.com");
			result.EmergencyAddress.ShouldNotBeNull();
			result.EmergencyAddress.AddressLine1.ShouldBe("55 Almaden Boulevard");
			result.EmergencyAddress.AddressLine2.ShouldBe("1002 Airport Way S");
			result.EmergencyAddress.City.ShouldBe("SAN JOSE");
			result.EmergencyAddress.Country.ShouldBe("US");
			result.EmergencyAddress.Id.ShouldBe("CCc8zYT1SN60i7uDMzDbXA");
			result.EmergencyAddress.StateCode.ShouldBe("CA");
			result.EmergencyAddress.Zip.ShouldBe("95113");
			result.ExtensionId.ShouldBe("nNGsNx2zRDyiIXWVI23FCQ");
			result.ExtensionNumber.ShouldBe(100012347);
			result.Id.ShouldBe("NL3cEpSdRc-c2t8aLoZqiw");
			result.PhoneNumbers.ShouldNotBeNull();
			result.PhoneNumbers.ShouldNotBeEmpty();
			result.PhoneNumbers[0].PhoneNumberId.ShouldBe("---M1padRvSUtw7YihN7sA");
			result.PhoneNumbers[0].PhoneNumber.ShouldBe("14232058798");
			result.PhoneUserId.ShouldBe("u7pnC468TaS46OuNoEw6GA");
			result.SiteAdmin.ShouldBeTrue();
			result.SiteId.ShouldBe("8f71O6rWT8KFUGQmJIFAdQ");
			result.Status.ShouldBe(PhoneCallUserStatus.Active);
		}

		#endregion
	}
}
