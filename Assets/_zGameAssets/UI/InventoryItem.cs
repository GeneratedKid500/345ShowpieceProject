using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System / Item")]
public class InventoryItem : ScriptableObject
{
    public enum ItemType
    {
        Helmet,
        Chestpiece,
        Legs,
        Feet,
        Weapon
    }

    public ItemType itemType;

    public string title;
    public string description;

    public string icon;
    public SkinnedMeshRenderer mesh;
}
