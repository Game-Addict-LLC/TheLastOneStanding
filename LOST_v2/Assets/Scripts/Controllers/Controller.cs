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

    private Transform player;
    private float previousX;
    private float previousY;
    private float lockOnTimer;
    private float abilityTimer;
    private bool ableToLockOn = true;
    private bool grounded;
    [HideInInspector] public bool buttonReset = false;

    // Use this for initialization
    void Start()
    {
        pawn = GetComponent<Pawn>();
        pawn.controller = this;
        player = gameObject.transform;
        health = GetComponent<Health>();

        if (Camera.main.GetComponent<CameraFollowPlayer>().targetTf == null)
        {
            Camera.main.GetComponent<CameraFollowPlayer>().targetTf = player;
        }

        if (playerID == "P1")
        {
            GameManager.instance.playerOne = this;
        }
        else if (playerID == "P2")
        {
            GameManager.instance.playerTwo = this;
        }
        lockOnTimer = lockOnTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.instance.TogglePause();     // Swap paused state
        }

        if (GameManager.instance.isPaused)
        {
            return;     // DO NOTHING
        }

        if (Input.GetButtonDown(playerID + "LockOn") && ableToLockOn == true)
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

        if (grounded)
        {
            Debug.DrawRay(player.position, player.up * -1.5f, Color.blue, 0.5f);

            RaycastHit raycastData;
            Physics.Raycast(player.position, player.up * -1, out raycastData, 1.5f);

            if (Input.GetButtonDown(playerID + "Jump"))
            {
                grounded = false;
                pawn.Jump();
            }
            if (raycastData.collider == null)
            {
                grounded = false;
                pawn.anim.SetBool("OnGround", false);
            }
        }
        else
        {
            Debug.DrawRay(player.position, player.up * -1.5f, Color.red, 0.5f);

            RaycastHit raycastData;
            Physics.Raycast(player.position, player.up * -1, out raycastData, 1.5f);
            if (raycastData.collider != null)
            {
                grounded = true;
                pawn.anim.SetBool("OnGround", true);
            }
        }

        if (Input.GetButtonDown(playerID + "Attack") && buttonReset == false)
        {
            if (pawn.specialWeapon != null)
            {
                Debug.Log("Special weapon");
                pawn.specialWepScript.OnShoot();
                buttonReset = true;
            }
            else if (pawn.baseWeapon != null)
            {
                Debug.Log(pawn.baseWepScript);
                pawn.baseWepScript.OnShoot();
            }

            Debug.Log("Main attack");
        }
        else if (Input.GetAxis(playerID + "Attack") != 0 && buttonReset == false)
        {
            if (pawn.specialWeapon != null)
            {
                Debug.Log("Special weapon");
                pawn.specialWepScript.OnShoot();
                buttonReset = true;
            }
            else if (pawn.baseWeapon != null)
            {
                Debug.Log(pawn.baseWepScript);
                pawn.baseWepScript.OnShoot();
            }

            Debug.Log("Main attack");
        }
        else if (Input.GetButtonDown(playerID + "Melee"))
        {
            if (pawn.meleeWeapon != null)
            {
                Debug.Log("Melee weapon");
                pawn.meleeWepScript.OnShoot();
            }

            Debug.Log("Melee attack");
        }
        else if (Input.GetAxis(playerID + "Attack") == 0 && buttonReset == true)
        {
            buttonReset = false;
        }

        if (abilityTimer <= 0)
        {
            if (Input.GetButtonDown(playerID + "Ability"))
            {
                abilityTimer = abilityResetTime;
                GameManager.instance.combatUI.UseAbility(this);
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
            if (Mathf.Abs(Input.GetAxis(playerID + "Horizontal")) > previousX || Mathf.Abs(Input.GetAxis(playerID + "Vertical")) > previousY)
            {
                float targetAngle = Mathf.Atan2(Input.GetAxis(playerID + "Horizontal"), Input.GetAxis(playerID + "Vertical")) * Mathf.Rad2Deg;
                Quaternion targetQuat = Quaternion.Euler(0, targetAngle, 0);
                player.rotation = Quaternion.RotateTowards(player.rotation, targetQuat, Time.deltaTime * rotateSpeed * 60);
                //player.LookAt(new Vector3(Input.GetAxis("Horizontal") + player.position.x, player.position.y, Input.GetAxis("Vertical") + player.position.z));
            }

            lockOnTimer += Time.deltaTime * lockOnRecharge;
        }
        else
        {
            if (opponent)
            {
                float targetAngle = Mathf.Atan2(opponent.transform.position.x - player.position.x,opponent.transform.position.z - player.position.z) * Mathf.Rad2Deg;
                Quaternion targetQuat = Quaternion.Euler(0, targetAngle, 0);
                player.rotation = Quaternion.RotateTowards(player.rotation, targetQuat, Time.deltaTime * rotateSpeed * 60);
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

        previousX = Input.GetAxis(playerID + "Horizontal");
        previousY = Input.GetAxis(playerID + "Vertical");
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
