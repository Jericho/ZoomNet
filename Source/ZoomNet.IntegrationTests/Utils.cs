using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace ZoomNet.IntegrationTests
{
	internal static class Utils
	{
		public static (Stream ImageStream, string ImageName) GetRandomImage(int maxKb = -1)
		{
			var samplePicturesFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Samples");
			if (!Directory.Exists(samplePicturesFolder)) throw new Exception("The folder containing sample pictures does not exist. Unable to get a random file.");

			var samplePictures = Directory.EnumerateFiles(samplePicturesFolder, "*.jpg");
			if (samplePictures.Count() == 0) throw new Exception("The folder containing sample pictures does not contain any JPG files. Unable to get a random file.");

			if (maxKb > 0) samplePictures = samplePictures.Where(f => new FileInfo(f).Length <= maxKb * 1024);

			var randomIndex = RandomNumberGenerator.GetInt32(samplePictures.Count());
			var samplePicture = samplePictures.ElementAt(randomIndex);
			return (File.OpenRead(samplePicture), Path.GetFileName(samplePicture));
		}
	}
}
