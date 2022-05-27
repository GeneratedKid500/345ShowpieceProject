using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuelEnemyManager : MonoBehaviour
{
    GameObject[] duelEnemies;
    bool[] isDefeated; 

    void Start()
    {
        duelEnemies = GameObject.FindGameObjectsWithTag("Duel");
        isDefeated = new bool[duelEnemies.Length];
        for (int i = 0; i < isDefeated.Length; i++)
        {
            isDefeated[i] = false;
        }
    }

    public void DuelEnemyDefeated(GameObject enemy)
    {
        for (int i = 0; i < duelEnemies.Length; i++)
        {
            if (duelEnemies[i] == enemy)
            {
                isDefeated[i] = true;
                break;
            }
        }

        for (int i = 0; i < isDefeated.Length; i++)
        {
            if (!isDefeated[i]) return;
        }

        // Game Win
    }
}
