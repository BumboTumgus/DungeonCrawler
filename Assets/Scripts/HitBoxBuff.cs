using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxBuff : MonoBehaviour
{
    public BuffsManager.BuffType buff;

    [SerializeField] private bool hitEnemies = false;
    [SerializeField] private bool hitPlayers = false;
    [SerializeField] private bool hitSelf = false;

    public void BuffSelf()
    {
        //Debug.Log("buffing self");
        if (hitSelf)
            transform.root.GetComponent<BuffsManager>().NewBuff(buff);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("col detected");
        if (other.CompareTag("Enemy") && hitEnemies)
        {
            other.GetComponent<BuffsManager>().NewBuff(buff);
            //Debug.Log("adding buff to enemy");
        }
        else if (other.CompareTag("Player") && hitPlayers)
        {
            other.GetComponent<BuffsManager>().NewBuff(buff);
            //Debug.Log("adding buff to player");
        }
    }
}

