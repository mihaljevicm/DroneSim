using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void OnGameStart();
    public delegate void OnGameQuit();
    public delegate void OnContinue();

    public static event OnGameStart onGameStart;
    public static event OnGameQuit onGameQuit;
    public static event OnContinue onContinue;

    public void StartGame()
    {
        onGameStart();
    }

    public void QuitGame()
    {
        onGameQuit();
    }

    public void ContinueMenu()
    {
        onContinue();
    }
}
