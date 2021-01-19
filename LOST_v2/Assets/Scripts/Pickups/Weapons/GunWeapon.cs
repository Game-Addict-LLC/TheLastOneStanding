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

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Base wep equip status" + equipped);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnEquip(Pawn pawn)
    {
        parentPawn = pawn;

        if (equipped == false)
        {
            if (parentPawn.baseWepScript == null)
            {
                parentPawn.baseWepScript = this;

                if (useRightHand)
                {
                    //parentPawn.rightTarget[1] = rightHandTf;
                }
                if (useLeftHand)
                {
                    //parentPawn.leftTarget[1] = leftHandTf;
                }
            }
            else
            {
                parentPawn.specialWepScript = this;

                if (useRightHand)
                {
                    //parentPawn.rightTarget[2] = rightHandTf;
                }
                if (useRightHand)
                {
                    //parentPawn.leftTarget[2] = leftHandTf;
                }

                UpdateAmmoText();
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
                    Physics.Raycast(shootPoint.position, shootPoint.forward * range, out raycastData);
                    if (raycastData.collider)
                    {
                        if (raycastData.collider.GetComponent<Health>())
                        {
                            raycastData.collider.GetComponent<Health>().TakeDamage(damage);
                        }
                    }
                    Debug.DrawRay(shootPoint.position, shootPoint.forward * range, Color.red, 5);
                }
            }

            if (gunShot != null)
            {
                AudioSource.PlayClipAtPoint(gunShot, shootPoint.position, GameManager.instance.sfxVolume);
            }

            if (!infiniteAmmo)
            {
                ammoCount--;
                if (ammoCount <= 0)
                {
                    Destroy(gameObject);
                }
            }

            UpdateAmmoText();
            timer = Time.time + 60 / rateOfFire;
        }
    }

    public override void UpdateAmmoText()
    {

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

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
            Debug.Log("Changed");
        }
    }
}
#endif

#endregion
