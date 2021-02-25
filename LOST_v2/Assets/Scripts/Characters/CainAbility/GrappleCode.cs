﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleCode : MonoBehaviour
{
    public CainSpecial parentAttack;

    public GameObject chain;
    public GameObject hook;

    // Start is called before the first frame update
    void Start()
    {
        chain.GetComponent<ChainStretch>().pointOne = hook.transform;
        chain.GetComponent<ChainStretch>().pointTwo = parentAttack.spawnLocation.transform;

        StartCoroutine(hookLoop());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHit(GameObject hitObject)
    {
        Debug.Log("Hit");
        StopAllCoroutines();

        StartCoroutine(dragBack(hitObject));
    }

    IEnumerator hookLoop()
    {
        //moves hook forwards until maximum range is reached
        while ((hook.transform.position - gameObject.transform.position).magnitude < parentAttack.range)
        {
            hook.transform.position += transform.forward * parentAttack.hookSpeed * Time.deltaTime;
            yield return null;
        }

        //moves hook backwards until it returns to the player
        while ((hook.transform.position - gameObject.transform.position).magnitude > 0.5f)
        {
            hook.transform.position -= transform.forward * parentAttack.hookSpeed * 2 * Time.deltaTime;
            yield return null;
        }

        parentAttack.OnEnd();
        yield return null;
    }

    IEnumerator dragBack(GameObject draggedObj)
    {
        //moves hook back until it returns to the player
        while ((hook.transform.position - gameObject.transform.position).magnitude > 0.5f)
        {
            hook.transform.position -= transform.forward * parentAttack.hookSpeed  * Time.deltaTime;
            draggedObj.transform.position -= transform.forward * parentAttack.hookSpeed * Time.deltaTime;
            yield return null;
        }

        parentAttack.parentPawn.meleeWepScript.gameObject.SetActive(true);
        parentAttack.parentPawn.anim.SetTrigger("MeleeAttack");
        draggedObj.GetComponent<Pawn>().controller.immobile = false;

        while (parentAttack.parentPawn.anim.IsInTransition(0) == false)
        {
            yield return null;
        }

        parentAttack.parentPawn.meleeWepScript.gameObject.SetActive(false);
        parentAttack.OnEnd();

        yield return null;
    }
}
