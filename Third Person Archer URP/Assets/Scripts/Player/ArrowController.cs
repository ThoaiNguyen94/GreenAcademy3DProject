using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private GameObject bloodSprayPrefab;

    #region Private Variables
    private readonly int speed = 30;
    private readonly float timeToDestroy = 0.5f;
    private GameObject playerObject;
    private GameObject bloodSpray;
    private AimBow aimBow;
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
        aimBow = playerObject.GetComponent<AimBow>();
    }

    void Update()
    {
        transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime), Quaternion.LookRotation(aimBow.ray.direction));

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
            int totalDamage = playerStats.attackDamage + playerStats.weaponDamage;
            collision.gameObject.GetComponent<EnemiesStatsSystem>().TakeDamage(totalDamage);
            bloodSpray = Instantiate(bloodSprayPrefab, target, Quaternion.identity);
        }
        Destroy(gameObject, timeToDestroy); // Have time to destroy the arrow fly throught the object
        Destroy(bloodSpray, timeToDestroy);
    }
}
