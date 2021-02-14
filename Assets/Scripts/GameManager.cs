using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton config
    private static GameManager _instance;

    public static GameManager Instance => _instance;
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
    
    // End singleton config

    private const string HAS_SEEN_INTRO_KEY = "hasSeenIntro";
    
    public enum GameState
    {
        InGame,
        GameOver,
        Loading,
        Paused,
        InMenu
    }
    
    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }

    [SerializeField] private PlayableDirector _playableDirector;

    private GameState _currentState = GameState.Loading;

    public GameState CurrentState
    {
        get => _currentState;
        set => _currentState = value;
    }

    [SerializeField] private GameObject mainMenuPanel;
    
    [SerializeField] private GameObject creditsPanel;
    
    [SerializeField] private GameObject confirmModalPanel;

    

    // Start is called before the first frame update
    void Start()
    {
        //SceneManager.LoadScene("Screens");

        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            int hasSeenIntro = PlayerPrefs.GetInt(HAS_SEEN_INTRO_KEY, 0);
        
            if (hasSeenIntro == 0)
            {
                _playableDirector.Play();
            
                PlayerPrefs.SetInt(HAS_SEEN_INTRO_KEY, 1);
            }
            else
            {
                _playableDirector.time = _playableDirector.duration;
                _playableDirector.Play();
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
        DisableAllScreens();
    }

    private void DisableAllScreens()
    {
        mainMenuPanel.SetActive(false);
        
        creditsPanel.SetActive(false);
        
        confirmModalPanel.SetActive(false);
    }

    public void GameOver()
    {
        //StartCoroutine(WaitSeconds(5));
        
        DisableAllScreens();
        
        confirmModalPanel.SetActive(true);
    }

    private IEnumerator ShowMainMenu(float secondsToWait)
    {
        yield return new WaitForSeconds(secondsToWait);
        
        DisableAllScreens();

        SceneManager.LoadScene("Screens");
    }
    
    public void RestartGame()
    {
        Debug.Log("Restart pressed");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitToMainMenu()
    {
        PlayerPrefs.SetInt(HAS_SEEN_INTRO_KEY, 0);
        
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);

        DisableAllScreens();
        
        mainMenuPanel.SetActive(true);

        StartCoroutine(ShowMainMenu(2));
    }

    public void ShowCredits()
    {
        DisableAllScreens();
        
        creditsPanel.SetActive(true);

        StartCoroutine(ShowMainMenu(5));
    }

    public void OnCloseGame()
    {
        Application.Quit();
    }
}
