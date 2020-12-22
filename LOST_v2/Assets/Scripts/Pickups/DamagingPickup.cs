using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingPickup : Pickup {

    public float damageAmount;

    protected override void OnPickup(GameObject target)
    {
        Health targetHealth = target.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damageAmount);
        }
        else
        {
            return;
        }

        base.OnPickup(target);
    }

}
