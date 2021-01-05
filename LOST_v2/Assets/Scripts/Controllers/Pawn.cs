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
    [HideInInspector] public WeaponBase baseWepScript;

    public GameObject specialWeapon;
    [HideInInspector] public WeaponBase specialWepScript;

    public GameObject meleeWeapon;
    [HideInInspector] public WeaponBase meleeWepScript;

    public Transform weaponPoint;
    public Transform rightPoint;
    public Transform leftPoint;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        tf = GetComponent<Transform>();
        pawnCollider = GetComponent<CapsuleCollider>();

        StartEquip();
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void StartEquip()
    {
        Destroy(specialWeapon);
        rightPoint = null;
        leftPoint = null;

        GameObject tempWeapon;
        if (meleeWeapon != null)
        {
            tempWeapon = Instantiate(meleeWeapon);

            tempWeapon.layer = gameObject.layer;
            tempWeapon.transform.parent = weaponPoint;
            tempWeapon.transform.position = weaponPoint.transform.position;
            tempWeapon.transform.rotation = weaponPoint.transform.rotation;

            if (tempWeapon.GetComponent<WeaponBase>())
            {
                meleeWepScript = tempWeapon.GetComponent<WeaponBase>();
                meleeWepScript.OnEquip(this);
                meleeWepScript.equipped = true;
            }
        }

        if (baseWeapon != null)
        {
            tempWeapon = Instantiate(baseWeapon);

            tempWeapon.layer = gameObject.layer;
            tempWeapon.transform.parent = weaponPoint;
            tempWeapon.transform.position = weaponPoint.transform.position;
            tempWeapon.transform.rotation = weaponPoint.transform.rotation;

            if (tempWeapon.GetComponent<WeaponBase>())
            {
                baseWepScript = tempWeapon.GetComponent<WeaponBase>();
                baseWepScript.OnEquip(this);
                baseWepScript.equipped = true;
                anim.SetInteger("CurrentWep", (int)baseWepScript.weaponType);
            }

            Debug.Log(baseWepScript);
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
                anim.SetFloat("Vertical", 1);
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
        if (newWeapon.GetComponent<WeaponBase>().equipped == false)
        {
            Destroy(specialWeapon);
            rightPoint = null;
            leftPoint = null;
            anim.SetInteger("CurrentWep", 0);
            specialWeapon = newWeapon;
            specialWeapon.layer = gameObject.layer;
            specialWeapon.transform.parent = weaponPoint;
            specialWeapon.transform.position = weaponPoint.transform.position;
            specialWeapon.transform.rotation = weaponPoint.transform.rotation;
            specialWepScript = specialWeapon.GetComponent<WeaponBase>();
            specialWepScript.OnEquip(this);
            specialWepScript.equipped = true;
            anim.SetInteger("CurrentWep", (int)specialWepScript.weaponType);
        }
    }

    public void OnAnimatorIK(int layerIndex)
    {
        if (!specialWepScript)
        {
            return; //do nothing
        }
        if (specialWepScript.rightHandTf)
        {
            anim.SetIKPosition(AvatarIKGoal.RightHand, rightPoint.position);
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            anim.SetIKRotation(AvatarIKGoal.RightHand, rightPoint.rotation);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);

            anim.SetIKPosition(AvatarIKGoal.LeftHand, leftPoint.position);
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
            anim.SetIKRotation(AvatarIKGoal.LeftHand, leftPoint.rotation);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
        }
        else
        {
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0f);

            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0f);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0f);
        }
    }

    public void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag == "Weapon" && gameObject.layer == 9) {
            OnEquip(collider.gameObject);
        }
    }
}