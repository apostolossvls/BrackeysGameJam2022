using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int health = 100;

    void Start()
    {
        health = maxHealth;
    }

    public virtual void AdjustHealth(int value){
        health += value;

        if (health >= maxHealth){
            health = maxHealth;
            //DOTO indicate reach max
        }

        if (health <= 0){
            Die();
        } 
    }

    protected virtual void Die(){
        Debug.Log("Death ("+name+").");
    }
}

