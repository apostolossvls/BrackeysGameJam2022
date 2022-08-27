using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static bool onGame;
    public static GameManager instance;
    public GameObject hintScreen;
    public GameObject gameplayScreen;
    public GameObject gameoverScreen;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI scoreText;
    public int score;
    public BallGenerator ballGenerator;
    bool hintfromStart;

    void Awake()
    {
        onGame = false;
        instance = this;
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
        score = 0;

        if (SceneData.Instance.fromMenu)
        {
            hintScreen.SetActive(true);
            hintfromStart = true;
            SceneData.Instance.fromMenu = false;
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

    public void AddScore()
    {
        score++;
        scoreText.text = score.ToString();

        ballGenerator.IncreaseDifficulty();
    }

    IEnumerator StartCountdown()
    {
        gameplayScreen.SetActive(true);
        countdownText.gameObject.SetActive(true);
        scoreText.text = score.ToString();
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
        gameoverScreen.transform.Find("Blocker").gameObject.SetActive(false);
        gameplayScreen.SetActive(false);
        hintScreen.SetActive(false);

        //Time.timeScale = 0;
    }

    public void Restart()
    {
        gameoverScreen.transform.Find("Blocker").gameObject.SetActive(true);
        SceneManager.LoadSceneAsync("SampleScene");
    }
}
