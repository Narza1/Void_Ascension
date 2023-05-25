using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Windows;
using Directory = System.IO.Directory;
using File = System.IO.File;

public class GameManager : MonoBehaviour
{
    
    // Start is called before the first frame update
    private string saveFolderPath = "saveFiles/";
    private string saveFileName = "save.va";
    void Start()
    {

        LoadKeys();
    }
    public void DeleteKeys()
    {
        PlayerPrefs.DeleteKey("K");
        PlayerPrefs.DeleteKey("I");
    }
    public void LoadKeys()
    {
        if (PlayerPrefs.HasKey("K") && PlayerPrefs.HasKey("I"))
        {
            Debug.Log("eNCONTRADAS KEYS");
                
                EncryptionUtility.Key = Convert.FromBase64String(PlayerPrefs.GetString("K"));
                EncryptionUtility.IV = Convert.FromBase64String(PlayerPrefs.GetString("I"));
            Debug.Log(EncryptionUtility.Key);
            Debug.Log(EncryptionUtility.IV);
        }
        else
        {
            EncryptionUtility.GenerateKeys();
        }
    }
    public void LoadFile()
    {
        string filePath = Path.Combine(saveFolderPath, saveFileName);
        if (File.Exists(filePath))
        {
            try
            {
                PlayerData decryptedObj = EncryptionUtility.DecryptFromFile<PlayerData>(filePath);
                Debug.Log("Archivo cargado: " + filePath );
                Debug.Log("maxFloor: " + decryptedObj.maxFloor);
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error al cargar el archivo: " + ex.Message);
            }
        }
        else
        {
            Debug.LogError("El archivo no existe en la ruta especificada: " + filePath);
        }
    }



public void SaveFile()
    {
        PlayerData playerData = new PlayerData();

        playerData.maxFloor = 99;

        string filePath = Path.Combine(saveFolderPath, saveFileName);

        // Crea la carpeta si no existe
        if (!Directory.Exists(saveFolderPath))
        {
            Directory.CreateDirectory(saveFolderPath);
        }

        // Encripta y guarda el archivo
        EncryptionUtility.EncryptToFile(filePath, playerData);

        Debug.Log("Archivo guardado en: " + filePath);
    }


}

[Serializable]
public class PlayerData
{
    //public List<Character> charactersData;
    public int maxFloor;//, currentFloor, currentCoins, savedCoins, totalCoins;
    //public List<InventorySlot> equipment;
    //public List<InventorySlot> inventory;


}
