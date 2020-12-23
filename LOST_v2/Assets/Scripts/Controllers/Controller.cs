﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    public Pawn pawn;
    public KeyCode lockOnKey = KeyCode.Q;

    public float rotateSpeed = 10;
    public float movementSpeed = 5;
    public float jumpHeight = 10;
    public GameObject opponent;
    public float lockOnDrain = 1;
    public float lockOnRecharge = 0.5f;
    public float lockOnTime = 5;
    [HideInInspector] public bool isLockedOn = false;

    private Transform player;
    private float previousX;
    private float previousY;
    private float lockOnTimer;
    private bool grounded;

    // Use this for initialization
    void Start()
    {
        pawn = GetComponent<Pawn>();
        pawn.controller = this;
        player = gameObject.transform;
        if (Camera.main.GetComponent<CameraFollowPlayer>().targetTf == null)
        {
            Camera.main.GetComponent<CameraFollowPlayer>().targetTf = player;
        }
        GameManager.instance.playerOne = this;

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

        if (Input.GetButtonDown("LockOn"))
        {
            isLockedOn = !isLockedOn;
        }

        Rotation();
        Movement();

        if (Input.GetButtonDown("Attack"))
        {
            if (pawn.specialWeapon != null)
            {
                Debug.Log("Special weapon");
                pawn.specialWepScript.OnShoot();
            }
            else if (pawn.baseWeapon != null)
            {
                Debug.Log("Base weapon");
                pawn.baseWepScript.OnShoot();
            }

            Debug.Log("Main attack");
        }
        else if (Input.GetAxis("Attack") != 0)
        {
            if (pawn.specialWeapon != null)
            {
                Debug.Log("Special weapon");
                pawn.specialWepScript.OnShoot();
            }
            else if (pawn.baseWeapon != null)
            {
                Debug.Log("Base weapon");
                pawn.baseWepScript.OnShoot();
            }

            Debug.Log("Main attack");
        }
        else if (Input.GetButtonDown("Melee"))
        {
            if (pawn.meleeWeapon != null)
            {
                Debug.Log("Melee weapon");
                pawn.meleeWepScript.OnShoot();
            }

            Debug.Log("Melee attack");
        }
    }

    private void FixedUpdate()
    {
        if (grounded)
        {
            Debug.DrawRay(player.position, player.up * -1.5f, Color.blue, 0.5f);

            RaycastHit raycastData;
            Physics.Raycast(player.position, player.up * -1, out raycastData, 1.5f);

            if (Input.GetButtonDown("Jump"))
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
    }

    void Rotation()
    {
        if (!isLockedOn)
        {
            if (Mathf.Abs(Input.GetAxis("Horizontal")) > previousX || Mathf.Abs(Input.GetAxis("Vertical")) > previousY)
            {
                float targetAngle = Mathf.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Mathf.Rad2Deg;
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
        }

        previousX = Input.GetAxis("Horizontal");
        previousY = Input.GetAxis("Vertical");
        lockOnTimer = Mathf.Clamp(lockOnTimer, 0, lockOnTime);

        if (lockOnTimer <= 0)
        {
            isLockedOn = false;
        }
    }  

    void Movement()
    {
        //Move
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1.0f);

        pawn.Move(moveDirection);
    }
}
