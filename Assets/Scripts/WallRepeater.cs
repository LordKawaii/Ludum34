using UnityEngine;
using System.Collections;

public class WallRepeater : MonoBehaviour {

    public  GameObject wallOne;
    public GameObject wallTwo;
    public GameObject floor;
    public float wallSpeed = .20f;



    public Vector3 initialPositionOne;
    public Vector3 initialPositionTwo;
    public double wallOneMaxLimit = 0;
    public double wallTwoMaxLimit = 0;


    // Use this for initialization
    void Start () {
    initialPositionOne = wallOne.transform.position;
    initialPositionTwo = wallTwo.transform.position;
        wallOneMaxLimit -= wallOne.transform.lossyScale.y ;
        wallTwoMaxLimit -= wallTwo.transform.lossyScale.y;
       
    }
	
	// Update is called once per frame
	void Update () {
        wallOne.transform.position -= (new Vector3(0, wallSpeed));
        wallTwo.transform.position -= (new Vector3(0, wallSpeed));
        if (wallOne.transform.position.y <= (floor.transform.lossyScale.y * -1) -5) wallOne.transform.position = initialPositionOne;
        if (wallTwo.transform.position.y <= (floor.transform.lossyScale.y * -1) - 5) wallTwo.transform.position = initialPositionOne;
    }
}
