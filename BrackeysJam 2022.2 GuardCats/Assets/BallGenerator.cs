using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BallGenerator : MonoBehaviour
{
    public float staticTimerNeeded = 5f;
    float timerNeeded = 0;
    public float timerOffset = 1f;
    public float timer = 0;
    public Transform generatorPivot, generatorPoint;
    public float minAngle = -180f;
    public float maxAngle = 180f;
    public GameObject[] ballsOriginal;
    public Transform ballsParent;

    public void StartGenerate()
    {
        timerNeeded = 0;
        timer = 0;
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
        GameObject ball = GameObject.Instantiate(ballsOriginal[Random.Range(0, ballsOriginal.Length)], ballsParent);
        ball.transform.position = generatorPoint.position;
        ball.SetActive(true);
        ball.GetComponent<Ball>().Setup(transform);
        /*
         * GameObject guard = GameObject.Instantiate(guardOriginal[index], guardsParent);
            guard.transform.position = previewTransform.position;
            guard.GetComponent<Guard>().Setup(previewPivot);
            guard.SetActive(true);
        */
    }
}
