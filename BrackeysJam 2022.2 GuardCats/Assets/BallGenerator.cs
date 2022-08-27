using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BallGenerator : MonoBehaviour
{
    public float staticTimerNeededStart = 3.5f;
    public float staticTimerNeededEnd = 1f;
    public float staticTimerNeeded = 5f;
    float timerNeeded = 0;
    public float timerOffsetStart = 1f;
    public float timerOffsetEnd = 0.3f;
    public float timerOffset = 1f;
    public float ballSpeedStart = 0.7f;
    public float ballSpeedEnd = 1.5f;
    public float ballSpeed = 0.7f;
    public float timer = 0;
    public Transform generatorPivot, generatorPoint;
    public float minAngle = -180f;
    public float maxAngle = 180f;
    public GameObject[] ballsOriginal;
    public Transform ballsParent;
    public float[] startRandomBallPercentages;
    public float[] endRandomBallPercentages;
    public float[] currentRandomBallPercentages;
    public int maxScoreDifficulty = 100;

    public void StartGenerate()
    {
        timerNeeded = 0;
        timer = 0;
        currentRandomBallPercentages = new float[startRandomBallPercentages.Length];
        for (int i = 0; i < currentRandomBallPercentages.Length; i++)
        {
            currentRandomBallPercentages[i] = startRandomBallPercentages[i];
        }

        staticTimerNeeded = staticTimerNeededStart;

        timerOffset = timerOffsetStart;

        ballSpeed = ballSpeedStart;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.onGame) return;

        timer += Time.deltaTime;
        if (timer >= timerNeeded)
        {
            timer = 0;
            timerNeeded = Random.Range(staticTimerNeeded - timerOffset, staticTimerNeeded + timerOffset);

            CreateBall();
        }
    }

    void CreateBall()
    {
        generatorPivot.rotation = Quaternion.Euler(0f, 0f, Random.Range(minAngle, maxAngle));
        GameObject ball = GameObject.Instantiate(ballsOriginal[RandomBallIndex()], ballsParent);
        ball.transform.position = generatorPoint.position;
        ball.SetActive(true);
        ball.GetComponent<Ball>().Setup(transform, ballSpeed);

        /*
         * GameObject guard = GameObject.Instantiate(guardOriginal[index], guardsParent);
            guard.transform.position = previewTransform.position;
            guard.GetComponent<Guard>().Setup(previewPivot);
            guard.SetActive(true);
        */
    }

    int RandomBallIndex()
    {
        float r = Random.Range(0f, 1f);
        if (r < currentRandomBallPercentages[0])
        {
            return 0;
        }
        else if (r < currentRandomBallPercentages[1])
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

    public void IncreaseDifficulty()
    {
        float scoreDiv = Mathf.Min(((float)GameManager.instance.score / maxScoreDifficulty), 1f);
        for (int i = 0; i < currentRandomBallPercentages.Length; i++)
        {
            currentRandomBallPercentages[i] = Mathf.Lerp(
                startRandomBallPercentages[i],
                endRandomBallPercentages[i],
                scoreDiv);
        }

        staticTimerNeeded = Mathf.Lerp(staticTimerNeededStart, staticTimerNeededEnd, scoreDiv);

        timerOffset = Mathf.Lerp(timerOffsetStart, timerOffsetEnd, scoreDiv);

        ballSpeed = Mathf.Lerp(ballSpeedStart, ballSpeedEnd, scoreDiv);
    }
}
