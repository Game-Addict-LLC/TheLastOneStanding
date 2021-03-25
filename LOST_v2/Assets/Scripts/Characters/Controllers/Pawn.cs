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

    public enum DominantHand { Left, Right };
    public DominantHand dominantHand;

    public List<Transform> rightTarget;
    public Transform rightAnchor;
    public List<Transform> leftTarget;
    public Transform leftAnchor;

    [HideInInspector] public bool useFullAnim = false;

    // Use this for initialization
    void Start() {
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
    void Update() {

        if (specialWepScript != null)
        {
            specialWepScript.transform.position = rightAnchor.transform.position + (specialWepScript.transform.position - specialWepScript.GetComponent<GunWeapon>().rightHandTf.position);
        }
        else if (baseWepScript != null)
        {
            baseWepScript.transform.position = rightAnchor.transform.position + (baseWepScript.transform.position - baseWepScript.GetComponent<GunWeapon>().rightHandTf.position);
        }

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
            tempWeapon.transform.parent = rightAnchor;
            tempWeapon.transform.position = rightAnchor.transform.position + (tempWeapon.transform.position - tempWeapon.GetComponent<GunWeapon>().rightHandTf.position);
            tempWeapon.transform.rotation = rightAnchor.transform.rotation;

            if (tempWeapon.GetComponent<GunWeapon>())
            {
                tempWeapon.GetComponent<GunWeapon>().OnEquip(this);
            }
        }

        if (specialWeapon != null)
        {
            tempWeapon = Instantiate(specialWeapon);

            tempWeapon.layer = gameObject.layer;
            tempWeapon.transform.parent = rightAnchor;
            tempWeapon.transform.position = rightAnchor.transform.position + (tempWeapon.transform.position - tempWeapon.GetComponent<GunWeapon>().rightHandTf.position);
            tempWeapon.transform.rotation = rightAnchor.transform.rotation;

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

        StartCoroutine(JumpingAnim());
    }

    public void OnEquip(GameObject newWeapon)
    {
        if (newWeapon.GetComponent<GunWeapon>().equipped == false)
        {
            controller.buttonReset = true;

            if (specialWepScript != null)
            {
                Destroy(specialWepScript.gameObject);
            }

            rightTarget[1] = null;
            leftTarget[1] = null;
            GameObject tempWeapon = newWeapon;
            tempWeapon.layer = gameObject.layer;
            tempWeapon.transform.parent = rightAnchor;
            tempWeapon.transform.position = rightAnchor.transform.position + (tempWeapon.transform.position - tempWeapon.GetComponent<GunWeapon>().rightHandTf.position);
            tempWeapon.transform.rotation = rightAnchor.transform.rotation;
            specialWepScript = tempWeapon.GetComponent<GunWeapon>();
            specialWepScript.OnEquip(this);
        }
    }

    public void UseAbility()
    {
        if (baseWepScript != null)
        {
            baseWepScript.Deactivate();
        }
        if (specialWepScript != null)
        {
            specialWepScript.Deactivate();
        }
        if (meleeWepScript != null)
        {
            meleeWepScript.Deactivate();
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
                    if ((int)(child as LimbHealth).objectLocation > targetLimb && (int)(child as LimbHealth).objectLocation != 3 && (int)(child as LimbHealth).objectLocation != 2) //Remove != statements
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
                GetComponent<Rigidbody>().isKinematic = true;
                anim.SetTrigger("Dismember");

                controller.opponent.transform.position = tf.position + (tf.forward * 0.7f) /*+ (tf.right * 0.2f)*/;
                controller.opponent.transform.LookAt(tf);
                controller.opponent.GetComponent<Controller>().immobile = true;
                controller.opponent.GetComponent<Rigidbody>().isKinematic = true;
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
                            if ((child as Health).currentHealth >= 0)
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
                            if ((child as Health).currentHealth >= 0)
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
                    baseWepScript.Deactivate();
                }
                if (specialWepScript != null)
                {
                    specialWepScript.Deactivate();
                }
                if (meleeWepScript != null)
                {
                    meleeWepScript.Deactivate();
                }

                StartCoroutine(Dismemberment(targetLimb));
            }
        }
    }

    public LimbHealth GetDismembered(float limbIndex)
    {
        Health.Location limbToDismember = (Health.Location)limbIndex;
        foreach (IDamageable<float> child in controller.health.listOfChildScripts)
        {
            if (child is LimbHealth)
            {
                if ((int)(child as LimbHealth).objectLocation == limbIndex)
                {
                    (child as LimbHealth).OnDismember();
                    return child as LimbHealth;
                }
            }
        }

        return null;
    }

    public void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag == "Weapon" && LayerMask.LayerToName(gameObject.layer).Contains("Player"))
        {
            OnEquip(collider.gameObject);
        }
    }

    void KickstarterDismembered() //DELETE THIS CODE BEFORE FULL RELEASE
    {
        StartCoroutine(DeactivateGun(1.5f));
    }

    IEnumerator JumpingAnim() //DELETE THIS CODE BEFORE FULL RELEASE
    {
        useFullAnim = true;
        yield return new WaitForSeconds(1.25f);
        useFullAnim = false;
    }

    IEnumerator DeactivateGun(float time)
    {
        Debug.Log("Disable gun");
        if (specialWepScript != null)
        {
            specialWepScript.gameObject.SetActive(false);
        }
        else if (baseWepScript != null)
        {
            baseWepScript.gameObject.SetActive(false);
        }
        useFullAnim = true;
        Debug.Log("set anim to full");
        yield return new WaitForSeconds(time);

        if (specialWepScript != null)
        {
            specialWepScript.gameObject.SetActive(true);
        }
        else if (baseWepScript != null)
        {
            baseWepScript.gameObject.SetActive(true);
        }
        useFullAnim = false;
        Debug.Log("set anim to false");
    }

    IEnumerator Dismemberment(int targetLimb)
    {
        controller.opponent.GetComponent<Pawn>().anim.SetTrigger("GetDismembered");
        controller.opponent.GetComponent<Pawn>().KickstarterDismembered();

        yield return new WaitForSeconds(1);
        if (controller.opponent.GetComponent<Pawn>() != null)
        {
            LimbHealth tempScript;
            tempScript = controller.opponent.GetComponent<Pawn>().GetDismembered(targetLimb);

            if (tempScript != null)
            {
                if (anim.GetBool("UseRightHand") == true)
                {
                    GameObject tempLimb = Instantiate(tempScript.limbToSpawn);
                    tempLimb.transform.position = controller.opponent.transform.position;
                    tempLimb.transform.rotation = controller.opponent.transform.rotation;
                }
                else if (anim.GetBool("UseRightHand") == false)
                {
                    GameObject tempLimb = Instantiate(tempScript.limbToSpawn);
                    tempLimb.transform.position = controller.opponent.transform.position;
                    tempLimb.transform.rotation = controller.opponent.transform.rotation;
                }
            }
        }

        yield return new WaitForSeconds(0.8f);
        useFullAnim = false;
        if (specialWepScript != null)
        {
            specialWepScript.gameObject.SetActive(true);
        }
        else if (baseWepScript != null)
        {
            baseWepScript.gameObject.SetActive(true);
        }
        else if (meleeWepScript != null)
        {
            meleeWepScript.gameObject.SetActive(true);
        }

        controller.opponent.GetComponent<Controller>().immobile = false;
        controller.opponent.GetComponent<Rigidbody>().isKinematic = false;
        controller.immobile = false;
        GetComponent<Rigidbody>().isKinematic = false;
    }

    public void OnAnimatorIK(int layerIndex)
    {
        if (useFullAnim || !controller.grounded || !specialWepScript && !baseWepScript)
        {
            return; //do nothing
        }

        if (specialWepScript != null)
        {
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