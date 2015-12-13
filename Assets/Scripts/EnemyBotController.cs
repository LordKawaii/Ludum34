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
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "NanoBot")
        {
            other.GetComponent<NanoBotController>().Attack(gameObject);
        }
    }
	
}
