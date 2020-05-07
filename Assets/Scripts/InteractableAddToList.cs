using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableAddToList : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject.layer == 13)
        {
            if (GetComponent<Item>() != null)
            {
                Debug.Log("adding us onto the items list.");
                other.GetComponent<Inventory>().ItemInRange(GetComponent<Item>());
            }
            else
            {
                Debug.Log("adding us onto the interactables list.");
                other.GetComponent<Inventory>().interactablesInRange.Add(gameObject);
            }
        }
    }
}
