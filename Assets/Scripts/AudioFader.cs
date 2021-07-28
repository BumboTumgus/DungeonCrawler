using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFader : MonoBehaviour
{
    public bool fadeInOnAwake = false;

    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (fadeInOnAwake)
            FadeIn(2f);
    }

    public void FadeOut(float fadeTime)
    {
        StartCoroutine(FadeOutRoutine(fadeTime));
    }

    IEnumerator FadeOutRoutine(float timer)
    {
        float targetTimer = timer;
        float currentTimer = 0;
        float originalVolume = audioSource.volume;

        while(currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            audioSource.volume = (1 - currentTimer / targetTimer) * originalVolume;

            yield return null;
        }

        audioSource.volume = 0;
    }
    public void FadeIn(float fadeTime)
    {
        StartCoroutine(FadeInRoutine(fadeTime));
    }

    IEnumerator FadeInRoutine(float timer)
    {
        float targetTimer = timer;
        float currentTimer = 0;
        float originalVolume = audioSource.volume;
        if (originalVolume == 0)
            originalVolume = 1;

        while (currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            audioSource.volume = (currentTimer / targetTimer) * originalVolume;

            yield return null;
        }

        audioSource.volume = originalVolume;
    }

}
