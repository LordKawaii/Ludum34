using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{

    public List<Image> heroLives;
    public Text attachedBots;
    public Text youWinTxt;
    public Text youLooseTxt;

    PlayerController playerController;
    GameController gameCon;

    // Use this for initialization
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        gameCon = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Update the number of lives images so that they match the number of player lives
        if (playerController.lives < 3)
           heroLives[playerController.lives].enabled = false;

        attachedBots.text = "x " + playerController.GetTotalBotCount();

        if (gameCon.CheckBossHasDied())
        {
            youWinTxt.enabled = true;
        }

        if (gameCon.CheckPlayerHasDied())
        {
            youLooseTxt.enabled = true;
        }

    }
}

