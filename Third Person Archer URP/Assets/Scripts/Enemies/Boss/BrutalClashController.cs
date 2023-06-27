using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BrutalClashController : MonoBehaviour
{
    [SerializeField] private GameObject brutalClashVFX;
    [SerializeField] private Transform  brutalClashPoint;

    #region Private Variables
    private MeleeBossStatsSystem meleeBossStats;
    private NavMeshAgent         agent;
    private GameObject           brutalClashHit;
    private Animator             animator;
    private readonly int         attackBuff = 2;
    private readonly float       skillRange = 3;
    private readonly Vector3     offsetSphere = new(0f, 1.7f, 0f);
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
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("BrutalClashState"))
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }
    }

    private void BrutalClashSpawnVFX()
    {
        Quaternion rotation = Quaternion.Euler(transform.rotation.eulerAngles.x - 60, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - 20);
        Vector3 offsetPosition = new(0f, -2f, 0f);
        brutalClashHit = Instantiate(brutalClashVFX, brutalClashPoint.position + offsetPosition, rotation);
        GetComponent<EnemiesAudioManager>().EnemiesSound("BrutalClash");
        Destroy(brutalClashHit, 1f);
    }

    private void BrutalClashHit()
    {
        Vector3 spherePosition = (transform.position + offsetSphere) + (transform.forward * (skillRange / 2));
        StartCoroutine(DisableMovement());
        int totalDamage = meleeBossStats.attackDamage * attackBuff;
        Collider[] colliders = Physics.OverlapSphere(spherePosition, skillRange / 2);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player") && canAttack)
            {
                if(collider.GetComponent<PlayerStatsSystem>().currentHealth > 0)
                {
                    collider.GetComponent<PlayerStatsSystem>().TakeDamage(totalDamage);
                }
            }
            if (collider.CompareTag("Allies") && canAttack)
            {
                if(collider.GetComponent<AlliesStatsSystem>().currentHealth > 0)
                {
                    collider.GetComponent<AlliesStatsSystem>().TakeDamage(totalDamage);
                }
            }
        }
    }

    private IEnumerator DisableMovement()
    {
        agent.enabled = false;
        yield return new WaitForSeconds(1);
        agent.enabled = true;
    }

    
    // Draw a sphere to debug attack range
    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere((transform.position + offsetSphere) + (transform.forward * (skillRange / 2)), skillRange / 2);
    //}

}
