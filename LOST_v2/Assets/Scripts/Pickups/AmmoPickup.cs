using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : Pickup {

    public int ammoAmount;
    protected override void OnPickup(GameObject target)
    {
        Pawn pawn = target.GetComponent<Pawn>();
        if (pawn != null)
        {
            pawn.specialWepScript.ammoCount += ammoAmount;
            pawn.specialWepScript.UpdateAmmoText();
        }
        else
        {
            return;
        }

        base.OnPickup(target);
    }
}
