namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of login.
	/// </summary>
	public enum LoginType
	{
		/// <summary>Facebook OAuth.</summary>
		Facebook = 0,

		/// <summary>Google OAuth.</summary>
		Google = 1,

		/// <summary>Apple OAuth.</summary>
		Apple = 24,

		/// <summary>Microsoft OAuth.</summary>
		Microsoft = 27,

		/// <summary>Mobile device.</summary>
		MobileDevice = 97,

		/// <summary>RingCentral OAuth.</summary>
		RingCentral = 98,

		/// <summary>API user.</summary>
		ApiUser = 99,

		/// <summary>Zoom Work email.</summary>
		ZoomWorkEmail = 100,

		/// <summary>Single Sign-On (SSO).</summary>
		Sso = 101,

		/// <summary> Phone number.</summary>
		/// <remarks>Only available in China.</remarks>
		PhoneNumber = 11,

		/// <summary>WeChat.</summary>
		/// <remarks>Only available in China.</remarks>
		WeChat = 21,

		/// <summary>Alipay.</summary>
		/// <remarks>Only available in China.</remarks>
		Alipay = 23,
	}
}
