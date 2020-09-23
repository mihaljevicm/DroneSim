using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public GameObject panel; 

    private void OnEnable()
    {

        EventManager.onGameStart += OnGameStart;
        EventManager.onGameQuit += OnGameQuitDialogue;
        EventManager.onContinue += ContinueMenu;

    }

    public void OnGameStart()
    {
        SceneManager.LoadScene("FlyScene");
    }

    public void OnGameQuitDialogue()
    {
        panel.SetActive(true);
        EventManager.onGameQuit += QuitGame;
        EventManager.onGameQuit -= OnGameQuitDialogue;

    }

    public void ContinueMenu()
    {
        panel.SetActive(false);
        EventManager.onGameQuit += OnGameQuitDialogue;
    }

    public void QuitGame()
    {
        Debug.Log("quitting");
        Application.Quit();
    }
}
