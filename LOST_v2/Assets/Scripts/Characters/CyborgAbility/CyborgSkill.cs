using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyborgSkill : SpecialAttack
{
    public SkinnedMeshRenderer meshToDisable;
    public Railgun weaponScript;

    // Start is called before the first frame update
    void Start()
    {
        weaponScript.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnUse()
    {
        parentPawn.anim.SetTrigger("SpecialAttack");

        meshToDisable.enabled = false;
        weaponScript.gameObject.SetActive(true);

        weaponScript.OnUse();
        base.OnUse();
    }

    public override void OnEnd()
    {
        meshToDisable.enabled = true;
        weaponScript.gameObject.SetActive(false);

        base.OnEnd();
    }
}
