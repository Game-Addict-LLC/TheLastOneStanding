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
            gameObject.GetComponent<Animator>().SetTrigger("OnHit");

            if (parentPawn != null)
            {
                StopAllCoroutines();
                parentPawn.controller.immobile = true;
                StartCoroutine(HitStun(0.5f));
            }
        }

        base.TakeDamage(amount);
    }

    public IEnumerator HitStun(float stunTimer)
    {
        yield return new WaitForSeconds(stunTimer);

        parentPawn.controller.immobile = false;

        yield return null;
    }
}
