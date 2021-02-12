using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime : Hazard
{
    public float damageToDeal;
    public float tickRate;

    public List<GameObject> objectsInField;
    private float tickTimer;

    // Start is called before the first frame update
    void Start()
    {
        
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
            if (objectsInField != null)
            {
                foreach (GameObject obj in objectsInField)
                {
                    obj.GetComponent<PlayerHealth>().TakeDamage(damageToDeal);
                }
            }
            tickTimer = 1 / tickRate;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<PlayerHealth>() != null && objectsInField.Contains(collision.gameObject) == false)
        {
            objectsInField.Add(collision.gameObject);
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damageToDeal);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.GetComponent<PlayerHealth>() != null && objectsInField.Contains(collision.gameObject))
        {
            objectsInField.Remove(collision.gameObject);
        }
    }
}
