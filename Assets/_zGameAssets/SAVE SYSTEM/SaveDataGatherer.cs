using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataGatherer : MonoBehaviour
{
    public Vector3 playerPos;
    public Quaternion playerRot;

    public int[] clothes;

    public bool isWeapon2;

    public bool[] bossesKilled;

    public SaveDataGatherer()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;

        playerPos = player.position;
        playerRot = player.rotation;

        clothes = player.GetComponent<PlayerMainStateManager>().extractCostume();

        isWeapon2 = player.GetComponent<PlayerActionDistributor>().isWeapon2();

        DuelEnemyManager temp = GameObject.FindGameObjectWithTag("SpawnPoint").GetComponentInParent<DuelEnemyManager>();
        bossesKilled = temp.getDuelBossesDown();
    }
}
