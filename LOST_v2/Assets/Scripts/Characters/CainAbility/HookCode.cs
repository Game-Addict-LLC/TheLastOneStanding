﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookCode : MonoBehaviour
{
    public GrappleCode grappleCode;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<Pawn>())
        {
            collider.GetComponent<Pawn>().controller.immobile = true;

            grappleCode.OnHit(collider.gameObject);
        }
    }
}
