using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerGearSwitcher : MonoBehaviour
{
    public Item[] itembank;

    public PlayerGearManager pgm;

    public int[] gearToHide;

    private void Start()
    {
        pgm = GetComponent<PlayerGearManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(SwitchingGear());
            Camera.main.transform.root.GetComponent<Animator>().SetTrigger("Spin");
        }
    }

    IEnumerator SwitchingGear()
    {
        int itemIndex = 0;

        while(itemIndex < itembank.Length)
        {
            if (gearToHide[itemIndex] > 0)
                pgm.HideItem(itembank[gearToHide[itemIndex]]);
            pgm.ShowItem(itembank[itemIndex]);

            yield return new WaitForSeconds(0.5f);
            itemIndex++;
        }
        yield return null;
        
    }
}
