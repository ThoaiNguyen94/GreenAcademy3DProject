using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileController : MonoBehaviour
{
    [SerializeField] private GameObject projectileExplosionPrefab;
    private EnemiesStatsSystem enemiesStats;
    private GameObject projectileExplosion;
    private Transform projectileParent;
    private readonly int speed = 30;
    private readonly int timeToDestroy = 1;

    public Vector3 target { get; set; }

    public bool hit { get; set; }

    private void OnEnable()
    {
        Destroy(gameObject, timeToDestroy);
    }

    // Start is called before the first frame update
    void Start()
    {
        projectileParent = transform.parent;
        enemiesStats = GetComponentInParent<EnemiesStatsSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime), Quaternion.LookRotation(target));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(collision.gameObject.GetComponent<PlayerStatsSystem>().currentHealth > 0)
            {
                collision.gameObject.GetComponent<PlayerStatsSystem>().TakeDamage(enemiesStats.attackDamage);
                projectileExplosion = Instantiate(projectileExplosionPrefab, transform.position, Quaternion.identity, projectileParent);
            }
        }
        else if (collision.gameObject.CompareTag("Allies"))
        {
            if(collision.gameObject.GetComponent<AlliesStatsSystem>().currentHealth > 0)
            {
                collision.gameObject.GetComponent<AlliesStatsSystem>().TakeDamage(enemiesStats.attackDamage);
                projectileExplosion = Instantiate(projectileExplosionPrefab, transform.position, Quaternion.identity, projectileParent);
            }
        }
        Destroy(gameObject);
        Destroy(projectileExplosion, 1f);
    }
}
