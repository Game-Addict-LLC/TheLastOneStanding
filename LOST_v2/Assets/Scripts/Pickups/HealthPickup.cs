using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup {

    public float healAmount;
    public bool overhealBool;
    protected override void OnPickup(GameObject target)
    {
        Health targetHealth = target.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.Heal(healAmount, overhealBool);
        }
        else
        {
            return;
        }

        base.OnPickup(target);
    }
}
