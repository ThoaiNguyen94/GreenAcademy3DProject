using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

[RequireComponent(typeof(GetEnemyLayer))]
public class EnemiesSaveLoadSystem : MonoBehaviour
{
    private EnemiesStatsSystem enemiesStatsSystem;
    private GetEnemyLayer enemyLayer;
    List<IDictionary> items = new List<IDictionary>();

    // Start is called before the first frame update
    void Start()
    {
        enemiesStatsSystem = GetComponent<EnemiesStatsSystem>();
        enemyLayer = GetComponent<GetEnemyLayer>();
        LoadData();
    }

    private void ReadData()
    {
        using(StreamReader reader = new StreamReader("Assets/Data/EnemiesStats.json"))
        {
            string item = reader.ReadToEnd();
            items = JsonConvert.DeserializeObject<List<IDictionary>>(item);
        }
    }

    private void LoadData()
    {
        ReadData();
        foreach (var item in items)
        {
            if ((string)item["enemyLayer"] == enemyLayer.LayerName())
            {
                // ID enemy
                enemiesStatsSystem.enemyLayer = item["enemyLayer"].ToString();

                // Survival stats
                enemiesStatsSystem.maxHealth     = int.Parse(item["maxHealth"].ToString());
                enemiesStatsSystem.currentHealth = int.Parse(item["currentHealth"].ToString());
                enemiesStatsSystem.regenHealth   = int.Parse(item["regenHealth"].ToString());

                // Movement stats
                enemiesStatsSystem.moveSpeed = float.Parse(item["moveSpeed"].ToString());
                enemiesStatsSystem.runSpeed  = float.Parse(item["runSpeed"].ToString());

                // Combat stats
                enemiesStatsSystem.detectRange  = float.Parse(item["detectRange"].ToString());
                enemiesStatsSystem.attackDamage = int.  Parse(item["attackDamage"].ToString());
                enemiesStatsSystem.attackSpeed  = float.Parse(item["attackSpeed"].ToString());
                enemiesStatsSystem.attackRange  = float.Parse(item["attackRange"].ToString());
            }
        }
    }
}
