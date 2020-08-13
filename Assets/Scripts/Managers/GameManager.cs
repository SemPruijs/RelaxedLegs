﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public State state;
    public GameObject player;
    public GameObject startEmptyChunks;
    public int coinsCollected;
    public ScoreBoardManager scoreBoardManager;
    
    private static GameManager _instance;
         public static GameManager Instance {
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

    public enum State
    {
        InGame,
        Menu,
        PlayAgain,
        Credit,
        Settings,
        Pause,
        SpecialThanks,
        Tutorial
    };

    void Start()
    {
        Menu();
        Application.targetFrameRate = 300;
        ShowTutorial();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadCurrentScene();
        }
    }

    public void Menu()
    {
        state = State.Menu;
        AudioManager.Instance.MenuMusic();
        DisplayManager.Instance.UpdateMenu();
        Time.timeScale = 1f;
    }

    public void InGame()
    {
        state = State.InGame;
        // AudioManager.Instance.Stop();
        player.GetComponent<Rigidbody2D>().gravityScale = 6;
        AudioManager.Instance.InGameMusic();
        // Advertisement.Initialize(_appStoreId, useAds);
        DisplayManager.Instance.UpdateMenu();
        Time.timeScale = 1f;
    }

    public void Credit()
    {
        state = State.Credit;
        DisplayManager.Instance.UpdateMenu();
    }

    public void Settings()
    {
        state = State.Settings;
        DisplayManager.Instance.UpdateMenu();
    }

    public void PlayAgain()
    {
        state = State.PlayAgain;
        DisplayManager.Instance.UpdateMenu();
        scoreBoardManager.PostScoreOnLeaderBoard(coinsCollected);
    }

    public void Pause()
    {
        state = State.Pause;
        player.GetComponent<Rigidbody2D>().gravityScale = 0;
        DisplayManager.Instance.UpdateMenu();
        Time.timeScale = 0f;
    }

    public void SpecialThanks()
    {
        state = State.SpecialThanks;
        DisplayManager.Instance.UpdateMenu();
    }

    public void Tutorial()
    {
        state = State.Tutorial;
        DisplayManager.Instance.UpdateMenu();
    }
    
    public void ReloadCurrentScene() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RemoveAllGameObjectsWithTag(string tag)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag (tag);

        foreach (var g in gameObjects)
        {
            Destroy(g);
        }

    }

    public void Reset()
    {
        RemoveAllGameObjectsWithTag("Chunk");
        RemoveAllGameObjectsWithTag("Spike");
        player.transform.position = new Vector3(0f, -1.5f, 0f);
        player.SetActive(true);
        Instantiate(startEmptyChunks, new Vector3(0, 0, 0), Quaternion.identity);
        coinsCollected = 0;
        // AudioManager.Instance.InGameMusic();
        InGame();
    }

    public void ShowTutorial()
    {
        if (PlayerPrefs.GetInt("FirstTime", 0) == 0)
        {
            Tutorial();
            PlayerPrefs.SetInt("FirstTime", 1);
        }
    }
}
