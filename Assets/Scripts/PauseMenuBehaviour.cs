using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class PauseMenuBehaviour : MonoBehaviour
{
    [SerializeField] Animator fadeOutAnim;

    [SerializeField] GameObject panelMainMenu;
    [SerializeField] GameObject panelOptionsMenu;
    [SerializeField] GameObject panelOptionsComfirm;
    [SerializeField] GameObject panelComfirmQuit;
    [SerializeField] GameObject panelOptionsMenuDarkenedOverlay;
    [SerializeField] GameObject panelComfirmBackToMenu;
    [SerializeField] GameObject panelMainMenuDarkenedOverlay;

    [SerializeField] AudioMixer mainMixer;
    [SerializeField] Dropdown resolutionDropdown;
    [SerializeField] Dropdown graphicsDropdown;
    [SerializeField] PostProcessProfile ppp;
    [SerializeField] Bloom bloom;

    [SerializeField] Slider volumeMaster;
    [SerializeField] Slider volumeEffects;
    [SerializeField] Slider volumeMusic;
    [SerializeField] Slider sliderScreenshake;
    [SerializeField] Slider sliderBloom;
    [SerializeField] Toggle fullscreenToggle;


    Resolution[] resolutions;

    // Start is called before the first frame update
    void Start()
    {

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currenResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                currenResolutionIndex = i;
        }
        resolutionDropdown.AddOptions(options);

        resolutionDropdown.value = currenResolutionIndex;
        graphicsDropdown.value = QualitySettings.GetQualityLevel();

        bloom = ppp.GetSetting<Bloom>();



        //fadeOutAnim.gameObject.SetActive(true);

        ButtonPressResetOptionsMenuFromPreferences();
    }

    // Used to launch the game into the prep scene before we load scene 1
    public void ButtonPressLaunchMainMenuScene()
    {
        StartCoroutine(BackToMenu());
    }

    IEnumerator BackToMenu()
    {
        fadeOutAnim.gameObject.SetActive(true);
        fadeOutAnim.SetTrigger("FadeOut");
        fadeOutAnim.updateMode = AnimatorUpdateMode.UnscaledTime;

        yield return new WaitForSecondsRealtime(1f);

        GameManager.instance.PreMenuSceneCleanup();
        SceneManager.LoadScene("MainMenu"); 
        Time.timeScale = 1;
    }

    // Used when we press the options button on the main menu, launch the optiosn panel
    public void ButtonPressOptionsLaunch()
    {
        panelOptionsMenu.SetActive(true);
        panelOptionsMenu.GetComponent<CanvasGroup>().interactable = true;

        panelMainMenu.SetActive(false);
        panelMainMenu.GetComponent<CanvasGroup>().interactable = false;
    }

    // Used when we press the back button to go back to the main menu
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
        panelComfirmBackToMenu.SetActive(false);
        panelComfirmBackToMenu.GetComponent<CanvasGroup>().interactable = false;
        panelMainMenuDarkenedOverlay.SetActive(false);
    }

    // USed when we press the back button on the options menu, if we have some unsaved options then we make the popup appear asking if they want to save them.
    public void ButtonPressBackFromOptionsMenu()
    {
        bool optionsSaved = true;

        // Compare our values to our options, if theyre different the options were not saved.
        if (volumeMaster.value != PlayerPrefs.GetFloat("VolumeMaster"))
            optionsSaved = false;
        if (volumeEffects.value != PlayerPrefs.GetFloat("VolumeEffects"))
            optionsSaved = false;
        if (volumeMusic.value != PlayerPrefs.GetFloat("VolumeMusic"))
            optionsSaved = false;
        if (sliderBloom.value != PlayerPrefs.GetFloat("Bloom"))
            optionsSaved = false;
        if (sliderScreenshake.value != PlayerPrefs.GetFloat("Screenshake"))
            optionsSaved = false;
        if (graphicsDropdown.value != PlayerPrefs.GetInt("QualityLevel"))
            optionsSaved = false;
        if (fullscreenToggle.isOn && PlayerPrefs.GetInt("Fullscreen") == 0 || !fullscreenToggle.isOn && PlayerPrefs.GetInt("Fullscreen") == 1)
            optionsSaved = false;
        if (graphicsDropdown.value != PlayerPrefs.GetInt("QualityLevel"))
            optionsSaved = false;
        if (resolutionDropdown.value != PlayerPrefs.GetInt("ResolutionsIndex"))
            optionsSaved = false;

        if (optionsSaved)
        {
            // disable all the other panels and open the menu here.
            panelMainMenu.SetActive(true);
            panelMainMenu.GetComponent<CanvasGroup>().interactable = true;

            panelOptionsMenu.SetActive(false);
            panelOptionsMenu.GetComponent<CanvasGroup>().interactable = false;
            panelOptionsComfirm.SetActive(false);
            panelOptionsComfirm.GetComponent<CanvasGroup>().interactable = false;
            panelComfirmQuit.SetActive(false);
            panelComfirmQuit.GetComponent<CanvasGroup>().interactable = false;
            panelComfirmBackToMenu.SetActive(false);
            panelComfirmBackToMenu.GetComponent<CanvasGroup>().interactable = false;
        }
        else
        {
            // disable all the other panels and open the menu here.
            panelOptionsComfirm.SetActive(true);
            panelOptionsComfirm.GetComponent<CanvasGroup>().interactable = true;

            panelOptionsMenu.GetComponent<CanvasGroup>().interactable = false;

            panelOptionsMenuDarkenedOverlay.SetActive(true);
        }
    }

    // Used when we presss the wuit button. Launches the popup asking for comfirmation.
    public void ButtonPressQuitGame()
    {
        panelMainMenu.GetComponent<CanvasGroup>().interactable = false;

        panelComfirmQuit.SetActive(true);
        panelComfirmQuit.GetComponent<CanvasGroup>().interactable = true;
        panelMainMenuDarkenedOverlay.SetActive(true);
    }

    // Used when we presss the menu button. ASks for comfirmation to go back to the menu.
    public void ButtonPressBackToMainMenuScene()
    {
        panelMainMenu.GetComponent<CanvasGroup>().interactable = false;

        panelComfirmBackToMenu.SetActive(true);
        panelComfirmBackToMenu.GetComponent<CanvasGroup>().interactable = true;
        panelMainMenuDarkenedOverlay.SetActive(true);
    }

    // Used when we comfirm the quit game button press in the popup. Closes the appllication.
    public void ButtonPressCloseApplication()
    {
        Application.Quit();
    }

    // Used when we press the save option in the options menu. We compare the values we have to the values on our player prefs. If theyre different we set them and write the new file.\
    public void ButtonPressSaveOptions()
    {
        bool writeFile = false;

        if (volumeMaster.value != PlayerPrefs.GetFloat("VolumeMaster"))
        {
            writeFile = true;
            PlayerPrefs.SetFloat("VolumeMaster", volumeMaster.value);
        }
        if (volumeEffects.value != PlayerPrefs.GetFloat("VolumeEffects"))
        {
            writeFile = true;
            PlayerPrefs.SetFloat("VolumeEffects", volumeEffects.value);
        }
        if (volumeMusic.value != PlayerPrefs.GetFloat("VolumeMusic"))
        {
            writeFile = true;
            PlayerPrefs.SetFloat("VolumeMusic", volumeMusic.value);
        }
        if (sliderBloom.value != PlayerPrefs.GetFloat("Bloom"))
        {
            writeFile = true;
            PlayerPrefs.SetFloat("Bloom", sliderBloom.value);
        }
        if (sliderScreenshake.value != PlayerPrefs.GetFloat("Screenshake"))
        {
            writeFile = true;
            PlayerPrefs.SetFloat("Screenshake", sliderScreenshake.value);
        }
        if (graphicsDropdown.value != PlayerPrefs.GetInt("QualityLevel"))
        {
            writeFile = true;
            PlayerPrefs.SetInt("QualityLevel", graphicsDropdown.value);
        }
        if (fullscreenToggle.isOn && PlayerPrefs.GetInt("Fullscreen") == 0 || !fullscreenToggle.isOn && PlayerPrefs.GetInt("Fullscreen") == 1)
        {
            writeFile = true;
            if (fullscreenToggle.isOn)
                PlayerPrefs.SetInt("Fullscreen", 1);
            else
                PlayerPrefs.SetInt("Fullscreen", 0);
        }
        if (resolutionDropdown.value != PlayerPrefs.GetInt("ResolutionsIndex"))
        {
            writeFile = true;
            PlayerPrefs.SetInt("ResolutionsIndex", resolutionDropdown.value);
        }


        if (writeFile)
            PlayerPrefs.Save();

        // disable all the other panels and open the menu here.
        panelMainMenu.SetActive(true);
        panelMainMenu.GetComponent<CanvasGroup>().interactable = true;

        panelOptionsMenu.SetActive(false);
        panelOptionsMenu.GetComponent<CanvasGroup>().interactable = false;
        panelOptionsComfirm.SetActive(false);
        panelOptionsComfirm.GetComponent<CanvasGroup>().interactable = false;
        panelComfirmQuit.SetActive(false);
        panelComfirmQuit.GetComponent<CanvasGroup>().interactable = false;
        panelOptionsMenuDarkenedOverlay.SetActive(false);
        panelComfirmBackToMenu.SetActive(false);
        panelComfirmBackToMenu.GetComponent<CanvasGroup>().interactable = false;

    }

    // Used to reset the options to the currently in use ones
    public void ButtonPressResetOptionsMenuFromPreferences()
    {
        if (!PlayerPrefs.HasKey("VolumeMaster"))
            PlayerPrefs.SetFloat("VolumeMaster", 0);
        if (!PlayerPrefs.HasKey("VolumeMusic"))
            PlayerPrefs.SetFloat("VolumeMusic", 0);
        if (!PlayerPrefs.HasKey("VolumeEffects"))
            PlayerPrefs.SetFloat("VolumeEffects", 0);
        if (!PlayerPrefs.HasKey("Screenshake"))
            PlayerPrefs.SetFloat("Screenshake", 100);
        if (!PlayerPrefs.HasKey("Bloom"))
            PlayerPrefs.SetFloat("Bloom", 10);
        if (!PlayerPrefs.HasKey("QualityLevel"))
            PlayerPrefs.SetInt("QualityLevel", 3);
        if (!PlayerPrefs.HasKey("Fullscreen"))
            PlayerPrefs.SetInt("Fullscreen", 1);
        if (!PlayerPrefs.HasKey("ResolutionsCount"))
            PlayerPrefs.SetInt("ResolutionsCount", resolutions.Length);
        if (!PlayerPrefs.HasKey("ResolutionsIndex"))
            PlayerPrefs.SetInt("ResolutionsIndex", resolutionDropdown.value);

        // CHeck to see if the reolustions count differs, if they do we might have a new monitor and should just stick with the default.
        if (resolutions.Length != PlayerPrefs.GetInt("ResolutionsCount"))
        {
            // keep the default and set our prefs to it.
            PlayerPrefs.SetInt("ResolutionsCount", resolutions.Length);
            PlayerPrefs.SetInt("ResolutionsIndex", resolutionDropdown.value);
            PlayerPrefs.Save();
        }
        else
        {
            resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionsIndex");
        }

        volumeMaster.value = PlayerPrefs.GetFloat("VolumeMaster");
        volumeEffects.value = PlayerPrefs.GetFloat("VolumeEffects");
        volumeMusic.value = PlayerPrefs.GetFloat("VolumeMusic");
        sliderScreenshake.value = PlayerPrefs.GetFloat("Screenshake");
        sliderBloom.value = PlayerPrefs.GetFloat("Bloom");
        graphicsDropdown.value = PlayerPrefs.GetInt("QualityLevel");
        if (PlayerPrefs.GetInt("Fullscreen") == 1)
            fullscreenToggle.isOn = true;
        else
            fullscreenToggle.isOn = false;
        resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionsIndex");

        //mainMixer.SetFloat("volumeMaster", volumeMaster.value);
        //mainMixer.SetFloat("volumeEffects", volumeEffects.value);
        //mainMixer.SetFloat("volumeMusic", volumeMusic.value);
        //bloom.intensity.value = sliderBloom.value;
        //QualitySettings.SetQualityLevel(graphicsDropdown.value);
        // disable all the other panels and open the menu here.

        panelMainMenu.SetActive(true);
        panelMainMenu.GetComponent<CanvasGroup>().interactable = true;

        panelOptionsMenu.SetActive(false);
        panelOptionsMenu.GetComponent<CanvasGroup>().interactable = false;
        panelOptionsComfirm.SetActive(false);
        panelOptionsComfirm.GetComponent<CanvasGroup>().interactable = false;
        panelComfirmQuit.SetActive(false);
        panelComfirmQuit.GetComponent<CanvasGroup>().interactable = false;
        panelOptionsMenuDarkenedOverlay.SetActive(false);
        panelMainMenuDarkenedOverlay.SetActive(false);
        panelComfirmBackToMenu.SetActive(false);
        panelComfirmBackToMenu.GetComponent<CanvasGroup>().interactable = false;

    }



    // Used when we want to set the volume to the value in the volume slider.
    public void SetMasterVolume(float volume)
    {
        mainMixer.SetFloat("volumeMaster", volume);
    }
    // Used when we want to set the volume to the value in the volume slider.
    public void SetEffectsVolume(float volume)
    {
        mainMixer.SetFloat("volumeEffects", volume);
    }
    // Used when we want to set the volume to the value in the volume slider.
    public void SetMusicVolume(float volume)
    {
        mainMixer.SetFloat("volumeMusic", volume);
    }

    // Used to set the quality of the game.
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    // Used to set the game to fullscreen or windowed.
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    // USed to update the resolution of the game
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    // USed to set the bloom amount of the game
    public void SetBloom(float bloomValue)
    {
        bloom.intensity.value = bloomValue;
    }

    // used to reset the menu in the event of an unpause then repauser
    public void ResetMenu()
    {
        // disable all the other panels and open the menu here.
        panelMainMenu.SetActive(true);
        panelMainMenu.GetComponent<CanvasGroup>().interactable = true;

        panelOptionsMenu.SetActive(false);
        panelOptionsMenu.GetComponent<CanvasGroup>().interactable = false;
        panelOptionsComfirm.SetActive(false);
        panelOptionsComfirm.GetComponent<CanvasGroup>().interactable = false;
        panelComfirmQuit.SetActive(false);
        panelComfirmQuit.GetComponent<CanvasGroup>().interactable = false;
        panelComfirmBackToMenu.SetActive(false);
        panelComfirmBackToMenu.GetComponent<CanvasGroup>().interactable = false;
        panelOptionsMenuDarkenedOverlay.SetActive(false);
        panelMainMenuDarkenedOverlay.SetActive(false);
    }

}
