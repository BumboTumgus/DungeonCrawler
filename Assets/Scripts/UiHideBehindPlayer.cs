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
            if(ui != null)
            {
                /**
                Quaternion directionTowardsUI = Quaternion.Euler(Vector3.Normalize(ui.target.position - transform.position));
                float angleDifference = Quaternion.Angle(transform.rotation, directionTowardsUI);
                // Debug.Log(directionTowardsUI + " | "  + angleDifference);

                if (angleDifference > ANGULAR_THRESHOLD)
                    ui.gameObject.SetActive(false);
                else
                    ui.gameObject.SetActive(true);
                */

                Vector3 toTarget = (ui.target.position - transform.position).normalized;

                if (Vector3.Dot(toTarget, transform.forward) > 0)
                    ui.gameObject.SetActive(true);
                else
                    ui.gameObject.SetActive(false);

            }
        }
    }
}
