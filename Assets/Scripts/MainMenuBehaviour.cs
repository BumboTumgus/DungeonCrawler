using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBehaviour : MonoBehaviour
{
    [SerializeField] GameObject GameManagerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Used to launch the game into the prep scene before we load scene 1
    public void LaunchGame()
    {
        Instantiate(GameManagerPrefab);
    }
}
