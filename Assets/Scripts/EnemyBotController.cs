using UnityEngine;
using System.Collections;

public class EnemyBotController : NanoBotController {

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
	
}
