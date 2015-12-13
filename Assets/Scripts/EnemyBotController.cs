using UnityEngine;
using System.Collections;

public class EnemyBotController : NanoBotController {
    public int hp = 3;

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
        Swarm();
        CheckForDeath();
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
