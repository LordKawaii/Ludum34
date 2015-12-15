using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
    public int TotalNumWaves = 3;
    public GameObject boss;
    public GameObject enemyWave;
    public bool enemyIsSpawned = false;
    public float timeTillFirstWave = 1;
    public List<AudioClip> beenHitSounds;

    AudioSource auSource;
    bool bossHasDied = false;
    bool playerHasDied = false;
    bool hasPlayedHitSound = false;
    int totalCompletedWaves = 0;
    PlayerController playerCon;
	// Use this for initialization
	void Start () {
        playerCon = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        timeTillFirstWave += Time.time;
        auSource = gameObject.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {

        if (!enemyIsSpawned && totalCompletedWaves < TotalNumWaves && Time.time >= timeTillFirstWave)
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
            
        }

        if (Input.GetKeyDown(KeyCode.R) && (bossHasDied || playerHasDied))
        {
            Application.LoadLevel(Application.loadedLevel);
        }


        if (!auSource.isPlaying && playerCon.CheckIfHit() && !hasPlayedHitSound)
        {
            auSource.clip = beenHitSounds[Random.Range(0, beenHitSounds.Count)];
            auSource.Play();
            hasPlayedHitSound = true;
        }
        if (!playerCon.CheckIfHit() && !playerHasDied)
            hasPlayedHitSound = false;

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
