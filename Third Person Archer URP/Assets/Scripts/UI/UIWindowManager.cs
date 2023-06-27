using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIWindowManager : MonoBehaviour
{
    [SerializeField] private GameObject skillWindow;
    private bool isSkillWindowOpen = false;
    private readonly string skillWindowKey = "k";

    [SerializeField] private GameObject statsWindow;
    private bool isStatsWindowOpen = false;
    private readonly string statsWindowKey = "c";

    private bool isCursorLock = false;

    private SkillManager skillManager;
    private PlayerInput playerInput;


    // Start is called before the first frame update
    private void Start()
    {
        skillManager = FindObjectOfType<SkillManager>();
        playerInput = FindObjectOfType<PlayerInput>();
        Cursor.lockState = CursorLockMode.Locked;
        isSkillWindowOpen = false;
        isStatsWindowOpen = false;
        isCursorLock = false;
    }

    // Update is called once per frame
    private void Update()
    {
        ToggleSkillWindow();
        ToggleStatsWindow();
        ToggleCursor();
    }

    public void CloseWindow(GameObject window)
    {
        window.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        playerInput.enabled = true;
    }

    private void ToggleSkillWindow()
    {
        // Open window
        if (Input.GetKeyDown(skillWindowKey) && skillWindow.activeSelf == false && isSkillWindowOpen == false)
        {
            skillWindow.SetActive(true);
            isSkillWindowOpen = true;
            skillManager.DisplaySkillLevel();
            Cursor.lockState = CursorLockMode.None;
            playerInput.enabled = false; // Disable rotate camera when open UI window
        }
        // Close window
        else if (Input.GetKeyDown(skillWindowKey) && skillWindow.activeSelf == true && isSkillWindowOpen == true)
        {
            skillWindow.SetActive(false);
            isSkillWindowOpen = false;
            Cursor.lockState = CursorLockMode.Locked;
            playerInput.enabled = true;
        }
    }

    private void ToggleStatsWindow()
    {
        // Open window
        if (Input.GetKeyDown(statsWindowKey) && isStatsWindowOpen == false)
        {
            statsWindow.SetActive(true);
            isStatsWindowOpen = true;
            statsWindow.GetComponent<UpdateCharStats>().UpdateCharacterStats();
            Cursor.lockState = CursorLockMode.None;
            playerInput.enabled = false; // Disable rotate camera when open UI window
        }
        // Close window
        else if (Input.GetKeyDown(statsWindowKey) && isStatsWindowOpen == true)
        {
            statsWindow.SetActive(false);
            isStatsWindowOpen = false;
            Cursor.lockState = CursorLockMode.Locked;
            playerInput.enabled = true;
        }
    }

    private void ToggleCursor()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && isCursorLock == false)
        {
            // Unlock cursor
            isCursorLock = true;
            Cursor.lockState = CursorLockMode.None;
            playerInput.enabled = false;
        }
        else if(Input.GetKeyDown(KeyCode.LeftControl) && isCursorLock == true)
        {
            // Lock cursor
            isCursorLock = false;
            Cursor.lockState = CursorLockMode.Locked;
            playerInput.enabled = true;
        }
    }

    public void ToggleSkillWindowByKey()
    {
        // Open window
        if (skillWindow.activeSelf == false && isSkillWindowOpen == false)
        {
            skillWindow.SetActive(true);
            isSkillWindowOpen = true;
            skillManager.DisplaySkillLevel();
            Cursor.lockState = CursorLockMode.None;
            playerInput.enabled = false; // Disable rotate camera when open UI window
        }
        // Close window
        else if (skillWindow.activeSelf == true && isSkillWindowOpen == true)
        {
            skillWindow.SetActive(false);
            isSkillWindowOpen = false;
            Cursor.lockState = CursorLockMode.Locked;
            playerInput.enabled = true;
        }
    }

    public void ToggleStatsWindowByKey()
    {
        // Open window
        if (statsWindow.activeSelf == false && isStatsWindowOpen == false)
        {
            statsWindow.SetActive(true);
            isStatsWindowOpen = true;
            statsWindow.GetComponent<UpdateCharStats>().UpdateCharacterStats();
            Cursor.lockState = CursorLockMode.None;
            playerInput.enabled = false; // Disable rotate camera when open UI window
        }
        // Close window
        else if (statsWindow.activeSelf == true && isStatsWindowOpen == true)
        {
            statsWindow.SetActive(false);
            isStatsWindowOpen = false;
            Cursor.lockState = CursorLockMode.Locked;
            playerInput.enabled = true;
        }
    }

}
