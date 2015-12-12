using UnityEngine;
using System.Collections;

public class NanoBotController : MonoBehaviour {
    public float swarmSpeed;
    public float swarmRadius;
    public float rotationSpeed;
    public float frequencyOfChange = 1;

    protected float timeTillChange;
    protected bool hasBeenFired = false;
    protected bool hasBeenPickedUp = false;
    protected bool isCharging = false;
    protected Rigidbody2D rb2d;

    // Use this for initialization
    protected virtual void Start () {
        timeTillChange = Time.time + Random.Range(0, frequencyOfChange);
        rb2d = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        Swarm();

    }

    protected virtual void Swarm()
    {
        if (!hasBeenFired && hasBeenPickedUp && !isCharging)
        {

            Vector3 targetDirection = (transform.parent.position - transform.position);
            float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            Quaternion rotationStep = Quaternion.AngleAxis(targetAngle, Vector3.forward);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationStep, Time.deltaTime * rotationSpeed);

            try
            {
                if (timeTillChange <= Time.time)
                {
                    rb2d.AddForce(new Vector2(targetDirection.x + Random.Range(-swarmRadius, swarmRadius), targetDirection.y + Random.Range(-swarmRadius, swarmRadius)) * swarmSpeed * Time.deltaTime);
                    //transform.position = Vector3.Lerp(transform.position,transform.parent.position + new Vector3((Random.Range(-swarmRadius, swarmRadius)), (Random.Range(-swarmRadius, swarmRadius))), Time.deltaTime * swarmSpeed);
                    //transform.Translate(new Vector3(transform.parent.position.x + Random.Range(-swarmRadius, swarmRadius), transform.parent.position.y + Random.Range(-swarmRadius, swarmRadius)) * Time.deltaTime);
                    timeTillChange = Time.time + Random.Range(0, frequencyOfChange);
                }
            }
            catch
            {
            }
        }
    } 

    public virtual void PickUp()
    {
        hasBeenPickedUp = true;
        timeTillChange = Time.time + Random.Range(0, frequencyOfChange);
    }

    public virtual void Charge()
    {
        isCharging = true;
    }
}


public static class UnityExtensions
{
    public static Vector3 ScreenToWorldLength(this Camera camera, Vector3 position)
    {
        return camera.ScreenToWorldPoint(position) - camera.ScreenToWorldPoint(Vector3.zero);
    }
}