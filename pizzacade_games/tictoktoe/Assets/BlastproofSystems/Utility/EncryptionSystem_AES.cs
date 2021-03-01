﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Blastproof.Systems.Core
{
	public class EncryptionSystem_AES
	{
		static byte[] key =
		{
			123, 217, 19, 11, 24, 26, 85, 45, 114, 184, 27, 162, 37, 112, 222, 209, 241, 24, 175, 144, 173, 53, 196, 29, 24, 26,
			17, 218, 131, 236, 53, 209
		};

		static byte[] vector = {146, 64, 191, 111, 23, 3, 113, 119, 231, 121, 221, 112, 79, 32, 114, 156};
		ICryptoTransform encryptor;
		ICryptoTransform decryptor;
		UTF8Encoding encoder;

		public EncryptionSystem_AES()
		{
            var rm = new RijndaelManaged();
			encryptor = rm.CreateEncryptor(key, vector);
			decryptor = rm.CreateDecryptor(key, vector);
			encoder = new UTF8Encoding();
		}

		public string Encrypt(string unencrypted)
		{
			return Convert.ToBase64String(Encrypt(encoder.GetBytes(unencrypted)));
		}

		public string Decrypt(string encrypted)
		{
			return encoder.GetString(Decrypt(Convert.FromBase64String(encrypted)));
		}

		byte[] Encrypt(byte[] buffer)
		{
			return Transform(buffer, encryptor);
		}

		byte[] Decrypt(byte[] buffer)
		{
			return Transform(buffer, decryptor);
		}

		protected byte[] Transform(byte[] buffer, ICryptoTransform transform)
		{
			var stream = new MemoryStream();
			using (var cs = new CryptoStream(stream, transform, CryptoStreamMode.Write))
			{
				cs.Write(buffer, 0, buffer.Length);
			}

			return stream.ToArray();
		}
	}
}