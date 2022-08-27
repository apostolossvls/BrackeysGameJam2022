using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    public GuardGenerator generator;
    public Transform center;
    public float health = 1f;

    void Update()
    {
        if (center)
        {
            transform.right = center.position - transform.position;
            transform.localRotation *= Quaternion.Euler(0, 0, 90);
        }
    }

    public void Setup(GuardGenerator guardGenerator, Transform center)
    {
        this.center = center;

        int count = transform.childCount;
        int r = Random.Range(0, count);
        for (int i = 0; i < count; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == r);
        }

        generator = guardGenerator;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        generator.NotifyDeath(this);
        Destroy(gameObject);
    }
}
