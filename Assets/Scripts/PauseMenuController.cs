using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenu;
    public PlayerMovementController pmc;
    [SerializeField] PauseMenuBehaviour pauseMenuBehaviour;
    [HideInInspector] public bool paused = false;
    [SerializeField] AudioSource menuOpenAudio;
    [SerializeField] AudioSource menuCloseAudio;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !pmc.GetComponent<PlayerStats>().dead)
        {
            if (paused)
            {
                HideMenu();
            }
            else if (pmc.elementalWindowOpen)
            {
                pmc.HideElementalFountainWindow();
            }
            else if (pmc.inventoryMenuOpen)
            {
                pmc.HideInventoryWindow();
            }
            else
            {
                paused = true;
                pauseMenu.SetActive(true);
                pmc.freezePlayerMovementForMenu = true;
                pmc.pauseMenuOpen = true;
                pauseMenuBehaviour.ResetMenu();
                Camera.main.transform.parent.GetComponent<CameraControls>().menuOpen = true;
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                menuOpenAudio.Play();
            }
        }
    }

    public void HideMenu()
    {
        Debug.Log("Hiding the pause window.");
        paused = false;
        pauseMenu.SetActive(false);
        pmc.pauseMenuOpen = false;
        menuCloseAudio.Play();
        pmc.CheckWhichPanelsAreOpen();
    }

    public void HideElementalTranspositionPanel()
    {
        pmc.HideElementalFountainWindow();
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
