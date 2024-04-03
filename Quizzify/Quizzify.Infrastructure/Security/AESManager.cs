using System.Security.Cryptography;
using System.Text;

namespace Quizzify.Infrastructure.Security;

public class AESManager
{
    private readonly byte[] Key = Encoding.UTF8.GetBytes("$AES256@ENCRYPT$");
    private static readonly byte[] IV = Encoding.UTF8.GetBytes("$AES256@DECRYPT$");

    public string Encrypt(string value, string salt)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = IV;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
                msEncrypt.Write(saltBytes, 0, saltBytes.Length);

                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    byte[] textBytes = Encoding.UTF8.GetBytes(value);
                    csEncrypt.Write(textBytes, 0, textBytes.Length);
                }

                byte[] encryptedBytes = msEncrypt.ToArray();
                string encryptedData = Convert.ToBase64String(encryptedBytes);

                return encryptedData;
            }
        }
    }

    public string Decrypt(string hash)
    {
        byte[] encryptedData = Convert.FromBase64String(hash);

        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = IV;

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
            {
                const ushort SaltBuffer = 36;
                byte[] saltBytes = new byte[SaltBuffer];
                msDecrypt.Read(saltBytes, 0, saltBytes.Length);

                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        string decryptedData = srDecrypt.ReadToEnd();
                        return decryptedData;
                    }
                }
            }
        }
    }
}