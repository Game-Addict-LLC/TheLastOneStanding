using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbHealth : Health
{
    [SerializeField] private PlayerHealth parentScript;
    [SerializeField] private SkinnedMeshRenderer parentMesh;
    [SerializeField] private Material removableMaterial;
    [SerializeField] private int materialIndex;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
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
        StartCoroutine(RemovalState());
    }

    IEnumerator RemovalState()
    {
        parentScript.gameObject.GetComponent<Animator>().SetBool("Stunned", true);
        StartCoroutine(parentScript.HitStun(3));

        Material tempMaterial;
        Material[] matArray = parentMesh.materials;
        tempMaterial = matArray[materialIndex];
        matArray[materialIndex] = removableMaterial;
        parentMesh.materials = matArray;

        yield return new WaitForSeconds(3);

        matArray[materialIndex] = tempMaterial;
        parentMesh.materials = matArray;

        parentScript.gameObject.GetComponent<Animator>().SetBool("Stunned", false);
    }
}
