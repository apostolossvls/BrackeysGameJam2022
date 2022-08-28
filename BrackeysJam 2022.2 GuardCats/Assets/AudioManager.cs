using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }
    public AudioSource music;
    public AudioSource[] playerHurt;
    public AudioSource[] countdown;
    public AudioSource[] addScore;
    public AudioSource[] missed;
    public AudioSource[] createCat;
    public AudioSource gameOver;
    public AudioMixer audioMixer;
    public Slider[] audioSliders; //ui volume sliders
    public string[] audioParameterNames;

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

    public void GetSlidersFromGameManager(Slider[] sliders)
    {
        audioSliders = new Slider[sliders.Length];
        for (int i = 0; i < audioSliders.Length; i++)
        {
            audioSliders[i] = sliders[i];
        }
        for (int i = 0; i < audioSliders.Length; i++)
        {
            SetVolume(i);
        }
    }

    public void ChangeVolume(Slider s)
    {
        int index = GetAudioIndex(s);
        float v = s.value;
        //from 0 to 1 to dB (x=20*Log10(y)?)
        float x;
        if (v > 0) x = 20 * Mathf.Log10(v);
        else x = -80;
        audioMixer.SetFloat(audioParameterNames[index], x);
    }

    public void SetVolume(int index)
    {
        float volume;
        audioMixer.GetFloat(audioParameterNames[index], out volume);
        audioSliders[index].value = Mathf.Pow(10.0f, volume / 20.0f);
    }

    int GetAudioIndex(Slider s)
    {
        for (int i = 0; i < audioSliders.Length; i++)
        {
            if (audioSliders[i] == s) return i;
        }
        return -1;
    }

    public void PlayerHurt()
    {
        if (!GameManager.onGame) return;

        if (!playerHurt[0].isPlaying) playerHurt[0].Play();
        else playerHurt[1].Play();
    }

    public void GameOver()
    {
        gameOver.Play();
    }

    public void Countdown(int index)
    {
        countdown[index].Play();
    }

    public void AddPoint()
    {
        if (!GameManager.onGame) return;

        if (!addScore[0].isPlaying) addScore[0].Play();
        else if (!addScore[1].isPlaying) addScore[1].Play();
        else addScore[2].Play();
    }

    public void Missed()
    {
        if (!GameManager.onGame) return;

        if (!missed[0].isPlaying) missed[0].Play();
        else if (!missed[1].isPlaying) missed[1].Play();
        else if (!missed[2].isPlaying) missed[2].Play();
        else missed[3].Play();
    }

    public void CreateCat()
    {
        if (!createCat[0].isPlaying) createCat[0].Play();
        else createCat[1].Play();
    }
}
