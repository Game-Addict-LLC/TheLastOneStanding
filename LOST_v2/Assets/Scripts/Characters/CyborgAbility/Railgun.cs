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
        Physics.Raycast(firePoint.position, firePoint.forward, out raycastData);
        if (raycastData.collider)
        {
            if (raycastData.distance <= range)
            {
                Debug.Log("hin in range");
                tempEffect.GetComponent<LineRenderer>().SetPosition(1, raycastData.point);

                if (raycastData.collider.GetComponent<Health>())
                {
                    raycastData.collider.GetComponent<Health>().TakeDamage(damage);
                }
            }
            else
            {
                Debug.Log("not in range");
                tempEffect.GetComponent<LineRenderer>().SetPosition(1, firePoint.forward * range);
            }
        }
        else
        {
            Debug.Log("Complete miss");
            tempEffect.GetComponent<LineRenderer>().SetPosition(1, firePoint.forward * range);
        }

        Color tempStartColor = tempEffect.GetComponent<LineRenderer>().startColor;
        Color tempEndColor = tempEffect.GetComponent<LineRenderer>().endColor;

        for (float i = 1; i > 0; i -= 1 * Time.deltaTime)
        {
            tempEffect.GetComponent<LineRenderer>().startColor = new Color(tempStartColor.r, tempStartColor.g, tempStartColor.b, i);
            tempEffect.GetComponent<LineRenderer>().endColor = new Color(tempEndColor.r, tempEndColor.g, tempEndColor.b, i);
            yield return null;
        }

        Destroy(tempEffect);

        OnEnd();
    }
}
