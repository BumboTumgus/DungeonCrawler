using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource[] audioSources;
    public List<int> audioRangeStartIndexes;
    public List<int> audioRangeStopIndexes;

    public void PlayAudio(int index)
    {
        audioSources[index].Play();
    }

    public void PlayAudioInRange(int index)
    {
        int audioIndex = Random.Range(audioRangeStartIndexes[index], audioRangeStopIndexes[index] + 1);

        audioSources[audioIndex].Play();
    }

    public void StopAudio(int index)
    {
        audioSources[index].Stop();
    }

    public void MuteAudio(int index)
    {
        audioSources[index].volume = 0;
    }

    public void WipeAllAudioSources()
    {
        for (int index = 0; index < audioSources.Length; index++)
            Destroy(audioSources[index]);
    }
}
