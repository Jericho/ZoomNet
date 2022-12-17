#if NET48
#else
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Web;
using ZoomNet.Json;

namespace ZoomNet.App
{
	/// <summary>
	/// A helper class for working with ZoomApps - see https://marketplace.zoom.us/docs/zoom-apps .
	/// </summary>
	public partial class ZoomAppHelper
	{
		/// <summary>
		/// Decrypts the ZoomAppContext as provided by appssdk.getAppContext
		/// - see https://marketplace.zoom.us/docs/zoom-apps/js-sdk/reference/#getAppContext .
		/// </summary>
		/// <param name="context">The encrypted AppContext</param>
		/// <param name="clientSecret">Your ZoomApp clientSecret</param>
		/// <returns>A <see cref="AppContext" /></returns>
		public virtual (AppContext AppContext, string RawJson) DecryptAppContext(string context, string clientSecret)
		{
			UnpackedAppContext unpackedContext = Unpack(context);

			using (SHA256 sha256Hash = SHA256.Create())
			{
				using (var decryptor = new AesGcm(sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(clientSecret))))
				{
					var plaintextBytes = new byte[unpackedContext.Encrypt.Length];
					decryptor.Decrypt(unpackedContext.Iv, unpackedContext.Encrypt, unpackedContext.Tag, plaintextBytes, unpackedContext.Aad);
					string json = Encoding.UTF8.GetString(plaintextBytes);

					var converter = new JsonFormatter();
					var result = (AppContext)converter.Deserialize(typeof(AppContext), new MemoryStream(plaintextBytes), null, null);
					return (result, json);
				}
			}
		}

		private static UnpackedAppContext Unpack(string context)
		{
			string _context = UrlTokenDecode(context);
			byte[] contextBytes = Convert.FromBase64String(_context);

			// [ivLength: 1 byte][iv][aadLength: 2 bytes][aad][cipherTextLength: 4 bytes][cipherText][tag: 16 bytes]
			using (Stream stream = new MemoryStream(contextBytes))
			{
				// read iv
				int ivLength = stream.ReadByte();
				byte[] iv = new byte[ivLength];
				for (int i = 0; i < ivLength; i++)
					iv[i] = (byte)stream.ReadByte();

				// read aad
				int aadLength = 0;
				for (int i = 0; i < 2; i++)
					aadLength += stream.ReadByte();
				byte[] aad = new byte[aadLength];
				for (int i = 0; i < aadLength; i++)
					aad[i] = (byte)stream.ReadByte();

				// read cipher text
				int cipherLength = 0;
				for (int i = 0; i < 4; i++)
					cipherLength += stream.ReadByte();
				byte[] encrypt = new byte[cipherLength];
				for (int i = 0; i < cipherLength; i++)
					encrypt[i] = (byte)stream.ReadByte();

				// read tag which is always 16 length
				byte[] tag = new byte[16];
				for (int i = 0; i < 16; i++)
					tag[i] = (byte)stream.ReadByte();

				int finalResult = stream.ReadByte();
				if (finalResult != -1)
					throw new InvalidOperationException("The context is not balanced.");

				return new UnpackedAppContext
				{
					Iv = iv,
					Aad = aad,
					Encrypt = encrypt,
					Tag = tag
				};
			}
		}

		private static string UrlTokenDecode(string input)
		{
			if (input == null)
				throw new ArgumentNullException("input");

			int len = input.Length;
			if (len < 1)
				return string.Empty;

			string base64Pre = input
				.Replace("-", "+")
				.Replace("_", "/");

			string base64 = base64Pre.PadRight(base64Pre.Length + (4 - base64Pre.Length % 4) % 4, '=');

			return base64;
		}
	}
}
#endif
