using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }
    public AudioSource music;
    public AudioSource[] playerHurt;
    public AudioSource gameOver;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        music.Play();
    }

    public void PlayerHurt()
    {
        if (!playerHurt[0].isPlaying) playerHurt[0].Play();
        else playerHurt[1].Play();
    }

    public void GameOver()
    {
        gameOver.Play();
    }
}
