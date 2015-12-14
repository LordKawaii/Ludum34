using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{

    public List<Image> heroLives;

    PlayerController playerController;

    // Use this for initialization
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Update the number of lives images so that they match the number of player lives
        if (playerController.lives < 3)
           heroLives[playerController.lives].enabled = false;

    }
}

