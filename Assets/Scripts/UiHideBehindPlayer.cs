using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiHideBehindPlayer : MonoBehaviour
{
    public List<UiFollowTarget> targets = new List<UiFollowTarget>();

    private const float DISTANCE_THRESHOLD = 1000;

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
                Vector3 distanceToTarget = (ui.target.position - transform.position);

                if (Vector3.Dot(toTarget, transform.forward) > 0)
                {
                    if (distanceToTarget.sqrMagnitude <= DISTANCE_THRESHOLD || ui.GetComponent<UiFollowTarget>().ignoreCameraDistanceCull)
                        ui.gameObject.SetActive(true);
                    else
                    {
                        ui.gameObject.SetActive(false);
                        ui.transform.localPosition = new Vector3(1000, 1000, 0);
                    }
                }
                else if (ui.gameObject.CompareTag("PopUpNumber"))
                    Destroy(ui.gameObject);
                else
                {
                    ui.gameObject.SetActive(false);
                    ui.transform.localPosition = new Vector3(1000, 1000, 0);
                }

            }
        }
    }
}
