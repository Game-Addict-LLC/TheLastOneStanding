using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class EnemyController : MonoBehaviour {


    private Transform character;
    private GameObject target;
    private NavMeshAgent navMeshAgent;
    private Animator anim;
    public Pawn pawn;
    public float moveSpeed;
    public float maxRange;
    [HideInInspector] public EnemySpawner parentSpawner;
    public List<GameObject> weaponList;


    // Use this for initialization
    void Start()
    {
        pawn = GetComponent<Pawn>();
        character = gameObject.transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (GameManager.instance.playerOne)
        {
            target = GameManager.instance.playerOne.gameObject;
        }
        anim = GetComponent<Animator>();

        WeaponInitialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (!target)
        {
            navMeshAgent.isStopped = true;
            anim.SetFloat("Horizontal", 0f);
            anim.SetFloat("Vertical", 0f);

            if (GameManager.instance.playerOne)
            {
                target = GameManager.instance.playerOne.gameObject;
                navMeshAgent.isStopped = false;
            }
            return;
        }
        else
        { 
            Movement();
            if (Vector3.Angle(character.forward, target.transform.position - character.position) < pawn.specialWepScript.spread && Vector3.Distance(character.position, target.transform.position) <= maxRange)
            {
                pawn.specialWepScript.OnAttack();
                pawn.specialWepScript.ammoCount++;
            }
        }
    }

    void WeaponInitialize()
    {
        GameObject tempWeapon = Instantiate(weaponList[Random.Range(0, weaponList.Count)]);
        pawn.OnEquip(tempWeapon);
    }

    void Movement()
    {
        //Move
        navMeshAgent.SetDestination(target.transform.position);
        Vector3 desiredVelocity = navMeshAgent.desiredVelocity * moveSpeed;
        Vector3 input = Vector3.MoveTowards(desiredVelocity, navMeshAgent.desiredVelocity, navMeshAgent.acceleration * Time.deltaTime);
        input = transform.InverseTransformDirection(input);
        anim.SetFloat("Horizontal", input.x);
        anim.SetFloat("Vertical", input.z);
    }

    public void RemoveFromList()
    {
        parentSpawner.objectList.Remove(gameObject);
    }


    private void OnAnimatorMove()
    {
        navMeshAgent.velocity = anim.velocity;
    }
}
