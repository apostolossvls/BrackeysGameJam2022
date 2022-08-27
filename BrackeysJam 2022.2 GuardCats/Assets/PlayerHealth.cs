using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int hearts;
    public GameObject[] heartsObjects;
    public CameraControl cameraControl;

    // Start is called before the first frame update
    void Start()
    {
        hearts = 5;

        foreach (GameObject heartObj in heartsObjects)
        {
            heartObj.SetActive(true);
        }
        heartsObjects[0].transform.parent.gameObject.SetActive(true);
    }

    public void TakeDamage(float damage)
    {
        PutHeart(-1);
    }

    public void PutHeart(int value)
    {
        if (!GameManager.onGame) return;

        hearts = hearts + value;
        if (hearts > 5) hearts = 5;

        if (value < 0)
        {
            //hurt sound
        }

        int heartsLeft = hearts;
        for (int i = 0; i < 5; i++)
        {
            Animator a = heartsObjects[i].GetComponent<Animator>();

            if (heartsLeft > 0)
            {
                a.SetBool("Visible", true);

                heartsLeft = heartsLeft - 1;
            }
            else
            {
                a.SetBool("Visible", false);
            }
        }

        if (hearts <= 0)
        {
            Die();
        }
        else
        {
            cameraControl.PlayerHitShake();
        }
    }

    void Die()
    {
        Debug.Log("Player Death");
        GameManager.instance.GameOver();
    }
}
