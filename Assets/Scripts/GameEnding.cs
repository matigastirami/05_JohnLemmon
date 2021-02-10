using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnding : MonoBehaviour
{
    public float fadeDuration = 3f, 
        timer,
        displayImageDuration;
    private bool isPlayerAtExit, 
        isPlayerCaught,
        hasAudioPlayed;

    [SerializeField] private GameObject player;
    [SerializeField] private CanvasGroup exitImageCanvasGroup, caughtImageCanvasGroup;
    [SerializeField] private AudioSource exitAudioSource, caughtAudioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerAtExit)
        {
            EndLevel(exitImageCanvasGroup, false, exitAudioSource);
        }
        else if (isPlayerCaught)
        {
            EndLevel(caughtImageCanvasGroup, true, caughtAudioSource);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerAtExit = true;
        }
    }

    /// <summary>
    /// Launches an end level image
    /// </summary>
    /// <param name="imageCanvasGroup">Image to show (Exit or caught)</param>
    /// <param name="doRestart">Determine if the game has to restart</param>
    /// <param name="audioSource">Audio to play on end level</param>
    public void EndLevel(CanvasGroup imageCanvasGroup, bool doRestart, AudioSource audioSource)
    {
        if (!hasAudioPlayed)
        {
            audioSource.Play();
            hasAudioPlayed = true;
        }
        
        timer += Time.deltaTime;
        imageCanvasGroup.alpha = Mathf.Clamp(timer / fadeDuration, 0, 1);

        if (timer > fadeDuration + displayImageDuration)
        {
            if (doRestart)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                Application.Quit();
            }
        }
    }

    public void CatchPlayer()
    {
        isPlayerCaught = true;
    }
}
