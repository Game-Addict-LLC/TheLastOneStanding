using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AldoSpecial : SpecialAttack
{
    public GameObject gunPrefab;
    public GunWeapon gunOne;
    public GunWeapon gunTwo;

    // Start is called before the first frame update
    void Start()
    {
        gunOne = Instantiate(gunPrefab, parentPawn.leftAnchor.transform).GetComponent<GunWeapon>();
        gunOne.transform.position = parentPawn.leftAnchor.transform.position;
        gunOne.transform.rotation = parentPawn.leftAnchor.transform.rotation;
        gunOne.equipped = true;

        gunTwo = Instantiate(gunPrefab, parentPawn.rightAnchor.transform).GetComponent<GunWeapon>();
        gunTwo.transform.position = parentPawn.rightAnchor.transform.position;
        gunTwo.transform.rotation = parentPawn.rightAnchor.transform.rotation;
        gunTwo.equipped = true;

        gunOne.gameObject.SetActive(false);
        gunTwo.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnUse()
    {
        gunOne.gameObject.SetActive(true);
        gunTwo.gameObject.SetActive(true);

        parentPawn.controller.immobile = true;
        parentPawn.anim.SetTrigger("SpecialAttack");

        StartCoroutine(attackLoop());
    }

    public override void OnEnd()
    {
        parentPawn.controller.immobile = false;
        base.OnEnd();
    }

    private IEnumerator attackLoop()
    {
        for (int i = 0; i < 6; i++)
        {
            yield return new WaitForSeconds(0.3f);
            gunOne.OnAttack();
            yield return new WaitForSeconds(0.3f);
            gunTwo.OnAttack();
        }

        gunOne.gameObject.SetActive(false);
        gunTwo.gameObject.SetActive(false);
        OnEnd();
    }
}
