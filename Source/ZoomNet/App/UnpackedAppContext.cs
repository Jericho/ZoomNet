namespace ZoomNet.App
{
	internal class UnpackedAppContext
	{
		public byte[] Iv { get; set; }

		public byte[] Aad { get; set; }

		public byte[] Encrypt { get; set; }

		public byte[] Tag { get; set; }
	}
}
