﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayManager : MonoBehaviour
{
    public GameObject menu;
    public GameObject inGame;
    public GameObject playAgain;
    public GameObject credit;
    public GameObject settings;
    public GameObject pause;

    public Text coinsPickedUpText;
    public Text coinsPickedUpTextPlayAgain;
    
    private static DisplayManager _instance;
    public static DisplayManager Instance {
        get {
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        } 
    }
    
    void Update()
    {
        if (GameManager.Instance.state == GameManager.State.InGame)
        {
            coinsPickedUpText.text = GameManager.Instance.coinsCollected.ToString("F0");
        } else if (GameManager.Instance.state == GameManager.State.PlayAgain)
        {
            coinsPickedUpTextPlayAgain.text = GameManager.Instance.coinsCollected.ToString("F0");
        }
    }

    public void UpdateMenu()
    {
        menu.SetActive(GameManager.Instance.state == GameManager.State.Menu);
        inGame.SetActive(GameManager.Instance.state == GameManager.State.InGame);
        playAgain.SetActive(GameManager.Instance.state == GameManager.State.PlayAgain);
        credit.SetActive(GameManager.Instance.state == GameManager.State.Credit);
        settings.SetActive(GameManager.Instance.state == GameManager.State.Settings);
        pause.SetActive(GameManager.Instance.state == GameManager.State.Pause);
    }
}
