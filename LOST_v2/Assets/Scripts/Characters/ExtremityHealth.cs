using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtremityHealth : MonoBehaviour, IDamageable<float>
{
    [SerializeField] private Health parentScript;

    // Start is called before the first frame update
    void Start()
    {
        //parentScript.listOfChildScripts.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float amount)
    {
        parentScript.TakeDamage(amount);
    }
}
