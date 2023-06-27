using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemiesSystem : MonoBehaviour
{
    #region Serialize Field Private Variables
    [SerializeField] private GameObject enemyPrefab1;
    [SerializeField] private GameObject enemyPrefab2;
    [SerializeField] private GameObject enemyPrefab3;
    [SerializeField] private GameObject enemyPrefab4;
    [SerializeField] private Transform  alliesDropPosition;
    #endregion

    #region Private Variables
    private int xPosRandom;
    private int zPosRandom;
    private int enemyType;
    private GameObject[] enemiesCount;
    #endregion

    #region Read Only Private Variables
    private readonly int quantityToStopGenerate = 11;
    private readonly float timeSpawnUnder16 = 4f;
    private readonly float timeSpawnUnder31 = 6f;
    private readonly float timeCheckQuantity = 1f;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckObjectsQuantity(timeCheckQuantity));
        StartCoroutine(EnemiesDrop());
    }

    private IEnumerator EnemiesDrop()
    {
        while(true)
        {
            xPosRandom = Random.Range(1, 5);
            zPosRandom = Random.Range(1, 5);
            Vector3 offset = new(alliesDropPosition.position.x + xPosRandom, alliesDropPosition.position.y, alliesDropPosition.position.z + zPosRandom);
            
            if (Time.time > 29)
            {
                if (enemiesCount.Length < 16)
                {
                    enemyType = Random.Range(1, 5);
                    if (enemyType == 1)
                    {
                        Instantiate(enemyPrefab1, offset, Quaternion.identity);
                    }
                    else if (enemyType == 2)
                    {
                        Instantiate(enemyPrefab2, offset, Quaternion.identity);
                    }
                    else if (enemyType == 3)
                    {
                        Instantiate(enemyPrefab3, offset, Quaternion.identity);
                    }
                    else if (enemyType == 4)
                    {
                        int bossSpawnChange = Random.Range(1, 101);
                        if (bossSpawnChange <= 51)
                        {
                            Instantiate(enemyPrefab4, offset, Quaternion.identity);
                        }
                    }
                    yield return new WaitForSeconds(timeSpawnUnder16);
                }
                else if (enemiesCount.Length < 31)
                {
                    enemyType = Random.Range(1, 5);
                    if (enemyType == 1)
                    {
                        Instantiate(enemyPrefab1, offset, Quaternion.identity);
                    }
                    else if (enemyType == 2)
                    {
                        Instantiate(enemyPrefab2, offset, Quaternion.identity);
                    }
                    else if (enemyType == 3)
                    {
                        Instantiate(enemyPrefab3, offset, Quaternion.identity);
                    }
                    else if (enemyType == 4)
                    {
                        int bossSpawnChange = Random.Range(1, 101);
                        if (bossSpawnChange <= 51)
                        {
                            Instantiate(enemyPrefab4, offset, Quaternion.identity);
                        }
                    }
                    yield return new WaitForSeconds(timeSpawnUnder31);
                }
            }
            else
            {
                if (enemiesCount.Length < 16)
                {
                    enemyType = Random.Range(1, 5);
                    if (enemyType == 1)
                    {
                        Instantiate(enemyPrefab1, offset, Quaternion.identity);
                    }
                    else if (enemyType == 2)
                    {
                        Instantiate(enemyPrefab2, offset, Quaternion.identity);
                    }
                    else if (enemyType == 3)
                    {
                        Instantiate(enemyPrefab3, offset, Quaternion.identity);
                    }
                    yield return new WaitForSeconds(timeSpawnUnder16);
                }
                else if (enemiesCount.Length < 31)
                {
                    enemyType = Random.Range(1, 5);
                    if (enemyType == 1)
                    {
                        Instantiate(enemyPrefab1, offset, Quaternion.identity);
                    }
                    else if (enemyType == 2)
                    {
                        Instantiate(enemyPrefab2, offset, Quaternion.identity);
                    }
                    else if (enemyType == 3)
                    {
                        Instantiate(enemyPrefab3, offset, Quaternion.identity);
                    }
                    yield return new WaitForSeconds(timeSpawnUnder31);
                }
            }

            // Check the enemies if more than 10 stop generate and wait to under 30 countinue generate
            if (enemiesCount.Length >= quantityToStopGenerate)
            {
                while (enemiesCount.Length >= quantityToStopGenerate)
                {
                    yield return null;
                }
            }
        }
    }

    private IEnumerator CheckObjectsQuantity(float time)
    {
        while (true)
        {
            enemiesCount = GameObject.FindGameObjectsWithTag("Enemies");
            yield return new WaitForSeconds(time);
        }
    }

}
