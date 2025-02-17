using Newtonsoft.Json;
using RevitSyncFramework.Models;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace RevitSyncFramework.Utilities
{
    public class TokenEncryptionUtility
    {
        private static readonly string EncryptionKey = "0123456789ABCDEF0123456789ABCDEF"; // 32 bytes for 256-bit key
        private static readonly string Iv = "0123456789ABCDEF"; // 16 bytes for 128-bit IV

        // Encrypt the token data
        public static byte[] EncryptTokenData(TokenData tokenData)
        {
            using (Aes aesAlg = Aes.Create())
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(EncryptionKey);
                aesAlg.Key = keyBytes;
                aesAlg.IV = Encoding.UTF8.GetBytes(Iv);

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        string jsonTokenData = JsonConvert.SerializeObject(tokenData);
                        swEncrypt.Write(jsonTokenData);
                    }
                    return msEncrypt.ToArray();
                }
            }
        }

        // Decrypt the token data
        public static TokenData DecryptTokenData(byte[] encryptedTokenData)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                aesAlg.IV = Encoding.UTF8.GetBytes(Iv);

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(encryptedTokenData))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {
                    string jsonTokenData = srDecrypt.ReadToEnd();
                    return JsonConvert.DeserializeObject<TokenData>(jsonTokenData);
                }
            }
        }
    }
}
