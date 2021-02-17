using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    [HideInInspector] public Pawn parentPawn;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TakeDamage(float amount)
    {
        if (gameObject.GetComponent<Animator>())
        {
            //gameObject.GetComponent<Animator>().SetTrigger("OnHit");

            if (parentPawn != null)
            {
                StopAllCoroutines();
                //StartCoroutine(HitStun(0.5f));
            }
        }

        base.TakeDamage(amount);
    }

    public IEnumerator HitStun(float stunTimer)
    {
        parentPawn.controller.immobile = true;

        yield return new WaitForSeconds(stunTimer);

        parentPawn.controller.immobile = false;
    }
}
