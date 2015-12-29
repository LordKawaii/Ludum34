using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBotController : NanoBotController {
    public int hp = 3;
    public bool isBossMinon = false;
    public int numBotsSpawnedOnDeath = 2;
    public GameObject nanoBot;
    public List<AudioClip> hitSounds;
    public List<AudioClip> talkSounds;
    public float talkRate = .5f;

    Transform BossTransform;
    bool movingTowardsPlayer = false;
    bool canStartAttack = false;
    Vector3 playersLocation;
    Vector3 startingPoint;
    AudioSource auSource;
    float timeTillTalk;

	// Override for NanoBotController Start()
	override protected void Start () {
        try
        {
            timeTillTalk = Time.time + (talkRate / Random.Range(0f, 1f));
            hasBeenPickedUp = true;
            timeTillChange = Time.time + Random.Range(0, frequencyOfChange);
            rb2d = gameObject.GetComponent<Rigidbody2D>();
            auSource = GetComponent<AudioSource>();
        }
        catch
        {
        }
	}

    override protected void Update()
    {

        CheckForDeath();


        if (Time.time >= timeTillTalk)
        {
            timeTillTalk = Time.time + 1f / (talkRate * Random.Range(0f, 1f));
            if (!auSource.isPlaying)
            {
                auSource.volume = .5f;
                auSource.clip = talkSounds[Random.Range(0, talkSounds.Count)];
                auSource.Play();
            }

        }
    }

    void FixedUpdate()
    {
        if (canStartAttack)
        { 
            MoveToAttack();
            if(isBossMinon)
                Swarm();
        }
    }

    void LateUpdate()
    {
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    void CheckForDeath()
    {
        if (hp <= 0)
        {
            foreach (Transform nanobotTransform in GetComponentInChildren<Transform>())
            {
                nanobotTransform.gameObject.GetComponent<NanoBotController>().EndAttack();
            }
            for (int i = 0; i < numBotsSpawnedOnDeath; i++)
            {
                GameObject nano = Instantiate(nanoBot, new Vector3(transform.position.x + Random.Range(-.2f, .2f), transform.position.y + Random.Range(-.2f, .2f)), new Quaternion(0, 0, 0, 0)) as GameObject;
                nano.GetComponent<Rigidbody2D>().AddForce(Vector2.down * 150);
            }
        }
    }

    void MoveToAttack()
    {
        
        if (movingTowardsPlayer)
        {
            
            transform.position = Vector3.MoveTowards(transform.position, playersLocation,  fireSpeed * Time.deltaTime);
            RotateTowards(playersLocation);
            if (transform.position == playersLocation)
                movingTowardsPlayer = false;
        }
        else if (isAttacking)
        {

            transform.position = Vector3.MoveTowards(transform.position, startingPoint, fireSpeed * Time.deltaTime);
            RotateTowards(startingPoint);
            if (transform.position == startingPoint)
            { 
                isAttacking = false;
                rb2d.isKinematic = false;
                if (isBossMinon)
                {
                    transform.parent = BossTransform; 
                }
                    
            }
        }
    }

    public override void Attack(GameObject target)
    {
        if (canStartAttack)
        { 
            movingTowardsPlayer = true;
            isAttacking = true;
            if (isBossMinon)
            { 
                BossTransform = transform.parent;
                startingPoint = BossTransform.position; 
            }
            else
                startingPoint = new Vector3(transform.position.x, transform.position.y);

            rb2d.velocity = Vector2.zero;
            rb2d.isKinematic = true;
            transform.parent = null;
            playersLocation = new Vector3(target.transform.position.x, target.transform.position.y);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "NanoBot")
        {
            NanoBotController nanoCon = other.GetComponent<NanoBotController>();
            if (!nanoCon.CheckIfAttacking() && !nanoCon.CheckIfSwarming())
            { 
                hp--;

                other.GetComponent<NanoBotController>().Attack(gameObject);
                if (!auSource.isPlaying)
                {
                    auSource.volume = 1f;
                    auSource.clip = hitSounds[Random.Range(0, hitSounds.Count)];
                    auSource.Play();
                }
            }

        }
    }

    public bool checkCanAttack()
    {
        return canStartAttack;
    }

    public void setCanAttack(bool canAttack)
    {
        canStartAttack = canAttack;
    }
	
}
