using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
//using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerStatsSystem))]
public class PlayerSaveLoadSystem : MonoBehaviour
{
    //private readonly string newDataPath   = "Assets/Data/NewGamePlayerStats.json";
    //private readonly string savedDataPath = "Assets/Data/LoadGamePlayerStats.json";

    // Get the current path to this file
    private readonly string newDataPath = Directory.GetCurrentDirectory() + "\\Assets\\Data\\NewGamePlayerStats.json";
    private readonly string savedDataPath = Directory.GetCurrentDirectory() + "\\Assets\\Data\\LoadGamePlayerStats.json";

    private PlayerStatsSystem playerStatsSystem;
    private GetEnemyLayer playerLayer;
    List<IDictionary> items = new();

    // Start is called before the first frame update
    void Awake()
    {
        playerStatsSystem = GetComponent<PlayerStatsSystem>();
        playerLayer = GetComponent<GetEnemyLayer>();

        // Use for testing scene
        //LoadNewData();

        // Use for gameplay
        int loadGameSceneID = PlayerPrefs.GetInt("LoadGameButton");
        if (loadGameSceneID != 1)
        {
            LoadSavedData();
        }
        else
        {
            LoadNewData();
        }

        // Use for 3 maps in the game
        //if (SceneManager.GetActiveScene().name == "NewGameplay" || SceneManager.GetActiveScene().name == "TestingScene")
        //{
        //    LoadNewData();
        //}
        //else
        //{
        //    LoadSavedData();
        //}
    }

