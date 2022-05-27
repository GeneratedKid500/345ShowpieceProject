using UnityEngine;

public class DuelEnemyManager : MonoBehaviour
{
    GameObject[] duelEnemies;
    bool[] isDefeated;

    private void Awake()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        if (data != null) isDefeated = data.bossesKilled;
    }

    void Start()
    {
        duelEnemies = GameObject.FindGameObjectsWithTag("Duel");
        if (isDefeated == null || isDefeated.Length < 0)
        {
            isDefeated = new bool[duelEnemies.Length];

            for (int i = 0; i < isDefeated.Length; i++)
            {
                isDefeated[i] = false;
            }
        }
        else
        {
            for(int i = 0; i < duelEnemies.Length; i++)
            {
                if (isDefeated[i]) Destroy(duelEnemies[i]);
            }
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

        UnityEngine.SceneManagement.SceneManager.LoadScene("FinalScene");
    }

    public bool[] getDuelBossesDown()
    {
        return isDefeated;
    }
}
