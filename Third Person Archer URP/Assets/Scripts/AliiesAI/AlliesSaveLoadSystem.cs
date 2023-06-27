using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(GetEnemyLayer))]
public class AlliesSaveLoadSystem : MonoBehaviour
{
    private AlliesStatsSystem alliesStatsSystem;
    private GetEnemyLayer alliesLayer;
    List<IDictionary> items = new List<IDictionary>();

    // Start is called before the first frame update
    void Start()
    {
        alliesStatsSystem = GetComponent<AlliesStatsSystem>();
        alliesLayer = GetComponent<GetEnemyLayer>();
        LoadData();
    }

    private void ReadData()
    {
        using (StreamReader reader = new StreamReader("Assets/Data/AlliesStats.json"))
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
            if ((string)item["alliesLayer"] == alliesLayer.LayerName())
            {
                // ID enemy
                alliesStatsSystem.alliesLayer = item["alliesLayer"].ToString();

                // Survival stats
                alliesStatsSystem.maxHealth     = int.Parse(item["maxHealth"].ToString());
                alliesStatsSystem.currentHealth = int.Parse(item["currentHealth"].ToString());
                alliesStatsSystem.regenHealth   = int.Parse(item["regenHealth"].ToString());

                // Movement stats
                alliesStatsSystem.moveSpeed = float.Parse(item["moveSpeed"].ToString());
                alliesStatsSystem.runSpeed  = float.Parse(item["runSpeed"].ToString());

                // Combat stats
                alliesStatsSystem.detectRange  = float.Parse(item["detectRange"].ToString());
                alliesStatsSystem.attackDamage = int.Parse(item["attackDamage"].ToString());
                alliesStatsSystem.attackSpeed  = float.Parse(item["attackSpeed"].ToString());
                alliesStatsSystem.attackRange  = float.Parse(item["attackRange"].ToString());
            }
        }
    }
}
