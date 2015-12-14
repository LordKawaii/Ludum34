using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
    const float TRANSFORM_RAND_RANGE = .2f;

    public float movementSpeed = 1;
    public float nanobotChargeRate = .5f;
    public float nanobotGenRate = 1;
    public GameObject nanobot;
    public int lives = 3;
    public float invulerableTime = 1;
    public float percentFlashTakes = 2f;

    List<GameObject> chargedNanobots;
    Stack<GameObject> nanobotsCollected;
    Rigidbody2D rb2D;
    SpriteRenderer spriteRend;
    bool isCharging = false;
    bool isInvulnerable = false;
    bool hasSetInvTime = false;
    float timeTillNextCharge;
    float timeTillNextBotGen;
    float timeTillInvEnds;
    float timeTillNextFlash;
    float InvFlashTime;

    // Use this for initialization
    void Start () {
        spriteRend = gameObject.GetComponent<SpriteRenderer>();
        chargedNanobots = new List<GameObject>();
        nanobotsCollected = new Stack<GameObject>();
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        InvFlashTime = invulerableTime * (percentFlashTakes / 100);
        timeTillNextFlash = Time.time + InvFlashTime;
        timeTillInvEnds = Time.time + invulerableTime;
        timeTillNextCharge = Time.time + nanobotChargeRate;
        timeTillNextBotGen = Time.time + timeTillNextBotGen;
        

    }
	
	// Update is called once per frame
	void Update () {
        MovePlayer();
        CheckIfShooting();

        if (isInvulnerable)
            MakeInvulnerable();

        if((nanobotsCollected.Count + chargedNanobots.Count) < 3 && Time.time >= timeTillNextBotGen)
        { 
            GenerateBot();
        }

        if (lives <= 0)
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    void MovePlayer()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");

        if (horizontalAxis != 0)
        {
            transform.Translate(new Vector3(horizontalAxis * movementSpeed, 0));
        }
        if (verticalAxis != 0)
        {
            transform.Translate(new Vector3(0, verticalAxis * movementSpeed));
        }
    }

    void CheckIfShooting()
    {
        if (Input.GetButton("Fire1"))
        {
            try
            {
                isCharging = true;
                if (nanobotsCollected.Count > 0 && Time.time >= timeTillNextCharge)
                {
                    chargedNanobots.Add(nanobotsCollected.Pop());
                    chargedNanobots[chargedNanobots.Count - 1].GetComponent<NanoBotController>().Charge(chargedNanobots.Count);

                    timeTillNextCharge = Time.time + nanobotChargeRate;
                }
            }
            catch
            {
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            if (chargedNanobots.Count > 0)
            {
                foreach (GameObject nanobot in chargedNanobots)
                {
                    nanobot.GetComponent<NanoBotController>().Fire();
                }
                chargedNanobots = new List<GameObject>();
                isCharging = false;
            }
        }
    }

    void GenerateBot()
    {
        if (nanobot != null)
        {
            Instantiate(nanobot, new Vector3
                (transform.position.x + Random.Range(-TRANSFORM_RAND_RANGE, TRANSFORM_RAND_RANGE), 
                transform.position.y + Random.Range(-TRANSFORM_RAND_RANGE, TRANSFORM_RAND_RANGE)), transform.rotation);
            //End Instantiate

            timeTillNextBotGen = Time.time + nanobotGenRate;
        }
    }

    void MakeInvulnerable()
    {
        if (!hasSetInvTime)
        { 
            timeTillInvEnds = Time.time + invulerableTime;
            hasSetInvTime = true;
        }

        if (Time.time >= timeTillNextFlash)
        {
            if (spriteRend.enabled)
                spriteRend.enabled = false;
            else
                spriteRend.enabled = true;

            timeTillNextFlash = Time.time + InvFlashTime;
        }

        if (Time.time >= timeTillInvEnds)
        { 
            isInvulnerable = false;
            spriteRend.enabled = true;
            hasSetInvTime = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "NanoBot")
        {
            other.transform.parent = transform;
            other.gameObject.GetComponent<NanoBotController>().PickUp();
            nanobotsCollected.Push(other.gameObject);
        }

        if (other.tag == "Enemy" || other.tag == "Wall")
        {
            if (!isInvulnerable)
            {
                lives--;
                isInvulnerable = true;
            }
        }
    }

}
