using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;

public class SaveManager : MonoBehaviour
{
    public string saveStringSuffix;

    public void SaveClothes(List<Clothes> clothesList)
    {
        try
        {
            ClothesContainer clothesContainer = new ClothesContainer(clothesList);
            string json = JsonUtility.ToJson(clothesContainer);
            PlayerPrefs.SetString($"{saveStringSuffix}Clothes", json);
            PlayerPrefs.Save();
            Debug.Log($"<color=cyan>SaveClothes</color>");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"<color=red>Error saving clothes: {e.Message}</color>");
        }
    }

    public void SaveClothes(List<Clothes> clothesList, string key)
    {
        try
        {
            ClothesContainer clothesContainer = new ClothesContainer(clothesList);
            string json = JsonUtility.ToJson(clothesContainer);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
            Debug.Log($"<color=cyan>SaveClothes with key {key}</color>");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"<color=red>Error saving clothes: {e.Message}</color>");
        }
    }

    public void SaveInventory(List<Clothes> clotheList, int gold)
    {
        SaveClothes(clotheList);
        SaveWallet(gold);
    }

    public List<Clothes> LoadClothes()
    {
        string filePath = PlayerPrefs.GetString($"{saveStringSuffix}Clothes");
        if (string.IsNullOrEmpty(filePath))
        {
            return null;
        }
        else
        {
            return JsonUtility.FromJson<ClothesContainer>(filePath).clothesList;
        }
    }

    public List<Clothes> LoadClothes(string key)
    {
        string filePath = PlayerPrefs.GetString(key);
        if (string.IsNullOrEmpty(filePath))
        {
            return null;
        }
        else
        {
            return JsonUtility.FromJson<ClothesContainer>(filePath).clothesList;
        }
    }

    [Button("DeleteAllSavedData")]
    private void DeleteAllSavedData()
    {
        PlayerPrefs.DeleteAll();
    }

    private void SaveWallet(int amount)
    {
        PlayerPrefs.SetInt($"{saveStringSuffix}GoldAmount", amount);
        PlayerPrefs.Save();
    }

    public int LoadWallet()
    {
        return PlayerPrefs.GetInt($"{saveStringSuffix}GoldAmount");
    }

}

[System.Serializable]
public class ClothesContainer
{
    public ClothesContainer (List<Clothes> clothes) 
    { 
        this.clothesList = clothes;
    }

    [SerializeField]
    public  List<Clothes> clothesList;
}
