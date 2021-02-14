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
            if (_currentState == GameState.InMenu)
            {
                StartCoroutine(ShowMainMenu(0));   
            }

            if (_playableDirector != null)
            {
                _playableDirector.Play();
            }
            
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainScene" && _playableDirector.state == PlayState.Playing && Input.GetKeyDown(KeyCode.Escape))
        {
            SkipIntro();
        }
    }

    private void SkipIntro()
    {
        _playableDirector.time = _playableDirector.duration;
        _playableDirector.Play();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Loading");

        StartCoroutine(AsyncLoad());
    }

    IEnumerator AsyncLoad()
    {
        AsyncOperation levelLoad = SceneManager.LoadSceneAsync("MainScene");

        while (!levelLoad.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
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

        SceneManager.LoadScene("Screens");
    }
    
    public void RestartGame()
    {
        Debug.Log("Restart pressed");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitToMainMenu()
    {
        //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);

        DisableAllScreens();
        
        //mainMenuPanel.SetActive(true);

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
