using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowbar_Melee2 : MonoBehaviour
{
    public GameObject crowbar;
    private bool visibility;

    // Start is called before the first frame update
    void Start()
    {
        visibility = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            visibility = true;
            StartCoroutine(ComboTime());
        }
        // Melee Combo 2
        if (Input.GetKeyDown(KeyCode.N))
        {
            visibility = true;
            StartCoroutine(ComboTime());
        }
        // Melee Combo 3
        if (Input.GetKeyDown(KeyCode.M))
        {
            visibility = true;
            StartCoroutine(ComboTime());
        }

        if (visibility == true)
        {
            crowbar.SetActive(true);
        }
        else
        {
            crowbar.SetActive(false);
        }

        IEnumerator ComboTime()
        {
            yield return new WaitForSeconds(3);
            visibility = false;
        }
    }
}
