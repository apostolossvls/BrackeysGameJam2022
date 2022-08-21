using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHealth : Health
{
    public override void AdjustHealth(int value){
        health += value;

        if (health >= maxHealth){
            health = maxHealth;
            //DOTO indicate reach max
        }

        if (health <= 0){
            Die();
        } 
    }

    protected override void Die()
    {
        base.Die();
        Debug.Log("GameOver");
        this.enabled = false;
    }
}
