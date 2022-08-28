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
    public GameObject pauseScreen;
    public GameObject gameoverScreen;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI scoreText, scoreGameOverText;
    public int score;
    public BallGenerator ballGenerator;
    bool hintfromStart;
    bool paused;

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && onGame){
            if (paused) Resume();
            else Pause();
        }
    }

    public void Setup()
    {
        gameplayScreen.SetActive(false);
        gameplayScreen.transform.Find("PauseButton").GetComponent<UnityEngine.UI.Button>().interactable = false;
        gameoverScreen.SetActive(false);
        onGame = false;
        paused = false;
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
        if (!onGame) return;

        score++;
        scoreText.text = score.ToString();
        scoreText.GetComponentInParent<Animator>().SetTrigger("Pop");

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
        countdownText.text = "GO!!!";
        StartGame();
        yield return new WaitForSeconds(0.6f);
        countdownText.gameObject.SetActive(false);
        yield return null;

    }

    void StartGame()
    {
        onGame = true;

        gameplayScreen.transform.Find("PauseButton").GetComponent<UnityEngine.UI.Button>().interactable = true;
        ballGenerator.StartGenerate();
    }

    public void Pause()
    {
        paused = true;
        pauseScreen.SetActive(true);
        AudioManager.Instance.music.Pause();
        Time.timeScale = 0;
    }
    public void Resume()
    {
        paused = false;
        pauseScreen.SetActive(false);
        AudioManager.Instance.music.UnPause();
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        onGame = false;

        gameoverScreen.SetActive(true);
        gameoverScreen.transform.Find("Blocker").gameObject.SetActive(false);
        gameplayScreen.SetActive(false);
        hintScreen.SetActive(false);
        //Time.timeScale = 0;
        StartCoroutine("GameOverCo");
    }

    IEnumerator GameOverCo()
    {
        gameoverScreen.GetComponent<Animator>().SetTrigger("GameOver");
        scoreGameOverText.text = "0";
        int finalScore = score;
        int displayScore = 0;
        float timer = 0;
        AudioManager.Instance.PlayerHurt();
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.GameOver();
        while (timer < 2)
        {
            displayScore = Mathf.CeilToInt(Mathf.Lerp(0, finalScore, timer / 3));
            scoreGameOverText.text = displayScore.ToString();
            timer += Time.deltaTime;
            yield return null;
        }
        scoreGameOverText.text = finalScore.ToString();

        yield return null;
    }

    public void Restart()
    {
        gameoverScreen.transform.Find("Blocker").gameObject.SetActive(true);
        SceneManager.LoadSceneAsync("SampleScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
