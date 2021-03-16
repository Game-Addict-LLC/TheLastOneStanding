using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour {

    public Pawn parentPawn;
    public bool useRightHand;
    public Transform rightHandTf;
    public bool useLeftHand;
    public Transform leftHandTf;

    public bool equipped = false;
    public float damage;

    public bool usesLifetime;
    public float lifetime;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void OnEquip(Pawn pawn)
    {
        if (gameObject.GetComponent<Outline>())
        {
            gameObject.GetComponent<Outline>().enabled = false;
        }
        equipped = true;
    }

    public virtual void OnAttack()
    {
        
    }

    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public virtual void UpdateAmmoText(Pawn pawn)
    {

    }
}