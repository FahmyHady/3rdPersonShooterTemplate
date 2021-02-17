using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] PlayerData playerDataSO;
    [SerializeField] LevelData[] levelsDatas;
    LevelData currentLevel;
    GameplayUIHandle uIHandle;
    MainCharacter playerChar;
    public float timeScale = 1;
    bool transitionDone;
    [HideInInspector] public bool gameplayPaused=true;

    private void OnEnable()
    {
        EventManager.StartListening("Player Died", PlayerDied);
        EventManager.StartListening("Transition Done", TransitionDone);
        EventManager.StartListening("Level Ended", CheckWinConditions);
        EventManager.StartListening("Add Score", AddScore);
        EventManager.StartListening("Decrease Max Score", DecreaseMaxScore);
    }


    private void OnDisable()
    {
        EventManager.StopListening("Player Died", PlayerDied);
        EventManager.StopListening("Transition Done", TransitionDone);
        EventManager.StopListening("Level Ended", CheckWinConditions);
        EventManager.StopListening("Add Score", AddScore);
        EventManager.StopListening("Decrease Max Score", DecreaseMaxScore);

    }
    private void DecreaseMaxScore(int amountToRemove)
    {
        currentLevel.maxScore -= amountToRemove;
    }

    private void AddScore(int scoreToAdd)
    {
        currentLevel.currentScore += scoreToAdd;
        EventManager.TriggerEvent("Score Updated", currentLevel.currentScore);
    }

    private void CheckWinConditions()
    {
        int stars = currentLevel.CalculateStars();
        gameplayPaused = true;
        if (stars > 0)
        {
            EventManager.TriggerEvent("Level Won");
            playerDataSO.currentLevel++;
            playerDataSO.SaveLevel();
        }
        else
        {
            EventManager.TriggerEvent("Level Lost");
        }
    }

    private void Awake()
    {
        gameplayPaused = true;
        Time.timeScale = timeScale;
        playerChar = FindObjectOfType<MainCharacter>();
        uIHandle = GetComponent<GameplayUIHandle>();
    }
    private void Start()
    {
        playerDataSO.LoadData();
        IntializeLevel();
      SceneLoader.Instance.LoadANewScene(currentLevel.levelNumber);
    }
    public void RestartLevel()
    {
        StartCoroutine(RestartLevelRoutine());
    }
    IEnumerator RestartLevelRoutine()
    {
        EventManager.TriggerEvent("Fade Out");
        yield return new WaitUntil(() => transitionDone == true);
        transitionDone = false;

        ExcuteRestartLevel();

        yield return new WaitForSeconds(1);
        EventManager.TriggerEvent("Fade In");
        yield return new WaitUntil(() => transitionDone == true);
        transitionDone = false;
    }
    private void ExcuteRestartLevel()
    {
        SceneLoader.Instance.LoadANewScene(currentLevel.levelNumber);
        playerChar.ResetCharacter();
        currentLevel.currentScore = 0;
        EventManager.TriggerEvent("Level Refreshed");
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadNextLevelRoutine());
    }
    IEnumerator LoadNextLevelRoutine()
    {
        EventManager.TriggerEvent("Fade Out");
        yield return new WaitUntil(() => transitionDone == true);
        transitionDone = false;

        ExcuteLoadNextLevel();

        yield return new WaitForSeconds(1);
        EventManager.TriggerEvent("Fade In");
        yield return new WaitUntil(() => transitionDone == true);
        transitionDone = false;
    }

    private void ExcuteLoadNextLevel()
    {
        IntializeLevel();
        SceneLoader.Instance.LoadANewScene(currentLevel.levelNumber);
        playerChar.ResetCharacter();
        EventManager.TriggerEvent("Level Refreshed");
    }



    void IntializeLevel()
    {
        currentLevel = levelsDatas[(playerDataSO.currentLevel - 1) % levelsDatas.Length];
        uIHandle.UpdateLevel(playerDataSO.currentLevel);
    }

    private void TransitionDone()
    {
        transitionDone = true;
    }

    public void StartGame()
    {
        gameplayPaused = false;
        EventManager.TriggerEvent("Gameplay Started");
    }
    private void PlayerDied()
    {
        gameplayPaused = true;
        int cashToGet = 50;
        playerDataSO.Cash += cashToGet;
        uIHandle.UpdateCashWon(false, cashToGet);
        EventManager.TriggerEvent("Level Lost");
    }




    private void LevelWon()
    {
        playerDataSO.currentLevel++;
        playerDataSO.Cash += 100;
        uIHandle.UpdateCashWon(true, 100);
        playerDataSO.SaveLevel();
    }



}
