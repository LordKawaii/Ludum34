using UnityEngine;
using System.Collections;

public class WallRepeater : MonoBehaviour {

    public  GameObject wall; 
    public GameObject floor;
    public float wallSpeed = .20f; 
       
    Renderer rendF;
    Renderer rendW;
     

    // Use this for initialization
    void Start () {
        rendF = GetComponent<Renderer>();
        rendW = wall.GetComponent<Renderer>(); 
    }
	
	// Update is called once per frame
	void Update ()
    { 
        Vector2 offsetF = rendF.material.GetTextureOffset("_MainTex");
        offsetF -= new Vector2(0, wallSpeed * .05f );
        rendF.material.SetTextureOffset("_MainTex", new Vector2(0, offsetF.y));

        Vector2 offsetW = rendW.material.GetTextureOffset("_MainTex");
        offsetW -= new Vector2(0,  wallSpeed *.05f);
        rendW.material.SetTextureOffset("_MainTex", new Vector2(0, offsetW.y));

    }
}
