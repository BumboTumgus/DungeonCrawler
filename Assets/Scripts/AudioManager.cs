﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource[] audioSources;
    public int[] audioRangeSteps;

    public void PlayAudio(int index)
    {
        audioSources[index].Play();
    }

    public void PlayAudioInRange(int index)
    {
        audioSources[Random.Range(index, index + audioRangeSteps[index] + 1)].Play();
    }
}
