using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
    public int TotalNumWaves = 3;
    public GameObject boss;
    public GameObject enemyWave;
    public bool enemyIsSpawned = false;

    bool bossHasDied = false;
    bool playerHasDied = false;
    int totalCompletedWaves = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!enemyIsSpawned && totalCompletedWaves <= TotalNumWaves)
        {
            if (TotalNumWaves - totalCompletedWaves == 1)
            {
                Instantiate(boss);
                totalCompletedWaves++;
                enemyIsSpawned = true;
            }
            else
            {
                Instantiate(enemyWave);
                totalCompletedWaves++;
                enemyIsSpawned = true;
            }

            Debug.Log(totalCompletedWaves);
        }

        if (Input.GetKeyDown(KeyCode.R) && (bossHasDied || playerHasDied))
        {
            Application.LoadLevel(Application.loadedLevel);
        }

	}

    public void SetBossHasDied(bool bossDied)
    {
        bossHasDied = bossDied;
    }

    public bool CheckBossHasDied()
    {
        return bossHasDied;
    }

    public void SetPlayerHasDied(bool isDead)
    {
        playerHasDied = isDead;
    }

    public bool CheckPlayerHasDied()
    {
        return playerHasDied;
    }
}
