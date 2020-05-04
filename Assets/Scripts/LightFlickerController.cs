using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlickerController : MonoBehaviour
{
    public float maximumChangePercentage = 0.5f;
    public float maximumWait = 1f;
    public float minimumWait = 0.4f;

    Light lightSource;
    float lightBaseIntensity;
    float lightBaseRange;

    float lightCurrentIntensity;
    float lightCurrentRange;
    float lightTargetIntensity;
    float lightTargetRange;


    // Start is called before the first frame update
    void Start()
    {
        lightSource = GetComponent<Light>();
        lightBaseIntensity = lightSource.intensity;
        lightBaseRange = lightSource.range;
        StartCoroutine(MoveToTargets(Random.Range(1 - maximumChangePercentage, 1 + maximumChangePercentage)));
    }
    
    IEnumerator MoveToTargets(float targetModifier)
    {
        Debug.Log("Mobing to new target");
        lightTargetIntensity = lightBaseIntensity * targetModifier;
        lightTargetRange = lightBaseRange * targetModifier;

        float currentTimer = 0;
        float targetTimer = Random.Range(minimumWait, maximumWait);

        float lightIntensityChange = (lightTargetIntensity - lightCurrentIntensity) / targetTimer;
        float lightRangeChange = (lightTargetRange - lightCurrentRange) / targetTimer;

        while(currentTimer < targetTimer)
        {
            currentTimer += Time.deltaTime;
            lightCurrentIntensity += lightIntensityChange * Time.deltaTime;
            lightCurrentRange += lightRangeChange * Time.deltaTime;

            lightSource.intensity = lightCurrentIntensity;
            lightSource.range = lightCurrentRange;

            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(MoveToTargets(Random.Range(1 - maximumChangePercentage, 1 + maximumChangePercentage)));
    }
}
