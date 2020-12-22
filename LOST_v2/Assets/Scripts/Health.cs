using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    [SerializeField] private float health;

    public float maxHealth;
    public Text healthText;
    public Image healthBar;
    public AudioClip deathSound;

	// Use this for initialization
	void Start () {
        health = maxHealth;

        if (gameObject.layer == 9)
        {
            healthText = GameObject.Find("Health Text").GetComponent<Text>();
            foreach (Image child in GameObject.Find("Screen Canvas").GetComponentsInChildren<Image>())
            {
                if (child.gameObject.name == "Health Bar")
                {
                    healthBar = child;
                }
            }
        }

        if (healthBar == null)
        {
            foreach (Image child in gameObject.GetComponentsInChildren<Image>())
            {
                if (child.gameObject.name == "Health Bar")
                {
                    healthBar = child;
                }
            }
        }

        displayHealth();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage(float amount)
    {
        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth);

        displayHealth();

        if (health <= 0)
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
        health += amount;

        if (!overHeal) //allows overhealing only if intentional
        {
            //locks health to max health if not set to over heal
            health = Mathf.Clamp(health, 0, maxHealth);
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
            healthText.text = Mathf.Ceil(health / maxHealth * 100) + "%";
        }

        if (healthBar != null)
        {
            healthBar.fillAmount = health / maxHealth;
        }
    }
}
