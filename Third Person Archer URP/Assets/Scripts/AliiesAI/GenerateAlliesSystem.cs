using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateAlliesSystem : MonoBehaviour
{
    #region Serialize Field Private Variables
    [SerializeField] private GameObject alliesPrefab1;
    [SerializeField] private GameObject alliesPrefab2;
    [SerializeField] private Transform  alliesDropPosition;
    #endregion

    #region Private Variables
    private int xPosRandom;
    private int zPosRandom;
    private int alliesType;
    private GameObject[] alliesCount;
    #endregion

    #region Read Only Private Variables
    private readonly int quantityToStopGenerate = 7;
    private readonly float timeSpawn = 4f;
    private readonly float timeCheckQuantity = 1f;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckObjectsQuantity(timeCheckQuantity));
        StartCoroutine(AlliesDrop());
    }

    private IEnumerator AlliesDrop()
    {
        while (true)
        {
            xPosRandom = Random.Range(1, 5);
            zPosRandom = Random.Range(1, 5);
            Vector3 offset = new(alliesDropPosition.position.x + xPosRandom, alliesDropPosition.position.y, alliesDropPosition.position.z + zPosRandom);
            if (alliesCount.Length < 16)
            {
                alliesType = Random.Range(1, 3);
                if (alliesType == 1)
                {
                    Instantiate(alliesPrefab1, offset, Quaternion.identity);
                }
                else if (alliesType == 2)
                {
                    Instantiate(alliesPrefab2, offset, Quaternion.identity);
                }
                yield return new WaitForSeconds(timeSpawn);
            }

            // Check the allies if more than 10 stop generate and wait to under 30 countinue generate
            if (alliesCount.Length >= quantityToStopGenerate)
            {
                while (alliesCount.Length >= quantityToStopGenerate)
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
            alliesCount = GameObject.FindGameObjectsWithTag("Allies");
            yield return new WaitForSeconds(time);
        }
    }
}
