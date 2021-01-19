using System.Collections;
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
        rightTarget = null;
        leftTarget = null;

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
                baseWepScript = tempWeapon.GetComponent<GunWeapon>();
                Debug.Log("Base wep equip status" + baseWepScript.equipped);
                baseWepScript.OnEquip(this);
            }
        }

        if (meleeWeapon != null)
        {
            tempWeapon = Instantiate(meleeWeapon);

            tempWeapon.layer = gameObject.layer;
            tempWeapon.transform.parent = rightAnchor;
            tempWeapon.transform.position = rightAnchor.transform.position;
            tempWeapon.transform.rotation = rightAnchor.transform.rotation;

            if (tempWeapon.GetComponent<MeleeWeapon>())
            {
                meleeWepScript = tempWeapon.GetComponent<MeleeWeapon>();
                Debug.Log("Melee equip status" + meleeWepScript.equipped);
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
        Debug.Log("Jump");
        anim.SetBool("OnGround", false);
    }

    public void OnEquip(GameObject newWeapon)
    {
        if (newWeapon.GetComponent<GunWeapon>().equipped == false)
        {
            controller.buttonReset = true;
            Destroy(specialWeapon);
            rightTarget = null;
            leftTarget = null;
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
        baseWepScript.gameObject.SetActive(false);
        specialWepScript.gameObject.SetActive(false);
        meleeWepScript.gameObject.SetActive(false);

        controller.abilityTimer = controller.abilityResetTime;
        GameManager.instance.combatUI.UseAbility(controller);

        anim.SetTrigger("SpecialAttack");
        useFullAnim = true;

        specialAbility.OnUse();
    }

    public void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag == "Weapon" && LayerMask.LayerToName(gameObject.layer).Contains("Player"))
        {
            Debug.Log("Equip weapon from ground");
            OnEquip(collider.gameObject);
        }
    }

    public void OnAnimatorIK(int layerIndex)
    {
        //if (!controller.grounded || useFullAnim || !specialWepScript && !baseWepScript)
        //{
        //    return; //do nothing
        //}

        //if (specialWepScript != null)
        //{
        //    if (specialWepScript.useRightHand && specialWepScript.rightHandTf)
        //    {
        //        anim.SetIKPosition(AvatarIKGoal.RightHand, rightTarget[2].position);
        //        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        //        anim.SetIKRotation(AvatarIKGoal.RightHand, rightTarget[2].rotation);
        //        anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
        //    }
        //    else
        //    {
        //        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
        //        anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0f);
        //    }

        //    if (specialWepScript.useLeftHand && specialWepScript.leftHandTf)
        //    {
        //        anim.SetIKPosition(AvatarIKGoal.LeftHand, leftTarget[2].position);
        //        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        //        anim.SetIKRotation(AvatarIKGoal.LeftHand, leftTarget[2].rotation);
        //        anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
        //    }
        //    else
        //    {
        //        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0f);
        //        anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0f);
        //    }
        //}
        //else if (baseWepScript != null)
        //{
        //    if (baseWepScript.useRightHand && baseWepScript.rightHandTf)
        //    {
        //        anim.SetIKPosition(AvatarIKGoal.RightHand, rightTarget[1].position);
        //        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        //        anim.SetIKRotation(AvatarIKGoal.RightHand, rightTarget[1].rotation);
        //        anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
        //    }
        //    else
        //    {
        //        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
        //        anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0f);
        //    }

        //    if (baseWepScript.useLeftHand && baseWepScript.leftHandTf)
        //    {
        //        anim.SetIKPosition(AvatarIKGoal.LeftHand, leftTarget[1].position);
        //        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        //        anim.SetIKRotation(AvatarIKGoal.LeftHand, leftTarget[1].rotation);
        //        anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
        //    }
        //    else
        //    {
        //        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0f);
        //        anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0f);
        //    }
        //}
    }
}