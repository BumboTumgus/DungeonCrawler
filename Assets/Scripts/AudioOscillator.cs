using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOscillator : MonoBehaviour
{
    public float timeBetweenPlays = 5;
    public bool randomnessBetweenIntervals = true;
    public float timeRandomnessPercentage = 0.25f;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(OscilateAudio());
    }


    IEnumerator OscilateAudio()
    {
        audioSource.Play();

        while(true)
        {
            yield return new WaitForSeconds(timeBetweenPlays + timeBetweenPlays * Random.Range(timeRandomnessPercentage * -1, timeRandomnessPercentage));

            audioSource.Play();
        }
    }
}
