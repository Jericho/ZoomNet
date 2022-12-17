using Shouldly;
using Xunit;
using ZoomNet.App;

namespace ZoomNet.UnitTests
{
	public class ZoomAppTests
	{
		[Fact]
		public void DecryptAppContext()
		{
			// Arrange
			// Test values from https://marketplace.zoom.us/docs/zoom-apps/zoomappcontext/
			string appContext = "DG7HCXYGApQWw9J4nAAAdQAAAKJI45T4UDBcUUrburGWMYVryK6DCYoR1f_xPqlf3-MEDXRT6T3wftRLow-NE3UYqfDORa8tjPzdK8fouUZw0wQDhBT1wF7Whi94JxfgEeorpKb6KErIAZeS-AcnkVBAHs9ZdrrJHg3Svff4irl-ypyYKQIMqNkssqij8Sqb5K3UMaQdOME";
			string appSecret = "6pTg05u9xBHmFKkhdRieOatMZIihN3m8";
			var helper = new ZoomAppHelper();

			// Act
			var result = helper.DecryptAppContext(appContext, appSecret);

			// Assert
			// Test values from https://marketplace.zoom.us/docs/zoom-apps/zoomappcontext/
			result.AppContext.ContextType.ShouldBe("panel");
			result.AppContext.UserId.ShouldBe("77A6G6xIS62MkqTlFWJhbg");
			result.AppContext.Dev.ShouldBe("qAAqvyeJcTFUDxoW5XzkUfND/nftgjro08GA+niqXwg");
			result.AppContext.CreatedTimestamp.ShouldBe(1608618226564L);
		}
	}
}
