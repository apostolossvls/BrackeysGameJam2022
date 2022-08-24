using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform playerTransform;
    public ParticleSystem psDeath;
    public ParticleSystem psHit;
    int hits;
    bool alive;
    Vector3 lastPos;

    void LateUpdate(){
        lastPos = transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy"){
            HitSuccess();
        }
        else {
            Die();
        }
    }

    void HitSuccess(){
        hits++;
        psHit.Play();
        psHit.transform.SetParent(null);
        psHit.transform.position = lastPos;
    }

    void Die(){
        if (!alive) return;
        alive = false;
        
        psDeath.Play();
        psDeath.transform.SetParent(null);
        psDeath.transform.position = lastPos;

        Destroy(gameObject, 1f);
        
        if (hits > 1){
            Debug.Log("Bullet success");
        }
        else{
            Debug.Log("Bullet fail");
        }
    }

    public void Setup (Transform playerT){
        hits = 0;
        alive = true;
        playerTransform = playerT;
    }
}
