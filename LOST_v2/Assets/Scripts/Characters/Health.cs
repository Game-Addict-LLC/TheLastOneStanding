﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    [HideInInspector] public float currentHealth;
    public float maxHealth;
    public AudioClip deathSound;

	// Use this for initialization
	void Start () {
        currentHealth = maxHealth;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        GameManager.instance.combatUI.UpdateHealthUI();
        
        if (currentHealth <= 0)
        {
            if (deathSound)
            {
                AudioSource.PlayClipAtPoint(deathSound, transform.position);
            }

            if (gameObject.GetComponent<Controller>())
            {
                Debug.Log("Player death");

                if (gameObject.GetComponent<Controller>().playerID == "P1")
                {
                    GameManager.instance.combatUI.p2Wins++;
                    Debug.Log("P2 Wins!");
                }
                else if (gameObject.GetComponent<Controller>().playerID == "P2")
                {
                    GameManager.instance.combatUI.p1Wins++;
                    Debug.Log("P1 Wins!");
                }
                else
                {
                    Debug.Log("Invalid player ID");
                }

                GameManager.instance.combatUI.UpdateWins();
            }

            Debug.Log("Running death");

            OnDeath();
        }
    }

    public virtual void Heal(float amount, bool overHeal = false)
    {
        currentHealth += amount;

        if (!overHeal) //allows overhealing only if intentional
        {
            //locks health to max health if not set to over heal
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }
    }

    public virtual void OnDeath()
    {
        Destroy(gameObject);
    }

    public virtual void OnDeath(float delay)
    {
        Destroy(gameObject, delay);
    }
}
