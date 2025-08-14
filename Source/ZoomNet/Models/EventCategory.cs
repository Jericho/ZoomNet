using System.Runtime.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// Enumeration to indicate the type of attendee experience for an event.
	/// </summary>
	public enum EventCategory
	{
		/// <summary>Education and family.</summary>
		[EnumMember(Value = "Education & Family")]
		EducationAndFamily,

		/// <summary>Business and Networking.</summary>
		[EnumMember(Value = "Business & Networking")]
		BusinessAndNetworking,

		/// <summary>Entertainment and Visual Arts.</summary>
		[EnumMember(Value = "Entertainment & Visual Arts")]
		EntertainmentAndVisualArts,

		/// <summary>Food and Drink.</summary>
		[EnumMember(Value = "Food & Drink")]
		FoodAndDrink,

		/// <summary>Fitness and Health.</summary>
		[EnumMember(Value = "Fitness & Health")]
		FitnessAndHealth,

		/// <summary>Home and Lifestyle.</summary>
		[EnumMember(Value = "Home & Lifestyle")]
		HomeAndLifestyle,

		/// <summary>Community and Spirituality.</summary>
		[EnumMember(Value = "Community & Spirituality")]
		CommunityAndSpirituality
	}
}
