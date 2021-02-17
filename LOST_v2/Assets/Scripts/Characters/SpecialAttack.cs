using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpecialAttack : MonoBehaviour
{
    public Pawn parentPawn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnUse()
    {

    }

    public virtual void OnEnd()
    {
        if (parentPawn.specialWepScript != null)
        {
            parentPawn.specialWepScript.gameObject.SetActive(true);
        }
        else if (parentPawn.baseWepScript != null)
        {
            parentPawn.baseWepScript.gameObject.SetActive(true);
        }
        parentPawn.useFullAnim = false;
    }
}
