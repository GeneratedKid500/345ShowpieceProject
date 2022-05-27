using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float[] pos;

    public float[] rot;

    public int[] clothes;

    public bool weapon2;

    public bool[] bossesKilled;

    public PlayerData(SaveDataGatherer sdg)
    {
        pos = new float[3];
        pos[0] = sdg.playerPos[0];
        pos[1] = sdg.playerPos[1];
        pos[2] = sdg.playerPos[2];

        rot = new float[4];
        rot[0] = sdg.playerRot[0];
        rot[1] = sdg.playerRot[1];
        rot[2] = sdg.playerRot[2];
        rot[3] = sdg.playerRot[3];

        clothes = sdg.clothes;
        Debug.Log(clothes);

        weapon2 = sdg.isWeapon2;

        bossesKilled = sdg.bossesKilled;
    }
}
