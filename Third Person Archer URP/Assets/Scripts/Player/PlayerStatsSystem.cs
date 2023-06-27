using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatsSystem : MonoBehaviour
{
    #region Serialize Protected Variables
    [SerializeField] protected GameObject floatingTextPrefab;
    [SerializeField] protected Transform  floatingTextParent;
    [SerializeField] protected TMP_Text   healthText;
    [SerializeField] protected TMP_Text manaText;
    #endregion

    //[SerializeField] private CharacterStatsData playerStatsNewData;
    //[SerializeField] private CharacterStatsData playerStatsCurrentData;
    //[SerializeField] private WeaponStatsData weaponStats;

    #region Private Variables
    private readonly float regenDelay = 1f;
    private GameObject     healthBarImage;
    private GameObject     manaBarImage;
    private Animator       animator;
    #endregion

    #region Public ID player character
    public string playerLayer;
    #endregion

    #region Public Movement Variables
    public float moveSpeed;
    public int   runSpeed;
    public int   rotationSpeed;
    public int   jumpForce;
    #endregion

    #region Public Survival Variables
    public int maxHealth;
    public int currentHealth;
    public int regenHealth;
    public int maxMana;
    public int currentMana;
    public int regenMana;
    #endregion

    #region Public Combat Variables
    public int attackDamage;
    public float attackRange;
    #endregion

    #region Public Level and Skill Variables
    public int   level;
    public int   skillPoint;
    public float currentExp;
    #endregion

    #region Public Weapon Stats
    public int weaponDamage;
    public float weaponFireRate;
    public float weaponRange;
    #endregion

    public Vector3 playerPosition;
    public Vector3 playerRotation;

    private void Start()
    {
        animator        = GetComponent<Animator>();
        healthBarImage  = GameObject.FindGameObjectWithTag("HealthBar");
        manaBarImage    = GameObject.FindGameObjectWithTag("ManaBar");
        healthText.text = currentHealth.ToString("#,##0") + "/" + maxHealth.ToString("#,##0");
        manaText.text   = currentMana.ToString("#,##0") + "/" + maxMana.ToString("#,##0");
        
        SetMaxHealthManaImage();
        StartCoroutine(HealthRegeneration());
        StartCoroutine(ManaRegeneration());
    }

    private void SetMaxHealthManaImage()
    {
        healthBarImage.GetComponent<Image>().fillAmount = 1f;
        manaBarImage.GetComponent<Image>().fillAmount = 1f;
        UpdateCurrentHealth(maxHealth, currentHealth);
        UpdateCurrentMana(maxMana, currentMana);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateCurrentHealth(maxHealth, currentHealth);
        animator.SetTrigger("GetHit");
        ShowFloatingText(damage);
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        animator.SetTrigger("Death");
        gameObject.tag = "Untagged";
        UpdateCurrentHealth(maxHealth, currentHealth);
        GetComponent<PlayerAudioManager>().PlayerSound("Death");
    }

    private void ShowFloatingText(int damage)
    {
        var hitObject = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity, floatingTextParent);
        hitObject.GetComponent<TextMesh>().text = damage.ToString("#,##0");
    }

    public void UpdateCurrentHealth(int maxHealth, int currentHealth)
    {
        healthBarImage.GetComponent<Image>().fillAmount = (float)currentHealth / (float)maxHealth;
        healthText.text = currentHealth.ToString("#,##0") + "/" + maxHealth.ToString("#,##0");
    }

    public void UpdateCurrentMana(int maxMana, int currentMana)
    {
        manaBarImage.GetComponent<Image>().fillAmount = (float)currentMana / (float)maxMana;
        manaText.text = currentMana.ToString("#,##0") + "/" + maxMana.ToString("#,##0");
    }

    private IEnumerator HealthRegeneration()
    {
        while (true)
        {
            yield return new WaitForSeconds(regenDelay);

            if (currentHealth > 0 && currentHealth < maxHealth)
            {
                currentHealth += regenHealth;
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
                UpdateCurrentHealth(maxHealth, currentHealth);
            }
        }
    }

    private IEnumerator ManaRegeneration()
    {
        while (true)
        {
            yield return new WaitForSeconds(regenDelay);

            if (currentMana > 0 && currentMana < maxMana)
            {
                currentMana += regenMana;
                currentMana = Mathf.Clamp(currentMana, 0, maxMana);
                UpdateCurrentMana(maxMana, currentMana);
            }
        }
    }

    public void IncreaseStatsPerLevel(int level)
    {
        // Increase Health
        maxHealth += Mathf.FloorToInt((currentHealth * 0.01f) * ((100 - level) * 0.1f));
        currentHealth = maxHealth;
        UpdateCurrentHealth(maxHealth, currentHealth);

        // Increase Mana
        maxMana += Mathf.FloorToInt((currentMana * 0.01f) * ((100 - level) * 0.1f));
        currentMana = maxMana;
        UpdateCurrentHealth(maxMana, currentMana);

        // Increase Regen Rate
        if((level % 5) == 0)
        {
            regenHealth++;
            regenMana++;
        }

        //Increase Attack Damage
        attackDamage += 2;

        //Increase Skill Point
        if(level == 1)
        {
            skillPoint = 0;
        }
        else
        {
            skillPoint++;
        }
    }

    //private readonly Vector3 offSphere = new(0f, 0.8f, 0f);
    //// Draw a sphere to debug attack range in scene
    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere((transform.position + offSphere) + transform.forward * 7f, 7f);
    //}

}
