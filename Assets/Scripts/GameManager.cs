using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        InMenu,
        PlayingCinematic
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

    [SerializeField] private GameObject pausePanel;

    private AudioSource[] _audioSources;

    // Start is called before the first frame update
    void Start()
    {
        //SceneManager.LoadScene("Screens");
        

        _audioSources = FindObjectsOfType<AudioSource>().Where(source => source.gameObject.CompareTag("Event Sound") == false).ToArray();

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name == "MainScene")
            {
                if (_playableDirector.state == PlayState.Playing)
                {
                    SkipIntro();

                    _currentState = GameState.InGame;
                }
                else
                {
                    if (_currentState == GameState.InGame)
                    {
                        TogglePause(GameState.Paused);
                    }
                    else
                    {
                        TogglePause(GameState.InGame);   
                    }
                }
            }
        }
        
    }

    private void SkipIntro()
    {
        _playableDirector.time = _playableDirector.duration - 0.1;
        _playableDirector.Play();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Loading");

        _currentState = GameState.InGame;

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
        
        pausePanel.SetActive(false);
    }

    public void GameOver()
    {
        DisableAllScreens();

        _currentState = GameState.GameOver;
        
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
        DisableAllScreens();

        _currentState = GameState.InMenu;

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

    public void ToggleAudio(bool pause)
    {
        foreach (AudioSource audio in _audioSources)
        {
            if (pause)
            {
                audio.Pause();
            }
            else
            {
                audio.Play();
            }
        }
    }

    public void TogglePause(GameState stateTo)
    {
        _currentState = stateTo;
        
        Time.timeScale = stateTo == GameState.Paused ? 0 : 1;
        
        pausePanel.SetActive(stateTo == GameState.Paused);

        ToggleAudio(stateTo == GameState.Paused);
    }
}
