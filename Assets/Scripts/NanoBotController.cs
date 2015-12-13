using UnityEngine;
using System.Collections;

public class NanoBotController : MonoBehaviour {
    public float swarmSpeed;
    public float swarmRadius;
    public float rotationSpeed;
    public float frequencyOfChange = 1;
    public float chargeDistance = .06f;
    public float fireSpeed = 10;
    
    protected float timeTillChange;
    protected bool hasBeenFired = false;
    protected bool hasBeenPickedUp = false;
    protected bool isCharging = false;
    protected bool isAttacking = false;
    protected Rigidbody2D rb2d;
    protected GameObject swarmTarget;

    // Use this for initialization
    protected virtual void Start () {
        timeTillChange = Time.time + Random.Range(0, frequencyOfChange);
        rb2d = gameObject.GetComponent<Rigidbody2D>();


    }

    // Update is called once per frame
    protected virtual void Update () {
        Swarm();

        if (transform.position.y > 15 || transform.position.y < -15)
        {
            Destroy(gameObject);
        }

    }

    protected virtual void Swarm()
    {
        if (!hasBeenFired && hasBeenPickedUp && !isCharging && !isAttacking)
        {
            try
            {
                if (transform.parent != null)
                    swarmTarget = transform.parent.gameObject;

                Vector3 targetDirection = (swarmTarget.transform.position - transform.position);
                float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
                Quaternion rotationStep = Quaternion.AngleAxis(targetAngle, Vector3.forward);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationStep, Time.deltaTime * rotationSpeed);


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

    public virtual void Attack(GameObject target)
    {
        if (!isAttacking && !hasBeenPickedUp)
        {
            rb2d.velocity = new Vector2(0, 0);
            rb2d.isKinematic = true;
            transform.parent = target.transform;
            isAttacking = true;
        }
    }

    public virtual void EndAttack()
    {
        if (isAttacking)
        {
            rb2d.velocity = new Vector2(0, -1);
            rb2d.isKinematic = false;
            transform.parent = null;
            isAttacking = false;
        }
    }

    public virtual void PickUp()
    {
        if (!isAttacking)
        { 
            swarmTarget = transform.parent.gameObject;
            hasBeenPickedUp = true;
            hasBeenFired = false;
            timeTillChange = Time.time + Random.Range(0, frequencyOfChange);
        }
    }

    public virtual void Charge(int numberCharged)
    {
        isCharging = true;
        rb2d.velocity = new Vector2(0,0);
        transform.position = new Vector3(transform.parent.position.x, transform.parent.position.y + (chargeDistance * numberCharged));
        transform.rotation = Quaternion.Euler(0, 0, 90);
    }

    public virtual void Fire()
    {
        hasBeenPickedUp = false;
        isCharging = false;
        hasBeenFired = true;
        transform.parent = null;
        rb2d.AddForce(Vector2.up * fireSpeed);
    }

    public bool CheckIfAttacking()
    {
        return isAttacking;
    }

}


public static class UnityExtensions
{
    public static Vector3 ScreenToWorldLength(this Camera camera, Vector3 position)
    {
        return camera.ScreenToWorldPoint(position) - camera.ScreenToWorldPoint(Vector3.zero);
    }
}