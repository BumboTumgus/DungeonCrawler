using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float targetTimer = 5;
    public bool attachedHitBox = false;
    private float currentTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (attachedHitBox)
            StartCoroutine(FlickerHitbox());
    }

    // Update is called once per frame
    void Update()
    {
        currentTimer += Time.deltaTime;
        if (currentTimer > targetTimer)
            Destroy(gameObject);
    }

    // USed to make the attached hitbox, if applicable, flicekr to hit enemies.
    IEnumerator FlickerHitbox()
    {
        Collider col = GetComponent<Collider>();
        col.enabled = true;
        yield return new WaitForFixedUpdate();
        col.enabled = false;
    }
}
