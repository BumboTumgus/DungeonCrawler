using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndicatorController : MonoBehaviour
{
    public Transform targetAnchor;
    public Transform originAnchor;

    private float currentDuration = 0;
    private bool flickering = false;

    private LineRenderer lineRenderer;

    [SerializeField] private float duration = 3f;
    [SerializeField] private float flickerDuration = 1f;

    [SerializeField] private LayerMask rayMask;

    [SerializeField] private float range = 20f;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit rayhit;
        Vector3 lineRendererTarget = targetAnchor.transform.position;
        float magnitude = (targetAnchor.transform.position - originAnchor.transform.position).magnitude;

        if (magnitude > range)
            magnitude = range;

        // Snapping to target Logic.
        if (Physics.Raycast(new Ray(originAnchor.position, targetAnchor.position - originAnchor.position), out rayhit, magnitude, rayMask))
            lineRendererTarget = rayhit.point;


        Debug.DrawRay(originAnchor.position, targetAnchor.position - originAnchor.position, Color.green);

        lineRenderer.SetPositions(new Vector3[] { originAnchor.position, lineRendererTarget + Vector3.up });



        // Lifetime Logic
        currentDuration += Time.deltaTime;

        if(!flickering && currentDuration >= duration - flickerDuration)
        {
            flickering = true;
            GetComponent<Animator>().SetTrigger("Flicker");
        }

        if(currentDuration >= duration)
            Destroy(gameObject);
    }
}
