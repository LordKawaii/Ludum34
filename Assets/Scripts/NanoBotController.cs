using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NanoBotController : MonoBehaviour {
    public float swarmSpeed;
    public float swarmRadius;
    public float rotationSpeed;
    public float frequencyOfChange = 1;
    public float chargeDistance = .06f;
    public float fireSpeed = 10;
    public float maxVelocity = 10;

    protected float timeTillChange;
    protected bool hasBeenFired = false;
    protected bool hasBeenPickedUp = false;
    protected bool isCharging = false;
    protected bool isAttacking = false;
    protected bool isSwarming = false;
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
                isSwarming = true;
                if (transform.parent != null)
                    swarmTarget = transform.parent.gameObject;

                RotateTowards(new Vector3(swarmTarget.transform.position.x, swarmTarget.transform.position.y));


                if (timeTillChange <= Time.time)
                {
                    float xDist = (float)(transform.position.x - swarmTarget.transform.position.x);
                    float yDist = (float)(transform.position.y - swarmTarget.transform.position.y);
                    float distanceBetween = Mathf.Sqrt((yDist * yDist) - (xDist * xDist));
                    if (distanceBetween > swarmRadius)
                    {
                        rb2d.AddForce(-rb2d.velocity);
                    }
                    Vector3 targetDirection = (swarmTarget.transform.position - transform.position);
                    rb2d.AddForce(new Vector2(targetDirection.x + Random.Range(-swarmRadius, swarmRadius), targetDirection.y + Random.Range(-swarmRadius, swarmRadius)) * swarmSpeed * Time.deltaTime);
                    rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, maxVelocity);                    //transform.position = Vector3.Lerp(transform.position,transform.parent.position + new Vector3((Random.Range(-swarmRadius, swarmRadius)), (Random.Range(-swarmRadius, swarmRadius))), Time.deltaTime * swarmSpeed);
                    //transform.Translate(new Vector3(transform.parent.position.x + Random.Range(-swarmRadius, swarmRadius), transform.parent.position.y + Random.Range(-swarmRadius, swarmRadius)) * Time.deltaTime);
                    timeTillChange = Time.time + Random.Range(0, frequencyOfChange);
                }
            }
            catch
            {
            }
        }
    }

    public virtual void RotateTowards(Vector3 target)
    {
        Vector3 targetDirection = (target - transform.position);
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        Quaternion rotationStep = Quaternion.AngleAxis(targetAngle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationStep, Time.deltaTime * rotationSpeed);
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
            hasBeenFired = false; 
            hasBeenPickedUp = true;
            isCharging = false;
            isAttacking = false;
            isSwarming = true;
            timeTillChange = 0;
        }
    }

    public virtual void Charge(int numberCharged)
    {
        if (!isCharging)
        { 
            isCharging = true;
            rb2d.velocity = new Vector2(0,0);
            transform.position = new Vector3(transform.parent.position.x, transform.parent.position.y + (chargeDistance * numberCharged));
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }

    public virtual void Fire()
    {
        isSwarming = false;
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

    public bool CheckIfSwarming()
    {
        return isSwarming;
    }

}


public static class UnityExtensions
{
    public static Vector3 ScreenToWorldLength(this Camera camera, Vector3 position)
    {
        return camera.ScreenToWorldPoint(position) - camera.ScreenToWorldPoint(Vector3.zero);
    }
}