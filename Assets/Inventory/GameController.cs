using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class ItemDetails
{
    public string Name;
    public string GUID;
    public Sprite Icon;
    public bool CanDrop;
    public ObjectType objectType;
    public int quantity;
    
}
public class EquipmentDetails: ItemDetails
{
    public EquipmentType equipmentType;
    public int upgradeLevel;
    public int baseAttack;

}
public enum ObjectType
{
    Equipment,
    Accesory,
    Consumable
}

public enum EquipmentType
{
    Sword,
    Bow,
    Staff
}
public class Consumable : ItemDetails
{
    public int recoveryValue;

}

public class Throwing : ItemDetails
{
    public int damage;

}

public enum InventoryChangeType
{
    Pickup,
    Drop
}
public delegate void OnInventoryChangedDelegate(string[] itemGuid, InventoryChangeType change);

/// <summary>
/// Generates and controls access to the Item Database and Inventory Data
/// </summary>
public class GameController : MonoBehaviour
{
    [SerializeField]
    public List<Sprite> IconSprites;
    private static Dictionary<string, ItemDetails> m_ItemDatabase = new Dictionary<string, ItemDetails>();
    private List<ItemDetails> m_PlayerInventory = new List<ItemDetails>();
    public static event OnInventoryChangedDelegate OnInventoryChanged = delegate { };


    private void Awake()
    {
        PopulateDatabase();
    }

    private void Start()
    {
        //Add the ItemDatabase to the players inventory and let the UI know that some items have been picked up
        m_PlayerInventory.AddRange(m_ItemDatabase.Values);

      
        OnInventoryChanged.Invoke(m_PlayerInventory.Select(x => x.GUID).ToArray(), InventoryChangeType.Pickup);
    }

    /// <summary>
    /// Populate the database
    /// </summary>
    public void PopulateDatabase()
    {
        m_ItemDatabase.Add("8B0EF21A-F2D9-4E6F-8B79-031CA9E202BA", new EquipmentDetails()
        {
            Name = "Broken Sword",
            GUID = "8B0EF21A-F2D9-4E6F-8B79-031CA9E202BA",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("broken_sword")),
            CanDrop = true,
            objectType = ObjectType.Equipment,
            equipmentType = EquipmentType.Sword
        });