    private bool CheckPathIsExist(string path)
    {
        if (File.Exists(path))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ReadNewData()
    {
        if (CheckPathIsExist(newDataPath))
        {
            using (StreamReader reader = new(newDataPath))
            {
                string item = reader.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<IDictionary>>(item);
            }
        }
        else
        {
            Debug.Log("File " + newDataPath + " does not existed");
        }
    }

    public void ReadSavedData()
    {
        if (CheckPathIsExist(savedDataPath))
        {
            using (StreamReader reader = new(savedDataPath))
            {
                string item = reader.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<IDictionary>>(item);
            }
        }
        else
        {
            Debug.Log("File " + savedDataPath + " does not existed");
        }
    }

    public void WriteData()
    {
        foreach (var item in items)
        {
            // Player character
            item["playerLayer"] = playerStatsSystem.playerLayer;

            // Movement stats
            item["moveSpeed"] = playerStatsSystem.moveSpeed.ToString();
            item["runSpeed"] = playerStatsSystem.runSpeed.ToString();
            item["rotationSpeed"] = playerStatsSystem.rotationSpeed.ToString();
            item["jumpForce"] = playerStatsSystem.jumpForce.ToString();

            // Survival stats
            item["maxHealth"] = playerStatsSystem.maxHealth.ToString();
            item["currentHealth"] = playerStatsSystem.currentHealth.ToString();
            item["regenHealth"] = playerStatsSystem.regenHealth.ToString();
            item["maxMana"] = playerStatsSystem.maxMana.ToString();
            item["currentMana"] = playerStatsSystem.currentMana.ToString();
            item["regenMana"] = playerStatsSystem.regenMana.ToString();

            // Combat stats
            item["attackDamage"] = playerStatsSystem.attackDamage.ToString();
            item["attackRange"] = playerStatsSystem.attackRange.ToString();

            // Level and Skill Variables
            item["level"] = playerStatsSystem.level.ToString();
            item["skillPoint"] = playerStatsSystem.skillPoint.ToString();
            item["currentExp"] = playerStatsSystem.currentExp.ToString();

            // Transform position and rotation
            //item["playerPositionX"] = playerStatsSystem.playerPosition.x;
            //item["playerPositionY"] = playerStatsSystem.playerPosition.y;
            //item["playerPositionZ"] = playerStatsSystem.playerPosition.z;
            //item["playerRotationX"] = playerStatsSystem.playerRotation.x;
            //item["playerRotationY"] = playerStatsSystem.playerRotation.y;
            //item["playerRotationZ"] = playerStatsSystem.playerRotation.z;

        }
        //Vector3 playerPosition = playerStatsSystem.playerPosition;
        //Vector3Data vectorData = new(playerPosition);
        string updatedJsonString = JsonConvert.SerializeObject(items, Formatting.Indented);
        File.WriteAllText(savedDataPath, updatedJsonString);
    }

    public void LoadNewData()
    {
        ReadNewData();
        foreach (var item in items)
        {
            if ((string)item["playerLayer"] == playerLayer.LayerName())
            {
                // Player character
                playerStatsSystem.playerLayer = item["playerLayer"].ToString();

                // Movement stats
                playerStatsSystem.moveSpeed = float.Parse(item["moveSpeed"].ToString());
                playerStatsSystem.runSpeed = int.Parse(item["runSpeed"].ToString());
                playerStatsSystem.rotationSpeed = int.Parse(item["rotationSpeed"].ToString());
                playerStatsSystem.jumpForce = int.Parse(item["jumpForce"].ToString());

                // Survival stats
                playerStatsSystem.maxHealth = int.Parse(item["maxHealth"].ToString());
                playerStatsSystem.currentHealth = int.Parse(item["currentHealth"].ToString());
                playerStatsSystem.regenHealth = int.Parse(item["regenHealth"].ToString());
                playerStatsSystem.maxMana = int.Parse(item["maxMana"].ToString());
                playerStatsSystem.currentMana = int.Parse(item["currentMana"].ToString());
                playerStatsSystem.regenMana = int.Parse(item["regenMana"].ToString());

                // Combat stats
                playerStatsSystem.attackDamage = int.Parse(item["attackDamage"].ToString());
                playerStatsSystem.attackRange = float.Parse(item["attackRange"].ToString());

                // Level and Skill Variables
                playerStatsSystem.level = int.Parse(item["level"].ToString());
                playerStatsSystem.skillPoint = int.Parse(item["skillPoint"].ToString());
                playerStatsSystem.currentExp = float.Parse(item["currentExp"].ToString());

                // Transform position and rotation
                playerStatsSystem.playerPosition.x = float.Parse(item["playerPositionX"].ToString());
                playerStatsSystem.playerPosition.y = float.Parse(item["playerPositionY"].ToString());
                playerStatsSystem.playerPosition.z = float.Parse(item["playerPositionZ"].ToString());
                playerStatsSystem.playerRotation.x = float.Parse(item["playerRotationX"].ToString());
                playerStatsSystem.playerRotation.y = float.Parse(item["playerRotationY"].ToString());
                playerStatsSystem.playerRotation.z = float.Parse(item["playerRotationZ"].ToString());
            }
        }
    }

    public void LoadSavedData()
    {
        ReadSavedData();
        foreach (var item in items)
        {
            if ((string)item["playerLayer"] == playerLayer.LayerName())
            {
                // Player character
                playerStatsSystem.playerLayer = item["playerLayer"].ToString();

                // Movement stats
                playerStatsSystem.moveSpeed = float.Parse(item["moveSpeed"].ToString());
                playerStatsSystem.runSpeed = int.Parse(item["runSpeed"].ToString());
                playerStatsSystem.rotationSpeed = int.Parse(item["rotationSpeed"].ToString());
                playerStatsSystem.jumpForce = int.Parse(item["jumpForce"].ToString());

                // Survival stats
                playerStatsSystem.maxHealth = int.Parse(item["maxHealth"].ToString());
                playerStatsSystem.currentHealth = int.Parse(item["currentHealth"].ToString());
                playerStatsSystem.regenHealth = int.Parse(item["regenHealth"].ToString());
                playerStatsSystem.maxMana = int.Parse(item["maxMana"].ToString());
                playerStatsSystem.currentMana = int.Parse(item["currentMana"].ToString());
                playerStatsSystem.regenMana = int.Parse(item["regenMana"].ToString());

                // Combat stats
                playerStatsSystem.attackDamage = int.Parse(item["attackDamage"].ToString());
                playerStatsSystem.attackRange = float.Parse(item["attackRange"].ToString());

                // Level and Skill Variables
                playerStatsSystem.level = int.Parse(item["level"].ToString());
                playerStatsSystem.skillPoint = int.Parse(item["skillPoint"].ToString());
                playerStatsSystem.currentExp = float.Parse(item["currentExp"].ToString());

                // Transform position and rotation
                playerStatsSystem.playerPosition.x = float.Parse(item["playerPositionX"].ToString());
                playerStatsSystem.playerPosition.y = float.Parse(item["playerPositionY"].ToString());
                playerStatsSystem.playerPosition.z = float.Parse(item["playerPositionZ"].ToString());
                playerStatsSystem.playerRotation.x = float.Parse(item["playerRotationX"].ToString());
                playerStatsSystem.playerRotation.y = float.Parse(item["playerRotationY"].ToString());
                playerStatsSystem.playerRotation.z = float.Parse(item["playerRotationZ"].ToString());
            }
        }
    }
}
