using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biker_P1 : MonoBehaviour
{
    public Animator animator;

    // Combo Variables //
    List<string> comboList = new List<string>(new string[] { "Biker_P1_Melee1", "Biker_P1_Melee2", "Biker_P1_Melee3" });
    public int comboNum;
    public float reset;
    public float resetTime;

    public GameObject melee;
    public GameObject pistol;
    public GameObject rifle;
    public GameObject shotgun;
    public GameObject special;

    private int x = 0;

    // Start is called before the first frame update
    void Start()
    {

    }
    
    // Update is called once per frame
    void Update()
    {

    

        if (rifle.activeSelf == true || shotgun.activeSelf == true)
        {

            x = 0;

            /*** Forward Animation Conditions ***/
            // W
            if (Input.GetKey(KeyCode.W))
            {
                animator.ResetTrigger("Biker_P1_Idle");
                animator.ResetTrigger("Biker_P1_MeleeIdle");
                animator.ResetTrigger("Biker_P1_Melee1");
                animator.ResetTrigger("Biker_P1_Melee2");
                animator.ResetTrigger("Biker_P1_Melee3");
                animator.ResetTrigger("Biker_P1_Shoot");
                animator.SetTrigger("Biker_P1_Move");
            }

            // S
            if (Input.GetKey(KeyCode.S))
            {
                animator.ResetTrigger("Biker_P1_Idle");
                animator.ResetTrigger("Biker_P1_MeleeIdle");
                animator.ResetTrigger("Biker_P1_Melee1");
                animator.ResetTrigger("Biker_P1_Melee2");
                animator.ResetTrigger("Biker_P1_Melee3");
                animator.ResetTrigger("Biker_P1_Shoot");
                animator.SetTrigger("Biker_P1_Move");
            }

            // A
            if (Input.GetKey(KeyCode.A))
            {
                animator.ResetTrigger("Biker_P1_Idle");
                animator.ResetTrigger("Biker_P1_MeleeIdle");
                animator.ResetTrigger("Biker_P1_Melee1");
                animator.ResetTrigger("Biker_P1_Melee2");
                animator.ResetTrigger("Biker_P1_Melee3");
                animator.ResetTrigger("Biker_P1_Shoot");
                animator.SetTrigger("Biker_P1_Move");
            }

            // D
            if (Input.GetKey(KeyCode.D))
            {
                animator.ResetTrigger("Biker_P1_Idle");
                animator.ResetTrigger("Biker_P1_MeleeIdle");
                animator.ResetTrigger("Biker_P1_Melee1");
                animator.ResetTrigger("Biker_P1_Melee2");
                animator.ResetTrigger("Biker_P1_Melee3");
                animator.ResetTrigger("Biker_P1_Shoot");
                animator.SetTrigger("Biker_P1_Move");
            }

            // W + A
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
            {
                animator.ResetTrigger("Biker_P1_Idle");
                animator.ResetTrigger("Biker_P1_MeleeIdle");
                animator.ResetTrigger("Biker_P1_Melee1");
                animator.ResetTrigger("Biker_P1_Melee2");
                animator.ResetTrigger("Biker_P1_Melee3");
                animator.ResetTrigger("Biker_P1_Shoot");
                animator.SetTrigger("Biker_P1_Move");
            }

            // W + D
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
            {
                animator.ResetTrigger("Biker_P1_Idle");
                animator.ResetTrigger("Biker_P1_MeleeIdle");
                animator.ResetTrigger("Biker_P1_Melee1");
                animator.ResetTrigger("Biker_P1_Melee2");
                animator.ResetTrigger("Biker_P1_Melee3");
                animator.ResetTrigger("Biker_P1_Shoot");
                animator.SetTrigger("Biker_P1_Move");
            }

            // S + A
            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
            {
                animator.ResetTrigger("Biker_P1_Idle");
                animator.ResetTrigger("Biker_P1_MeleeIdle");
                animator.ResetTrigger("Biker_P1_Melee1");
                animator.ResetTrigger("Biker_P1_Melee2");
                animator.ResetTrigger("Biker_P1_Melee3");
                animator.ResetTrigger("Biker_P1_Shoot");
                animator.SetTrigger("Biker_P1_Move");
            }

            // S + D
            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
            {
                animator.ResetTrigger("Biker_P1_Idle");
                animator.ResetTrigger("Biker_P1_MeleeIdle");
                animator.ResetTrigger("Biker_P1_Melee1");
                animator.ResetTrigger("Biker_P1_Melee2");
                animator.ResetTrigger("Biker_P1_Melee3");
                animator.ResetTrigger("Biker_P1_Shoot");
                animator.SetTrigger("Biker_P1_Move");
            }


            /*** Shoot Animation Conditions ***/
            if (Input.GetMouseButtonDown(0))
            {
                animator.ResetTrigger("Biker_P1_Idle");
                animator.ResetTrigger("Biker_P1_Move");
                animator.ResetTrigger("Biker_P1_MeleeIdle");
                animator.ResetTrigger("Biker_P1_Melee1");
                animator.ResetTrigger("Biker_P1_Melee2");
                animator.ResetTrigger("Biker_P1_Melee3");
                animator.SetTrigger("Biker_P1_Shoot");
            }

            /*** Idle Animation Conditions ***/
            // If no P1 keys down, then play idle

            if (Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.S) == false &&
                Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.D) == false && Input.GetMouseButtonDown(0) == false)
            {
                animator.ResetTrigger("Biker_P1_Move");
                animator.ResetTrigger("Biker_P1_Shoot");
                animator.ResetTrigger("Biker_P1_MeleeIdle");
                animator.SetTrigger("Biker_P1_Idle");
            }


            // If no P1 move keys down, then play idle for legs

            if (Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.S) == false &&
                Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.D) == false)
            {
                animator.ResetTrigger("Biker_P1_Move");
                animator.ResetTrigger("Biker_P1_Shoot");
                animator.ResetTrigger("Biker_P1_MeleeIdle");
                animator.SetTrigger("Biker_P1_Idle");
            }
        }

        /*** Melee Animation Conditions ***/
        if (melee.activeSelf == true)
        {
            if (x == 0)
            {
                    animator.ResetTrigger("Biker_P1_Move");
                    animator.ResetTrigger("Biker_P1_Shoot");
                    animator.ResetTrigger("Biker_P1_Idle");
                    animator.SetTrigger("Biker_P1_MeleeIdle");

                x = 1;
            }
            else
            {

                if (Input.GetKeyDown(KeyCode.R) && comboNum < 3)
                {
                    animator.SetTrigger(comboList[comboNum]);
                    comboNum++;
                    reset = 0f;
                }

                if (comboNum > 0)
                {
                    reset += Time.deltaTime;
                    if (reset > resetTime)
                    {
                        animator.SetTrigger("Biker_P1_MeleeReset");
                        comboNum = 0;
                    }

                    if (comboNum == 3)
                    {
                        resetTime = 3f;
                        comboNum = 0;
                    }
                    else
                    {
                        resetTime = 1f;
                    }
                }
            }    
        }
    }
}
