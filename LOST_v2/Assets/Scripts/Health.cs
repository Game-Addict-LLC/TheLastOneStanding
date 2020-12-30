using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    [HideInInspector] public float currentHealth;
    public float maxHealth;
    public Text healthText;
    public Image healthBar;
    public AudioClip deathSound;

	// Use this for initialization
	void Start () {
        currentHealth = maxHealth;

        GameManager.instance.combatUI.UpdateHealthUI();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (gameObject.layer == 9)
        {
            GameManager.instance.combatUI.UpdateHealthUI();
        }
        
        if (currentHealth <= 0)
        {
            if (gameObject.GetComponent<EnemyController>())
            {
                foreach (Image child in gameObject.GetComponentsInChildren<Image>())
                {
                    child.enabled = false;
                }
            }

            AudioSource.PlayClipAtPoint(deathSound, transform.position);

            if (gameObject.GetComponent<EnemyController>())
            {
                if (gameObject.GetComponent<EnemyController>().parentSpawner != null)
                {
                    gameObject.GetComponent<EnemyController>().RemoveFromList();
                    gameObject.GetComponent<EnemyController>().enabled = false;
                }
            }

            Destroy(gameObject);
        }
    }

    public void Heal(float amount, bool overHeal = false)
    {
        currentHealth += amount;

        if (!overHeal) //allows overhealing only if intentional
        {
            //locks health to max health if not set to over heal
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }

        if (healthText != null)
        {
            displayHealth();
        }
    }

    public void displayHealth()
    {
        if (healthText != null)
        {
            healthText.text = Mathf.Ceil(currentHealth / maxHealth * 100) + "%";
        }

        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
    }
}
