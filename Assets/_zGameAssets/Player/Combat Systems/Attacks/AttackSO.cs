using UnityEngine;

[CreateAssetMenu(menuName = "Combat System / Attack")]
public class AttackSO : ScriptableObject
{
    [Header("Attack Information")]
    public string[] attackAnimatorReferences;
    public int[] attackInputs;

    [Header("Attack Stats")]
    public float attackDamage;
    public float attackKnockback;


}
