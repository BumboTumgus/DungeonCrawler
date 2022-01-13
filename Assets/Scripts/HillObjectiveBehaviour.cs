﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HillObjectiveBehaviour : MonoBehaviour
{
    private int currentPlayerCount = 0;
    private int maximumPlayerCount = 1;

    private float chargeRate = 0f;

    float currentValue = 0f;
    float targetValue = 60f;


    private void Start()
    {
        maximumPlayerCount = GameManager.instance.currentPlayers.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (chargeRate > 0)
        {
            currentValue += chargeRate * Time.deltaTime;
            GameManager.instance.UpdateObjectiveCount(currentValue / targetValue * 100);

            if (currentValue >= targetValue)
                Destroy(gameObject);
        }
    }

    /// <summary>
    /// Update our charge rate based on how many players are in the zone.
    /// </summary>
    private void UpdateCounterSpeed()
    {
        if ((float)currentPlayerCount / (float)maximumPlayerCount >= 0.5f)
            chargeRate = 1f;
        else if (currentPlayerCount == 0)
            chargeRate = 0;
        else
            chargeRate = 0.5f;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            currentPlayerCount++;

        UpdateCounterSpeed();
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            currentPlayerCount--;

        UpdateCounterSpeed();
    }
}
