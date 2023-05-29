using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Windows;
using static PlayerData;
using static Unity.Burst.Intrinsics.X86;
using Directory = System.IO.Directory;
using File = System.IO.File;
using Random = UnityEngine.Random;

[Serializable]
public class GameManager : MonoBehaviour
{

    public PlayerData playerData = new PlayerData();




    // Start is called before the first frame update
    private static string saveFolderPath = "saveFiles/";
    private static string saveFileName = "save.va";
    void Start()
    {

        LoadKeys();
        if (SaveFileExists())
        {
            LoadFile();
            Debug.Log(playerData.InventoryItems.Select(x => x.guid).ToArray());
            Debug.Log(playerData.InventoryItems.Select(x => x.quantity).ToArray());
            GetComponent<GameController>().RecoverInventory(playerData.InventoryItems.Select(x => x.guid).ToArray(), playerData.InventoryItems.Select(x => x.quantity).ToArray());
        }
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

    public static bool SaveFileExists()
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
        var player = GameObject.Find("Player").GetComponent<AvatarController>();
        string filePath = Path.Combine(saveFolderPath, saveFileName);
        playerData.currentCharacter = player.currentCharacter;
        playerData.characters = player.characters;
        var playerInventory = player.inventory;
        if (playerData.InventoryItems.Count != 0)
        {
            playerData.InventoryItems.Clear();

        }
        foreach (var item in player.inventory.InventoryItems)
        {
            playerData.InventoryItems.Add(new InventoryItem(item.ItemGuid, item.quantity));
        }
        if (playerData.SetItems.Count != 0)
        {
            playerData.SetItems.Clear();

        }
        //
        //
        //
        //
        //Me falta asignar los slots de equipo, molto importante
        //
        //
        //
        //
        foreach (var item in player.inventory.SetSlots)
        {
            playerData.SetItems.Add(new InventoryItem(item.ItemGuid, item.quantity));
        }
             
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



        gameObject.GetComponent<DatabaseTest>().SaveCharacterData(playerData.deaths, currentCharacter, playerData.characters[currentCharacter].Level, playerData.currentFloor, playerData.currentMoney, currentSet[aux].guid, currentSet[aux + 1].guid, currentSet[aux + 2].guid, playerData.RandomDrop());
    }
}
[Serializable]
public class PlayerData
{
    public int currentFloor, maxFloor, currentMoney, totalMoney, deaths, currentCharacter;
    public List<InventoryItem> InventoryItems = new List<InventoryItem>(), SetItems = new List<InventoryItem>();
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
        var aux = GameObject.Find("Player").GetComponent<AvatarController>().inventory.InventoryItems;
        List<InventoryItem> inventoryOcuppied = new List<InventoryItem>();
        foreach (var inventoryItem in InventoryItems)
        {
            if (inventoryItem.guid != "")
            {
                inventoryOcuppied.Add(inventoryItem);
            }

        }
        var size = inventoryOcuppied.Count();
        if (size == 0)
        {
            return new DropItem("", 0);
        }

        InventoryItem item = inventoryOcuppied[Random.Range(0, size - 1)];
        DropItem drop = new DropItem(item.guid, item.quantity);
        foreach (var item2 in aux)
        {
            if (item2.ItemGuid.Equals(drop.itemGUID)){
                item2.DropItem();
                break;
            }
        }
        return drop;
    }

    [Serializable]
    public class InventoryItem
    {
        public string guid;
        public int quantity;

        public InventoryItem(string guid, int quantity)
        {
            this.guid = guid;
            this.quantity = quantity;
        }
    }
    }

