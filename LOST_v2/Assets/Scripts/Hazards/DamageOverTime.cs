using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime : Hazard
{
    public float damageToDeal;
    public float tickRate;
    public bool usesLifetime;
    public float lifetime;

    private List<IDamageable<float>> objectsInField = new List<IDamageable<float>>();
    private float tickTimer;
    private ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        if (usesLifetime)
        {
            Destroy(gameObject, lifetime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tickTimer > 0)
        {
            tickTimer -= Time.deltaTime;
        }
        else
        {
            tickTimer = 1 / tickRate;
            if (objectsInField != null)
            {
                foreach (IDamageable<float> obj in objectsInField)
                {
                    obj.TakeDamage(damageToDeal);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        List<IDamageable<float>> interfaceList = new List<IDamageable<float>>();
        MonoBehaviour[] list = collision.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour mb in list)
        {
            if (mb is IDamageable<float>)
            {
                
                interfaceList.Add((IDamageable<float>)mb);
            }
        }

        if (interfaceList.Count > 0)
        {
            if (objectsInField.Contains(interfaceList[0]) == false)
            {
                objectsInField.Add(interfaceList[0]);
                interfaceList[0].TakeDamage(damageToDeal);
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        List<IDamageable<float>> interfaceList = new List<IDamageable<float>>();
        MonoBehaviour[] list = collision.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour mb in list)
        {
            if (mb is IDamageable<float>)
            {
                interfaceList.Add((IDamageable<float>)mb);
            }
        }

        if (interfaceList.Count > 0)
        {
            if (objectsInField.Contains(interfaceList[0]) == true)
            {
                objectsInField.Remove(interfaceList[0]);
            }
        }
    }
}
