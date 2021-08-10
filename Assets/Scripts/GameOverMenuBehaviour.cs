using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenuBehaviour : MonoBehaviour
{
    [SerializeField] Animator fadeOutAnimGameOver;
    [SerializeField] Animator fadeOutAnimSwitchScenes;
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] string[] gameOverMessages;
    [SerializeField] Text gameOverMessageText;


    // Used to launch the game into the prep scene before we load scene 1
    public void ButtonPressLaunchMainMenuScene()
    {
        StartCoroutine(BackToMenu());
    }

    IEnumerator BackToMenu()
    {
        fadeOutAnimSwitchScenes.gameObject.SetActive(true);
        fadeOutAnimSwitchScenes.SetTrigger("FadeOut");
        fadeOutAnimSwitchScenes.updateMode = AnimatorUpdateMode.UnscaledTime;

        yield return new WaitForSecondsRealtime(1f);

        GameManager.instance.PreMenuSceneCleanup();
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameOverMenu.SetActive(false);
        gameOverMenu.GetComponent<CanvasGroup>().interactable = false;
        gameOverMenu.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }


    // Used to enable the gameover menu and start the death screen animation.
    public void GameOverScreenFadeIn()
    {
        gameOverMenu.SetActive(true);
        gameOverMenu.GetComponent<CanvasGroup>().interactable = true;
        gameOverMenu.GetComponent<CanvasGroup>().blocksRaycasts = true;
        gameOverMessageText.text = gameOverMessages[Random.Range(0, gameOverMessages.Length)];
        fadeOutAnimGameOver.SetTrigger("GameOver");
    }



}
