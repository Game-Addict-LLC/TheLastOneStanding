using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cain_Anim_Controls : MonoBehaviour
{
    public Animator animator;

    public ChainAbility_P1 chain;
    public GameObject chainHand;
    bool chainThrow = false;

    #region Melee Combo Variables
    List<string> comboList = new List<string>(new string[] { "Cain_Melee1", "Cain_Melee2", "Cain_Melee3" });
    public int comboNum;
    public float reset;
    public float resetTime;
    private int x = 0;
    #endregion

    #region GameObject Character Weapons

    // standard
    public GameObject melee;
    public GameObject machineGun;
    public GameObject shotgun;

    // special
    
    public GameObject pistol_9mm;
    public GameObject pistol_Deagle;
    public GameObject rocketLauncher;

    #endregion

    // Update is called once per frame
    void Update()
    {
        if (machineGun.activeSelf == true || shotgun.activeSelf == true || pistol_9mm.activeSelf == true || pistol_Deagle.activeSelf == true || rocketLauncher.activeSelf == true)
        {
            x = 0;

            /*** Movement Animation Conditions ***/
            #region Movement Animations
            // W
            if (Input.GetKey(KeyCode.W))
            {
                animator.ResetTrigger("Cain_Idle");
                animator.SetTrigger("Cain_Move");
            }

            // S
            if (Input.GetKey(KeyCode.S))
            {
                animator.ResetTrigger("Cain_Idle");
                animator.SetTrigger("Cain_Move");
            }

            // A
            if (Input.GetKey(KeyCode.A))
            {
                animator.ResetTrigger("Cain_Idle");
                animator.SetTrigger("Cain_Move");
            }

            // D
            if (Input.GetKey(KeyCode.D))
            {
                animator.ResetTrigger("Cain_Idle");
                animator.SetTrigger("Cain_Move");
            }

            // W + A
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
            {
                animator.ResetTrigger("Cain_Idle");
                animator.SetTrigger("Cain_Move");
            }

            // W + D
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
            {
                animator.ResetTrigger("Cain_Idle");
                animator.SetTrigger("Cain_Move");
            }

            // S + A
            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
            {
                animator.ResetTrigger("Cain_Idle");
                animator.SetTrigger("Cain_Move");
            }

            // S + D
            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
            {
                animator.ResetTrigger("Cain_Idle");
                animator.SetTrigger("Cain_Move");
            }
            #endregion
            
            #region Leg Idle Animation Conditions
            // If no P1 keys down, then play idle
            if (Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.S) == false &&
                Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.D) == false)
            {

                animator.ResetTrigger("Cain_Move");
                animator.SetTrigger("Cain_Idle");

            }
            #endregion

            /*** Shoot Animation Conditions ***/
            #region Shotgun Animations
            if (shotgun.activeSelf == true)
            {
                animator.ResetTrigger("Cain_Melee_Idle");
                animator.ResetTrigger("Cain_Melee1");
                animator.ResetTrigger("Cain_Melee2");
                animator.ResetTrigger("Cain_Melee3");
                animator.ResetTrigger("Cain_MachineGun_Idle");
                animator.ResetTrigger("Cain_9mm_Idle");
                animator.ResetTrigger("Cain_Deagle_Idle");
                animator.ResetTrigger("Cain_RocketLauncher_Idle");
                animator.ResetTrigger("Cain_Ability");
                animator.SetTrigger("Cain_Shotgun_Idle");
            }

            if (Input.GetMouseButtonDown(0) && shotgun.activeSelf == true)
            {
                animator.ResetTrigger("Cain_Shotgun_Idle");
                animator.SetTrigger("Cain_Shotgun_Shoot");
            }
            
            if (Input.GetMouseButtonDown(0) == false && shotgun.activeSelf == true)
            {
                animator.ResetTrigger("Cain_Shotgun_Shoot");
                animator.SetTrigger("Cain_Shotgun_Idle");
            }
            #endregion

            #region MachineGun Animations
            if (machineGun.activeSelf == true)
            {
                animator.ResetTrigger("Cain_Melee_Idle");
                animator.ResetTrigger("Cain_Melee1");
                animator.ResetTrigger("Cain_Melee2");
                animator.ResetTrigger("Cain_Melee3");
                animator.ResetTrigger("Cain_Shotgun_Idle");
                animator.ResetTrigger("Cain_9mm_Idle");
                animator.ResetTrigger("Cain_Deagle_Idle");
                animator.ResetTrigger("Cain_RocketLauncher_Idle");
                animator.ResetTrigger("Cain_Ability");
                animator.SetTrigger("Cain_MachineGun_Idle");
            }


            if (Input.GetMouseButtonDown(0) && machineGun.activeSelf == true || Input.GetMouseButton(0) && machineGun.activeSelf == true)
            {
                animator.ResetTrigger("Cain_MachineGun_Idle");
                animator.SetTrigger("Cain_MachineGun_Shoot");
            }

            if ((Input.GetMouseButtonDown(0) == false && machineGun.activeSelf == true) && (Input.GetMouseButton(0) == false && machineGun.activeSelf == true))
            {
                animator.ResetTrigger("Cain_MachineGun_Shoot");
                animator.SetTrigger("Cain_MachineGun_Idle");
            }
            #endregion

            #region Pistol_9mm Animations
            if (pistol_9mm.activeSelf == true)
            {
                animator.ResetTrigger("Cain_Melee_Idle");
                animator.ResetTrigger("Cain_Melee1");
                animator.ResetTrigger("Cain_Melee2");
                animator.ResetTrigger("Cain_Melee3");
                animator.ResetTrigger("Cain_Shotgun_Idle");
                animator.ResetTrigger("Cain_MachineGun_Idle");
                animator.ResetTrigger("Cain_Deagle_Idle");
                animator.ResetTrigger("Cain_RocketLauncher_Idle");
                animator.ResetTrigger("Cain_Ability");
                animator.SetTrigger("Cain_9mm_Idle");
            }


            if (Input.GetMouseButtonDown(0) && pistol_9mm.activeSelf == true)
            {
                animator.ResetTrigger("Cain_9mm_Idle");
                animator.SetTrigger("Cain_9mm_Shoot");
            }

            if ((Input.GetMouseButtonDown(0) == false && pistol_9mm.activeSelf == true))
            {
                animator.ResetTrigger("Cain_9mm_Shoot");
                animator.SetTrigger("Cain_9mm_Idle");
            }
            #endregion

            #region Pistol_Deagle Animations
            if (pistol_Deagle.activeSelf == true)
            {
                animator.ResetTrigger("Cain_Melee_Idle");
                animator.ResetTrigger("Cain_Melee1");
                animator.ResetTrigger("Cain_Melee2");
                animator.ResetTrigger("Cain_Melee3");
                animator.ResetTrigger("Cain_Shotgun_Idle");
                animator.ResetTrigger("Cain_MachineGun_Idle");
                animator.ResetTrigger("Cain_9mm_Idle");
                animator.ResetTrigger("Cain_RocketLauncher_Idle");
                animator.ResetTrigger("Cain_Ability");
                animator.SetTrigger("Cain_Deagle_Idle");
            }


            if (Input.GetMouseButtonDown(0) && pistol_Deagle.activeSelf == true)
            {
                animator.ResetTrigger("Cain_Deagle_Idle");
                animator.SetTrigger("Cain_Deagle_Shoot");
            }

            if ((Input.GetMouseButtonDown(0) == false && pistol_Deagle.activeSelf == true))
            {
                animator.ResetTrigger("Cain_Deagle_Shoot");
                animator.SetTrigger("Cain_Deagle_Idle");
            }
            #endregion

            #region Rocket Launcher Animations
            if (rocketLauncher.activeSelf == true)
            {
                animator.ResetTrigger("Cain_Melee_Idle");
                animator.ResetTrigger("Cain_Melee1");
                animator.ResetTrigger("Cain_Melee2");
                animator.ResetTrigger("Cain_Melee3");
                animator.ResetTrigger("Cain_Shotgun_Idle");
                animator.ResetTrigger("Cain_MachineGun_Idle");
                animator.ResetTrigger("Cain_9mm_Idle");
                animator.ResetTrigger("Cain_Deagle_Idle");
                animator.ResetTrigger("Cain_Ability");
                animator.SetTrigger("Cain_RocketLauncher_Idle");
            }


            if (Input.GetMouseButtonDown(0) && rocketLauncher.activeSelf == true)
            {
                animator.ResetTrigger("Cain_RocketLauncher_Idle");
                animator.SetTrigger("Cain_RocketLauncher_Shoot");
            }

            if ((Input.GetMouseButtonDown(0) == false && rocketLauncher.activeSelf == true))
            {
                animator.ResetTrigger("Cain_RocketLauncher_Shoot");
                animator.SetTrigger("Cain_RocketLauncher_Idle");
            }
            #endregion

            /*** Ability Animation Conditions ***/
            #region Cain Ability
            if(chainHand.activeSelf == true)
            {
                if (machineGun.activeSelf == true || shotgun.activeSelf == true || melee.activeSelf == true)
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {

                        animator.ResetTrigger("Cain_Melee_Idle");
                        animator.ResetTrigger("Cain_Melee1");
                        animator.ResetTrigger("Cain_Melee2");
                        animator.ResetTrigger("Cain_Melee3");
                        animator.ResetTrigger("Cain_Shotgun_Idle");
                        animator.ResetTrigger("Cain_MachineGun_Idle");
                        animator.SetTrigger("Cain_Ability");

                        chainThrow = true;
                        chain.CheckChain(chainThrow);
                    }
                    if (Input.GetKeyUp(KeyCode.F))
                    {
                        chainThrow = false;
                        chain.CheckChain(chainThrow);
                    }
                }
            }
            if(chainHand.activeSelf == false)
            {
                chainThrow = true;
                chain.CheckChain(chainThrow);
                if (Input.GetKeyDown(KeyCode.F))
                {

                }
            }

            #endregion

        }

        #region Cain Melee Animation Conditions
        if (melee.activeSelf == true)
        {
            if (x == 0)
            {
                animator.ResetTrigger("Cain_Move");
                animator.ResetTrigger("Cain_Shotgun_Idle");
                animator.ResetTrigger("Cain_MachineGun_Idle");
                animator.ResetTrigger("Cain_9mm_Idle");
                animator.SetTrigger("Cain_Idle");
                animator.SetTrigger("Cain_Melee_Idle");

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
                        animator.ResetTrigger("Cain_Melee1");
                        animator.ResetTrigger("Cain_Melee2");
                        animator.ResetTrigger("Cain_Melee3");
                        animator.SetTrigger("Cain_Melee_Reset");
                        comboNum = 0;
                    }

                    if (comboNum == 3)
                    {
                        resetTime = 1f;
                        comboNum = 0;
                    }
                    else
                    {
                        resetTime = 1f;
                    }
                }
            }
        }
        #endregion
    }
}
