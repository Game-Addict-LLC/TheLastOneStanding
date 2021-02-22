using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GunWeapon : WeaponBase
{
    public bool useProjectile;
    public GameObject bulletPrefab;
    public AudioClip gunShot;
    public Transform shootPoint;

    public int shotCount;
    public float spread;
    public bool infiniteAmmo;
    public int ammoCount;
    public float rateOfFire;
    public float range;
    public float recoilSpeed;
    public float recoilAngle;
    public float recoilResetSpeed;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnEquip(Pawn pawn)
    {
        if (equipped == false)
        {
            parentPawn = pawn;

            if (parentPawn.baseWepScript == null)
            {
                Debug.Log("Setting" + gameObject.name + "as base weapon");
                parentPawn.baseWepScript = this;

                if (useRightHand)
                {
                    parentPawn.rightTarget[0] = rightHandTf;
                    if (parentPawn.rightTarget[1] == null)
                    {
                        parentPawn.rightTarget[1] = null;
                    }
                }
                if (useLeftHand)
                {
                    parentPawn.leftTarget[0] = leftHandTf;
                    if (parentPawn.leftTarget[1] == null)
                    {
                        parentPawn.leftTarget[1] = null;
                    }
                }
            }
            else
            {
                Debug.Log("Setting" + gameObject.name + "as special weapon");
                parentPawn.specialWepScript = this;
                parentPawn.baseWepScript.gameObject.SetActive(false);

                if (useRightHand)
                {
                    parentPawn.rightTarget[1] = rightHandTf;
                }
                if (useRightHand)
                {
                    parentPawn.leftTarget[1] = leftHandTf;
                }

                UpdateAmmoText(parentPawn);
            }

            base.OnEquip(pawn);
        }
    }

    public override void OnAttack()
    {
        base.OnAttack();

        if (Time.time >= timer)
        {
            if (useProjectile)
            {
                GameObject tempObject;

                for (int i = 0; i < shotCount; i++)
                {
                    // Spawn a projectile
                    tempObject = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation * Quaternion.Euler(Random.onUnitSphere * spread));
                    tempObject.layer = gameObject.layer;
                    tempObject.GetComponent<BulletScript>().damage = damage;
                    Destroy(tempObject, 5);
                }
            }
            else
            {
                for (int i = 0; i < shotCount; i++)
                {
                    // Spawn a projectile
                    RaycastHit raycastData;
                    Physics.Raycast(shootPoint.position, shootPoint.forward, out raycastData);
                    if (raycastData.collider)
                    {
                        if (raycastData.distance <= range)
                        {
                            if (raycastData.collider.GetComponent<Health>())
                            {
                                raycastData.collider.GetComponent<Health>().TakeDamage(damage);
                            }
                        }
                    }
                    Debug.DrawRay(shootPoint.position, shootPoint.forward * range, Color.red, 5);
                }
            }

            StartCoroutine(Recoil());

            if (gunShot != null)
            {
                Debug.Log(GameManager.instance.sfxVolume);
                AudioSource.PlayClipAtPoint(gunShot, shootPoint.position, GameManager.instance.sfxVolume);
            }

            if (!infiniteAmmo)
            {
                ammoCount--;
                if (ammoCount <= 0)
                {
                    if (this == parentPawn.specialWepScript)
                    {
                        parentPawn.rightTarget[1] = null;
                        parentPawn.leftTarget[1] = null;
                        parentPawn.specialWepScript = null;

                        parentPawn.baseWepScript.gameObject.SetActive(true);
                    }
                    Destroy(gameObject);
                }
            }

            timer = Time.time + 60 / rateOfFire;
        }
    }

    public override void UpdateAmmoText(Pawn pawn)
    {
        GameManager.instance.combatUI.EquipWeapon(this, pawn.controller.playerID);
    }

    IEnumerator Recoil()
    {
        Vector3 tempRot = new Vector3(recoilAngle * -1, 0, 0);
        Quaternion startingRot = transform.localRotation;
        float tempTimer = recoilAngle / recoilSpeed;

        while (tempTimer > 0)
        {
            tempTimer -= Time.deltaTime;
            transform.localRotation = Quaternion.Euler(Quaternion.Slerp(startingRot, Quaternion.Euler(tempRot), 1 - (tempTimer / (recoilAngle / recoilSpeed))).eulerAngles.x, 0, 0);
            yield return null;
        }
        transform.localRotation = Quaternion.Euler(recoilAngle * -1, 0, 0);

        tempRot = new Vector3(0, 0, 0);
        startingRot = transform.localRotation;
        tempTimer = recoilAngle / recoilResetSpeed;
        Debug.Log(tempTimer);

        while (tempTimer > 0)
        {
            tempTimer -= Time.deltaTime;
            transform.localRotation = Quaternion.Euler(Quaternion.Slerp(startingRot, Quaternion.Euler(tempRot), 1 - (tempTimer / (recoilAngle / recoilResetSpeed))).eulerAngles.x, 0, 0);
            yield return null;
        }
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
}


#region EDITOR_CODE

#if UNITY_EDITOR
[CustomEditor(typeof(GunWeapon))]
public class GunWeapon_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        GunWeapon script = (GunWeapon)target;

        script.useProjectile = EditorGUILayout.Toggle("Use Projectile", script.useProjectile);

        if (script.useProjectile)
        {
            script.bulletPrefab = EditorGUILayout.ObjectField("Bullet Prefab", script.bulletPrefab, typeof(GameObject), true) as GameObject;
        }

        script.gunShot = EditorGUILayout.ObjectField("Shot Audio", script.gunShot, typeof(AudioClip), true) as AudioClip;

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

        script.shootPoint = EditorGUILayout.ObjectField("Fire Point", script.shootPoint, typeof(Transform), true) as Transform;

        script.range = EditorGUILayout.FloatField("Range", script.range);

        script.rateOfFire = EditorGUILayout.FloatField("Rate of Fire", script.rateOfFire);

        script.damage = EditorGUILayout.FloatField("Damage", script.damage);

        script.shotCount = EditorGUILayout.IntField("Shot Count", script.shotCount);

        script.spread = EditorGUILayout.FloatField("Weapon Spread", script.spread);

        script.infiniteAmmo = EditorGUILayout.Toggle("Infinite Ammo", script.infiniteAmmo);

        if (!script.infiniteAmmo)
        {
            script.ammoCount = EditorGUILayout.IntField("Ammo Count", script.ammoCount);
        }

        script.usesLifetime = EditorGUILayout.Toggle("Use Lifetime", script.usesLifetime);

        if (script.usesLifetime)
        {
            script.lifetime = EditorGUILayout.FloatField("Lifetime", script.lifetime);
        }

        script.recoilSpeed = EditorGUILayout.FloatField("Recoil Speed", script.recoilSpeed);
        script.recoilAngle = EditorGUILayout.FloatField("Recoil Angle", script.recoilAngle);
        script.recoilResetSpeed = EditorGUILayout.FloatField("Recoil Reset Speed", script.recoilResetSpeed);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
            Debug.Log("Changed");
        }
    }
}
#endif

#endregion
