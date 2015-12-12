using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public float movementSpeed = 1;
    public float jumpHight = 1;
    public float growthRate = 1;

    Rigidbody2D rb2D;
    float totalMass = 0;

    bool isJumping = false;
    // Use this for initialization
    void Start () {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        MovePlayer();
    }

    void MovePlayer()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");

        if (horizontalAxis != 0)
        {
            transform.Translate(new Vector3(horizontalAxis * movementSpeed, 0));
        }
        if (verticalAxis > 0 && !isJumping)
        {
            rb2D.AddForce(new Vector2(0, jumpHight), ForceMode2D.Impulse);
            isJumping = true;
        }
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Floor")
        {
            isJumping = false;
        }

        if (other.gameObject.name == "Mass")
        {
            Destroy(other.gameObject);
            totalMass++;
            transform.localScale += new Vector3(growthRate, growthRate);
        }
    }

}
