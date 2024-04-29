using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [Header("Sound Variable")]
    public AudioClip buttonSound;

    public void GoToMainMenu()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeState(GameManager.GameState.MainMenu);
        }
        PlaySound();
    }

    public void GoToOptions()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeState(GameManager.GameState.Options);
        }
        PlaySound();
    }

    public void GoToCredits()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeState(GameManager.GameState.Credits);
        }
        PlaySound();
    }

    public void BeginGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeState(GameManager.GameState.Gameplay);
        }
        PlaySound();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeState(GameManager.GameState.Pause);
        }
        PlaySound();
    }

    public void RestartGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeState(GameManager.GameState.Restart);
        }
        PlaySound();
    }

    public void GoBackToPreviousState()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeState(GameManager.Instance.previousState);
        }
        PlaySound();
    }

    public void BeginWaves()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.beginWaves = true;
        }
    }

    public void PlaySound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.Play(buttonSound);
        }
    }
}
