using System.Collections.Generic;
using UnityEngine;

public class AttackRegistryBuilder
{
    private AttackSO[] comboAttackRegister;
    private AttackSO[] squareInputAttackRegister;
    private AttackSO[] triangleInputAttackRegister;
    private AttackSO[] circleInputAttackRegister;

    public AttackRegistryBuilder(Animator anim)
    {
        AttackSO[] fullAttackArray = Resources.LoadAll<AttackSO>("");
        List<AttackSO> comboAttackList = new List<AttackSO>();
        List<AttackSO> squareInputAttackList = new List<AttackSO>();
        List<AttackSO> triangleInputAttackList = new List<AttackSO>();
        List<AttackSO> circleInputAttackList = new List<AttackSO>();

        for (int i = 0; i < fullAttackArray.Length; i++)
        {
            if (fullAttackArray[i].attackAnimatorReferences.Length > 1)
            {
                comboAttackList.Add(fullAttackArray[i]);
            }
            else
            {
                for (int j = 0; i < fullAttackArray[i].attackInputs.Length; i++)
                {
                    if (!comboAttackList.Contains(fullAttackArray[i]))
                    {
                        if (fullAttackArray[i].attackInputs[j] == "101")
                        {
                            squareInputAttackList.Add(fullAttackArray[i]);
                        }
                        else if (fullAttackArray[i].attackInputs[j] == "102")
                        {
                            triangleInputAttackList.Add(fullAttackArray[i]);
                        }
                        else if (fullAttackArray[i].attackInputs[j] == "103")
                        {
                            circleInputAttackList.Add(fullAttackArray[i]);
                        }
                    }
                }
            }
        }

        comboAttackRegister = comboAttackList.ToArray();
        squareInputAttackRegister = squareInputAttackList.ToArray();
        triangleInputAttackRegister = triangleInputAttackList.ToArray();
        circleInputAttackRegister = circleInputAttackList.ToArray();

        AttackSO[][] temp = new AttackSO[][] { comboAttackRegister, squareInputAttackRegister, triangleInputAttackRegister, circleInputAttackRegister };
        float speed = anim.GetFloat("ATTACK_ANIMATION_SPEED_MULTIPLIER");

        for (int i = 0; i < temp.Length; i++)
        {
            for (int j = 0; j < temp[i].Length; j++)
            {
                temp[i][j].SetAnimationClipLength(anim, speed);
            }
        }
    }

    public AttackSO[][] BuildRegistry()
    {
        AttackSO[][] temp = new AttackSO[][] { comboAttackRegister, squareInputAttackRegister, triangleInputAttackRegister, circleInputAttackRegister };
        return temp;
    }
}
