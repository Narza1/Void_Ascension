using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using static PlayerData;
using static Unity.Burst.Intrinsics.X86;
using Directory = System.IO.Directory;
using File = System.IO.File;
using Random = UnityEngine.Random;

[Serializable]
public class GameManager : MonoBehaviour
{

    public PlayerData playerData;
    private AvatarController player;
    private VisualElement exp;
    private Label level, currentCoins, totalCoins;

    private void Awake()
    {

        GameObject ui = GameObject.Find("UserInterface");
        var doc = ui.GetComponent<UIDocument>().rootVisualElement;
        level = doc.Q("Level") as Label;
        currentCoins = doc.Q("CurrentCoins") as Label;
        totalCoins = doc.Q("TotalCoins") as Label;
        exp = doc.Q("NextLevel");
        playerData = new PlayerData();
        player = GameObject.Find("Player").GetComponent<AvatarController>();
        player.characters = playerData.characters;
        LoadKeys();
        if (SaveFileExists())
            LoadFile();

    }


    // Start is called before the first frame update
    private static string saveFolderPath = "saveFiles/";
    private static string saveFileName = "save.va";
    void Start()
    {
        if (SaveFileExists())
        {
            GetComponent<GameController>().RecoverInventory(playerData.InventoryItems.Select(x => x.guid).ToArray(), playerData.InventoryItems.Select(x => x.quantity).ToArray());



            for (int i = 0; i < playerData.SetItems.Count; i++)
            {
                var aux = playerData.SetItems[i];
                if (aux.guid != "")
                {
                    player.inventory.SetSlots[i].HoldItem(GameController.GetItemByGuid(aux.guid));
                    player.inventory.SetSlots[i].quantity = aux.quantity;

                }
            }
            player.LoadCharacters();
            UpdateUI();
            if (playerData.currentFloor != 0)
            {
                foreach (var character in playerData.characters)
                {
                    if (character.isDead)
                    {

                       // GameObject.Find("Revenant").GetComponent<RevenantController>().StartChar(new CharacterData(character., character.Level, character.floor, character.money, character.weaponGUID, character.accessoryGUID, character.consumableGUID, character.drop));

                    }
                }
                GetComponent<DatabaseTest>().LoadRandomCharacterInFloor(playerData.currentFloor);

            }
        }
        //playerData.currentFloor = 20;
    }

    private void UpdateUI()
    {
        var character = playerData.characters[playerData.currentCharacter];
        level.text = $"{character.Level}/50";
        exp.style.width = Length.Percent(character.Exp / (character.Level * 150) * 100);
    }
    public void DeleteKeys()
    {
        PlayerPrefs.DeleteKey("K");
        PlayerPrefs.DeleteKey("I");
    }
    public static void LoadKeys()
    {
        if (PlayerPrefs.HasKey("K") && PlayerPrefs.HasKey("I"))
        {
            EncryptionUtility.Key = Convert.FromBase64String(PlayerPrefs.GetString("K"));
            EncryptionUtility.IV = Convert.FromBase64String(PlayerPrefs.GetString("I"));
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
            return true;
        return false;
    }

    public static void DeleteSaveFile()
    {
        string filePath = Path.Combine(saveFolderPath, saveFileName);
        if (GameManager.SaveFileExists())
            File.Delete(filePath);


    }
    public static int GetSavedFloor()
    {
        LoadKeys();

        string filePath = Path.Combine(saveFolderPath, saveFileName);
      
        if (File.Exists(filePath))
        {

            try
            {
                PlayerData decryptedObj = EncryptionUtility.DecryptFromFile<PlayerData>(filePath);
                return decryptedObj.currentFloor;
            }
            catch (System.Exception ex)
            {

                Debug.LogError("Error al cargar el archivo: " + ex.Message);
            }
        }
        return -1;
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
                currentCoins.text = playerData.currentMoney.ToString();
                totalCoins.text = (playerData.currentMoney + playerData.totalMoney).ToString();
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
        playerData.currentCharacter = player.currentCharacter;
        playerData.characters[playerData.currentCharacter] = player.characters[playerData.currentCharacter];
        var playerInventory = player.inventory;

        Debug.Log("Voy a empezar a guardar datos");
        if (playerData.InventoryItems.Count != 0)
            playerData.InventoryItems.Clear();
        foreach (var item in player.inventory.InventoryItems)
        {
            if (item.ItemGuid != "")
                playerData.InventoryItems.Add(new InventoryItem(item.ItemGuid, item.quantity));
        }
        if (playerData.SetItems.Count != 0)
            playerData.SetItems.Clear();



        foreach (var item in player.inventory.SetSlots)
        {
            playerData.SetItems.Add(new InventoryItem(item.ItemGuid, item.quantity));
        }

        // Crea la carpeta si no existe
        if (!Directory.Exists(saveFolderPath))
            Directory.CreateDirectory(saveFolderPath);

        // Encripta y guarda el archivo
        EncryptionUtility.EncryptToFile(filePath, playerData);

        Debug.Log("Archivo guardado en: " + filePath);
    }
    public void Death()
    {

        var currentCharacter = playerData.currentCharacter;
        var currentSet = playerData.SetItems;
        var aux = AvatarController.set1 ? 0 : 2;


        var floor = playerData.currentFloor;
        if (floor == 10 || floor == 20 || floor == 30)
            floor--;
        GetComponent<DatabaseTest>().SaveCharacterData(playerData.deaths, currentCharacter, playerData.characters[currentCharacter].Level, floor, playerData.currentMoney, currentSet[aux].guid, currentSet[aux + 1].guid, currentSet[aux + 2].guid, playerData.RandomDrop());
        playerData.Death();
        SaveFile();
    }

    public void DropCoinsExp(int dropCoins, int exp)
    {
        playerData.currentMoney += dropCoins;
        currentCoins.text = playerData.currentMoney.ToString();
        totalCoins.text = (playerData.currentMoney + playerData.totalMoney).ToString();
        if (
        playerData.characters[playerData.currentCharacter].LevelUP(exp))
            GameObject.Find("Player").GetComponent<AvatarController>().ChangeStats(playerData.currentCharacter);
        UpdateUI();


    }

}
[Serializable]
public class PlayerData
{
    public int currentFloor, maxFloor, currentMoney, currentExperience, totalMoney, deaths, currentCharacter;
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
        characters[currentCharacter].isDead=true;
        deaths++;
    }

    public DropItem RandomDrop()
    {
        var aux = GameObject.Find("Player").GetComponent<AvatarController>().inventory.InventoryItems;
        List<InventoryItem> inventoryOcuppied = new List<InventoryItem>();
        foreach (var inventoryItem in InventoryItems)
        {
            if (inventoryItem.guid != "")
                inventoryOcuppied.Add(inventoryItem);

        }
        var size = inventoryOcuppied.Count();
        if (size == 0)
            return new DropItem("", 0);

        InventoryItem item = inventoryOcuppied[Random.Range(0, size - 1)];
        DropItem drop = new DropItem(item.guid, item.quantity);
        foreach (var item2 in aux)
        {
            if (item2.ItemGuid.Equals(drop.itemGUID))
            {
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

