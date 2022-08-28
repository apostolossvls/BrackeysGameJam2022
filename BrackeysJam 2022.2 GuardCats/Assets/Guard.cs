using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Guard : MonoBehaviour
{
    public GuardGenerator generator;
    public Transform center;
    Vector3 startPos;
    public float health = 1f;

    void Update()
    {
        if (center)
        {
            transform.right = center.position - transform.position;
            transform.localRotation *= Quaternion.Euler(0, 0, 90);
        }
    }

    public void Setup(Vector3 pos, GuardGenerator guardGenerator, Transform center)
    {
        transform.position = pos;

        this.center = center;

        int count = transform.childCount - 1;
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

        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
        ps.transform.parent = null;
        ps.Emit(12);
        Destroy(ps.gameObject, 1f);

        Destroy(gameObject);
    }
}
