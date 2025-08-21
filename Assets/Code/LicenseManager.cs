using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public static class LicenseManager
{
    private const string LicenseKeyPref = "LicenseKey";
    private const string MachineIdPref = "MachineID";
    private const string ActivatedPref = "Activated";

    private static string encryptionKey = "MySuperSecretKey"; // 16/24/32 chars for AES
    private static string gameId = "b36f2f69-898c-4a83-bd2c-3b53181df255"; // replace with your actual game id

    public static bool IsActivated()
    {
        if (PlayerPrefs.GetInt(ActivatedPref, 0) == 1)
        {
            string encryptedKey = PlayerPrefs.GetString(LicenseKeyPref, "");
            string encryptedMachineId = PlayerPrefs.GetString(MachineIdPref, "");

            string savedKey = DecryptString(encryptedKey, encryptionKey);
            string savedMachineId = DecryptString(encryptedMachineId, encryptionKey);

            string currentMachineId = SystemInfo.deviceUniqueIdentifier;

            return savedMachineId == currentMachineId;
        }
        return false;
    }

    /// <summary>
    /// Attempts activation with the server. Call from a MonoBehaviour via StartCoroutine.
    /// </summary>
    public static IEnumerator Activate(string inputKey, Action<bool> onResult)
    {
        string url =
            $"https://indiegamezonese101.azurewebsites.net/api/games/{gameId}/activation-keys/{inputKey}/activation";

        using (UnityWebRequest request = UnityWebRequest.Put(url, string.Empty))
        {
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success && request.responseCode == 204)
            {
                // Success! Save locally
                string encryptedKey = EncryptString(inputKey, encryptionKey);
                string encryptedMachineId = EncryptString(
                    SystemInfo.deviceUniqueIdentifier,
                    encryptionKey
                );

                PlayerPrefs.SetInt(ActivatedPref, 1);
                PlayerPrefs.SetString(LicenseKeyPref, encryptedKey);
                PlayerPrefs.SetString(MachineIdPref, encryptedMachineId);
                PlayerPrefs.Save();

                onResult?.Invoke(true);
            }
            else if (request.responseCode == 400)
            {
                Debug.LogWarning("Activation failed: invalid key!");
                onResult?.Invoke(false);
            }
            else
            {
                Debug.LogError(
                    $"Activation request failed! Status: {request.responseCode}, Error: {request.error}"
                );
                onResult?.Invoke(false);
            }
        }
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
