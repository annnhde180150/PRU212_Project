using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ShopItem", menuName = "Scriptable Objects/New Shop Item", order = 1)]
public class ShopItemSo : ScriptableObject
{
    public string title;
    public string description;
    public int baseCost;
}
