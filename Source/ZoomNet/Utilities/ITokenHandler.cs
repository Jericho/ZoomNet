namespace ZoomNet.Utilities
{
	internal interface ITokenHandler
	{
		string RefreshTokenIfNecessary(bool forceRefresh);
	}
}
