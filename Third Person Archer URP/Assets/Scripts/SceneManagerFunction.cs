using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerFunction : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private readonly float crossfaceTime = 1f;

    private void Start()
    {
        Time.timeScale = 1;
        if(SceneManager.GetActiveScene().name == "NewGameplay" || SceneManager.GetActiveScene().name == "TestingScene")
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
        
    }

    // Change scene by name
    public void ChangeSceneName(string sceneName)
    {
        StartCoroutine(LoadLevelByName(sceneName));
    }

    // Change scene by index
    public void ChangeSceneIndex(int sceneIndex)
    {
        StartCoroutine(LoadLevelByIndex(sceneIndex));
    }

    public void ChangeNextScene()
    {
        StartCoroutine(LoadLevelByIndex(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void ChangePreviousScene()
    {
        StartCoroutine(LoadLevelByIndex(SceneManager.GetActiveScene().buildIndex - 1));
    }

    public void ReloadScene()
    {
        StartCoroutine(LoadLevelByIndex(SceneManager.GetActiveScene().buildIndex));
    }

    public void QuitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }

    public void LevelLoader(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    private IEnumerator LoadAsynchronously (int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        while(!operation.isDone)
        {
            Debug.Log(operation.progress);
            yield return null;
        }
    }

    private IEnumerator LoadLevelByIndex(int lvIndex)
    {
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(crossfaceTime);
        SceneManager.LoadScene(lvIndex);
    }

    private IEnumerator LoadLevelByName(string lvName)
    {
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(crossfaceTime);
        SceneManager.LoadScene(lvName);
    }

    public void LoadNewGame(int buttonID)
    {
        PlayerPrefs.DeleteKey("LoadGameButton");
        PlayerPrefs.SetInt("LoadGameButton", buttonID);
        SceneManager.LoadScene("NewGameplay");
    }

    public void LoadCurrentGame(int buttonID)
    {
        PlayerPrefs.DeleteKey("LoadGameButton");
        PlayerPrefs.SetInt("LoadGameButton", buttonID);
        SceneManager.LoadScene("CurrentGameplay");
    }
}
