using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbHealth : Health
{
    public PlayerHealth parentScript;
    [SerializeField] private bool meshBased;
    [SerializeField] private List<SkinnedMeshRenderer> parentMeshes;
    [SerializeField] private Material removableMaterial;
    [SerializeField] private int materialIndex;
    public GameObject limbToSpawn;
    [HideInInspector] public bool dismembered = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        parentScript.listOfChildScripts.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            OnDeath();
        }
    }

    public override void TakeDamage(float amount)
    {
        parentScript.TakeDamage(amount);

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }

    public override void OnDeath()
    {
        
    }

    public void OnDismember()
    {
        if (meshBased) //Disables mesh objects on limb removal
        {
            foreach (SkinnedMeshRenderer mesh in parentMeshes)
            {
                mesh.enabled = false;
            }
        }
        else //Sets textures to invisible on limb removal
        {
            Material[] matArray;

            foreach (SkinnedMeshRenderer mesh in parentMeshes)
            {
                matArray = mesh.materials;
                matArray[materialIndex] = removableMaterial;
                mesh.materials = matArray;
            }
        }

        GetComponent<Collider>().enabled = false;
        foreach (IDamageable<float> child in listOfChildScripts) //Disables colliders on limb removal to avoid false collisions or blockages
        {
            if ((child as Component).gameObject.GetComponent<Collider>() != null)
            {
                (child as Component).gameObject.GetComponent<Collider>().enabled = false;
            }
        }

        dismembered = true;
    }
}
