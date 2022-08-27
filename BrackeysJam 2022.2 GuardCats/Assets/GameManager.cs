using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static bool onGame;
    public GameObject hintScreen;
    public GameObject gameplayScreen;
    public GameObject gameoverScreen;
    public TextMeshProUGUI countdownText;
    public BallGenerator ballGenerator;
    bool hintfromStart;

    void Awake()
    {
        onGame = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    public void Setup()
    {
        gameplayScreen.SetActive(false);
        gameoverScreen.SetActive(false);
        onGame = false;

        if (SceneData.Instance.fromMenu)
        {
            hintScreen.SetActive(true);
            hintfromStart = true;
        }
        else
        {
            hintScreen.SetActive(false);
            hintfromStart = false;

            StopCoroutine("StartCountdown");
            StartCoroutine("StartCountdown");
        }
    }

    public void CloseHint()
    {
        hintScreen.SetActive(false);
        if (hintfromStart)
        {
            StopCoroutine("StartCountdown");
            StartCoroutine("StartCountdown");
        }
    }

    IEnumerator StartCountdown()
    {
        gameplayScreen.SetActive(true);
        countdownText.gameObject.SetActive(true);
        countdownText.text = "3";
        yield return new WaitForSeconds(1f);
        countdownText.text = "2";
        yield return new WaitForSeconds(1f);
        countdownText.text = "1";
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);
        StartGame();
        yield return null;

    }

    void StartGame()
    {
        onGame = true;

        ballGenerator.StartGenerate();
    }

    public void GameOver()
    {
        onGame = false;

        gameoverScreen.SetActive(true);
        gameplayScreen.SetActive(false);
        hintScreen.SetActive(false);

        //Time.timeScale = 0;
    }
}
