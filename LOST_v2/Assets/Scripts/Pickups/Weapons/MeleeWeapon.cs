using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MeleeWeapon : WeaponBase
{
    private bool hitThisAttack = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hitThisAttack)
        {
            if (parentPawn.anim.IsInTransition(1))
            {
                StartCoroutine(transitionDelay());
            }
        }
    }

    public override void OnEquip(Pawn pawn)
    {
        parentPawn = pawn;

        parentPawn.meleeWepScript = this;

        gameObject.SetActive(false);

        base.OnEquip(pawn);
    }

    public override void OnAttack()
    {
        if (parentPawn.specialWepScript != null)
        {
            parentPawn.specialWepScript.gameObject.SetActive(false);
        }
        else if (parentPawn.baseWepScript != null)
        {
            parentPawn.baseWepScript.gameObject.SetActive(false);
        }

        StopAllCoroutines();
        parentPawn.anim.SetTrigger("MeleeAttack");

        StartCoroutine(comboTimer(1.25f));
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<Health>())
        {
            hitThisAttack = true;
            collider.GetComponent<Health>().TakeDamage(damage);
        }
    }

    IEnumerator comboTimer(float pressDelay)
    {
        yield return new WaitForSeconds(pressDelay);
        parentPawn.anim.ResetTrigger("MeleeAttack");
        gameObject.SetActive(false);
        parentPawn.useFullAnim = false;

        if (parentPawn.specialWepScript != null)
        {
            parentPawn.specialWepScript.gameObject.SetActive(true);
        }
        else if (parentPawn.baseWepScript != null)
        {
            parentPawn.baseWepScript.gameObject.SetActive(true);
        }
    }

    IEnumerator transitionDelay()
    {
        yield return new WaitForSeconds(0.5f);
        hitThisAttack = false;
    }
}

#region EDITOR_CODE

#if UNITY_EDITOR
[CustomEditor(typeof(MeleeWeapon))]
public class MeleeWeapon_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        MeleeWeapon script = (MeleeWeapon)target;

        script.useLeftHand = EditorGUILayout.Toggle("Use Left Hand", script.useLeftHand);

        if (script.useLeftHand)
        {
            script.leftHandTf = EditorGUILayout.ObjectField("Left Hand Transform", script.leftHandTf, typeof(Transform), true) as Transform;
        }

        script.useRightHand = EditorGUILayout.Toggle("Use Right Hand", script.useRightHand);

        if (script.useRightHand)
        {
            script.rightHandTf = EditorGUILayout.ObjectField("Right Hand Transform", script.rightHandTf, typeof(Transform), true) as Transform;
        }

        script.damage = EditorGUILayout.FloatField("Damage", script.damage);

        script.usesLifetime = EditorGUILayout.Toggle("Use Lifetime", script.usesLifetime);

        if (script.usesLifetime)
        {
            script.lifetime = EditorGUILayout.FloatField("Lifetime", script.lifetime);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}

#endif

#endregion