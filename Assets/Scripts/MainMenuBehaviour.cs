using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBehaviour : MonoBehaviour
{
    [SerializeField] GameObject gameManagerPrefab;
    [SerializeField] Animator fadeOutAnim;

    [SerializeField] GameObject panelMainMenu;
    [SerializeField] GameObject panelOptionsMenu;
    [SerializeField] GameObject panelOptionsComfirm;
    [SerializeField] GameObject panelComfirmQuit;

    // Start is called before the first frame update
    void Start()
    {
        fadeOutAnim.gameObject.SetActive(true);
        fadeOutAnim.SetTrigger("FadeIn");

        panelMainMenu.SetActive(true);
        panelMainMenu.GetComponent<CanvasGroup>().interactable = true;

        panelOptionsMenu.SetActive(false);
        panelOptionsMenu.GetComponent<CanvasGroup>().interactable = false;
        panelOptionsComfirm.SetActive(false);
        panelOptionsComfirm.GetComponent<CanvasGroup>().interactable = false;
        panelComfirmQuit.SetActive(false);
        panelComfirmQuit.GetComponent<CanvasGroup>().interactable = false;
    }

    // Used to launch the game into the prep scene before we load scene 1
    public void ButtonPressLaunchGame()
    {
        Instantiate(gameManagerPrefab);
        fadeOutAnim.SetTrigger("FadeOut");
    }

    // USed when we press the options button on the main menu, launch the optiosn panel
    public void ButtonPressOptionsLaunch()
    {
        panelOptionsMenu.SetActive(true);
        panelOptionsMenu.GetComponent<CanvasGroup>().interactable = true;

        panelMainMenu.SetActive(false);
        panelMainMenu.GetComponent<CanvasGroup>().interactable = false;
    }

    // USed when we press the back button to go back to the main menu
    public void ButtonPressBackToMainMenu()
    {
        panelMainMenu.SetActive(true);
        panelMainMenu.GetComponent<CanvasGroup>().interactable = true;

        panelOptionsMenu.SetActive(false);
        panelOptionsMenu.GetComponent<CanvasGroup>().interactable = false;
        panelOptionsComfirm.SetActive(false);
        panelOptionsComfirm.GetComponent<CanvasGroup>().interactable = false;
        panelComfirmQuit.SetActive(false);
        panelComfirmQuit.GetComponent<CanvasGroup>().interactable = false;
    }

    // Used when we presss the wuit button. Launches the popup asking for comfirmation.
    public void ButtonPressQuitGame()
    {
        panelMainMenu.GetComponent<CanvasGroup>().interactable = false;

        panelComfirmQuit.SetActive(true);
        panelComfirmQuit.GetComponent<CanvasGroup>().interactable = true;
    }

    // Used when we comfirm the quit game button press in the popup. Closes the appllication.
    public void ButtonPressCloseApplication()
    {
        Application.Quit();
    }

}
