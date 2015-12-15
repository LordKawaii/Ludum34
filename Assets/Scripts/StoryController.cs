using UnityEngine;
using System.Collections;

public class StoryController : MonoBehaviour {
    public float storyTime = 3;

	// Use this for initialization
	void Start () {
        storyTime += Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time >= storyTime)
        {
            Application.LoadLevel(2);
        }
	}
}
