using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CainSpecial : SpecialAttack
{
    public GameObject hookPrefab;
    public GameObject spawnLocation;
    public float range;
    public float hookSpeed;

    private GameObject hookObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnUse()
    {
        parentPawn.controller.immobile = true;
        StartCoroutine(specialAttack());
    }

    public override void OnEnd()
    {
        Destroy(hookObject);
        parentPawn.controller.immobile = false;
        base.OnEnd();
    }

    public IEnumerator specialAttack()
    {
        yield return new WaitForSeconds(1);
        hookObject = Instantiate(hookPrefab, spawnLocation.transform.position, gameObject.transform.rotation);
        hookObject.GetComponent<GrappleCode>().parentAttack = this;
        yield return null;
    }
}
