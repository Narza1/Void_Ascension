
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class DatabaseTest : MonoBehaviour
{
    public InputField Name;
    private string userID;
    DatabaseReference dbReference;
    void Start()
    {
        userID = SystemInfo.deviceUniqueIdentifier;
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }


    public void Fuck()
    {
       
        SaveCharacterData(1, 48, 12, 4500, "wwww", "aaaa", "ccccc");
       

    }
    public CharacterData GetRandomCharacterInfo()
    {
        CharacterData characterData = null;
        StartCoroutine(GetRandomCharacterByFloor(12, (CharacterData character) =>
        {
            
            if (character != null)
            {
                characterData = character;
                // Haz algo con el personaje aleatorio obtenido
                Debug.Log("Personaje aleatorio encontrado: " + character.lv);
       

            }
            else
            {
                // No se encontró ningún personaje en el piso especificado
                Debug.Log("No se encontraron personajes en el piso especificado");
            }
        }));
        return characterData;

    }

    public void SaveCharacterData(int characterType, int level, int floor, int money, string weaponGUID, string accessoryGUID, string consumableGUID)
    {
        DropItem drop = new DropItem("test", 4);
        CharacterData characterData = new CharacterData();
        characterData.characterType = characterType;
        characterData.lv = level;
        characterData.floor = floor;
        characterData.money = money;
        characterData.weaponGUID = weaponGUID;
        characterData.accessoryGUID = accessoryGUID;
        characterData.consumableGUID = consumableGUID;
        characterData.drop = drop;

        string json = JsonUtility.ToJson(characterData);
        dbReference.Child("characters").Child(userID).SetRawJsonValueAsync(json);
       
        //User newUser = new User(Name.text);
        //string json3 = JsonUtility.ToJson(newUser);
        //dbReference.Child("Users").Child(userID).SetRawJsonValueAsync(json3);
    }

    

    //public void SaveCharacterData(int characterType, int level, int floor, int money, string weaponGUID, string accessoryGUID, string consumableGUID, DropItem drop)
    //{
    //    CharacterData characterData = new CharacterData();
    //    characterData.CharacterType = characterType;
    //    characterData.Lv = level;
    //    characterData.Floor = floor;
    //    characterData.Money = money;
    //    characterData.WeaponGUID = weaponGUID;
    //    characterData.AccessoryGUID = accessoryGUID;
    //    characterData.ConsumableGUID = consumableGUID;
    //    characterData.Drop = drop;

    //    // Convertir el objeto DropItem a formato JSON
    //    string dropJson = JsonUtility.ToJson(drop);

    //    // Agregar el JSON del objeto DropItem a CharacterData
    //    characterData.dropJson = dropJson;

    //    string json = JsonUtility.ToJson(characterData);
    //    dbReference.Child("characters").Child(userID).SetRawJsonValueAsync(json);
    //}

    public IEnumerator GetRandomCharacterByFloor(int targetFloor, Action<CharacterData> onCallBack)
    {
        var charactersRef = dbReference.Child("characters");

        var query = charactersRef.OrderByChild("floor").EqualTo(targetFloor).GetValueAsync();
        yield return new WaitUntil(() => query.IsCompleted);

        if (query.Exception != null)
        {
            Debug.LogError($"Failed to retrieve character data: {query.Exception.Message}");
            yield break;
        }

        if (query.Result != null && query.Result.Value != null)
        {
            List<DataSnapshot> matchingCharacters = new List<DataSnapshot>();

            foreach (var childSnapshot in query.Result.Children)
            {
                matchingCharacters.Add(childSnapshot);
            }

            if (matchingCharacters.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, matchingCharacters.Count);
                string json = matchingCharacters[randomIndex].GetRawJsonValue();
                CharacterData characterData = JsonUtility.FromJson<CharacterData>(json);
                Debug.Log(characterData);
                onCallBack.Invoke(characterData);
                yield break;
            }
        }

        Debug.Log("No character data found with the specified floor.");
    }



    //public IEnumerator GetName(Action<string> onCallBack)
    //{
    //    var userNameData = dbReference.Child("users").Child(userID).Child("name").GetValueAsync();
    //    yield return new WaitUntil(predicate: () => userNameData.IsCompleted);
    //    if (userNameData != null)
    //    {
    //        DataSnapshot snapshot = userNameData.Result;
    //        onCallBack.Invoke(snapshot.Value.ToString());
    //    }
    //}

    //public void getUserInfo()
    //{
    //    StartCoroutine(GetName((string name) =>
    //    {
    //    NameText.text = name;
            
    //    }));
    //}
}
