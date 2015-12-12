using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public float movementSpeed = 1;

    Rigidbody2D rb2D;
    float totalNanoBots = 0;

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

        if (horizontalAxis != 0 && verticalAxis == 0)
        {
            transform.Translate(new Vector3(horizontalAxis * movementSpeed, 0));
        }
        if (verticalAxis != 0 && horizontalAxis ==0)
        {
            transform.Translate(new Vector3(0, verticalAxis * movementSpeed));
        }
        if (verticalAxis != 0 && horizontalAxis != 0)
        {
            transform.Translate(new Vector3(horizontalAxis * movementSpeed, verticalAxis * movementSpeed));
        }
    }

}
