using UnityEngine;
using System.Collections;

public class EnemyBossController : MonoBehaviour {
    bool movingRight = true;

    public float movementSpeed;
    public float travelWidth;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (movingRight && transform.position.x <= travelWidth)
            transform.Translate(Vector2.right * Time.deltaTime * movementSpeed);
        if (transform.position.x >= travelWidth)
            movingRight = false;
        if (!movingRight && transform.position.x >= -travelWidth)
            transform.Translate(Vector2.left * Time.deltaTime * movementSpeed);
        if (transform.position.x <= -travelWidth)
            movingRight = true;
    }
}
