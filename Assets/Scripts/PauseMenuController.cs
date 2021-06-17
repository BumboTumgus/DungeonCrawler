﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenu;
    public PlayerMovementController pmc;
    [SerializeField] PauseMenuBehaviour pauseMenuBehaviour;
    [HideInInspector] public bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(paused)
            {
                paused = false;
                pauseMenu.SetActive(false);
                if(!pmc.menuOpen)
                    Camera.main.transform.parent.GetComponent<CameraControls>().menuOpen = false;
                Time.timeScale = 1;
            }
            else
            {
                paused = true;
                pauseMenu.SetActive(true);
                pauseMenuBehaviour.ResetMenu();
                Camera.main.transform.parent.GetComponent<CameraControls>().menuOpen = true;
                Time.timeScale = 0;
            }
        }
    }

    // Used by the restart button to relaunch the scene.
    public void RestartScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    // Used to quit the game and exit the program.
    public void EndGame()
    {
        Application.Quit();
    }

}
