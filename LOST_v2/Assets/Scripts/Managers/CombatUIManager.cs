using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUIManager : MonoBehaviour
{
    [SerializeField] private GameObject p1Health;
    [SerializeField] private GameObject p1Ammo;
    [SerializeField] private GameObject p1WeaponBar;
    [SerializeField] private GameObject p1WeaponUI;
    [SerializeField] private GameObject p1AbilityUI;
    [SerializeField] private GameObject p1Icon;

    [SerializeField] private GameObject p2Health;
    [SerializeField] private GameObject p2Ammo;
    [SerializeField] private GameObject p2WeaponBar;
    [SerializeField] private GameObject p2WeaponUI;
    [SerializeField] private GameObject p2AbilityUI;
    [SerializeField] private GameObject p2Icon;

    [SerializeField] private GameObject timerUI;

    [SerializeField] private Gradient gradient;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.combatUI = this;

        InitializeUI();

        UseAbility("P1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeUI()
    {
        StartCoroutine(gameTimer(300));

        p1Health.GetComponent<Image>().fillAmount = 1;
        p1Icon.GetComponent<Image>().color = gradient.Evaluate(1);
        p1AbilityUI.SetActive(false);
        p1WeaponUI.SetActive(false);

        p2Health.GetComponent<Image>().fillAmount = 1;
        p2Icon.GetComponent<Image>().color = gradient.Evaluate(1);
        p2AbilityUI.SetActive(false);
        p2WeaponUI.SetActive(false);
    }

    public void UpdateHealthUI()
    {
        p1Health.GetComponent<Image>().fillAmount = GameManager.instance.playerOne.health.currentHealth / GameManager.instance.playerOne.health.maxHealth;
        p1Icon.GetComponent<Image>().color = gradient.Evaluate(GameManager.instance.playerOne.health.currentHealth / GameManager.instance.playerOne.health.maxHealth);

        p2Health.GetComponent<Image>().fillAmount = GameManager.instance.playerTwo.health.currentHealth / GameManager.instance.playerTwo.health.maxHealth;
        p2Icon.GetComponent<Image>().color = gradient.Evaluate(GameManager.instance.playerTwo.health.currentHealth / GameManager.instance.playerTwo.health.maxHealth);
    }

    public void UseAbility(string playerID)
    {
        if (playerID == "P1")
        {
            StartCoroutine(abilityTimer(p1AbilityUI.GetComponent<Image>(), GameManager.instance.playerOne.abilityTimer));
        }
        else if (playerID == "P1")
        {
            StartCoroutine(abilityTimer(p2AbilityUI.GetComponent<Image>(), GameManager.instance.playerTwo.abilityTimer));
        }
    }

    IEnumerator abilityTimer(Image targetImage, float timerLength)
    {
        targetImage.gameObject.SetActive(true);

        float resetTime = timerLength;

        while (resetTime > 0)
        {
            resetTime -= Time.deltaTime;
            targetImage.fillAmount = resetTime / timerLength;
            yield return null;
        }

        targetImage.gameObject.SetActive(false);
    }

    IEnumerator gameTimer(float gameTime)
    {
        float currentTime = gameTime;

        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            timerUI.GetComponent<Text>().text = (Mathf.RoundToInt(currentTime)).ToString();
            yield return null;
        }

        //TODO: END GAME
    }

    IEnumerator weaponTimer(Image targetImage, float timerLength)
    {
        targetImage.GetComponentInParent<GameObject>().SetActive(true);

        float resetTime = timerLength;

        while (resetTime > 0)
        {
            resetTime -= Time.deltaTime;
            targetImage.fillAmount = resetTime / timerLength;
            yield return null;
        }

        yield return null;

        targetImage.GetComponentInParent<GameObject>().SetActive(false);
    }
}
