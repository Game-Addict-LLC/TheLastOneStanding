using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbarianSkill : SpecialAttack
{
    public GameObject rageEffect;
    public float runSpeed;

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
        parentPawn.anim.SetTrigger("SpecialAttack");
        parentPawn.controller.immobile = true;

        StartCoroutine(Rage());

        base.OnUse();
    }

    public override void OnEnd()
    {
        base.OnEnd();
    }

    IEnumerator Rage()
    {
        GameObject tempObject = null;

        if (rageEffect != null)
        {
            tempObject = Instantiate(rageEffect, transform.position + (transform.up * 1.5f), transform.rotation, transform);
        }

        yield return new WaitForSeconds(2.65f);

        for (float i = 2f; i > 0; i -= Time.deltaTime)
        {
            transform.position += transform.forward * Time.deltaTime * runSpeed;
            yield return null;
        }

        if (tempObject != null)
        {
            Destroy(tempObject, 1);
        }

        parentPawn.anim.SetTrigger("EndSpecial");
        parentPawn.controller.immobile = false;
        OnEnd();
    }
}
