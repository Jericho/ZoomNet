using RichardSzalay.MockHttp;
using Shouldly;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ZoomNet.Json;
using ZoomNet.Models;
using ZoomNet.Resources;

namespace ZoomNet.UnitTests.Resources
{
	public class CloudRecordingsTests
	{
		#region FIELDS

		private const string SINGLE_CLOUD_RECORDING_JSON = @"{
			""uuid"": ""ODfDKShNRqKkXbGD09Sk4A=="",
			""id"": 94488262913,
			""account_id"": ""wTJw8LgeT8GlOSjLjOU9Hw"",
			""host_id"": ""uET9c2fCR06UoPbeqKed4A"",
			""topic"": ""ფებრილური ბავშვი და კოვიდ საშიში შემთხვევების მართვა - 8.2.2021"",
			""type"": 2,
			""start_time"": ""2021-02-11T11:40:38Z"",
			""timezone"": ""UTC"",
			""duration"": 120,
			""total_size"": 0,
			""recording_count"": 0,
			""share_url"": ""https://zoom.us/rec/share/X_pJ8dSmA_j4Ikbge2RGDaG3PVlBreUYs54RkvCHs7-P_15CVOKqcAbo8KQUfhFx.zqpEekmacjnoL_nG"",
			""recording_files"": [
			{
				""id"": ""d78d0d2d-5890-45be-ad69-85972ce0de82"",
				""meeting_id"": ""ODfDKShNRqKkXbGD09Sk4A=="",
				""recording_start"": ""2021-02-11T11:40:39Z"",
				""recording_end"": """",
				""file_type"": """",
				""file_extension"": """",
				""file_size"": 0,
				""play_url"": ""https://zoom.us/rec/play/3YUQu4dKV9c_sAwj-X33a_IGBr5V57LQFaTSwubo9R-hl3Gj_z-wSBSJNX9p5MFS8VsCEll4iAMyrDZi.h6gWLE07kjtTpZtX"",
				""download_url"": ""https://zoom.us/rec/download/3YUQu4dKV9c_sAwj-X33a_IGBr5V57LQFaTSwubo9R-hl3Gj_z-wSBSJNX9p5MFS8VsCEll4iAMyrDZi.h6gWLE07kjtTpZtX"",
				""status"": ""processing""
			}]
		}";

		private const string MULTIPLE_CLOUD_RECORDINGS_JSON = @"{
			""from"": ""2021-02-10"",
			""to"": ""2021-02-11"",
			""page_count"": 1,
			""page_size"": 30,
			""total_records"": 10,
			""next_page_token"": """",
			""meetings"": [
			{
				""uuid"": ""ODfDKShNRqKkXbGD09Sk4A=="",
				""id"": 94488262913,
				""account_id"": ""wTJw8LgeT8GlOSjLjOU9Hw"",
				""host_id"": ""uET9c2fCR06UoPbeqKed4A"",
				""topic"": ""ფებრილური ბავშვი და კოვიდ საშიში შემთხვევების მართვა - 8.2.2021"",
				""type"": 2,
				""start_time"": ""2021-02-11T11:40:38Z"",
				""timezone"": ""UTC"",
				""duration"": 120,
				""total_size"": 0,
				""recording_count"": 0,
				""share_url"": ""https://zoom.us/rec/share/X_pJ8dSmA_j4Ikbge2RGDaG3PVlBreUYs54RkvCHs7-P_15CVOKqcAbo8KQUfhFx.zqpEekmacjnoL_nG"",
				""recording_files"": [
				{
					""id"": ""d78d0d2d-5890-45be-ad69-85972ce0de82"",
					""meeting_id"": ""ODfDKShNRqKkXbGD09Sk4A=="",
					""recording_start"": ""2021-02-11T11:40:39Z"",
					""recording_end"": """",
					""file_type"": """",
					""file_extension"": """",
					""file_size"": 0,
					""play_url"": ""https://zoom.us/rec/play/3YUQu4dKV9c_sAwj-X33a_IGBr5V57LQFaTSwubo9R-hl3Gj_z-wSBSJNX9p5MFS8VsCEll4iAMyrDZi.h6gWLE07kjtTpZtX"",
					""download_url"": ""https://zoom.us/rec/download/3YUQu4dKV9c_sAwj-X33a_IGBr5V57LQFaTSwubo9R-hl3Gj_z-wSBSJNX9p5MFS8VsCEll4iAMyrDZi.h6gWLE07kjtTpZtX"",
					""status"": ""processing""
				}]
			},
			{
				""uuid"": ""v97o7qQASViWo36RwSMfWQ=="",
				""id"": 96598442130,
				""account_id"": ""wTJw8LgeT8GlOSjLjOU9Hw"",
				""host_id"": ""uET9c2fCR06UoPbeqKed4A"",
				""topic"": ""ფებრილური ბავშვი და კოვიდ საშიში შემთხვევების მართვა - 11.2.2021"",
				""type"": 2,
				""start_time"": ""2021-02-11T10:19:48Z"",
				""timezone"": ""UTC"",
				""duration"": 0,
				""total_size"": 90961,
				""recording_count"": 2,
				""share_url"": ""https://zoom.us/rec/share/C8v1I_luOMx3VqrKxJNdi0D_rxOmMh6Dzs_fYEP040e9sl8PF0QHNdOzbiSX0-iB.vG41YG43qNquTnJ_"",
				""recording_files"": [
				{
					""id"": ""3812c087-131e-4ebe-92b8-a868eb1a8e3c"",
					""meeting_id"": ""v97o7qQASViWo36RwSMfWQ=="",
					""recording_start"": ""2021-02-11T10:19:50Z"",
					""recording_end"": ""2021-02-11T10:19:52Z"",
					""file_type"": ""M4A"",
					""file_extension"": ""M4A"",
					""file_size"": 41483,
					""play_url"": ""https://zoom.us/rec/play/EfmkVJijpXd7tsVGULoDpI720muLJPhDvu_i9LbnO6fbIym8A5DiarmFYm6eXomI04xv3dmzkUZt_U5o.VFkbI8FNqsML25rz"",
					""download_url"": ""https://zoom.us/rec/download/EfmkVJijpXd7tsVGULoDpI720muLJPhDvu_i9LbnO6fbIym8A5DiarmFYm6eXomI04xv3dmzkUZt_U5o.VFkbI8FNqsML25rz"",
					""status"": ""completed"",
					""recording_type"": ""audio_only""
				},
				{
					""id"": ""c87d3cf2-b056-48b7-a528-398c6d7eaf9c"",
					""meeting_id"": ""v97o7qQASViWo36RwSMfWQ=="",
					""recording_start"": ""2021-02-11T10:19:50Z"",
					""recording_end"": ""2021-02-11T10:19:52Z"",
					""file_type"": ""MP4"",
					""file_extension"": ""MP4"",
					""file_size"": 49478,
					""play_url"": ""https://zoom.us/rec/play/-Iy_G5g0IZbXQGtIPuN78d9oVyH8fdmRhVJRX4gYN_TXMN7Z78YQn7bbxHun0AWmP2C7zMCL_QtjCNAS.U8hv_RMSm-mm-WeQ"",
					""download_url"": ""https://zoom.us/rec/download/-Iy_G5g0IZbXQGtIPuN78d9oVyH8fdmRhVJRX4gYN_TXMN7Z78YQn7bbxHun0AWmP2C7zMCL_QtjCNAS.U8hv_RMSm-mm-WeQ"",
					""status"": ""completed"",
					""recording_type"": ""shared_screen_with_speaker_view""
				}]
			},
			{
				""uuid"": ""twb2XumZShuj3WrNdnPvnA=="",
				""id"": 96598442130,
				""account_id"": ""wTJw8LgeT8GlOSjLjOU9Hw"",
				""host_id"": ""uET9c2fCR06UoPbeqKed4A"",
				""topic"": ""ფებრილური ბავშვი და კოვიდ საშიში შემთხვევების მართვა - 11.2.2021"",
				""type"": 2,
				""start_time"": ""2021-02-11T10:18:58Z"",
				""timezone"": ""UTC"",
				""duration"": 0,
				""total_size"": 237464,
				""recording_count"": 2,
				""share_url"": ""https://zoom.us/rec/share/bmLzxqWfhL2pDi3ZmbInt3o7moKu6y6XwRxQ_gpU5l1xPEmVSkJSQCiGv-tJMAF_.McjFybbFNSE40V0a"",
				""recording_files"": [
				{
					""id"": ""3309bd0c-d9f2-4c1e-a8ba-ee61d01fe47a"",
					""meeting_id"": ""twb2XumZShuj3WrNdnPvnA=="",
					""recording_start"": ""2021-02-11T10:19:03Z"",
					""recording_end"": ""2021-02-11T10:19:10Z"",
					""file_type"": ""M4A"",
					""file_extension"": ""M4A"",
					""file_size"": 109721,
					""play_url"": ""https://zoom.us/rec/play/jJJFQoLwHdghVuwozFGI-M1Di9EqK_T5TVToLRlSaBCABgr0WMDQEoCPl3oZkFZn_2qMCVpakAdKkOcx.4pZ7T_Im45EnFl6l"",
					""download_url"": ""https://zoom.us/rec/download/jJJFQoLwHdghVuwozFGI-M1Di9EqK_T5TVToLRlSaBCABgr0WMDQEoCPl3oZkFZn_2qMCVpakAdKkOcx.4pZ7T_Im45EnFl6l"",
					""status"": ""completed"",
					""recording_type"": ""audio_only""
				},
				{
					""id"": ""a3aa2202-804f-4ce3-8cd0-2713fa431c52"",
					""meeting_id"": ""twb2XumZShuj3WrNdnPvnA=="",
					""recording_start"": ""2021-02-11T10:19:03Z"",
					""recording_end"": ""2021-02-11T10:19:10Z"",
					""file_type"": ""MP4"",
					""file_extension"": ""MP4"",
					""file_size"": 127743,
					""play_url"": ""https://zoom.us/rec/play/ckbqAKYPsmO-SaSlt_yoAgxGV6EDFPU0kiTRFEHP0EZCzFuBXtze6kCuGDC2iM7NykdJ3SkVYRdacMzA.U3f8Bb64bwRUM2T3"",
					""download_url"": ""https://zoom.us/rec/download/ckbqAKYPsmO-SaSlt_yoAgxGV6EDFPU0kiTRFEHP0EZCzFuBXtze6kCuGDC2iM7NykdJ3SkVYRdacMzA.U3f8Bb64bwRUM2T3"",
					""status"": ""completed"",
					""recording_type"": ""shared_screen_with_speaker_view""
				}]
			},
			{
				""uuid"": ""Qza5PJa+Tb2nCMZlkNFksg=="",
				""id"": 95945730983,
				""account_id"": ""wTJw8LgeT8GlOSjLjOU9Hw"",
				""host_id"": ""uET9c2fCR06UoPbeqKed4A"",
				""topic"": ""ბავშვის განვითარება - 10.2.2021"",
				""type"": 2,
				""start_time"": ""2021-02-10T13:03:56Z"",
				""timezone"": ""UTC"",
				""duration"": 99,
				""total_size"": 385928864,
				""recording_count"": 3,
				""share_url"": ""https://zoom.us/rec/share/rHdTJ3_zIKm7EoVrKurgUPMpA-jHnNwN7s26KYQ5iQ0d0eoFcg2BZlT6QEJYbG6X.8d-fguKENSJJzT2Q"",
				""recording_files"": [
				{
					""id"": ""6a80f2e2-b88c-4a62-b093-6b0752b0ffad"",
					""meeting_id"": ""Qza5PJa+Tb2nCMZlkNFksg=="",
					""recording_start"": ""2021-02-10T13:03:58Z"",
					""recording_end"": ""2021-02-10T14:43:26Z"",
					""file_type"": ""MP4"",
					""file_extension"": ""MP4"",
					""file_size"": 290952791,
					""play_url"": ""https://zoom.us/rec/play/2If8cB5yK4o0jcCSIjcFSfQo1sRROi_K8TVYZWv5qjLhBZdFvUz9K58bcipm21r81QcB-w7baaIvOQte.3Ydz05Cluv64CxNa"",
					""download_url"": ""https://zoom.us/rec/download/2If8cB5yK4o0jcCSIjcFSfQo1sRROi_K8TVYZWv5qjLhBZdFvUz9K58bcipm21r81QcB-w7baaIvOQte.3Ydz05Cluv64CxNa"",
					""status"": ""completed"",
					""recording_type"": ""shared_screen_with_speaker_view""
				},
				{
					""id"": ""bf32ad2d-daf1-4812-8152-9bb5900cb71d"",
					""meeting_id"": ""Qza5PJa+Tb2nCMZlkNFksg=="",
					""recording_start"": ""2021-02-10T13:03:58Z"",
					""recording_end"": ""2021-02-10T14:43:26Z"",
					""file_type"": ""M4A"",
					""file_extension"": ""M4A"",
					""file_size"": 94975872,
					""play_url"": ""https://zoom.us/rec/play/I8cND5mGrbhgLBa-viYPOrfoh0G4gVO_5j_Sfo9r0cOtotLoEoIM_ke1QF7BTlnyD-sZxOtuAR0XsRm8.cwtLGYtc8l8Q4EVp"",
					""download_url"": ""https://zoom.us/rec/download/I8cND5mGrbhgLBa-viYPOrfoh0G4gVO_5j_Sfo9r0cOtotLoEoIM_ke1QF7BTlnyD-sZxOtuAR0XsRm8.cwtLGYtc8l8Q4EVp"",
					""status"": ""completed"",
					""recording_type"": ""audio_only""
				},
				{
					""id"": ""e4d65c41-941d-4aaa-b3fe-cd9994bfdfaa"",
					""meeting_id"": ""Qza5PJa+Tb2nCMZlkNFksg=="",
					""recording_start"": ""2021-02-10T13:03:58Z"",
					""recording_end"": ""2021-02-10T14:43:26Z"",
					""file_type"": ""CHAT"",
					""file_extension"": ""TXT"",
					""file_size"": 201,
					""play_url"": ""https://zoom.us/rec/play/6k1lX93Si74SVNyflhHNdnJPCQW4wtvOqsw-juXhTB1iT5FNA8AE9CzUE2va_4fzwnntdtpeem1Xyq3t.IxJXlul9YAT3FlLu"",
					""download_url"": ""https://zoom.us/rec/download/6k1lX93Si74SVNyflhHNdnJPCQW4wtvOqsw-juXhTB1iT5FNA8AE9CzUE2va_4fzwnntdtpeem1Xyq3t.IxJXlul9YAT3FlLu"",
					""status"": ""completed"",
					""recording_type"": ""chat_file""
				}]
			},
			{
				""uuid"": ""ARc9z3/7SPaCgxFB52ihXw=="",
				""id"": 92843931107,
				""account_id"": ""wTJw8LgeT8GlOSjLjOU9Hw"",
				""host_id"": ""uET9c2fCR06UoPbeqKed4A"",
				""topic"": ""ბავშვის განვითარება - 10.2.2021"",
				""type"": 2,
				""start_time"": ""2021-02-10T13:03:21Z"",
				""timezone"": ""UTC"",
				""duration"": 0,
				""total_size"": 771261,
				""recording_count"": 2,
				""share_url"": ""https://zoom.us/rec/share/0n_l_VxS_0YmehUSrocPbDX1yWNO30dsjzCfboAoYtkfoD9epmP1Y0_XE3G_W1XE.olTsUz-2dh0hlD4w"",
				""recording_files"": [
				{
					""id"": ""7cbee731-7eaa-48d2-ac44-d452269aaf19"",
					""meeting_id"": ""ARc9z3/7SPaCgxFB52ihXw=="",
					""recording_start"": ""2021-02-10T13:03:22Z"",
					""recording_end"": ""2021-02-10T13:03:45Z"",
					""file_type"": ""MP4"",
					""file_extension"": ""MP4"",
					""file_size"": 412022,
					""play_url"": ""https://zoom.us/rec/play/UJNJh8AxU22ec9ESKOg1mhj1cyP0H2xMrc8igojguhCrRoewQwW8xFmKGCHNq4RIHOTchx02kgbekSK6.678VCSZKHFIwvFBj"",
					""download_url"": ""https://zoom.us/rec/download/UJNJh8AxU22ec9ESKOg1mhj1cyP0H2xMrc8igojguhCrRoewQwW8xFmKGCHNq4RIHOTchx02kgbekSK6.678VCSZKHFIwvFBj"",
					""status"": ""completed"",
					""recording_type"": ""shared_screen_with_speaker_view""
				},
				{
					""id"": ""b6ecb675-ce58-4c25-a89f-cff13ba8bfdd"",
					""meeting_id"": ""ARc9z3/7SPaCgxFB52ihXw=="",
					""recording_start"": ""2021-02-10T13:03:22Z"",
					""recording_end"": ""2021-02-10T13:03:45Z"",
					""file_type"": ""M4A"",
					""file_extension"": ""M4A"",
					""file_size"": 359239,
					""play_url"": ""https://zoom.us/rec/play/DXSvVnBYqhkrm3YgeFP8yyvTX80vfzVT98aeJ2WyCF64LglzaGkCNrK1MqSiHSKloqf1VuVibstFT52Q.EARkYw8fl5ij7kR8"",
					""download_url"": ""https://zoom.us/rec/download/DXSvVnBYqhkrm3YgeFP8yyvTX80vfzVT98aeJ2WyCF64LglzaGkCNrK1MqSiHSKloqf1VuVibstFT52Q.EARkYw8fl5ij7kR8"",
					""status"": ""completed"",
					""recording_type"": ""audio_only""
				}]
			},
			{
				""uuid"": ""ldj3JebJQVGCOBDKW97oNQ=="",
				""id"": 95795500362,
				""account_id"": ""wTJw8LgeT8GlOSjLjOU9Hw"",
				""host_id"": ""uET9c2fCR06UoPbeqKed4A"",
				""topic"": ""ბავშვის განვითარება - 10.2.2021"",
				""type"": 2,
				""start_time"": ""2021-02-10T13:02:35Z"",
				""timezone"": ""UTC"",
				""duration"": 0,
				""total_size"": 3671781,
				""recording_count"": 2,
				""share_url"": ""https://zoom.us/rec/share/7Ik5_chBBEkUcK9dUF61IYF-DZuILzH-K4dXkWStxDlU8qhJoZOGaWMp39zq_WNe.K1uRDpR-UeCIaqDL"",
				""recording_files"": [
				{
					""id"": ""9c0f3b9f-5c62-4d47-8489-996884719eea"",
					""meeting_id"": ""ldj3JebJQVGCOBDKW97oNQ=="",
					""recording_start"": ""2021-02-10T13:02:37Z"",
					""recording_end"": ""2021-02-10T13:03:18Z"",
					""file_type"": ""MP4"",
					""file_extension"": ""MP4"",
					""file_size"": 3016139,
					""play_url"": ""https://zoom.us/rec/play/yv4HUCzRYCJrsdq_pb-frM4R22BrMQJrimFAemhts9cKcGVqPgWf4NCglxDOzvlUU_4nH3DgkEaEMnwa.DHrBW9SEIueENiFU"",
					""download_url"": ""https://zoom.us/rec/download/yv4HUCzRYCJrsdq_pb-frM4R22BrMQJrimFAemhts9cKcGVqPgWf4NCglxDOzvlUU_4nH3DgkEaEMnwa.DHrBW9SEIueENiFU"",
					""status"": ""completed"",
					""recording_type"": ""shared_screen_with_speaker_view""
				},
				{
					""id"": ""f698c666-e795-4ac5-b837-0a0afb75a730"",
					""meeting_id"": ""ldj3JebJQVGCOBDKW97oNQ=="",
					""recording_start"": ""2021-02-10T13:02:37Z"",
					""recording_end"": ""2021-02-10T13:03:18Z"",
					""file_type"": ""M4A"",
					""file_extension"": ""M4A"",
					""file_size"": 655642,
					""play_url"": ""https://zoom.us/rec/play/EyQS0LJ1ms3xxnM9OZilMRQ1J7mk2PmpN2liuGR7J4O824C23h-cCjcbvbEXEZmIuRNqozXmWEnfYPxl.nBMDi96qlXWMzGQG"",
					""download_url"": ""https://zoom.us/rec/download/EyQS0LJ1ms3xxnM9OZilMRQ1J7mk2PmpN2liuGR7J4O824C23h-cCjcbvbEXEZmIuRNqozXmWEnfYPxl.nBMDi96qlXWMzGQG"",
					""status"": ""completed"",
					""recording_type"": ""audio_only""
				}]
			},
			{
				""uuid"": ""Fss64aGbQZedceaiQl2vkw=="",
				""id"": 95945730983,
				""account_id"": ""wTJw8LgeT8GlOSjLjOU9Hw"",
				""host_id"": ""uET9c2fCR06UoPbeqKed4A"",
				""topic"": ""ბავშვის განვითარება - 10.2.2021"",
				""type"": 2,
				""start_time"": ""2021-02-10T13:02:16Z"",
				""timezone"": ""UTC"",
				""duration"": 0,
				""total_size"": 559850,
				""recording_count"": 2,
				""share_url"": ""https://zoom.us/rec/share/DbEEG6o3nuozCNx0XovWAtN7czZNSeFdYDzydlgVe6kDGjm5ZVFeAtxQwlBc-9dH.Se78Xwyjg9KMdMPD"",
				""recording_files"": [
				{
					""id"": ""0c1ed751-5319-4044-a9fa-909c9043e2ae"",
					""meeting_id"": ""Fss64aGbQZedceaiQl2vkw=="",
					""recording_start"": ""2021-02-10T13:02:20Z"",
					""recording_end"": ""2021-02-10T13:02:31Z"",
					""file_type"": ""MP4"",
					""file_extension"": ""MP4"",
					""file_size"": 386951,
					""play_url"": ""https://zoom.us/rec/play/HUDjZJb8F8wd3j7aBTGRgFdD4F8PEJm55alqp155eBJY9LZn2jxqLPo3gS17FmV_MOp31JkYfZi4CuQX.KtJmnNxJosQtGslN"",
					""download_url"": ""https://zoom.us/rec/download/HUDjZJb8F8wd3j7aBTGRgFdD4F8PEJm55alqp155eBJY9LZn2jxqLPo3gS17FmV_MOp31JkYfZi4CuQX.KtJmnNxJosQtGslN"",
					""status"": ""completed"",
					""recording_type"": ""shared_screen_with_speaker_view""
				},
				{
					""id"": ""4a12d56d-af6b-4cc6-8f4d-43f0c926fb01"",
					""meeting_id"": ""Fss64aGbQZedceaiQl2vkw=="",
					""recording_start"": ""2021-02-10T13:02:20Z"",
					""recording_end"": ""2021-02-10T13:02:31Z"",
					""file_type"": ""M4A"",
					""file_extension"": ""M4A"",
					""file_size"": 172899,
					""play_url"": ""https://zoom.us/rec/play/eFMPyk4yJKxCZoEd8g48iLARHfaB1RHRXXvOtX6gSR6JQo-80F6CgwuvitkXJ7MGI8fPoHR0BDqSo8qs.3nZ80yVNHQpyo6A-"",
					""download_url"": ""https://zoom.us/rec/download/eFMPyk4yJKxCZoEd8g48iLARHfaB1RHRXXvOtX6gSR6JQo-80F6CgwuvitkXJ7MGI8fPoHR0BDqSo8qs.3nZ80yVNHQpyo6A-"",
					""status"": ""completed"",
					""recording_type"": ""audio_only""
				}]
			},
			{
				""uuid"": ""T6jgPwKtSr+9+hiE+ToVtQ=="",
				""id"": 95945730983,
				""account_id"": ""wTJw8LgeT8GlOSjLjOU9Hw"",
				""host_id"": ""uET9c2fCR06UoPbeqKed4A"",
				""topic"": ""ბავშვის განვითარება - 10.2.2021"",
				""type"": 2,
				""start_time"": ""2021-02-10T13:00:12Z"",
				""timezone"": ""UTC"",
				""duration"": 1,
				""total_size"": 4595219,
				""recording_count"": 2,
				""share_url"": ""https://zoom.us/rec/share/oRQgOJjRlIeHUBGG_qWBnhBCXQpfukP2EOy5j6rPCEW95CRmMTfD3FLcsvk74rnM.KvwvNG1hjDTGqyVD"",
				""recording_files"": [
				{
					""id"": ""6a0a8d73-d61b-4a77-831b-2f277c2bc2a6"",
					""meeting_id"": ""T6jgPwKtSr+9+hiE+ToVtQ=="",
					""recording_start"": ""2021-02-10T13:00:14Z"",
					""recording_end"": ""2021-02-10T13:01:53Z"",
					""file_type"": ""MP4"",
					""file_extension"": ""MP4"",
					""file_size"": 3024946,
					""play_url"": ""https://zoom.us/rec/play/TDP5LEDABaXrNj_Xq5Brd8qezdCzDc8POiqkGIpHAzbsm67L6gXYXAWYoiRuLjfaccDLVUKSJtoeTVDg.xcGSqa0T61lmsp_D"",
					""download_url"": ""https://zoom.us/rec/download/TDP5LEDABaXrNj_Xq5Brd8qezdCzDc8POiqkGIpHAzbsm67L6gXYXAWYoiRuLjfaccDLVUKSJtoeTVDg.xcGSqa0T61lmsp_D"",
					""status"": ""completed"",
					""recording_type"": ""shared_screen_with_speaker_view""
				},
				{
					""id"": ""f0ff25ed-a7f4-4d28-b04c-9d5818d7c497"",
					""meeting_id"": ""T6jgPwKtSr+9+hiE+ToVtQ=="",
					""recording_start"": ""2021-02-10T13:00:14Z"",
					""recording_end"": ""2021-02-10T13:01:53Z"",
					""file_type"": ""M4A"",
					""file_extension"": ""M4A"",
					""file_size"": 1570273,
					""play_url"": ""https://zoom.us/rec/play/-QbcIKlx_mz_erMu_xSZl3tzJnfWgWtvm_29OAp3NULH7L0M8ETocWM-ujdXOH6cxigXIXFRowDWMf7r.qrid3omZEOaqdabM"",
					""download_url"": ""https://zoom.us/rec/download/-QbcIKlx_mz_erMu_xSZl3tzJnfWgWtvm_29OAp3NULH7L0M8ETocWM-ujdXOH6cxigXIXFRowDWMf7r.qrid3omZEOaqdabM"",
					""status"": ""completed"",
					""recording_type"": ""audio_only""
				}]
			},
			{
				""uuid"": ""jRK2LJiNR5Sx7VtuoreeWQ=="",
				""id"": 98657314042,
				""account_id"": ""wTJw8LgeT8GlOSjLjOU9Hw"",
				""host_id"": ""uET9c2fCR06UoPbeqKed4A"",
				""topic"": ""ფებრილური ბავშვი და კოვიდ საშიში შემთხვევების მართვა - 10.2.2021"",
				""type"": 2,
				""start_time"": ""2021-02-10T12:23:09Z"",
				""timezone"": ""UTC"",
				""duration"": 0,
				""total_size"": 179776,
				""recording_count"": 2,
				""share_url"": ""https://zoom.us/rec/share/DpqJzyDwxIsoxJXEjnfUjxN0dmK1RmqpujzEUGY_Sil8h98stw9Qa4lJVtSdN8Yb.yeE5p6ZqVxzUgPYD"",
				""recording_files"": [
				{
					""id"": ""77caf9eb-8b98-4796-b389-2a790007e610"",
					""meeting_id"": ""jRK2LJiNR5Sx7VtuoreeWQ=="",
					""recording_start"": ""2021-02-10T12:23:11Z"",
					""recording_end"": ""2021-02-10T12:23:16Z"",
					""file_type"": ""M4A"",
					""file_extension"": ""M4A"",
					""file_size"": 80184,
					""play_url"": ""https://zoom.us/rec/play/hWR67U2yI5Vx0Fz4evsjvt-Ve4deHZMXCDDl4kMdfBibjiUKaGMUKvHzd0EeY1x0bPYbXtOOwRn839LB.E8QfX3rzaltiaXPB"",
					""download_url"": ""https://zoom.us/rec/download/hWR67U2yI5Vx0Fz4evsjvt-Ve4deHZMXCDDl4kMdfBibjiUKaGMUKvHzd0EeY1x0bPYbXtOOwRn839LB.E8QfX3rzaltiaXPB"",
					""status"": ""completed"",
					""recording_type"": ""audio_only""
				},
				{
					""id"": ""ad2fd33f-d54b-4d97-8c0c-c3147c8f335a"",
					""meeting_id"": ""jRK2LJiNR5Sx7VtuoreeWQ=="",
					""recording_start"": ""2021-02-10T12:23:11Z"",
					""recording_end"": ""2021-02-10T12:23:16Z"",
					""file_type"": ""MP4"",
					""file_extension"": ""MP4"",
					""file_size"": 99592,
					""play_url"": ""https://zoom.us/rec/play/WPvU43Gf7jrINoVnUx8zsRVRUs2nOdLOIlAmcLik2Gt_z6Ax9IGmpWdbJ4gZXJg8FUS8x6H0-kEjqtdD.I3L-yCq5fh01l8nc"",
					""download_url"": ""https://zoom.us/rec/download/WPvU43Gf7jrINoVnUx8zsRVRUs2nOdLOIlAmcLik2Gt_z6Ax9IGmpWdbJ4gZXJg8FUS8x6H0-kEjqtdD.I3L-yCq5fh01l8nc"",
					""status"": ""completed"",
					""recording_type"": ""shared_screen_with_speaker_view""
				}]
			},
			{
				""uuid"": ""jGJDUGiuRTO4szXaQxN8Gw=="",
				""id"": 96368504000,
				""account_id"": ""wTJw8LgeT8GlOSjLjOU9Hw"",
				""host_id"": ""uET9c2fCR06UoPbeqKed4A"",
				""topic"": ""ბავშვები მწვავე დიარეით და რესპირატორული დაავადებებით - 6.2.2021"",
				""type"": 2,
				""start_time"": ""2021-02-10T06:48:47Z"",
				""timezone"": ""UTC"",
				""duration"": 218,
				""total_size"": 992643484,
				""recording_count"": 2,
				""share_url"": ""https://zoom.us/rec/share/YvB4kcUCercF3H9f-gpB12K0SzzArsVgwLAeLe-WndFvdw313AetB0weu9ZNAPWP.CPORWyaPTKjnpd1_"",
				""recording_files"": [
				{
					""id"": ""b2060ddf-160a-40c6-bb43-904685996a37"",
					""meeting_id"": ""jGJDUGiuRTO4szXaQxN8Gw=="",
					""recording_start"": ""2021-02-10T06:48:48Z"",
					""recording_end"": ""2021-02-10T10:27:38Z"",
					""file_type"": ""M4A"",
					""file_extension"": ""M4A"",
					""file_size"": 208938890,
					""play_url"": ""https://zoom.us/rec/play/dxkrODfCdJyf1mmO1ddCiY7Rv6oYDxvj0P-jlpN9tIU1jVylYjRhrXETJCERt39AfTS1Xg-CR5ovBquy.gTWnQmhCJfohWdrw"",
					""download_url"": ""https://zoom.us/rec/download/dxkrODfCdJyf1mmO1ddCiY7Rv6oYDxvj0P-jlpN9tIU1jVylYjRhrXETJCERt39AfTS1Xg-CR5ovBquy.gTWnQmhCJfohWdrw"",
					""status"": ""completed"",
					""recording_type"": ""audio_only""
				},
				{
					""id"": ""c3475960-a7b5-404a-a769-ac0c1ca0195d"",
					""meeting_id"": ""jGJDUGiuRTO4szXaQxN8Gw=="",
					""recording_start"": ""2021-02-10T06:48:48Z"",
					""recording_end"": ""2021-02-10T10:27:38Z"",
					""file_type"": ""MP4"",
					""file_extension"": ""MP4"",
					""file_size"": 783704594,
					""play_url"": ""https://zoom.us/rec/play/mYnWRnPI5BMUTOsG6pewpSRCRj17W4-OcSPV694AwAuc4m0dJDpszCWH-0aEk16CibF56D1j1eLFNyEA.JpB8hWV615wRdA1h"",
					""download_url"": ""https://zoom.us/rec/download/mYnWRnPI5BMUTOsG6pewpSRCRj17W4-OcSPV694AwAuc4m0dJDpszCWH-0aEk16CibF56D1j1eLFNyEA.JpB8hWV615wRdA1h"",
					""status"": ""completed"",
					""recording_type"": ""shared_screen_with_speaker_view""
				}]
			}
		]}";

		#endregion

		[Fact]
		public void Parse_json()
		{
			// Arrange

			// Act
			var result = JsonSerializer.Deserialize<Recording>(SINGLE_CLOUD_RECORDING_JSON, JsonFormatter.SerializerOptions);

			// Assert
			result.ShouldNotBeNull();
			result.Uuid.ShouldBe("ODfDKShNRqKkXbGD09Sk4A==");
			result.Id.ShouldBe(94488262913);
			result.AccountId.ShouldBe("wTJw8LgeT8GlOSjLjOU9Hw");
			result.HostId.ShouldBe("uET9c2fCR06UoPbeqKed4A");
			result.Topic.ShouldBe("ფებრილური ბავშვი და კოვიდ საშიში შემთხვევების მართვა - 8.2.2021");
			result.StartTime.ShouldBe(new DateTime(2021, 2, 11, 11, 40, 38, DateTimeKind.Utc));
			result.Duration.ShouldBe(120);
			result.TotalSize.ShouldBe(0);
			result.FilesCount.ShouldBe(0);
			result.ShareUrl.ShouldBe("https://zoom.us/rec/share/X_pJ8dSmA_j4Ikbge2RGDaG3PVlBreUYs54RkvCHs7-P_15CVOKqcAbo8KQUfhFx.zqpEekmacjnoL_nG");
			result.RecordingFiles.ShouldNotBeNull();
			result.RecordingFiles.Length.ShouldBe(1);
			result.RecordingFiles[0].Id.ShouldBe("d78d0d2d-5890-45be-ad69-85972ce0de82");
			result.RecordingFiles[0].MeetingId.ShouldBe("ODfDKShNRqKkXbGD09Sk4A==");
			result.RecordingFiles[0].StartTime.ShouldBe(new DateTime(2021, 2, 11, 11, 40, 39, DateTimeKind.Utc));
			result.RecordingFiles[0].EndTime.ShouldBeNull();
			result.RecordingFiles[0].FileType.ShouldBe(RecordingFileType.NotSpecified);
			result.RecordingFiles[0].Size.ShouldBe(0);
			result.RecordingFiles[0].PlayUrl.ShouldBe("https://zoom.us/rec/play/3YUQu4dKV9c_sAwj-X33a_IGBr5V57LQFaTSwubo9R-hl3Gj_z-wSBSJNX9p5MFS8VsCEll4iAMyrDZi.h6gWLE07kjtTpZtX");
			result.RecordingFiles[0].DownloadUrl.ShouldBe("https://zoom.us/rec/download/3YUQu4dKV9c_sAwj-X33a_IGBr5V57LQFaTSwubo9R-hl3Gj_z-wSBSJNX9p5MFS8VsCEll4iAMyrDZi.h6gWLE07kjtTpZtX");
			result.RecordingFiles[0].Status.ShouldBe(RecordingStatus.Processing);
			result.RecordingFiles[0].DeleteTime.ShouldBeNull();
			result.RecordingFiles[0].ContentType.ShouldBe(RecordingContentType.NotSpecified);
		}

		[Fact]
		public async Task GetRecordingsForUserAsync()
		{
			// Arrange
			var userId = "uET9c2fCR06UoPbeqKed4A";
			var from = new DateTime(2021, 2, 10, 0, 0, 0, DateTimeKind.Utc);
			var to = new DateTime(2021, 2, 11, 0, 0, 0, DateTimeKind.Utc);
			var recordsPerPage = 30;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.Expect(HttpMethod.Get, Utils.GetZoomApiUri("users", userId, "recordings")).Respond("application/json", MULTIPLE_CLOUD_RECORDINGS_JSON);

			var client = Utils.GetFluentClient(mockHttp);
			var recordings = new CloudRecordings(client);

			// Act
			var result = await recordings.GetRecordingsForUserAsync(userId, false, from, to, recordsPerPage, null, CancellationToken.None).ConfigureAwait(false);

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			result.ShouldNotBeNull();
			result.PageSize.ShouldBe(recordsPerPage);
			result.NextPageToken.ShouldBeEmpty();
			result.MoreRecordsAvailable.ShouldBeFalse();
			result.TotalRecords.ShouldBe(10);
			result.From.ShouldBe(from);
			result.To.ShouldBe(to);
			result.Records.ShouldNotBeNull();
			result.Records.Length.ShouldBe(10);
		}
	}
}
