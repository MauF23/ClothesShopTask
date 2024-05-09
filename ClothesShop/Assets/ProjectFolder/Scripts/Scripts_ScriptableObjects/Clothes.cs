using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "newClothes", menuName = "Clothes")]
public class Clothes:ScriptableObject
{
    [SerializeField]
    public enum ClothesType {pelvis, torso, hood}

    [SerializeField]
    public ClothesType clothesType;

    [SerializeField]
    public new string name;

    [SerializeField]
    public int price;

    [SerializeField]
    public Sprite clotheSprite;
    [SerializeField]
    private bool equiped;
}
