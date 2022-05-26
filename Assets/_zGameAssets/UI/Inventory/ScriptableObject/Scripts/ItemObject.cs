using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "NewItemObject", menuName = "Item System/Add Item", order = 2)]
public class ItemObject : ScriptableObject
{
    [SerializeField] bool fileName = true;

    [Header("Item Details")]
    public GameObject itemModel;
    public GameObject itemPrefab;
    public Sprite icon;
    public string itemName;
    [TextArea(4, 16)]
    public string itemDescription;

    [Header("Item Attributes")]
    public bool consumable = true;
    public bool forHealing;
    public int hpValue;
    public int maxStackQuantity;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (fileName)
        {
            string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
            itemName = Path.GetFileNameWithoutExtension(assetPath);
        }
        else
        {
            itemName = null;
        }
    }
#endif
}
