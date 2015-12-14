using UnityEngine;
using System.Collections;

public class EnemyBotController : NanoBotController {
    public int hp = 3;
    public bool isBossMinon = false;

    Transform BossTransform;
    bool movingTowardsPlayer = false;
    Vector3 playersLocation;
    Vector3 startingPoint;
	// Override for NanoBotController Start()
	override protected void Start () {
        try
        {
            hasBeenPickedUp = true;
            timeTillChange = Time.time + Random.Range(0, frequencyOfChange);
            rb2d = gameObject.GetComponent<Rigidbody2D>();
        }
        catch
        {
        }
	}

    override protected void Update()
    {

        CheckForDeath();

    }

    void FixedUpdate()
    {
        MoveToAttack();
        if(isBossMinon)
            Swarm();
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
                    transform.parent = BossTransform;
            }
        }
    }

    public override void Attack(GameObject target)
    {
        //base.Attack(target);
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
	
}
