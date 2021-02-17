using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject gameplayPanel;
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject losePanel;
    [SerializeField] Text cashText;
    [SerializeField] Text scoreText;
    [SerializeField] Animation scoreAddedAnimation;
    [SerializeField] Image transitionPanel;
    [SerializeField] float fadeInOutSpeed = 1;
    bool gameJustStarted = true;
    private void OnEnable()
    {
        EventManager.StartListening("Gameplay Started", EnableGameplayPanel);
        EventManager.StartListening("Level Won", EnableWonPanel);
        EventManager.StartListening("Level Lost", EnableLosePanel);
        EventManager.StartListening("Level Refreshed", EnableMainMenuPanel);
        EventManager.StartListening("Cash Updated", UpdateCash);
        EventManager.StartListening("Score Updated", UpdateScore);
        EventManager.StartListening("Fade Out", FadeOut);
        EventManager.StartListening("Fade In", FadeIn);
    }


    private void OnDisable()
    {

        EventManager.StopListening("Gameplay Started", EnableGameplayPanel);
        EventManager.StopListening("Level Won", EnableWonPanel);
        EventManager.StopListening("Level Lost", EnableLosePanel);
        EventManager.StopListening("Level Refreshed", EnableMainMenuPanel);
        EventManager.StopListening("Cash Updated", UpdateCash);
        EventManager.StopListening("Score Updated", UpdateScore);
        EventManager.StopListening("Fade Out", FadeOut);
        EventManager.StopListening("Fade In", FadeIn);
    }


    private void UpdateScore(int score)
    {
        scoreText.text = score.ToString();

        scoreAddedAnimation.Play();
    }

    private void Start()
    {
        transitionPanel.gameObject.SetActive(true);
        transitionPanel.color = Color.black;
        FadeIn();
    }
    private void FadeOut()
    {
        StartCoroutine(FadeRoutine(true));
    }
    private void FadeIn()
    {
        StartCoroutine(FadeRoutine(false));
    }
    IEnumerator FadeRoutine(bool fadeOut)
    {
        float target = 0;
        Color temp = Color.black;
        if (fadeOut)
        {
            transitionPanel.gameObject.SetActive(true);
            target = 1;
            temp.a = 0;
        }
        else
        {
            target = 0;
            temp.a = 1;
        }
        while (transitionPanel.color.a != target)
        {
            temp.a = Mathf.MoveTowards(temp.a, target, Time.deltaTime * fadeInOutSpeed);
            transitionPanel.color = temp;
            yield return null;
        }
        if (!fadeOut)
        {
            transitionPanel.gameObject.SetActive(false);
        }
        if (gameJustStarted)
        {
            gameJustStarted = false;
        }
        else
        {
            EventManager.TriggerEvent("Transition Done");
        }
    }


    private void UpdateCash(int cash)
    {
        cashText.text = cash.ToString();
    }



    private void EnableMainMenuPanel()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        scoreText.text = "0";
    }

    private void EnableWonPanel()
    {
        StartCoroutine(EnableWinLosePanel(true));
    }
    IEnumerator EnableWinLosePanel(bool win)
    {
        gameplayPanel.SetActive(false);
        yield return new WaitForSeconds(2);
        if (win)
        {
            winPanel.SetActive(true);
        }
        else
        {
            losePanel.SetActive(true);
        }
    }
    private void EnableLosePanel()
    {
        StartCoroutine(EnableWinLosePanel(false));
    }


    void EnableGameplayPanel()
    {
        gameplayPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

}
