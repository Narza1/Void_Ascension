using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Windows;
using static Unity.Burst.Intrinsics.X86;
using Directory = System.IO.Directory;
using File = System.IO.File;
using Random = UnityEngine.Random;

[Serializable]
public class GameManager : MonoBehaviour
{

    public PlayerData playerData;



    // Start is called before the first frame update
    private string saveFolderPath = "saveFiles/";
    private string saveFileName = "save.va";
    void Start()
    {

        LoadKeys();
        LoadFile();
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
            Debug.Log("ENCONTRADAS KEYS");

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

    public bool SaveFileExists()
    {
        string filePath = Path.Combine(saveFolderPath, saveFileName);
        if (File.Exists(filePath))
        {
            return true;
        }
        return false;
    }
    public void LoadFile()
    {
        string filePath = Path.Combine(saveFolderPath, saveFileName);
        if (File.Exists(filePath))
        {
            try
            {
                PlayerData decryptedObj = EncryptionUtility.DecryptFromFile<PlayerData>(filePath);
                Debug.Log("Archivo cargado: " + filePath);
                playerData = decryptedObj;
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error al cargar el archivo: " + ex.Message);
            }
        }
        else
        {
            //Debug.LogError("El archivo no existe en la ruta especificada: " + filePath);
            playerData = new PlayerData();
        }
    }



    public void SaveFile()
    {

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
    public void Death()
    {
        playerData.Death();
        var currentCharacter = playerData.currentCharacter;
        var currentSet = playerData.SetItems;
        var aux = AvatarController.set1 ? 2 : 5;



        gameObject.GetComponent<DatabaseTest>().SaveCharacterData(playerData.deaths, currentCharacter, playerData.characters[currentCharacter].Level, playerData.currentFloor, playerData.currentMoney, currentSet[aux].ItemGuid, currentSet[aux + 1].ItemGuid, currentSet[aux + 2].ItemGuid, playerData.RandomDrop());
    }
}
[Serializable]
public class PlayerData
{
    public int currentFloor, maxFloor, currentMoney, totalMoney, deaths, currentCharacter;
    public List<InventorySlot> InventoryItems, SetItems;
    public List<Character> characters;

    public PlayerData()
    {
        characters = new List<Character>();
        characters.Add(new MinionCharacter());
        characters.Add(new MageCharacter());
        characters.Add(new WarriorCharacter());
        characters.Add(new ArcherCharacter());
    }
    public void Death()
    {
        currentFloor = currentMoney = 0;
        deaths++;
        characters[currentCharacter].isDead = true;
        currentCharacter = -1;
    }

    public DropItem RandomDrop()
    {
        List<InventorySlot> inventoryOcuppied = new List<InventorySlot>();
        foreach (var inventoryItem in InventoryItems)
        {
            if (inventoryItem.ItemGuid != "")
            {
                inventoryOcuppied.Add(inventoryItem);
            }

        }
        var size = inventoryOcuppied.Count();
        if (size == 0)
        {
            return new DropItem("", 0);
        }

        InventorySlot item = inventoryOcuppied[Random.Range(0, size - 1)];
        DropItem drop = new DropItem(item.ItemGuid, item.quantity);
        item.DropItem();
        return drop;
    }
}

