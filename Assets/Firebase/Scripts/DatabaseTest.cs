
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class DatabaseTest : MonoBehaviour
{
    private string userID;
    DatabaseReference dbReference;
    public static int floor = 12;
    void Start()
    {
        userID = SystemInfo.deviceUniqueIdentifier;
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        Fuck2();

    }


    public void Fuck()
    {

        //SaveCharacterData(1, 1, 12, 4500, "1B9C6CAA-754E-412D-91BF-37F22C9A0E7B", "", "");


    }
    public void Fuck2()
    {

        StartCoroutine(GetRandomCharacterByFloor(floor, (CharacterData character) =>
        {
            GameObject.Find("Revenant").GetComponent<RevenantController>().StartChar(new CharacterData(character.characterType, character.lv, character.floor, character.money, character.weaponGUID, character.accessoryGUID, character.consumableGUID, character.drop));

        }));
    }
    public CharacterData GetRandomCharacterInfo()
    {
        CharacterData characterData = null;
        StartCoroutine(GetRandomCharacterByFloor(floor, (CharacterData character) =>
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

    public void SaveCharacterData(int deaths,int characterType, int level, int floor, int money, string weaponGUID, string accessoryGUID, string consumableGUID, DropItem drop)
    {
       
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
        try { 
        dbReference.Child("characters").Child(userID + deaths).SetRawJsonValueAsync(json);
        }catch(ArgumentException ae)
        {
            Debug.Log(ae.Message);
        }

        //User newUser = new User(Name.text);
        //string json3 = JsonUtility.ToJson(newUser);
        //dbReference.Child("Users").Child(userID).SetRawJsonValueAsync(json3);
    }



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
                onCallBack.Invoke(characterData);
                yield break;
            }
        }

        Debug.Log("No character data found with the specified floor.");
    }

}
