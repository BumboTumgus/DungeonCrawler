using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public GameObject primarySelectedObject;
    public GameObject secondarySelectedObject;

    [SerializeField] private Transform primarySelectionRing = null;
    [SerializeField] private Transform secondarySelectionRing = null;
    [SerializeField] private LayerMask interactableLayer = 1 << 9;
    [SerializeField] private LayerMask collidableLayer = 1 << 10;
    private bool rayshotPrimary = false;
    private bool rayshotSecondary = false;
    private bool deselectFinished = false;
    
    // Update is called once per frame
    void Update()
    {
        // Check the lock first so we dont shoot a useless raycast.
        if (Input.GetAxisRaw("Left Click") == 0)
            rayshotPrimary = false;
        // If we have the input pressed and we can shoot a ray, shoot the ray to check for an object.
        else if (Input.GetAxisRaw("Left Click") != 0 && !rayshotPrimary)
        {
            // Enables rayshot so that this code does not run more than once when the mouse is pressed.
            rayshotPrimary = true;
            
            RaycastHit rayhit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Shoot the ray out, it should only hit interactable objects.
            if (Physics.Raycast(ray, out rayhit, 100, interactableLayer))
            {
                // Disable the previous selected objecta and enable the new one.
                DeselectAll();

                primarySelectedObject = rayhit.transform.gameObject;
                rayhit.transform.GetComponent<Selectable>().selected = true;

                if (rayhit.transform.tag == "Player")
                {
                    primarySelectionRing.SetParent(rayhit.transform);
                    primarySelectionRing.localPosition = Vector3.zero;
                    PlayerStats ps = primarySelectedObject.GetComponent<PlayerStats>();
                    ps.healthBarSecondary.transform.parent.GetComponent<UiFollowTarget>().enabled = true;
                    ps.healthBar.transform.parent.GetComponent<UiFollowTarget>().enabled = false;
                    ps.healthBar.transform.parent.position = new Vector3(10000, 10000, 10000);
                    Debug.Log("Player Selected");
                }
                if (rayhit.transform.tag == "Chest")
                {
                    primarySelectionRing.SetParent(rayhit.transform);
                    primarySelectionRing.localPosition = Vector3.zero;
                    Debug.Log("Chest Selected");
                }
                if(rayhit.transform.tag == "Enemy")
                {
                    primarySelectionRing.SetParent(rayhit.transform);
                    primarySelectionRing.localPosition = Vector3.zero;
                    Debug.Log("Enemy Selected");
                }
            }
        }

        // if the deselct key is pressed, remove our selection.
        if (Input.GetAxisRaw("Deselect") != 0 && !deselectFinished && primarySelectedObject != null)
        {
            deselectFinished = true;
            DeselectAll();
        }
        else if (Input.GetAxisRaw("Deselect") == 0)
            deselectFinished = false;

        // Check the lock first so we don't do useless ray casts that consume memory
        if (Input.GetAxisRaw("Right Click") == 0)
            rayshotSecondary = false;
        // If we have a primary selection and then hit the seconary directive button,
        // We will tell the primary objects action manager to compelte an action.
        if (Input.GetAxisRaw("Right Click") != 0 && primarySelectedObject != null && primarySelectedObject.tag == "Player" && !rayshotSecondary)
        {
            rayshotSecondary = true;

            RaycastHit rayhit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Shoot the ray out, it should only hit interactable objects.
            if (Physics.Raycast(ray, out rayhit, 100, interactableLayer))
            {
                // If the ray hit ourselves, ignore it.
                if (rayhit.transform != primarySelectedObject.transform)
                {
                    // If it hit another object, set it as the target and interact with it.
                    if (secondarySelectedObject != null)
                        secondarySelectedObject.GetComponent<Selectable>().selected = false;
                    secondarySelectedObject = rayhit.transform.gameObject;
                    rayhit.transform.GetComponent<Selectable>().selected = true;
             
                    secondarySelectionRing.SetParent(rayhit.transform);
                    secondarySelectionRing.localPosition = Vector3.zero;
                    Debug.Log("Secondary Object Selected");
                    
                    primarySelectedObject.GetComponent<ActionQueueManager>().SetInteraction(secondarySelectedObject);
                }
            }
            // Shoot the same ray out, howveer instead of interactables, we check for collidab;e layers.
            // This means we are checking fro movement if no interactions are found.
            else if(Physics.Raycast(ray, out rayhit, 100, collidableLayer))
            {
                // If we have a secondary selection already, disable it and pass in the position for the movement command.
                if(secondarySelectedObject != null)
                    secondarySelectedObject.GetComponent<Selectable>().selected = false;
                DeselectSecondaryRing();

                primarySelectedObject.GetComponent<ActionQueueManager>().SetInteraction(rayhit.point);
            }
        }
    }

    // Used to deselect the secondary Ring
    public void DeselectSecondaryRing()
    {
        secondarySelectedObject = null;
        secondarySelectionRing.SetParent(null);
        secondarySelectionRing.position = new Vector3(100, 100, 100);
    }

    // USed to deselect both the rings.
    private void DeselectAll()
    {
        // Disable the primary selection and hide the selection ring.
        if (primarySelectedObject != null)
        {
            primarySelectedObject.GetComponent<Selectable>().selected = false;

            if (primarySelectedObject.tag == "Player")
            {
                PlayerStats ps = primarySelectedObject.GetComponent<PlayerStats>();
                ps.healthBarSecondary.transform.parent.GetComponent<UiFollowTarget>().enabled = false;
                ps.healthBarSecondary.transform.parent.position = new Vector3(10000, 10000, 10000);
                ps.healthBar.transform.parent.GetComponent<UiFollowTarget>().enabled = true;
            }
        }

        primarySelectedObject = null;
        primarySelectionRing.SetParent(null);
        primarySelectionRing.position = new Vector3(100, 100, 100);

        // Disable the secondary selection and hide the selection ring.
        if (secondarySelectedObject != null)
            secondarySelectedObject.GetComponent<Selectable>().selected = false;
        DeselectSecondaryRing();
    }
}
