using System.Collections;
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
        StopAllCoroutines();

        StartCoroutine(dragBack(hitObject));
    }

    IEnumerator hookLoop()
    {
        Debug.Log("Sending hook");
        while ((hook.transform.position - gameObject.transform.position).magnitude < parentAttack.range)
        {
            hook.transform.position += transform.forward * parentAttack.hookSpeed * Time.deltaTime;
            yield return null;
        }

        Debug.Log("Returning hook");
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
        while ((hook.transform.position - gameObject.transform.position).magnitude > 0.5f)
        {
            hook.transform.position -= transform.forward * parentAttack.hookSpeed  * Time.deltaTime;
            draggedObj.transform.position -= transform.forward * parentAttack.hookSpeed * Time.deltaTime;
            yield return null;
        }

        draggedObj.GetComponent<Pawn>().controller.immobile = false;
        parentAttack.OnEnd();
        yield return null;
    }
}
