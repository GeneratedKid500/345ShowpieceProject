using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

    public Combos[] combos;
    public int longestCombo;

    public void InitialiseCombos() 
    {
        List<Combos> list = new List<Combos>(Resources.LoadAll<Combos>("Combos"));

        foreach (Combos combo in list)
        {
            if (combo.weapon != this)
            {
                list.Remove(combo);
            }
        }

        combos = list.ToArray();

        for (int i = 0; i < combos.Length; i++)
        {
            if (combos[i].combo.Length > longestCombo)
            {
                longestCombo = combos[i].combo.Length;
            }
        }
    }

    public string FindNextCombo(int index, int input)
    {
        Debug.Log("Arrived");
        #region Validation
        if (input != 101 && input != 102)
        {
            Debug.LogError("WEAPON: " + input + " is invalid as 'input'");
            return null;
        }
        else if (index < 0)
        {
            Debug.LogError("WEAPON: " + index + " (index) too small!");
            return null;
        }
        else if (index > longestCombo-1)
        {
            Debug.LogError("WEAPON: " + index + " (index) too large!");
            return null;
        }
        #endregion

        if (input == 101)
        {
            for (int i = 0; i < combos.Length; i++)
            {
                if (combos[i].combo[index].Split(" ")[0] == "Light") 
                {
                    Debug.Log(combos[i].combo[index]);
                    return combos[i].combo[index];
                }
            }
        }
        else
        {
            for (int i = 0; i < combos.Length; i++)
            {
                if (combos[i].combo[index].Split(" ")[0] == "Heavy")
                {
                    Debug.Log(combos[i].combo[index]);
                    return combos[i].combo[index];
                }
            }
        }

        return null;
    }

    public string FindHeavyAttack(int index)
    {
        string[] split = combo[index].Split(" ");
        split[1] = "Heavy";

        string returnAttack = "";
        for (int i = 0; i < split.Length; i++)
        {
            if (split[i] == "1")
            {
                split[i] = "4";
            }

            returnAttack += split[i];

            returnAttack += " ";
        }
        return returnAttack.TrimEnd(' ');
    }

    private void OnValidate()
    {
        string path = AssetDatabase.GetAssetPath(this.GetInstanceID());
        weaponName = Path.GetFileNameWithoutExtension(path);
    }

}
