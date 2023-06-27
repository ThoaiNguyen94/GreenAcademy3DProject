using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JumpAttackController : MonoBehaviour
{
    [SerializeField] private GameObject jumpAttackVFX;

    #region Private Variables
    private MeleeBossStatsSystem meleeBossStats;
    private NavMeshAgent         agent;
    private GameObject           jumpAttackHit;
    private Animator             animator;
    private readonly int         attackBuff = 2;
    private bool                 canAttack = false;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        meleeBossStats = GetComponentInParent<MeleeBossStatsSystem>();
        animator       = GetComponent<Animator>();
        agent          = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("JumpAttackState"))
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }
    }

    private void CheckJumpAttack()
    {
        StartCoroutine(DisableMovement());
        int totalDamage = meleeBossStats.attackDamage * attackBuff;
        Collider[] colliders = Physics.OverlapSphere(transform.position, 6f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player") && canAttack)
            {
                if (collider.GetComponent<PlayerStatsSystem>().currentHealth > 0)
                {
                    collider.GetComponent<PlayerStatsSystem>().TakeDamage(totalDamage);
                }
            }
            else if (collider.CompareTag("Allies") && canAttack)
            {
                if (collider.GetComponent<AlliesStatsSystem>().currentHealth > 0)
                {
                    collider.GetComponent<AlliesStatsSystem>().TakeDamage(totalDamage);
                }
            }
        }
        GetComponent<EnemiesAudioManager>().EnemiesSound("JumpAttack");
        jumpAttackHit = Instantiate(jumpAttackVFX, transform.position, Quaternion.identity);
        Destroy(jumpAttackHit, 1f);
    }

    private IEnumerator DisableMovement()
    {
        agent.enabled = false;
        yield return new WaitForSeconds(1);
        agent.enabled = true;
    }
}
