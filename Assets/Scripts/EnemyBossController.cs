using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBossController : MonoBehaviour {
    bool movingRight = true;
    List<GameObject> miniBaddies;
    float timeTillNextAttack;
    GameObject player;
    GameController gameCon;
    UnityJellySprite jelly;

    public float attackRate = 1;
    public int hp = 6;
    public float movementSpeed;
    public float travelWidth;
    public bool realBoss = false;

    void Awake()
    {
        miniBaddies = new List<GameObject>();
        timeTillNextAttack = Time.time + attackRate;
        gameCon = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemy.transform.parent == transform)
            {
                miniBaddies.Add(enemy);
            } 
        }
        jelly = GetComponent<UnityJellySprite>();
    }
	
	// Update is called once per frame
	void Update ()
    {  
            Move();  
        
        

        if (Time.time >= timeTillNextAttack)
        {
            Attack();
            timeTillNextAttack = Time.time + attackRate;
        }
    }

    void LateUpdate()
    {
        if (hp <= 0)
            Destroy(gameObject);
    }

    void Move()
    {
       
        if (movingRight && transform.position.x <= travelWidth)
            transform.Translate(Vector2.right * Time.deltaTime * movementSpeed);
        if (transform.position.x >= travelWidth)
            movingRight = false;
        if (!movingRight && transform.position.x >= -travelWidth)
            transform.Translate(Vector2.left * Time.deltaTime * movementSpeed);
        if (transform.position.x <= -travelWidth)
            movingRight = true;
    }
     
     
    void MoveKinematic()
    { 
        if (movingRight && jelly.m_CentralBodyOffset.x <= travelWidth)
            jelly.m_CentralBodyOffset += Vector2.right * Time.deltaTime * movementSpeed;
        if (jelly.m_CentralBodyOffset.x >= travelWidth)
            movingRight = false;
        if (!movingRight && jelly.m_CentralBodyOffset.x >= -travelWidth)
            jelly.m_CentralBodyOffset +=  Vector2.left * Time.deltaTime * movementSpeed;
        if (jelly.m_CentralBodyOffset.x <= -travelWidth)
            movingRight = true;
    }

    void Attack()
    {
        int randIndex = Random.Range(0, miniBaddies.Count);
        EnemyBotController miniBaddieCon;
        if (miniBaddies[randIndex] != null)
        { 
            miniBaddieCon = miniBaddies[randIndex].GetComponent<EnemyBotController>();
            if (!miniBaddieCon.CheckIfAttacking())
                miniBaddieCon.Attack(player);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "NanoBot")
        {
            NanoBotController nanoCon = other.GetComponent<NanoBotController>();
            if (!nanoCon.CheckIfAttacking() && !nanoCon.CheckIfSwarming())
                hp--;

            other.GetComponent<NanoBotController>().Attack(gameObject);
        }
    }

    void OnDestroy()
    {
        if (realBoss)
        { 
            gameCon.SetBossHasDied(true);
        }
    }
}
