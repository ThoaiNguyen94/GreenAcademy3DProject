using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public CharacterDatabase characterDatabase;

    private GameObject characterPrefab;
    private int selectedOption = 0;

    private Transform playerTransform;

    // Start is called before the first frame update
    void Awake()
    {
        if (!PlayerPrefs.HasKey("selectedOption"))
        {
            selectedOption = 0;
        }
        else
        {
            LoadCharacter();
        }
        UpdateCharacter(selectedOption);
    }

    private void UpdateCharacter(int selectedOption)
    {
        Character character = characterDatabase.GetCharacter(selectedOption);
        characterPrefab = character.characterPrefab;
        SpawnPlayer(characterPrefab);
    }

    private void SpawnPlayer(GameObject player)
    {
        // Use for not save transform yet
        //PlayerPrefsManager.GetTransform("ExampleTransform", playerTransform);
        Instantiate(player, transform.position, transform.rotation);

        // Use for can save transform
        //PlayerStatsSystem playerStats = player.GetComponent<PlayerStatsSystem>();
        //int loadGameSceneID = PlayerPrefs.GetInt("LoadGameButton");
        //if (loadGameSceneID != 1)
        //{
        //    //LoadSavedData();
        //    Instantiate(player, playerStats.playerPosition, Quaternion.Euler(playerStats.playerPosition));
        //    //Debug.Log("Saved position: " + playerStats.playerPosition + "Saved rotationn: " + playerStats.playerPosition);
        //    Debug.Log("Saved position and rotaion");
        //}
        //else
        //{
        //    Instantiate(player, transform.position, transform.rotation);
        //    Debug.Log("Default position and rotation");
        //}

    }

    private void LoadCharacter()
    {
        selectedOption = PlayerPrefs.GetInt("selectedOption");
    }
}
