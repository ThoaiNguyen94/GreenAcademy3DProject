using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerArrowController : MonoBehaviour
{
    [SerializeField] private GameObject powerArrowHitPrefab;
    
    #region Private Variables
    private readonly int speed = 30;
    private readonly float timeToDestroy = 0.5f;
    private GameObject playerObject;
    private GameObject powerArrowHitVFX;
    private PowerShotAbility powerShotAbility;
    #endregion

    public Vector3 target { get; set; }

    public bool hit { get; set; }

    private void OnEnable()
    {
        Destroy(gameObject, timeToDestroy);
    }

    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        powerShotAbility = playerObject.GetComponent<PowerShotAbility>();
    }

    void Update()
    {
        transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime), Quaternion.LookRotation(powerShotAbility.ray.direction));

        if (Vector3.Distance(transform.position, target) < .01f)
        {
            Destroy(gameObject, timeToDestroy);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemies"))
        {
            PlayerStatsSystem playerStats = playerObject.GetComponent<PlayerStatsSystem>();
            WeaponSystem weaponStats = playerObject.GetComponent<WeaponSystem>();
            int totalDamage = playerStats.attackDamage + weaponStats.weaponDamage;
            collision.gameObject.GetComponent<EnemiesStatsSystem>().TakeDamage(totalDamage);
            powerArrowHitVFX = Instantiate(powerArrowHitPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject, timeToDestroy);
        Destroy(powerArrowHitVFX, 1f);
    }
}
