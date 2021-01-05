using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aldo_Anim_Controller : MonoBehaviour
{
    public Animator animator;

    //public RapidFire_Ability_P2 firing;
    public GameObject abilityHand;
    //bool abilityFire = false;

    #region Melee Combo Variables
    List<string> comboList = new List<string>(new string[] { "Aldo_Melee1", "Aldo_Melee2", "Aldo_Melee3" });
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
            // Up Arrow
            if (Input.GetKey(KeyCode.UpArrow))
            {
                animator.ResetTrigger("Aldo_Idle");
                animator.SetTrigger("Aldo_Move");
            }

            // Down Arrow
            if (Input.GetKey(KeyCode.DownArrow))
            {
                animator.ResetTrigger("Aldo_Idle");
                animator.SetTrigger("Aldo_Move");
            }

            // Left Arrow
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                animator.ResetTrigger("Aldo_Idle");
                animator.SetTrigger("Aldo_Move");
            }

            // Right Arrow
            if (Input.GetKey(KeyCode.RightArrow))
            {
                animator.ResetTrigger("Aldo_Idle");
                animator.SetTrigger("Aldo_Move");
            }

            // Up Arrow + Left Arrow
            if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow))
            {
                animator.ResetTrigger("Aldo_Idle");
                animator.SetTrigger("Aldo_Move");
            }

            // Up Arrow + Right Arrow
            if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow))
            {
                animator.ResetTrigger("Aldo_Idle");
                animator.SetTrigger("Aldo_Move");
            }

            // Down Arrow + Left Arrow
            if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftArrow))
            {
                animator.ResetTrigger("Aldo_Idle");
                animator.SetTrigger("Aldo_Move");
            }

            // Down Arrow + Right Arrow
            if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.RightArrow))
            {
                animator.ResetTrigger("Aldo_Idle");
                animator.SetTrigger("Aldo_Move");
            }
            #endregion

            #region Leg Idle Animation Conditions
            // If no P2 keys down, then play idle
            if (Input.GetKey(KeyCode.UpArrow) == false && Input.GetKey(KeyCode.DownArrow) == false &&
                Input.GetKey(KeyCode.LeftArrow) == false && Input.GetKey(KeyCode.RightArrow) == false)
            {

                animator.ResetTrigger("Aldo_Move");
                animator.SetTrigger("Aldo_Idle");

            }
            #endregion

            /*** Shoot Animation Conditions ***/
            #region Shotgun Animations
            if (shotgun.activeSelf == true)
            {
                animator.ResetTrigger("Aldo_Melee_Idle");
                animator.ResetTrigger("Aldo_Melee1");
                animator.ResetTrigger("Aldo_Melee2");
                animator.ResetTrigger("Aldo_Melee3");
                animator.ResetTrigger("Aldo_MachineGun_Idle");
                animator.ResetTrigger("Aldo_9mm_Idle");
                animator.ResetTrigger("Aldo_Deagle_Idle");
                animator.ResetTrigger("Aldo_RocketLauncher_Idle");
                animator.ResetTrigger("Aldo_Ability");
                animator.SetTrigger("Aldo_Shotgun_Idle");
            }

            if (Input.GetKeyDown(KeyCode.Keypad4) == true && shotgun.activeSelf == true)
            {
                animator.ResetTrigger("Aldo_Shotgun_Idle");
                animator.SetTrigger("Aldo_Shotgun_Shoot");
            }

            if (Input.GetKeyDown(KeyCode.Keypad4) == false && shotgun.activeSelf == true)
            {
                animator.ResetTrigger("Aldo_Shotgun_Shoot");
                animator.SetTrigger("Aldo_Shotgun_Idle");
            }
            #endregion

            #region MachineGun Animations
            if (machineGun.activeSelf == true)
            {
                animator.ResetTrigger("Aldo_Melee_Idle");
                animator.ResetTrigger("Aldo_Melee1");
                animator.ResetTrigger("Aldo_Melee2");
                animator.ResetTrigger("Aldo_Melee3");
                animator.ResetTrigger("Aldo_Shotgun_Idle");
                animator.ResetTrigger("Aldo_9mm_Idle");
                animator.ResetTrigger("Aldo_Deagle_Idle");
                animator.ResetTrigger("Aldo_RocketLauncher_Idle");
                animator.ResetTrigger("Aldo_Ability");
                animator.SetTrigger("Aldo_MachineGun_Idle");
            }


            if (Input.GetKeyDown(KeyCode.Keypad4) == true && machineGun.activeSelf == true || Input.GetKey(KeyCode.Keypad4) == true && machineGun.activeSelf == true)
            {
                animator.ResetTrigger("Aldo_MachineGun_Idle");
                animator.SetTrigger("Aldo_MachineGun_Shoot");
            }

            if ((Input.GetKeyDown(KeyCode.Keypad4) == false && machineGun.activeSelf == true) && (Input.GetKey(KeyCode.Keypad4) == false && machineGun.activeSelf == true))
            {
                animator.ResetTrigger("Aldo_MachineGun_Shoot");
                animator.SetTrigger("Aldo_MachineGun_Idle");
            }
            #endregion

            #region Pistol_9mm Animations
            if (pistol_9mm.activeSelf == true)
            {
                animator.ResetTrigger("Aldo_Melee_Idle");
                animator.ResetTrigger("Aldo_Melee1");
                animator.ResetTrigger("Aldo_Melee2");
                animator.ResetTrigger("Aldo_Melee3");
                animator.ResetTrigger("Aldo_Shotgun_Idle");
                animator.ResetTrigger("Aldo_MachineGun_Idle");
                animator.ResetTrigger("Aldo_Deagle_Idle");
                animator.ResetTrigger("Aldo_RocketLauncher_Idle");
                animator.ResetTrigger("Aldo_Ability");
                animator.SetTrigger("Aldo_9mm_Idle");
            }


            if (Input.GetKeyDown(KeyCode.Keypad4) == true && pistol_9mm.activeSelf == true)
            {
                animator.ResetTrigger("Aldo_9mm_Idle");
                animator.SetTrigger("Aldo_9mm_Shoot");
            }

            if ((Input.GetKeyDown(KeyCode.Keypad4) == false && pistol_9mm.activeSelf == true))
            {
                animator.ResetTrigger("Aldo_9mm_Shoot");
                animator.SetTrigger("Aldo_9mm_Idle");
            }
            #endregion

            #region Pistol_Deagle Animations
            if (pistol_Deagle.activeSelf == true)
            {
                animator.ResetTrigger("Aldo_Melee_Idle");
                animator.ResetTrigger("Aldo_Melee1");
                animator.ResetTrigger("Aldo_Melee2");
                animator.ResetTrigger("Aldo_Melee3");
                animator.ResetTrigger("Aldo_Shotgun_Idle");
                animator.ResetTrigger("Aldo_MachineGun_Idle");
                animator.ResetTrigger("Aldo_9mm_Idle");
                animator.ResetTrigger("Aldo_RocketLauncher_Idle");
                animator.ResetTrigger("Aldo_Ability");
                animator.SetTrigger("Aldo_Deagle_Idle");
            }


            if (Input.GetKeyDown(KeyCode.Keypad4) == true && pistol_Deagle.activeSelf == true)
            {
                animator.ResetTrigger("Aldo_Deagle_Idle");
                animator.SetTrigger("Aldo_Deagle_Shoot");
            }

            if ((Input.GetKeyDown(KeyCode.Keypad4) == false && pistol_Deagle.activeSelf == true))
            {
                animator.ResetTrigger("Aldo_Deagle_Shoot");
                animator.SetTrigger("Aldo_Deagle_Idle");
            }
            #endregion

            #region Rocket Launcher Animations
            if (rocketLauncher.activeSelf == true)
            {
                animator.ResetTrigger("Aldo_Melee_Idle");
                animator.ResetTrigger("Aldo_Melee1");
                animator.ResetTrigger("Aldo_Melee2");
                animator.ResetTrigger("Aldo_Melee3");
                animator.ResetTrigger("Aldo_Shotgun_Idle");
                animator.ResetTrigger("Aldo_MachineGun_Idle");
                animator.ResetTrigger("Aldo_9mm_Idle");
                animator.ResetTrigger("Aldo_Deagle_Idle");
                animator.ResetTrigger("Aldo_Ability");
                animator.SetTrigger("Aldo_RocketLauncher_Idle");
            }


            if (Input.GetKeyDown(KeyCode.Keypad4) == true && rocketLauncher.activeSelf == true)
            {
                animator.ResetTrigger("Aldo_RocketLauncher_Idle");
                animator.SetTrigger("Aldo_RocketLauncher_Shoot");
            }

            if ((Input.GetKeyDown(KeyCode.Keypad4) == false && rocketLauncher.activeSelf == true))
            {
                animator.ResetTrigger("Aldo_RocketLauncher_Shoot");
                animator.SetTrigger("Aldo_RocketLauncher_Idle");
            }
            #endregion

            /*** Ability Animation Conditions ***/
            #region Aldo Ability
            if (abilityHand.activeSelf == true)
            {
                if (machineGun.activeSelf == true || shotgun.activeSelf == true || melee.activeSelf == true)
                {
                    if (Input.GetKeyDown(KeyCode.Keypad6))
                    {

                        animator.ResetTrigger("Aldo_Melee_Idle");
                        animator.ResetTrigger("Aldo_Melee1");
                        animator.ResetTrigger("Aldo_Melee2");
                        animator.ResetTrigger("Aldo_Melee3");
                        animator.ResetTrigger("Aldo_Shotgun_Idle");
                        animator.ResetTrigger("Aldo_MachineGun_Idle");
                        animator.SetTrigger("Aldo_Ability");

                        //abilityFire = true;
                        
                    }
                    if (Input.GetKeyUp(KeyCode.Keypad6))
                    {
                        
                    }
                }
            }
            if (abilityHand.activeSelf == false)
            {
                if (Input.GetKeyDown(KeyCode.Keypad6))
                {

                }
            }

            #endregion

        }

        #region Aldo Melee Animation Conditions
        if (melee.activeSelf == true)
        {
            if (x == 0)
            {
                animator.ResetTrigger("Aldo_Move");
                animator.ResetTrigger("Aldo_Shotgun_Idle");
                animator.ResetTrigger("Aldo_MachineGun_Idle");
                animator.ResetTrigger("Aldo_9mm_Idle");
                animator.SetTrigger("Aldo_Idle");
                animator.SetTrigger("Aldo_Melee_Idle");

                x = 1;
            }
            else
            {

                if (Input.GetKeyDown(KeyCode.Keypad5) && comboNum < 3)
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
                        animator.ResetTrigger("Aldo_Melee1");
                        animator.ResetTrigger("Aldo_Melee2");
                        animator.ResetTrigger("Aldo_Melee3");
                        animator.SetTrigger("Aldo_Melee_Reset");
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

