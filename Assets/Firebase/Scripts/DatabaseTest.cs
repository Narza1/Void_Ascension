
using Firebase.Database;
using System;
using System.Collections;
using UnityEngine;
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

   private void CreateUser(string username)
    {
        User newUser = new User(Name.text);
        string json = JsonUtility.ToJson(newUser);
        dbReference.Child("users").Child(userID).SetRawJsonValueAsync(json);
    }
    
    public IEnumerator GetName(Action<string> onCallBack)
    {
        var userNameData = dbReference.Child("users").Child(userID).Child("name").GetValueAsync();
        yield return new WaitUntil(predicate: () => userNameData.IsCompleted);
        if(userNameData != null ) {
            DataSnapshot snapshot = userNameData.Result;
            onCallBack.Invoke(snapshot.Value.ToString());
        }
    }
}