        m_ItemDatabase.Add("992D3386-B743-4CD3-9BB7-0234A057C265", new EquipmentDetails()
        {
            Name = "Normal Sword",
            GUID = "992D3386-B743-4CD3-9BB7-0234A057C265",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("normal_sword")),
            CanDrop = true,
            objectType = ObjectType.Equipment,
            equipmentType = EquipmentType.Sword
        });

        m_ItemDatabase.Add("1B9C6CAA-754E-412D-91BF-37F22C9A0E7B", new EquipmentDetails()
        {
            Name = "Rare Sword",
            GUID = "1B9C6CAA-754E-412D-91BF-37F22C9A0E7B",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("rare_sword")),
            CanDrop = true,
            objectType = ObjectType.Equipment,
            equipmentType = EquipmentType.Sword
        });

        m_ItemDatabase.Add("BD51A9E6-F7D2-4C7A-9D78-23C95F806B91", new EquipmentDetails()
        {
            Name = "Common Bow",
            GUID = "BD51A9E6-F7D2-4C7A-9D78-23C95F806B91",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("common_bow")),
            CanDrop = true,
            objectType = ObjectType.Equipment,
            equipmentType = EquipmentType.Bow
        });

        m_ItemDatabase.Add("A431BCDF-8E20-4B9D-AC64-98E76A0F3C25", new EquipmentDetails()
        {
            Name = "Rare Bow",
            GUID = "A431BCDF-8E20-4B9D-AC64-98E76A0F3C25",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("rare_bow")),
            CanDrop = true,
            objectType = ObjectType.Equipment,
            equipmentType = EquipmentType.Bow
        });

        m_ItemDatabase.Add("82C5EDD7-654A-43F6-BD91-0A3B8E9C257F", new EquipmentDetails()
        {
            Name = "Skeleton Bow",
            GUID = "82C5EDD7-654A-43F6-BD91-0A3B8E9C257F",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("skeleton_bow")),
            CanDrop = true,
            objectType = ObjectType.Equipment,
            equipmentType = EquipmentType.Bow
        });

        m_ItemDatabase.Add("4FEB6D8A-2C73-4B1F-A37D-E1595C732A48", new EquipmentDetails()
        {
            Name = "Common Staff",
            GUID = "4FEB6D8A-2C73-4B1F-A37D-E1595C732A48",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("staff_common")),
            CanDrop = true,
            objectType = ObjectType.Equipment,
            equipmentType = EquipmentType.Staff
        });

        m_ItemDatabase.Add("7D9EF82A-1035-4924-88E0-5C6BA157B42D", new EquipmentDetails()
        {
            Name = "Incomplete Skeleton Staff",
            GUID = "7D9EF82A-1035-4924-88E0-5C6BA157B42D",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("staff_skeleton_broken")),
            CanDrop = true,
            objectType = ObjectType.Equipment,
            equipmentType = EquipmentType.Staff
        });

        m_ItemDatabase.Add("9A4BCE32-7D6A-4F6C-8E0D-51B9F83017C9", new EquipmentDetails()
        {
            Name = "Skeleton Staff",
            GUID = "9A4BCE32-7D6A-4F6C-8E0D-51B9F83017C9",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("staff_skeleton")),
            CanDrop = true,
            objectType = ObjectType.Equipment,
            equipmentType = EquipmentType.Staff
        });

        m_ItemDatabase.Add("F3C72E4B-B158-42CA-80F7-9AD8A6E90F26", new ItemDetails()
        {
            Name = "Broken Shield",
            GUID = "F3C72E4B-B158-42CA-80F7-9AD8A6E90F26",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("shield_broken")),
            CanDrop = true,
            objectType = ObjectType.Accesory
        });

        m_ItemDatabase.Add("563E91FA-A7E5-4D4C-8136-98E2B73F9A46", new ItemDetails()
        {
            Name = "Common Shield",
            GUID = "563E91FA-A7E5-4D4C-8136-98E2B73F9A46",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("shield_common")),
            CanDrop = true,
            objectType = ObjectType.Accesory
        });

        m_ItemDatabase.Add("9C5F237A-71E0-4D81-9B9D-FAFCD086B85A", new ItemDetails()
        {
            Name = "Rare Shield",
            GUID = "9C5F237A-71E0-4D81-9B9D-FAFCD086B85A",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("shield_rare")),
            CanDrop = true,
            objectType = ObjectType.Accesory
        });

        m_ItemDatabase.Add("C84D72B3-5F06-49B4-B0AE-1B6936C2E1AA", new ItemDetails()
        {
            Name = "Common Arrow",
            GUID = "C84D72B3-5F06-49B4-B0AE-1B6936C2E1AA",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("arrow1")),
            CanDrop = true,
            objectType = ObjectType.Accesory
        });

        m_ItemDatabase.Add("2AEDC153-7C97-462E-A6F7-8B0DAF9F6DB4", new ItemDetails()
        {
            Name = "Rare Arrow",
            GUID = "2AEDC153-7C97-462E-A6F7-8B0DAF9F6DB4",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("arrow2")),
            CanDrop = true,
            objectType = ObjectType.Accesory
        });

        m_ItemDatabase.Add("80F6D95E-ABF2-4127-923C-58A6E4A917B3", new Consumable()
        {
            Name = "Small Potion",
            GUID = "80F6D95E-ABF2-4127-923C-58A6E4A917B3",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("potion_small")),
            CanDrop = true,
            objectType = ObjectType.Consumable
        });

        m_ItemDatabase.Add("6B3D9A85-40A2-4D7E-B5A7-CEFAC4BC5F88", new Consumable()
        {
            Name = "Medium Potion",
            GUID = "6B3D9A85-40A2-4D7E-B5A7-CEFAC4BC5F88",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("potion_medium")),
            CanDrop = true,
            objectType = ObjectType.Consumable
        });

        m_ItemDatabase.Add("1F78B396-AE1D-4891-A6D4-70E6B2DF3E0C", new Consumable()
        {
            Name = "Large Potion",
            GUID = "1F78B396-AE1D-4891-A6D4-70E6B2DF3E0C",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("potion_large")),
            CanDrop = true,
            objectType = ObjectType.Consumable
        });

        m_ItemDatabase.Add("E6AD398F-9654-46A6-BCA7-25C809D046D8", new Throwing()
        {
            Name = "Throwing Knife",
            GUID = "E6AD398F-9654-46A6-BCA7-25C809D046D8",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("throwing_knife")),
            CanDrop = true,
            objectType = ObjectType.Consumable
        });

        m_ItemDatabase.Add("5FC3A2D7-3B89-4E53-BCE1-9E7FD542D678", new Throwing()
        {
            Name = "Rare Throwing Knife",
            GUID = "5FC3A2D7-3B89-4E53-BCE1-9E7FD542D678",
            Icon = IconSprites.FirstOrDefault(x => x.name.Equals("throwing_rare_knife")),
            CanDrop = true,
            objectType = ObjectType.Consumable
        });


    }

    /// <summary>
    /// Retrieve item details based on the GUID
    /// </summary>
    /// <param name="guid">ID to look up</param>
    /// <returns>Item details</returns>
    public static ItemDetails GetItemByGuid(string guid)
    {
        if (m_ItemDatabase.ContainsKey(guid))
        {
            return m_ItemDatabase[guid];
        }

        return null;
    }

}


    //Name = "Broken Sword",
    //GUID = "8B0EF21A-F2D9-4E6F-8B79-031CA9E202BA"
   


    //Name = "Normal Sword",
    //GUID = "992D3386-B743-4CD3-9BB7-0234A057C265"
 


    //Name = "Rare Sword",
    //GUID = "1B9C6CAA-754E-412D-91BF-37F22C9A0E7B"
    


    //Name = "Common Bow",
    //GUID = "BD51A9E6-F7D2-4C7A-9D78-23C95F806B91"
    


    //Name = "Rare Bow",
    //GUID = "A431BCDF-8E20-4B9D-AC64-98E76A0F3C25"
  

    //Name = "Skeleton Bow",
    //GUID = "82C5EDD7-654A-43F6-BD91-0A3B8E9C257F"
   

    //Name = "Common Staff",
    //GUID = "4FEB6D8A-2C73-4B1F-A37D-E1595C732A48"

    //Name = "Incomplete Skeleton Staff",
    //GUID = "7D9EF82A-1035-4924-88E0-5C6BA157B42D"
 


    //Name = "Skeleton Staff",
    //GUID = "9A4BCE32-7D6A-4F6C-8E0D-51B9F83017C9"
  


    //Name = "Broken Shield",
    //GUID = "F3C72E4B-B158-42CA-80F7-9AD8A6E90F26"
  

    //Name = "Common Shield",
    //GUID = "563E91FA-A7E5-4D4C-8136-98E2B73F9A46"
   


    //Name = "Rare Shield",
    //GUID = "9C5F237A-71E0-4D81-9B9D-FAFCD086B85A"
  

    //Name = "Common Arrow",
    //GUID = "C84D72B3-5F06-49B4-B0AE-1B6936C2E1AA"
   

    //Name = "Rare Arrow",
    //GUID = "2AEDC153-7C97-462E-A6F7-8B0DAF9F6DB4"
   

    //Name = "Small Potion",
    //GUID = "80F6D95E-ABF2-4127-923C-58A6E4A917B3"
  

    //Name = "Medium Potion",
    //GUID = "6B3D9A85-40A2-4D7E-B5A7-CEFAC4BC5F88"
  

    //Name = "Large Potion",
    //GUID = "1F78B396-AE1D-4891-A6D4-70E6B2DF3E0C"

    //Name = "Throwing Knife",
    //GUID = "E6AD398F-9654-46A6-BCA7-25C809D046D8"
 


    //Name = "Rare Throwing Knife",
    //GUID = "5FC3A2D7-3B89-4E53-BCE1-9E7FD542D678"
  