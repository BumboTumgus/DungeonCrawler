using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenuBehaviour : MonoBehaviour
{
    [SerializeField] Animator fadeOutAnim;
    [SerializeField] GameObject gameOverMenu;

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
        fadeOutAnim.SetTrigger("GameOver");
    }



}
