using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    public Pawn pawn;
    public KeyCode lockOn;
    public float rotateSpeed = 10;
    public float movementSpeed = 5;
    public GameObject opponent;
    public float lockOnDrain = 1;
    public float lockOnRecharge = 0.5f;
    public float lockOnTime = 5;
    [HideInInspector] public bool isLockedOn = false;

    private Transform player;
    private float previousX;
    private float previousY;
    private float lockOnTimer;

    // Use this for initialization
    void Start()
    {
        pawn = GetComponent<Pawn>();
        pawn.controller = this;
        player = gameObject.transform;
        Camera.main.GetComponent<CameraFollowPlayer>().targetTf = player;
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
        if (Input.GetKeyDown(lockOn))
        {
            isLockedOn = !isLockedOn;
        }

        Rotation();
        Movement();
        if (Input.GetMouseButtonDown(0))
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
            else if (pawn.meleeWeapon != null)
            {
                Debug.Log("Melee weapon");
                pawn.meleeWepScript.OnShoot();
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
