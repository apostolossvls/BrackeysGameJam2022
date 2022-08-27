using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    bool loading = false;

    // Start is called before the first frame update
    void Start()
    {
        loading = false;
    }

    public void PressPlay()
    {
        if (loading) return;

        loading=true;
        SceneManager.LoadSceneAsync("SampleScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
