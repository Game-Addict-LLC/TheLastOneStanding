using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUIManager : MonoBehaviour
{
    [SerializeField] private GameObject p1Icon;
    [SerializeField] private GameObject p1AbilityUI;
    [SerializeField] private GameObject p1Health;
    [SerializeField] private GameObject p1Ammo;
    [SerializeField] private GameObject p1WeaponBar;
    [SerializeField] private GameObject p1WeaponUI;
    [SerializeField] private GameObject p1LockOnBar;
    [SerializeField] private GameObject p1WinUI;

    [SerializeField] private GameObject p2Icon;
    [SerializeField] private GameObject p2AbilityUI;
    [SerializeField] private GameObject p2Health;
    [SerializeField] private GameObject p2Ammo;
    [SerializeField] private GameObject p2WeaponBar;
    [SerializeField] private GameObject p2WeaponUI;
    [SerializeField] private GameObject p2LockOnBar;
    [SerializeField] private GameObject p2WinUI;

    [SerializeField] private GameObject timerUI;
    [SerializeField] private GameObject countdownUI;

    [SerializeField] private Gradient healthGradient;

    [HideInInspector] public int p1Wins = 0;
    [HideInInspector] public int p2Wins = 0;

    private Coroutine p1LockOnCoroutine;
    private Coroutine p1WeaponCoroutine;
    private Coroutine p2LockOnCoroutine;
    private Coroutine p2WeaponCoroutine;

    private Coroutine mainTimer;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.combatUI = this;

        InitializeUI();
        UpdateWins();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeUI()
    {
        mainTimer = StartCoroutine(gameTimer(180));

        p1Health.GetComponent<Image>().fillAmount = 1;
        p1LockOnBar.GetComponent<Image>().fillAmount = 1;
        p1Icon.GetComponent<Image>().color = healthGradient.Evaluate(1);
        p1AbilityUI.SetActive(false);
        p1WeaponUI.SetActive(false);

        p2Health.GetComponent<Image>().fillAmount = 1;
        p2LockOnBar.GetComponent<Image>().fillAmount = 1;
        p2Icon.GetComponent<Image>().color = healthGradient.Evaluate(1);
        p2AbilityUI.SetActive(false);
        p2WeaponUI.SetActive(false);
    }

    public void UpdateHealthUI()
    {
        p1Health.GetComponent<Image>().fillAmount = GameManager.instance.playerOne.health.currentHealth / GameManager.instance.playerOne.health.maxHealth;
        p1Icon.GetComponent<Image>().color = healthGradient.Evaluate(GameManager.instance.playerOne.health.currentHealth / GameManager.instance.playerOne.health.maxHealth);

        if (GameManager.instance.playerTwo != null)
        {
            p2Health.GetComponent<Image>().fillAmount = GameManager.instance.playerTwo.health.currentHealth / GameManager.instance.playerTwo.health.maxHealth;
            p2Icon.GetComponent<Image>().color = healthGradient.Evaluate(GameManager.instance.playerTwo.health.currentHealth / GameManager.instance.playerTwo.health.maxHealth);
        }
    }

    public void EquipWeapon(GunWeapon weapon, string playerID)
    {
        Debug.Log("Call UI Equip");
        if (weapon.usesLifetime)
        {
            if (playerID == "P1")
            {
                if (p1WeaponCoroutine != null)
                {
                    StopCoroutine(p1WeaponCoroutine);
                }

                Debug.Log("P1 Equip UI");
                p1WeaponCoroutine = StartCoroutine(weaponTimer(p1WeaponBar.GetComponent<Image>(), p1Ammo.GetComponent<Text>(), weapon));
            }
            else if (playerID == "P2")
            {
                if (p2WeaponCoroutine != null)
                {
                    StopCoroutine(p2WeaponCoroutine);
                }

                Debug.Log("P2 Equip UI");
                p2WeaponCoroutine = StartCoroutine(weaponTimer(p2WeaponBar.GetComponent<Image>(), p2Ammo.GetComponent<Text>(), weapon));
            }
            else
            {
                Debug.Log("Invalid player ID: " + playerID);
            }
        }
    }

    public void UseAbility(Controller controller)
    {
        if (controller.playerID == "P1")
        {
            StartCoroutine(abilityTimer(p1AbilityUI.GetComponent<Image>(), GameManager.instance.playerOne.abilityResetTime));
        }
        else if (controller.playerID == "P2")
        {
            StartCoroutine(abilityTimer(p2AbilityUI.GetComponent<Image>(), GameManager.instance.playerTwo.abilityResetTime));
        }
        else
        {
            Debug.Log("Invalid player ID: " + controller.playerID);
        }
    }

    public void ActivateLockOn(Controller controller)
    {
        if (controller.playerID == "P1")
        {
            if (p1LockOnCoroutine != null)
            {
                StopCoroutine(p1LockOnCoroutine);
            }

            p1LockOnCoroutine = StartCoroutine(lockOnDrain(p1LockOnBar.GetComponent<Image>(), controller.lockOnDrain, controller.lockOnTime));
        }
        else if (controller.playerID == "P2")
        {
            if (p2LockOnCoroutine != null)
            {
                StopCoroutine(p2LockOnCoroutine);
            }

            p2LockOnCoroutine = StartCoroutine(lockOnDrain(p2LockOnBar.GetComponent<Image>(), controller.lockOnDrain, controller.lockOnTime));
        }
        else
        {
            Debug.Log("Invalid player ID: " + controller.playerID);
        }
    }

    public void DisableLockOn(Controller controller)
    {
        string playerID = controller.playerID;
        float rechargeSpeed = controller.lockOnRecharge;
        float cooldownTime = controller.lockOnCooldown;

        if (controller.playerID == "P1")
        {
            if (p1LockOnCoroutine != null)
            {
                StopCoroutine(p1LockOnCoroutine);
            }

            StartCoroutine(lockOnDisable(p1LockOnBar.GetComponent<Image>(), controller.lockOnCooldown));
            p1LockOnCoroutine = StartCoroutine(lockOnCharge(p1LockOnBar.GetComponent<Image>(), controller.lockOnRecharge, controller.lockOnTime));
        }
        else if (controller.playerID == "P2")
        {
            if (p2LockOnCoroutine != null)
            {
                StopCoroutine(p2LockOnCoroutine);
            }

            StartCoroutine(lockOnDisable(p2LockOnBar.GetComponent<Image>(), controller.lockOnCooldown));
            p2LockOnCoroutine = StartCoroutine(lockOnCharge(p2LockOnBar.GetComponent<Image>(), controller.lockOnRecharge, controller.lockOnTime));
        }
        else
        {
            Debug.Log("Invalid player ID: " + controller.playerID);
        }
    }

    public void UpdateWins()
    {
        if (GameManager.instance.playerOne != null && GameManager.instance.playerTwo != null)
        {
            if (GameManager.instance.playerOne.health.currentHealth > GameManager.instance.playerTwo.health.currentHealth)
            {
                p1Wins++;
            }
            else if (GameManager.instance.playerTwo.health.currentHealth > GameManager.instance.playerOne.health.currentHealth)
            {
                p2Wins++;
            }
            else
            {
                Debug.Log("Tie");
            }
        }

        p1WinUI.GetComponent<Image>().fillAmount = p1Wins / 2.0f;
        p2WinUI.GetComponent<Image>().fillAmount = p2Wins / 2.0f;

        if (GameManager.instance.playerOne != null)
        {
            Destroy(GameManager.instance.playerOne.gameObject);
        }
        if (GameManager.instance.playerTwo != null)
        {
            Destroy(GameManager.instance.playerTwo.gameObject);
        }

        StopAllCoroutines();

        InitializeUI();

        if (p1Wins == 2 || p2Wins == 2)
        {
            if (p1Wins > p2Wins)
            {
                GameManager.instance.winner = "P1 Wins!";
            }
            else if (p2Wins > p1Wins)
            {
                GameManager.instance.winner = "P2 Wins!";
            }
            else
            {
                GameManager.instance.winner = "Draw.";
            }
            GameManager.instance.winLossRecord = p1Wins + " - " + p2Wins;

            SceneSwapper.swapper.SwapScenes("VictoryScreen");
        }
    }

    public void EndGame()
    {
        SceneSwapper.swapper.SwapScenes("VictoryScreen");
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
        float startTime = 5;

        timerUI.SetActive(false);
        countdownUI.SetActive(true);

        if (GameManager.instance.playerOne != null && GameManager.instance.playerTwo != null)
        {
            GameManager.instance.playerOne.immobile = true;
            GameManager.instance.playerTwo.immobile = true;
        }
        while (startTime > 0)
        {
            startTime -= Time.deltaTime;
            countdownUI.GetComponent<Text>().text = Mathf.Ceil(startTime).ToString();
            countdownUI.GetComponent<Text>().fontSize = Mathf.RoundToInt(Mathf.Lerp(50, 120, startTime % 1));
            yield return null;
        }

        GameManager.instance.playerOne.immobile = false;
        GameManager.instance.playerTwo.immobile = false;

        float currentTime = gameTime;

        timerUI.SetActive(true);
        countdownUI.SetActive(false);

        while (currentTime > 5)
        {
            currentTime -= Time.deltaTime;
            timerUI.GetComponent<Text>().text = (Mathf.RoundToInt(currentTime)).ToString();
            yield return null;
        }

        timerUI.SetActive(false);
        countdownUI.SetActive(true);

        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            countdownUI.GetComponent<Text>().text = Mathf.Ceil(startTime).ToString();
            countdownUI.GetComponent<Text>().fontSize = Mathf.RoundToInt(Mathf.Lerp(50, 120, startTime % 1));
            yield return null;
        }

        UpdateWins();
    }

    IEnumerator weaponTimer(Image targetImage, Text targetText, GunWeapon weaponScript)
    {
        targetImage.transform.parent.gameObject.SetActive(true);

        float resetTime = weaponScript.lifetime;

        while (resetTime > 0 && weaponScript.ammoCount > 0)
        {
            targetText.text = weaponScript.ammoCount.ToString();

            resetTime -= Time.deltaTime;
            targetImage.fillAmount = resetTime / weaponScript.lifetime;
            yield return null;
        }

        targetImage.transform.parent.gameObject.SetActive(true);
    }

    IEnumerator lockOnDrain(Image targetImage, float drainSpeed, float totalLockOnTime)
    {
        while (true)
        {
            targetImage.fillAmount -= (drainSpeed / totalLockOnTime * Time.deltaTime);

            if (targetImage.fillAmount <= 0)
            {
                break;
            }

            yield return null;
        }
    }

    IEnumerator lockOnCharge(Image targetImage, float rechargeSpeed, float totalLockOnTime)
    {
        while (true)
        {
            targetImage.fillAmount += (rechargeSpeed / totalLockOnTime * Time.deltaTime);

            if (targetImage.fillAmount >= 1)
            {
                break;
            }

            yield return null;
        }
    }

    IEnumerator lockOnDisable(Image targetImage, float lockOnCooldown)
    {
        Color tempColor = targetImage.color;

        targetImage.color = Color.Lerp(targetImage.color, Color.grey, 0.75f);

        yield return new WaitForSeconds(lockOnCooldown);

        targetImage.color = tempColor;
    }
}
