using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class LicenseManager
{
    private const string LicenseKeyPref = "LicenseKey";
    private const string MachineIdPref = "MachineID";
    private const string ActivatedPref = "Activated";

    private static string encryptionKey = "MySuperSecretKey"; // 16/24/32 chars for AES
    private static string validKey = "ABC-123-XYZ";

    public static bool IsActivated()
    {
        if (PlayerPrefs.GetInt(ActivatedPref, 0) == 1)
        {
            string encryptedKey = PlayerPrefs.GetString(LicenseKeyPref, "");
            string encryptedMachineId = PlayerPrefs.GetString(MachineIdPref, "");

            string savedKey = DecryptString(encryptedKey, encryptionKey);
            string savedMachineId = DecryptString(encryptedMachineId, encryptionKey);

            string currentMachineId = SystemInfo.deviceUniqueIdentifier;

            return savedKey == validKey && savedMachineId == currentMachineId;
        }
        return false;
    }

    public static bool Activate(string inputKey)
    {
        if (inputKey == validKey)
        {
            string encryptedKey = EncryptString(inputKey, encryptionKey);
            string encryptedMachineId = EncryptString(
                SystemInfo.deviceUniqueIdentifier,
                encryptionKey
            );

            PlayerPrefs.SetInt(ActivatedPref, 1);
            PlayerPrefs.SetString(LicenseKeyPref, encryptedKey);
            PlayerPrefs.SetString(MachineIdPref, encryptedMachineId);
            PlayerPrefs.Save();

            return true;
        }
        return false;
    }

    public static void ResetLicense()
    {
        PlayerPrefs.DeleteKey(ActivatedPref);
        PlayerPrefs.DeleteKey(LicenseKeyPref);
        PlayerPrefs.DeleteKey(MachineIdPref);
        PlayerPrefs.Save();
    }

    private static string EncryptString(string plainText, string key)
    {
        byte[] iv = new byte[16];
        byte[] array;

        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (var encryptStream = new System.IO.MemoryStream())
            {
                using (
                    var cryptoStream = new CryptoStream(
                        encryptStream,
                        encryptor,
                        CryptoStreamMode.Write
                    )
                )
                using (var writer = new System.IO.StreamWriter(cryptoStream))
                {
                    writer.Write(plainText);
                }

                array = encryptStream.ToArray();
            }
        }

        return Convert.ToBase64String(array);
    }

    private static string DecryptString(string cipherText, string key)
    {
        byte[] iv = new byte[16];
        byte[] buffer = Convert.FromBase64String(cipherText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (var decryptStream = new System.IO.MemoryStream(buffer))
            using (
                var cryptoStream = new CryptoStream(decryptStream, decryptor, CryptoStreamMode.Read)
            )
            using (var reader = new System.IO.StreamReader(cryptoStream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
