using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake()
    {
        if (instance == null){
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Billboard1.SetupCamera();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
