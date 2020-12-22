using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainAbility_P1 : MonoBehaviour
{
    //Prefab
    public GameObject hookOBJ;
    public Camera camTarget;

    //Chain logic variables
    public bool chainActive = false;
    public int chainAmount = 0;

    //Movement
    public float speed = 10.0f;
    private float translation;
    private float straffe;

    //Camera
    public float lookSpeed = 3;
    private Vector2 rotation = Vector2.zero;
    private Transform cam;

    // Use this for initialization
    void Start()
    {
        cam = camTarget.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Fire hook toward hook cam forward view
        cam.localRotation = Quaternion.Euler(0 * lookSpeed, 0, 0);

        if (chainActive == true)
        {
            Hook();
        }
        if (chainActive == false)
        {
          
        }
        
    }

    void Hook()
    {
        if(chainAmount == 0)
        {
            //Creates prefab in front of camera using camera's rotation
            var hook = Instantiate(hookOBJ, cam.position + cam.forward, cam.rotation);
            hook.GetComponent<HookScript>().caster = transform;
            chainAmount = 1;
            StartCoroutine(AbilityCooldown());
        }
        
    }
    
    // Check if ability input is activated
    public void CheckChain(bool isActive)
    {
        chainActive = isActive;
    }

    // Ability Cooldown
    IEnumerator AbilityCooldown()
    {
        yield return new WaitForSeconds(10);
        chainAmount = 0;
    }
}
