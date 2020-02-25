using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiHideBehindPlayer : MonoBehaviour
{
    public List<UiFollowTarget> targets = new List<UiFollowTarget>();

    private const float ANGULAR_THRESHOLD = 90;

    // Update is called once per frame
    void Update()
    {
        foreach(UiFollowTarget ui in targets)
        {
            Quaternion directionTowardsUI = Quaternion.Euler(Vector3.Normalize(ui.target.position - transform.position));
            float angleDifference = Quaternion.Angle(transform.rotation, directionTowardsUI);

            if (angleDifference > ANGULAR_THRESHOLD)
                ui.gameObject.SetActive(false);
            else
                ui.gameObject.SetActive(true);
        }
    }
}
