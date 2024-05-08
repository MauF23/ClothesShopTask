using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newClothes", menuName = "ClothesInventory")]
public class Inventory : ScriptableObject
{
    public List<Clothes> clothes;
    public int wallet;
    public enum walletBallanceModifier { add, remove }

    public void AddClothes(Clothes newClothes)
    {
        if (!clothes.Contains(newClothes))
        {
            clothes.Add(newClothes);
        }
    }

    public void RemoveClothes(Clothes clothesToRemove)
    {
        if (clothes.Contains(clothesToRemove))
        {
            clothes.Remove(clothesToRemove);
        }
    }

    public void ModifyWalletBalance(int value, walletBallanceModifier modifier)
    {
        switch (modifier)
        {
            case walletBallanceModifier.add:
                wallet += value;
                break;

            case walletBallanceModifier.remove:
                wallet -= value;
                break;
        }

        if (wallet <= 0)
        {
            wallet = 0;
        }
    }
}
