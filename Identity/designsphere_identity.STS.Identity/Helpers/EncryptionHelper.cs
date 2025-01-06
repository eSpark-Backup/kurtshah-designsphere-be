using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace designsphere_identity.STS.Identity.Helpers;

    public static class EncryptionHelper
    {
        public static string Encrypt(string plainText, string key)
        {
            var keyBytes = AdjustKey(key, 32); // Using AES-256

            using var aes = Aes.Create();
            aes.Key = keyBytes;
            aes.GenerateIV(); // Generate a new IV for each encryption

            var iv = aes.IV; // Initialization vector

            using var encryptor = aes.CreateEncryptor(aes.Key, iv);
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }

            var encrypted = ms.ToArray();

            // Combine IV and encrypted data for storage/transmission
            var result = new byte[iv.Length + encrypted.Length];
            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
            Buffer.BlockCopy(encrypted, 0, result, iv.Length, encrypted.Length);

            return Convert.ToBase64String(result); // Return as Base64 for easy transfer
        }

        public static string Decrypt(string encryptedText, string key)
        {
            var keyBytes = AdjustKey(key, 32); // Using AES-256
            var encryptedBytes = Convert.FromBase64String(encryptedText);

            using var aes = Aes.Create();
            aes.Key = keyBytes;

            // Extract the IV and encrypted data
            var iv = new byte[aes.BlockSize / 8];
            var cipherText = new byte[encryptedBytes.Length - iv.Length];
            Buffer.BlockCopy(encryptedBytes, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(encryptedBytes, iv.Length, cipherText, 0, cipherText.Length);

            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(cipherText);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);

            return sr.ReadToEnd(); // Return the decrypted plaintext
        }

        private static byte[] AdjustKey(string key, int length)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);

            if (keyBytes.Length > length)
                return keyBytes.Take(length).ToArray();

            if (keyBytes.Length < length)
            {
                var paddedKey = new byte[length];
                Array.Copy(keyBytes, paddedKey, keyBytes.Length);
                return paddedKey;
            }

            return keyBytes;
        }
    }
