using UnityEngine;
using System.Collections;

public class NanoBotController : MonoBehaviour {
    public float swarmSpeed;
    public float swarmRadius;

    bool hasBeenFired = false;
    bool hasBeenPickedUp = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!hasBeenFired && hasBeenPickedUp)
        {
            try
            {
                //transform.position = Vector3.Lerp(transform.position,transform.parent.position + new Vector3((Random.Range(-swarmRadius, swarmRadius)), (Random.Range(-swarmRadius, swarmRadius))), Time.deltaTime * swarmSpeed);
                transform.Translate(new Vector3(transform.parent.position.x + Random.Range(-swarmRadius, swarmRadius), transform.parent.position.y + Random.Range(-swarmRadius, swarmRadius)) * Time.deltaTime);
            }
            catch
            {
            }
        }
	}
}
