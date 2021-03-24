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
            StartCoroutine(HitStun(0.75f));
        }

        base.TakeDamage(amount);
    }

    public IEnumerator HitStun(float stunTimer)
    {
        gameObject.GetComponent<Animator>().SetTrigger("OnHit");
        parentPawn.useFullAnim = true;

        yield return new WaitForSeconds(stunTimer);

        parentPawn.useFullAnim = false;
    }
}
