using System.IO;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Combat System / Weapon")]
public class Weapon : ScriptableObject
{
    [Header("Weapon Information")]
    public string weaponName;
    public GameObject modelPrefab;
    public int damage;

    [Header("Attack Animations")]
    public string[] lightAttacks = new string[3];
    public string sprintLight;

    public string[] heavyAttacks = new string[4];
    public string sprintHeavy;

    public string[] airAttacks = new string[2];

    public string[] combo;

    public string FindHeavyAttack(int index)
    {
        string[] split = combo[index].Split(" ");
        split[1] = "Heavy";

        string returnAttack = "";
        for (int i = 0; i < split.Length; i++)
        {
            if (split[i] == "1")
            {
                split[i] = heavyAttacks.Length.ToString();
            }

            returnAttack += split[i];

            returnAttack += " ";
        }
        return returnAttack.TrimEnd(' ');
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        string path = AssetDatabase.GetAssetPath(this.GetInstanceID());
        weaponName = Path.GetFileNameWithoutExtension(path);
    }
#endif

}
