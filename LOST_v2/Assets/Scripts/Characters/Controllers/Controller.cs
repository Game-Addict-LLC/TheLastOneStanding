using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public string playerID = "P1";

    public Pawn pawn;
    public Health health;

    public float rotateSpeed = 10;
    public float movementSpeed = 5;
    public float jumpHeight = 10;

    public GameObject opponent;

    public float lockOnDrain = 1;
    public float lockOnRecharge = 0.5f;
    public float lockOnTime = 5;
    public float lockOnCooldown = 3;
    [HideInInspector] public bool isLockedOn = false;

    public float abilityResetTime = 5;

    private Transform playerTf;
    private LayerMask objectLayer;
    private float previousX;
    private float previousY;
    private float lockOnTimer;
    private bool ableToLockOn = true;

    [HideInInspector] public float abilityTimer;
    [HideInInspector] public bool grounded;
    [HideInInspector] public bool buttonReset = false;
    [HideInInspector] public bool immobile = false;

    // Use this for initialization
    void Start()
    {
        pawn = GetComponent<Pawn>();
        pawn.controller = this;
        playerTf = gameObject.transform;
        objectLayer = gameObject.layer;
        health = GetComponent<Health>();

        if (playerID == "P1")
        {
            GameManager.instance.playerOne = this;
        }
        else if (playerID == "P2")
        {
            GameManager.instance.playerTwo = this;
        }

        GameManager.instance.SetTargets();
        GameObject.Find(playerID + " Camera").GetComponent<CameraFollowPlayer>().Retarget(playerID);

        lockOnTimer = lockOnTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            GameManager.instance.TogglePause();     // Swap paused state
        }

        if (GameManager.instance.isPaused)
        {
            return;     // DO NOTHING
        }

        if (!immobile)
        {
            if (Input.GetButtonDown(playerID + "Dismember"))
            {
                pawn.Dismember();
            }
            else if (Input.GetButtonDown(playerID + "LockOn") && ableToLockOn == true)
            {
                isLockedOn = !isLockedOn;

                if (isLockedOn)
                {
                    GameManager.instance.combatUI.ActivateLockOn(this);
                }
                else
                {
                    StartCoroutine(lockOnDisable());

                    GameManager.instance.combatUI.DisableLockOn(this);
                }
            }

            Rotation();
            Movement();
        }

        if (grounded)
        {
            Debug.DrawRay(playerTf.position + playerTf.up, playerTf.up * -3f, Color.blue, 0.5f);

            RaycastHit raycastData;
            Physics.Raycast(playerTf.position + playerTf.up, playerTf.up * -1, out raycastData, 3f, ~objectLayer);
            
            if (!immobile)
            {
                if (raycastData.collider == null)
                {
                    grounded = false;
                    pawn.anim.SetBool("OnGround", false);
                }
                else if (Input.GetButtonDown(playerID + "Jump"))
                {
                    grounded = false;
                    pawn.Jump();
                    Debug.Log(raycastData.collider.gameObject.layer);
                }
            }
        }
        else
        {
            Debug.DrawRay(playerTf.position + playerTf.up, playerTf.up * -3f, Color.red, 0.5f);

            RaycastHit raycastData;
            Physics.Raycast(playerTf.position + playerTf.up, playerTf.up * -1, out raycastData, 3f, ~objectLayer);
            if (raycastData.collider != null)
            {
                grounded = true;
                pawn.anim.SetBool("OnGround", true);
            }
        }

        if (!immobile)
        {
            if (Input.GetButtonDown(playerID + "Attack") && buttonReset == false)
            {
                if (pawn.specialWepScript != null)
                {
                    pawn.specialWepScript.OnAttack();
                    buttonReset = true;
                }
                else if (pawn.specialWepScript != null)
                {
                    pawn.baseWepScript.OnAttack();
                }

            }
            else if (Input.GetAxis(playerID + "Attack") != 0 && buttonReset == false)
            {
                if (pawn.specialWepScript != null)
                {
                    pawn.specialWepScript.OnAttack();
                    buttonReset = true;
                }
                else if (pawn.baseWepScript != null)
                {
                    pawn.baseWepScript.OnAttack();
                }
            }
            else if (Input.GetButtonDown(playerID + "Melee"))
            {
                if (pawn.meleeWeapon != null)
                {
                    pawn.meleeWepScript.gameObject.SetActive(true);
                    pawn.meleeWepScript.OnAttack();
                }

                pawn.anim.SetTrigger("MeleeAttack");
                pawn.useFullAnim = true;
            }
            else if (Input.GetAxis(playerID + "Attack") == 0 && buttonReset == true)
            {
                buttonReset = false;
            }
        }
        else
        {
            pawn.Move(new Vector3(0, 0, 0));
        }

        if (abilityTimer <= 0)
        {
            if (Input.GetButtonDown(playerID + "Ability") && !immobile)
            {
                pawn.UseAbility();
            }
        }
        else
        {
            abilityTimer -= Time.deltaTime;
        }
    }

    void Rotation()
    {
        if (!isLockedOn)
        {
            if (Mathf.Abs(Input.GetAxis(playerID + "Horizontal")) >= previousX && Mathf.Abs(Input.GetAxis(playerID + "Horizontal")) != 0 || Mathf.Abs(Input.GetAxis(playerID + "Vertical")) >= previousY && Mathf.Abs(Input.GetAxis(playerID + "Vertical")) != 0)
            {
                float targetAngle = Mathf.Atan2(Input.GetAxis(playerID + "Horizontal"), Input.GetAxis(playerID + "Vertical")) * Mathf.Rad2Deg;
                Quaternion targetQuat = Quaternion.Euler(0, targetAngle, 0);
                playerTf.rotation = Quaternion.RotateTowards(playerTf.rotation, targetQuat, Time.deltaTime * rotateSpeed * 60);
            }

            lockOnTimer += Time.deltaTime * lockOnRecharge;
        }
        else
        {
            if (opponent)
            {
                float targetAngle = Mathf.Atan2(opponent.transform.position.x - playerTf.position.x,opponent.transform.position.z - playerTf.position.z) * Mathf.Rad2Deg;
                Quaternion targetQuat = Quaternion.Euler(0, targetAngle, 0);
                playerTf.rotation = Quaternion.RotateTowards(playerTf.rotation, targetQuat, Time.deltaTime * rotateSpeed * 60);
            }

            lockOnTimer -= Time.deltaTime * lockOnDrain;

            if (lockOnTimer <= 0)
            {
                isLockedOn = false;
                lockOnTimer = 0;
                GameManager.instance.combatUI.DisableLockOn(this);
                StartCoroutine(lockOnDisable());
            }
        }

        previousX = Mathf.Abs(Input.GetAxis(playerID + "Horizontal"));
        previousY = Mathf.Abs(Input.GetAxis(playerID + "Vertical"));
        lockOnTimer = Mathf.Clamp(lockOnTimer, 0, lockOnTime);
    }  

    void Movement()
    {
        //Move
        Vector3 moveDirection = new Vector3(Input.GetAxis(playerID + "Horizontal"), 0, Input.GetAxis(playerID + "Vertical"));
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1.0f);

        pawn.Move(moveDirection);
    }

    IEnumerator lockOnDisable()
    {
        ableToLockOn = false;
        yield return new WaitForSeconds(lockOnCooldown);
        ableToLockOn = true;
    }
}
