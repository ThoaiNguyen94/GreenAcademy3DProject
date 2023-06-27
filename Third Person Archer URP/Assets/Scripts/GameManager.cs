using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text tiltleText;

    #region Game Window Variables
    [SerializeField] private GameObject gameWindow;
    private bool isGameWindowOpen = false;
    #endregion

    #region Pause Window Variables
    [SerializeField] private GameObject pauseWindow;
    private bool isPauseWindowOpen = false;
    #endregion

    #region Cutscene Variables
    // Cutscene when end game
    private GameObject playerCanvas;
    [SerializeField] private PlayableDirector playerBasePD;
    [SerializeField] private Camera playerBaseCam;
    [SerializeField] private PlayableDirector enemyBasePD;
    [SerializeField] private Camera enemyBaseCam;
    #endregion

    private PlayerStatsSystem playerStats;
    private AlliesStatsSystem alliesStats;
    private EnemiesStatsSystem enemiesStats;
    private PlayerInput playerInput;
    private PlayerSaveLoadSystem playerSaveLoadSystem;

    // Start is called before the first frame update
    void Start()
    {
        // Set game window false
        gameWindow.SetActive(false);
        isGameWindowOpen = false;

        // Set pause window false
        pauseWindow.SetActive(false);
        isPauseWindowOpen = false;

        playerStats  = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsSystem>();
        alliesStats  = FindObjectOfType<GenerateAlliesSystem>().GetComponent<AlliesStatsSystem>();
        enemiesStats = FindObjectOfType<GenerateEnemiesSystem>().GetComponent<EnemiesStatsSystem>();
        playerInput  = FindObjectOfType<PlayerInput>();
        playerSaveLoadSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSaveLoadSystem>();

        playerCanvas = GameObject.FindGameObjectWithTag("PlayerCanvas");
        playerBasePD.enabled = false;
        playerBaseCam.enabled = false;
        enemyBasePD.enabled = false;
        enemyBaseCam.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Minus)) // Lose game
        {
            playerStats.currentHealth -= 1000000;
        }
        if (Input.GetKeyDown(KeyCode.P)) // Lose game
        {
            alliesStats.currentHealth -= 1000000;
        }
        if (Input.GetKeyDown(KeyCode.O)) // Win game
        {
            enemiesStats.currentHealth -= 1000000;
        }
        CheckEndGame();
        CheckWinGame();
        TogglePauseWindow();
    }

    private void CheckEndGame()
    {
        if (playerStats.currentHealth <= 0 && isGameWindowOpen  == false && isPauseWindowOpen == false)
        {
            isGameWindowOpen = true;
            gameWindow.SetActive(true);
            tiltleText.text = "Defeated";
            Cursor.lockState = CursorLockMode.None;
            playerInput.enabled = false;
            playerCanvas.SetActive(false);
            //Time.timeScale = 0;
        }
        else if (alliesStats.currentHealth <= 0 && isGameWindowOpen == false && isPauseWindowOpen == false)
        {
            isGameWindowOpen = true;
            gameWindow.SetActive(true);
            tiltleText.text = "Defeated";
            Cursor.lockState = CursorLockMode.None;
            playerInput.enabled = false;
            playerBasePD.enabled = true;
            playerCanvas.SetActive(false);
            playerBaseCam.enabled = true;
            playerBasePD.Play();
        }
    }

    private void CheckWinGame()
    {
        if (playerStats.currentHealth > 0 && enemiesStats.currentHealth <= 0 && isGameWindowOpen  == false && isPauseWindowOpen == false)
        {
            isGameWindowOpen = true;
            gameWindow.SetActive(true);
            tiltleText.text = "Victory";
            Cursor.lockState = CursorLockMode.None;
            playerInput.enabled = false;
            enemyBasePD.enabled = true;
            playerCanvas.SetActive(false);
            enemyBaseCam.enabled = true;
            enemyBasePD.Play();
            //Time.timeScale = 0;
            playerSaveLoadSystem.WriteData();
        }
    }

    private void TogglePauseWindow()
    {
        // Open pause window
        if (Input.GetKeyDown(KeyCode.Escape) && pauseWindow.activeSelf == false && isPauseWindowOpen == false && isGameWindowOpen == false)
        {
            pauseWindow.SetActive(true);
            isPauseWindowOpen = true;
            Cursor.lockState = CursorLockMode.None;
            playerInput.enabled = false;
            Time.timeScale = 0;
        }
        // Close pause window
        else if (Input.GetKeyDown(KeyCode.Escape) && pauseWindow.activeSelf == true && isPauseWindowOpen == true && isGameWindowOpen == false)
        {
            pauseWindow.SetActive(false);
            isPauseWindowOpen = false;
            Cursor.lockState = CursorLockMode.Locked;
            playerInput.enabled = true;
            Time.timeScale = 1;
        }
    }

    public void ClosePauseWindow()
    {
        pauseWindow.SetActive(false);
        isPauseWindowOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerInput.enabled = true;
        Time.timeScale = 1;
    }

    public void CloseWindow(GameObject window)
    {
        window.SetActive(false);
    }

    public void SaveGameplay(GameObject window)
    {
        
        playerSaveLoadSystem.WriteData();
        window.SetActive(true);
    }

}
