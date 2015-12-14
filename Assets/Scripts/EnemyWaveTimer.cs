using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyWaveTimer : MonoBehaviour {
    public bool canStartDrop = false;
    public float stopingHight;
    public float desentSpeed;
    public float TimeTillMovingOn = 1;

    Vector2 stopingPosition;
    bool hasStopped = false;
    List<EnemyBotController> enemyControllers;

	// Use this for initialization
	void Start () {
        stopingPosition = new Vector2(0, stopingHight);
        TimeTillMovingOn += Time.time;
        enemyControllers = new List<EnemyBotController>();

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemy.GetComponent<EnemyBotController>() != null)
                enemyControllers.Add(enemy.GetComponent<EnemyBotController>());
        }

        foreach (EnemyBotController botCon in enemyControllers)
        {
            botCon.setCanAttack(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (canStartDrop)
        {
            if (!hasStopped)
            {
                transform.position = Vector2.MoveTowards(transform.position, stopingPosition, desentSpeed * Time.deltaTime);
            }
            else if (Time.time >= TimeTillMovingOn)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(0, -10), desentSpeed * Time.deltaTime);
            }

            if (transform.position == new Vector3(transform.position.x, stopingHight))
            {

                hasStopped = true;
                foreach (EnemyBotController botcon in enemyControllers)
                    botcon.setCanAttack(true);
            }
            else
                foreach (EnemyBotController botcon in enemyControllers)
                    botcon.setCanAttack(false);
            
        }
    }
}
