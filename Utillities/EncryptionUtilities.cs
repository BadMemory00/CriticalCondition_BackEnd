using System.Security.Cryptography;
using System.Text;

namespace CriticalConditionBackend.Utillities
{
    public static class EncryptionUtilities
    {
        public static string EncryptSubUserCode(string code, IConfiguration Configuration)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(Configuration["SubUserCodeKey"]);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using var memoryStream = new MemoryStream();
                using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                using (var streamWriter = new StreamWriter(cryptoStream))
                {
                    streamWriter.Write(code);
                }

                array = memoryStream.ToArray();
            }

            return Convert.ToBase64String(array);
        }

        public static string DecryptSubUserCode(string encryptedCode, IConfiguration Configuration)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(encryptedCode);

            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(Configuration["SubUserCodeKey"]);
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var memoryStream = new MemoryStream(buffer);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);

            return streamReader.ReadToEnd();
        }
    }
}
