using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemiesStatsSystem : MonoBehaviour
{
    #region Other Variables
    [SerializeField] protected GameObject floatingTextPrefab;
    [SerializeField] protected Transform  floatingTextParent;
    [SerializeField] protected TMP_Text   healthText;
    [SerializeField] protected Canvas     healthCanvas;

    protected EnemiesHealthBarManager healthBar;
    protected Animator                animator;
    private readonly float            regenDelay = 1f;
    private GetEnemyLayer objectLayer;
    #endregion

    #region Check Closest Target Variables
    public Transform closestTargetTransform;
    protected float timerCheckTarget;
    protected readonly float timeCheckTarget = 1;
    #endregion

    #region Public ID Enemies
    public string enemyLayer;
    public string enemyType; // melee or range
    #endregion

    #region Public Survival Variables
    public int maxHealth;
    public int currentHealth;
    public int regenHealth;
    #endregion

    #region Private Movement Variables
    public float moveSpeed;
    public float runSpeed;
    #endregion

    #region Public Combat Variables
    public float detectRange;
    public int attackDamage;
    public float attackSpeed;
    public float attackRange;
    #endregion

    

    // Start is called before the first frame update
    protected void Start()
    {
        animator  = GetComponent<Animator>();
        healthBar = GetComponent<EnemiesHealthBarManager>();
        objectLayer = GetComponent<GetEnemyLayer>();
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
        healthCanvas.enabled = false;
        StartCoroutine(HealthRegeneration());
    }

    protected virtual void Update()
    {
        if (currentHealth == maxHealth)
        {
            healthCanvas.enabled = false;
        }
        else if (currentHealth < maxHealth)
        {
            healthCanvas.enabled = true;
        }
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
                healthBar.UpdateHealthBar(currentHealth, maxHealth);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        if(objectLayer.LayerName() != "EnemyBase")
        {
            animator.SetTrigger("GetHit");
        }
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
        GetComponent<EnemiesAudioManager>().EnemiesSound("Death");
        if (enemyLayer == "MeleeFemale")
        {
            FindObjectOfType<LevelSystem>().GainXp(40);
        }
        else if (enemyLayer == "MeleeMale")
        {
            FindObjectOfType<LevelSystem>().GainXp(60);
        }
        else if (enemyLayer == "Range")
        {
            FindObjectOfType<LevelSystem>().GainXp(80);
        }
        else if (enemyLayer == "MeleeBoss")
        {
            FindObjectOfType<LevelSystem>().GainXp(400);
        }
        Destroy(gameObject, 2f);
    }

    protected void ShowFloatingText(int damage)
    {
        var hitObject = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity, floatingTextParent);
        hitObject.GetComponent<TextMesh>().text = damage.ToString();
    }

    protected void WalkSound()
    {
        GetComponent<EnemiesAudioManager>().EnemiesSound("Walk");
    }

    protected void RunSound()
    {
        GetComponent<EnemiesAudioManager>().EnemiesSound("Run");
    }

    protected Transform CheckClosestTarget(float detectRange)
    {
        Transform closestTarget = null;

        Collider[] colliders = Physics.OverlapSphere(transform.position, detectRange);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Player") || colliders[i].CompareTag("Allies"))
            {
                Transform target1 = colliders[i].transform;
                float distanceToTarget1 = Vector3.Distance(target1.position, transform.position);
                closestTarget = target1;

                // Check the next collider and compare the distance
                for (int j = 1; j < colliders.Length; j++)
                {
                    if (colliders[j].CompareTag("Player") || colliders[j].CompareTag("Allies"))
                    {
                        Transform target2 = colliders[j].transform;
                        float distanceToTarget2 = Vector3.Distance(target2.position, transform.position);
                        if (distanceToTarget1 > distanceToTarget2)
                        {
                            closestTarget = target2;
                        }
                    }
                }
            }
        }
        return closestTarget;
    }

    //private readonly Vector3 offSphere = new(0f, 0.8f, 0f);
    //// Draw a sphere to debug attack range in scene
    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, 20f);
    //}
}
