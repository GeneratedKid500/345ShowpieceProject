using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat System / Attack")]
public class AttackSO : ScriptableObject
{
    [Header("Attack Information")]
    public bool groundAttack;

    public string[] attackAnimatorReferences;
    public string[] attackInputs;
    public bool[] finalAttacks;
    public float[] attackLengths { get; private set; }


    [Header("Attack Stats")]
    public float attackDamage;
    public float attackKnockback;

    public void SetAnimationClipLength(Animator anim, float animSpeedMultiplier)
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        attackLengths = new float[attackAnimatorReferences.Length]; 
        for (int i = 0; i < attackLengths.Length; i++)
        {
            for (int j = 0; j < clips.Length; j++)
            {
                if (clips[j].name == attackAnimatorReferences[i])
                {
                    float length  = clips[j].length / animSpeedMultiplier;

                    attackLengths[i] = (float)Math.Round(length, 2);
                }
            }
        }
    }
}
