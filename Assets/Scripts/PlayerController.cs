using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
    const float TRANSFORM_RAND_RANGE = .2f;

    public float movementSpeed = 1;
    public float nanobotChargeRate = .5f;
    public float nanobotGenRate = 1;
    public GameObject nanobot;
    
    List<GameObject> nanobotsCollected;
    List<GameObject> chargedNanobots;
    Rigidbody2D rb2D;
    bool isCharging = false;
    float timeTillNextCharge;
    float timeTillNextBotGen;

    // Use this for initialization
    void Start () {
        chargedNanobots = new List<GameObject>();
        nanobotsCollected = new List<GameObject>();
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        timeTillNextCharge = Time.time + nanobotChargeRate;
        timeTillNextBotGen = Time.time + timeTillNextBotGen;
    }
	
	// Update is called once per frame
	void Update () {
        MovePlayer();
        CheckIfShooting();

        if(nanobotsCollected.Count < 3 && chargedNanobots.Count < 3 && Time.time >= timeTillNextBotGen)
        { 
            GenerateBot();
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
        if (Input.GetAxis("Fire1") != 0)
        {
            try
            {
                isCharging = true;
                if (nanobotsCollected.Count > 0 && Time.time >= timeTillNextCharge)
                {
                    chargedNanobots.Add(nanobotsCollected[nanobotsCollected.Count -1]);
                    nanobotsCollected.RemoveAt(nanobotsCollected.Count - 1);
                    chargedNanobots[chargedNanobots.Count - 1].GetComponent<NanoBotController>().Charge(chargedNanobots.Count);
                    timeTillNextCharge = Time.time + nanobotChargeRate;
                }
            }
            catch
            {
            }
        }
        if (Input.GetAxis("Fire1") == 0 && isCharging == true)
        {
            foreach(GameObject nanobot in chargedNanobots)
            {
                nanobot.GetComponent<NanoBotController>().Fire();
            }
            chargedNanobots = new List<GameObject>();
            isCharging = false;
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "NanoBot")
        {
            other.transform.parent = transform;
            other.gameObject.GetComponent<NanoBotController>().PickUp();
            nanobotsCollected.Add(other.gameObject);
        }
    }

}
