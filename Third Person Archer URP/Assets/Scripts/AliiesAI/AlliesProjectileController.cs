using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlliesProjectileController : MonoBehaviour
{
    [SerializeField] private GameObject projectileExplosionPrefab;
    private AlliesStatsSystem alliesStats;
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
        alliesStats = GetComponentInParent<AlliesStatsSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime), Quaternion.LookRotation(target));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemies"))
        {
            collision.gameObject.GetComponent<EnemiesStatsSystem>().TakeDamage(alliesStats.attackDamage);
            projectileExplosion = Instantiate(projectileExplosionPrefab, transform.position, Quaternion.identity, projectileParent);
        }
        Destroy(gameObject);
        Destroy(projectileExplosion, 1f);
    }
}
