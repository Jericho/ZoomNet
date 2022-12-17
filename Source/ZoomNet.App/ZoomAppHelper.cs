#if NET48
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.IO;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using System.Linq;
#else
#endif

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
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
		/// <returns>A <see cref="AppContext" />.</returns>
		public virtual (AppContext AppContext, string RawJson) DecryptAppContext(string context, string clientSecret)
		{
			UnpackedAppContext unpackedContext = Unpack(context);

			byte[] key;
			using (SHA256 sha256Hash = SHA256.Create())
				key = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(clientSecret));

			string json;
			byte[] plaintextBytes;
			byte[] encrypted;
#if NET48
			encrypted = unpackedContext.Encrypt.Concat(unpackedContext.Tag).ToArray();
			var keyParameter = new KeyParameter(key);
			var cipherParameters = new AeadParameters(keyParameter, unpackedContext.Tag.Length * 8, unpackedContext.Iv, unpackedContext.Aad);
			var cipher = new GcmBlockCipher(new AesEngine());
			cipher.Init(false, cipherParameters);
			plaintextBytes = new byte[cipher.GetOutputSize(encrypted.Length)];
			int len = cipher.ProcessBytes(encrypted, 0, encrypted.Length, plaintextBytes, 0);
			cipher.DoFinal(plaintextBytes, len);

			json = Encoding.UTF8.GetString(plaintextBytes);
#else
			encrypted = unpackedContext.Encrypt;
			plaintextBytes = new byte[unpackedContext.Encrypt.Length];
			using (var decryptor = new AesGcm(key))
			{
				decryptor.Decrypt(unpackedContext.Iv, encrypted, unpackedContext.Tag, plaintextBytes, unpackedContext.Aad);
				json = Encoding.UTF8.GetString(plaintextBytes);
			}
#endif

			var converter = new AppJsonFormatter();
			var result = (AppContext)converter.Deserialize(typeof(AppContext), new MemoryStream(plaintextBytes), null, null);
			return (result, json);
		}

		private UnpackedAppContext Unpack(string context)
		{
			string decodedContext = UrlTokenDecode(context);
			byte[] contextBytes = Convert.FromBase64String(decodedContext);

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

		private string UrlTokenDecode(string input)
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
