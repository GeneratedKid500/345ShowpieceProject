using UnityEngine;

[CreateAssetMenu(menuName = "Combat System / Combo Holder")]
public class Combos : ScriptableObject
{
    public bool groundCombo = true;

    public Weapon weapon;

    public string[] combo;
}
