using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayAfterDelay : MonoBehaviour
{
    [SerializeField] float timeToWait = 1f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().PlayDelayed(timeToWait);
    }

}
