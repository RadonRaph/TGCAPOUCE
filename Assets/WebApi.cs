using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebApi : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Register(string nickname, string password, Plant selectedPlant, string plantNickname, Action callback)
    {
        
        
        Hash128 hashedPass = new Hash128();
        hashedPass.Append("PolyPollen");
        hashedPass.Append(69420);
        hashedPass.Append(password);

        
        Account acc = CreateAccount(nickname, hashedPass.ToString());
        
        selectedPlant.nickname = plantNickname;
        selectedPlant.ownerId = acc.guid;
        selectedPlant.exchangesCount = 0;
        
        acc.plants.Add(selectedPlant);

        StartCoroutine(TryRegister(acc, selectedPlant));

    }

    public IEnumerator TryRegister(Account acc, Plant selectedPlant)
    {

        WWWForm accForm = new WWWForm();

        accForm.AddField("uuid", acc.guid.ToString());
        accForm.AddField("nickname", acc.nickname);
        accForm.AddField("passwordHash", acc.passwordHash);
        accForm.AddField("gardenCid", acc.garden.guid.ToString());
        accForm.AddField("plants", acc.plants.GetString());
        
        UnityWebRequest www = UnityWebRequest.Post("https://raccoonlabs.fr/polypollen/register.php", accForm);

        yield return www.SendWebRequest();
        
        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log(www.error);
        }
        else {
            Debug.Log("Account created!");
        }
        
        
        WWWForm gardenForm = new WWWForm();

        gardenForm.AddField("guid", acc.garden.guid.ToString());
        gardenForm.AddField("plantGrid", acc.garden.plantGrid.GetString());
        
        UnityWebRequest www2 = UnityWebRequest.Post("https://raccoonlabs.fr/polypollen/gardenCreate.php", gardenForm);

        yield return www2.SendWebRequest();
        
        if (www2.result != UnityWebRequest.Result.Success) {
            Debug.Log(www2.error);
        }
        else {
            Debug.Log("Garden created!");
        }
        
        WWWForm plantForm = new WWWForm();

        plantForm.AddField("guid", selectedPlant.plantId.ToString());
        plantForm.AddField("code", selectedPlant.plantCode);
        plantForm.AddField("owner", selectedPlant.ownerId.ToString());
        plantForm.AddField("nickname", selectedPlant.nickname);
        plantForm.AddField("exchangesCount", selectedPlant.exchangesCount);

        
        UnityWebRequest www3 = UnityWebRequest.Post("https://raccoonlabs.fr/polypollen/plantCreate.php", plantForm);

        yield return www3.SendWebRequest();
        
        if (www3.result != UnityWebRequest.Result.Success) {
            Debug.Log(www3.error);
        }
        else {
            Debug.Log("Plant created!");
        }
    }

    Account CreateAccount(string nickname, string passwordHashed)
    {
        Account acc = new Account();
        
        acc.nickname = name;
        acc.passwordHash = passwordHashed;
        acc.guid = Guid.NewGuid();
        acc.garden = new Garden();
        acc.plants = new PlantList();

        return acc;
    }

    
    
    
}

public class PlantList
{
    private List<Plant> _plants;

    public PlantList()
    {
        _plants = new List<Plant>();
    }

    public void Add(Plant p)
    {
        _plants.Add(p);
    }

    public string GetString()
    {
        string t = "[";

        for (int i = 0; i < _plants.Count; i++)
        {
            t += _plants[i].plantId;
            t += "!";
        }

        t += "]";
        return t;

    }
}

public class Garden
{
    public Guid guid;
    public PlantList plantGrid;
    public float lastmodified;

    public Garden()
    {
        guid = Guid.NewGuid();
        plantGrid = new PlantList();
    }
}

public struct Account
{
    public int id;
    public Guid guid;
    public string nickname;
    public string passwordHash;
    public Garden garden;
    public PlantList plants;



}
