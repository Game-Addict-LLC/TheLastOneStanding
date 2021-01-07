using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class WeaponBase : MonoBehaviour {

    public bool useProjectile;
    [HideInInspector] public GameObject bulletPrefab;
    public AudioClip gunShot;
    public bool useRightHand;
    [HideInInspector] public Transform rightHandTf;
    public bool useLeftHand;
    [HideInInspector] public Transform leftHandTf;
    public Transform shootPoint;

    public int shotCount;
    public float spread;
    public bool infiniteAmmo;
    [HideInInspector] public int ammoCount;
    [HideInInspector] public bool equipped = false;
    public float rateOfFire;
    public float damage;
    public float range;

    private float timer;
    private Text ammoText;

    public enum WeaponType { None = 0, Pistol = 1, Rifle = 2, Shotgun = 3 }
    public WeaponType weaponType;

	// Use this for initialization
	void Start () {
        //ammoText = GameObject.Find("Ammo Counter").GetComponent<Text>();
        //if (ammoText.text == "New Text") {
        //    ammoText.text = "No Weapon";
        //}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnEquip(Pawn pawn)
    {
        pawn.rightPoint = rightHandTf;
        pawn.leftPoint = leftHandTf;

        if (pawn.baseWepScript == null)
        {
            pawn.baseWepScript = this;
        }
        else
        {
            pawn.specialWepScript = this;

            UpdateAmmoText();
        }
    }

    public void OnShoot()
    {
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
                AudioSource.PlayClipAtPoint(gunShot, shootPoint.position, GameManager.instance.musicVolume);
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

    public void UpdateAmmoText()
    {
        if (ammoText && gameObject.layer == 9)
        {
            ammoText.text = ammoCount.ToString();
        }
    }
}

#region EDITOR_CODE

#if UNITY_EDITOR
[CustomEditor(typeof(WeaponBase))]
public class WeaponBase_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();

        WeaponBase script = (WeaponBase)target;

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
    }
}
#endif

#endregion