using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
{
    public static KitchenGameManager Instance { get; private set; }
    public event EventHandler OnstateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnPaused;

    enum State
    {
        WaitingToStart,
        CountDownToStart,
        GamePlaying,
        GameOver,
    }
    State state;
    float waitingToStartTimer = 1f;
    float countDownToStartTimer = 3f;
    float gamePlayingTimer;
    float gamePlayingTimerMax = 180f;
    bool isGamePaused = false;
    private void Awake()
    {
        if (Instance != null) { Debug.LogError("There is more than 1 kitchenGameManager"); return; }
        Instance = this;
        state = State.WaitingToStart;
        gamePlayingTimer = gamePlayingTimerMax;

    }
    void Start()
    {
        GameInput.Instance.OnPauseAction += GameInputOnPauseAction;  
    }

    private void GameInputOnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;
        if(isGamePaused)
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnPaused?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer < 0f)
                {
                    state = State.CountDownToStart;
                    OnstateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.CountDownToStart:
                countDownToStartTimer -= Time.deltaTime;
                if (countDownToStartTimer < 0f)
                {
                    state = State.GamePlaying;
                    OnstateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0f)
                {
                    state = State.GameOver;
                    OnstateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
    }
    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }
    public bool IsCountdownState()
    {
        return state == State.CountDownToStart;
    }
    public float GameStartCountdownToStartTimer()
    {
        return countDownToStartTimer;
    }
    public bool IsGameOverState()
    {
        return state == State.GameOver;
    }
    public float GetGamePlayingTimerNormalized()
    {
        return gamePlayingTimer / gamePlayingTimerMax;
    }
}
