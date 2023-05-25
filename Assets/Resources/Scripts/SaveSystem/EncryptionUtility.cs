using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using UnityEngine;

public static class EncryptionUtility
{
    
    public static byte[] Key = { /* Coloca aquí tu clave de encriptación, debe tener 16, 24 o 32 bytes */
};
    public static byte[] IV = { /* Coloca aquí tu vector de inicialización (IV), debe tener 16 bytes */ };

    public static void EncryptToFile<T>(string filePath, T obj)
    {
        using (var rijAlg = new RijndaelManaged())
        {
            rijAlg.Key = Key;
            rijAlg.IV = IV;

            byte[] encryptedBytes;

            using (var msEncrypt = new MemoryStream())
            {
                using (var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV))
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(csEncrypt, obj);
                    }

                    encryptedBytes = msEncrypt.ToArray();
                }
            }

            File.WriteAllBytes(filePath, encryptedBytes);
        }
    }

    public static T DecryptFromFile<T>(string filePath)
    {
        byte[] encryptedBytes = File.ReadAllBytes(filePath);
        T obj;

        using (var rijAlg = new RijndaelManaged())
        {
            rijAlg.Key = Key;
            rijAlg.IV = IV;

            using (var msDecrypt = new MemoryStream(encryptedBytes))
            {
                using (var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        obj = (T)formatter.Deserialize(csDecrypt);
                    }
                }
            }
        }

        return obj;
    }
    public static void GenerateKeys()
    {
        using (Aes aes = Aes.Create())
        {
            aes.GenerateKey();
            aes.GenerateIV();

            Key = aes.Key;
            IV = aes.IV;

        }

        Debug.Log(Key);
        Debug.Log(IV);
        Debug.Log("Conversion");
        string keyString = System.Convert.ToBase64String(Key);
        PlayerPrefs.SetString("K", keyString);
        string ivString = System.Convert.ToBase64String(IV);
        PlayerPrefs.SetString("I", ivString);
        PlayerPrefs.Save();
        Debug.Log(keyString);
        Debug.Log(ivString);

    }
}
