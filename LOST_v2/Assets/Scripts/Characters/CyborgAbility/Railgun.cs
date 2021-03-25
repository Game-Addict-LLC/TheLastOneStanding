using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railgun : MonoBehaviour
{
    public SpecialAttack parentScript;
    public int damage;
    public int range;
    public Transform firePoint;
    public GameObject chargeEffect;
    public GameObject fireEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnUse()
    {
        StartCoroutine(FiringSequence());
    }

    public void OnEnd()
    {
        parentScript.OnEnd();
    }

    IEnumerator FiringSequence()
    {
        Debug.Log("Firig main cannon");
        GameObject tempEffect = Instantiate(chargeEffect, firePoint.position, firePoint.rotation, firePoint);
        Destroy(tempEffect, 1.85f);
        yield return new WaitForSeconds(1.9f);

        tempEffect = Instantiate(fireEffect, Vector3.zero, Quaternion.identity);
        tempEffect.GetComponent<LineRenderer>().SetPosition(0, firePoint.position);

        RaycastHit raycastData;
        Physics.Raycast(firePoint.position, Vector3.ProjectOnPlane(firePoint.forward, Vector3.up), out raycastData);
        if (raycastData.collider)
        {
            if (raycastData.distance <= range)
            {
                if (raycastData.collider.GetComponent<Health>())
                {
                    raycastData.collider.GetComponent<Health>().TakeDamage(damage);
                }
               
                StartCoroutine(MakeShotEffect(tempEffect, firePoint.transform.position, raycastData.point));
            }
            else
            {
                StartCoroutine(MakeShotEffect(tempEffect, firePoint.transform.position, firePoint.transform.position + (Vector3.ProjectOnPlane(firePoint.forward, Vector3.up) * range)));
            }
        }
        else
        {
            StartCoroutine(MakeShotEffect(tempEffect, firePoint.transform.position, firePoint.transform.position + (Vector3.ProjectOnPlane(firePoint.forward, Vector3.up) * range)));
        }
    }

    IEnumerator MakeShotEffect(GameObject obj, Vector3 targetOne, Vector3 targetTwo)
    {
        Debug.Log("Shot effect");
        obj.GetComponent<LineRenderer>().SetPosition(0, targetOne);
        obj.GetComponent<LineRenderer>().SetPosition(1, targetTwo);
        Destroy(obj, 0.5f);

        Color tempStartColor = obj.GetComponent<LineRenderer>().startColor;
        Color tempEndColor = obj.GetComponent<LineRenderer>().endColor;

        for (float i = 1; i > 0; i -= 2 * Time.deltaTime)
        {
            obj.GetComponent<LineRenderer>().startColor = new Color(tempStartColor.r, tempStartColor.g, tempStartColor.b, i);
            obj.GetComponent<LineRenderer>().endColor = new Color(tempEndColor.r, tempEndColor.g, tempEndColor.b, i);
            yield return null;
        }

        Destroy(obj);

        OnEnd();
    }
}
