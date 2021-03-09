﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pawn : MonoBehaviour {

    public Animator anim;
    public Transform tf;
    public CapsuleCollider pawnCollider;
    public Controller controller;

    public GameObject baseWeapon;
    public GunWeapon baseWepScript;

    public GameObject specialWeapon;
    public GunWeapon specialWepScript;

    public GameObject meleeWeapon;
    public MeleeWeapon meleeWepScript;

    public SpecialAttack specialAbility;

    public Transform weaponPoint;

    public enum DominantHand { Left, Right };
    public DominantHand dominantHand;

    public List<Transform> rightTarget;
    public Transform rightAnchor;
    public List<Transform> leftTarget;
    public Transform leftAnchor;

    [HideInInspector] public bool useFullAnim = false;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        tf = GetComponent<Transform>();
        pawnCollider = GetComponent<CapsuleCollider>();

        GetComponent<PlayerHealth>().parentPawn = this;

        if (specialAbility != null)
        {
            specialAbility.parentPawn = this;
        }

        StartEquip();
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void StartEquip()
    {
        Destroy(specialWeapon);
        rightTarget[0] = null;
        rightTarget[1] = null;
        leftTarget[0] = null;
        leftTarget[1] = null;

        GameObject tempWeapon;

        if (baseWeapon != null)
        {
            tempWeapon = Instantiate(baseWeapon);

            tempWeapon.layer = gameObject.layer;
            tempWeapon.transform.parent = weaponPoint;
            tempWeapon.transform.position = weaponPoint.transform.position;
            tempWeapon.transform.rotation = weaponPoint.transform.rotation;

            if (tempWeapon.GetComponent<GunWeapon>())
            {
                tempWeapon.GetComponent<GunWeapon>().OnEquip(this);
            }
        }

        if (specialWeapon != null)
        {
            tempWeapon = Instantiate(specialWeapon);

            tempWeapon.layer = gameObject.layer;
            tempWeapon.transform.parent = weaponPoint;
            tempWeapon.transform.position = weaponPoint.transform.position;
            tempWeapon.transform.rotation = weaponPoint.transform.rotation;

            if (tempWeapon.GetComponent<GunWeapon>())
            {
                tempWeapon.GetComponent<GunWeapon>().OnEquip(this);
            }
        }

        if (meleeWeapon != null)
        {
            tempWeapon = Instantiate(meleeWeapon);

            tempWeapon.layer = gameObject.layer;

            if (dominantHand == DominantHand.Right)
            {
                tempWeapon.transform.parent = rightAnchor;
                tempWeapon.transform.position = rightAnchor.transform.position;
                tempWeapon.transform.rotation = rightAnchor.transform.rotation;
            }
            else if (dominantHand == DominantHand.Left)
            {
                tempWeapon.transform.parent = leftAnchor;
                tempWeapon.transform.position = leftAnchor.transform.position;
                tempWeapon.transform.rotation = leftAnchor.transform.rotation;
            }

            if (tempWeapon.GetComponent<MeleeWeapon>())
            {
                meleeWepScript = tempWeapon.GetComponent<MeleeWeapon>();
                meleeWepScript.OnEquip(this);
            }
        }

    }

    public void Move(Vector3 direction)
    {
        tf.position += direction * Time.deltaTime * controller.movementSpeed;

        if (controller.isLockedOn)
        {
            Vector3 tempVector = Quaternion.Euler(0, controller.transform.rotation.y * Mathf.PI * Mathf.Rad2Deg * -1, 0) * direction;
            
            anim.SetFloat("Vertical", tempVector.z);
            anim.SetFloat("Horizontal", tempVector.x);
        }
        else
        {
            if (direction.x != 0 || direction.z != 0)
            {
                anim.SetFloat("Vertical", direction.magnitude);
                anim.SetFloat("Horizontal", 0);
            }
            else
            {
                anim.SetFloat("Vertical", 0);
                anim.SetFloat("Horizontal", 0);
            }
        }
    }

    public void Jump()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * controller.jumpHeight * 50);
        anim.SetBool("OnGround", false);
    }

    public void OnEquip(GameObject newWeapon)
    {
        if (newWeapon.GetComponent<GunWeapon>().equipped == false)
        {
            controller.buttonReset = true;
            Destroy(specialWeapon);
            rightTarget[1] = null;
            leftTarget[1] = null;
            specialWeapon = newWeapon;
            specialWeapon.layer = gameObject.layer;
            specialWeapon.transform.parent = weaponPoint;
            specialWeapon.transform.position = weaponPoint.transform.position;
            specialWeapon.transform.rotation = weaponPoint.transform.rotation;
            specialWepScript = specialWeapon.GetComponent<GunWeapon>();
            specialWepScript.OnEquip(this);
        }
    }

    public void UseAbility()
    {
        if (baseWepScript != null)
        {
            baseWepScript.gameObject.SetActive(false);
        }
        if (specialWepScript != null)
        {
            specialWepScript.gameObject.SetActive(false);
        }
        if (meleeWepScript != null)
        {
            meleeWepScript.gameObject.SetActive(false);
        }

        controller.abilityTimer = controller.abilityResetTime;
        GameManager.instance.combatUI.UseAbility(controller);

        anim.SetTrigger("SpecialAttack");
        useFullAnim = true;

        specialAbility.OnUse();
    }

    public void Dismember()
    {
        if (useFullAnim == false)
        {
            if (controller.opponent.GetComponent<Controller>().health.listOfChildScripts == null) { return; }

            int targetLimb = 0;
            foreach (IDamageable<float> child in controller.opponent.GetComponent<Controller>().health.listOfChildScripts)
            {
                if (child is LimbHealth)
                {
                    if ((int)(child as LimbHealth).objectLocation > targetLimb)
                    {
                        if ((child as LimbHealth).currentHealth <= 0 && (child as LimbHealth).dismembered == false)
                        {
                            targetLimb = (int)(child as LimbHealth).objectLocation;
                        }
                    }
                }
            }

            if (targetLimb != 0 && Vector3.Distance(tf.position, controller.opponent.transform.position) < 2 && Vector3.Angle(tf.forward, controller.opponent.transform.position - tf.position) < 20)
            {
                useFullAnim = true;
                anim.SetTrigger("Dismember");

                controller.opponent.transform.position = tf.position + (tf.forward * 0.7f) + (tf.right * -0.2f);
                controller.opponent.transform.LookAt(tf);
                controller.opponent.GetComponent<Controller>().immobile = true;
                controller.immobile = true;

                if (targetLimb % 2 == 0)
                {
                    anim.SetBool("TargetRight", true);
                }
                else
                {
                    anim.SetBool("TargetRight", false);
                }

                if (targetLimb - 2 > 0)
                {
                    anim.SetBool("TargetArm", true);
                }
                else
                {
                    anim.SetBool("TargetArm", false);
                }

                if (dominantHand == DominantHand.Right)
                {
                    anim.SetBool("UseRightArm", false);

                    foreach (IDamageable<float> child in controller.health.listOfChildScripts)
                    {
                        if (child is Health)
                        {
                            if ((child as Health).currentHealth <= 0)
                            {
                                if ((child as Health).objectLocation == Health.Location.RightArm)
                                {
                                    anim.SetBool("UseRightArm", true);
                                }
                            }
                        }
                    }
                }
                else if (dominantHand == DominantHand.Left)
                {
                    anim.SetBool("UseRightArm", true);

                    foreach (IDamageable<float> child in controller.health.listOfChildScripts)
                    {
                        if (child is Health)
                        {
                            if ((child as Health).currentHealth <= 0)
                            {
                                if ((child as Health).objectLocation == Health.Location.LeftArm)
                                {
                                    anim.SetBool("UseRightArm", false);
                                }
                            }
                        }
                    }
                }

                if (baseWepScript != null)
                {
                    baseWepScript.gameObject.SetActive(false);
                }
                if (specialWepScript != null)
                {
                    specialWepScript.gameObject.SetActive(false);
                }
                if (meleeWepScript != null)
                {
                    meleeWepScript.gameObject.SetActive(false);
                }

                StartCoroutine(Dismemberment(targetLimb));
            }
        }
    }

    public void GetDismembered(float limbIndex)
    {
        Health.Location limbToDismember = (Health.Location)limbIndex;
        foreach (IDamageable<float> child in controller.health.listOfChildScripts)
        {
            if (child is LimbHealth)
            {
                if ((int)(child as LimbHealth).objectLocation == limbIndex)
                {
                    (child as LimbHealth).OnDismember();
                }
            }
        }
    }

    public void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag == "Weapon" && LayerMask.LayerToName(gameObject.layer).Contains("Player"))
        {
            OnEquip(collider.gameObject);
        }
    }

    IEnumerator Dismemberment(int targetLimb)
    {
        yield return new WaitForSeconds(1);
        if (controller.opponent.GetComponent<Pawn>() != null)
        {
            controller.opponent.GetComponent<Pawn>().GetDismembered(targetLimb);
        }

        yield return new WaitForSeconds(0.8f);
        useFullAnim = false;
        if (specialWepScript != null)
        {
            specialWepScript.gameObject.SetActive(false);
        }
        else if (baseWepScript != null)
        {
            baseWepScript.gameObject.SetActive(false);
        }
        else if (meleeWepScript != null)
        {
            meleeWepScript.gameObject.SetActive(false);
        }

        controller.opponent.GetComponent<Controller>().immobile = false;
        controller.immobile = false;
    }

    public void OnAnimatorIK(int layerIndex)
    {
        if (useFullAnim || !specialWepScript && !baseWepScript)
        {
            return; //do nothing
        }

        if (specialWepScript != null)
        {
            if (specialWepScript.useRightHand && specialWepScript.rightHandTf)
            {
                anim.SetIKPosition(AvatarIKGoal.RightHand, rightTarget[1].position);
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
                anim.SetIKRotation(AvatarIKGoal.RightHand, rightTarget[1].rotation);
                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
            }
            else
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0f);
            }

            if (specialWepScript.useLeftHand && specialWepScript.leftHandTf)
            {
                anim.SetIKPosition(AvatarIKGoal.LeftHand, leftTarget[1].position);
                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
                anim.SetIKRotation(AvatarIKGoal.LeftHand, leftTarget[1].rotation);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
            }
            else
            {
                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0f);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0f);
            }
        }
        else if (baseWepScript != null)
        {
            if (baseWepScript.useRightHand && baseWepScript.rightHandTf)
            {
                anim.SetIKPosition(AvatarIKGoal.RightHand, rightTarget[0].position);
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
                anim.SetIKRotation(AvatarIKGoal.RightHand, rightTarget[0].rotation);
                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
            }
            else
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0f);
            }

            if (baseWepScript.useLeftHand && baseWepScript.leftHandTf)
            {
                anim.SetIKPosition(AvatarIKGoal.LeftHand, leftTarget[0].position);
                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
                anim.SetIKRotation(AvatarIKGoal.LeftHand, leftTarget[0].rotation);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
            }
            else
            {
                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0f);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0f);
            }
        }
    }
}