using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallType{
    Normal = 0,
    IceBall = 1,
    FireBall = 2
}

public class Ball : MonoBehaviour
{
    public Transform target;
    Rigidbody2D rb;
    public float forceSpeed;
    public float torqueForceMin = 20f;
    public float torqueForceMax = 30f;
    public ForceMode2D forceMode;
    public BallType ballType = BallType.Normal;
    public float damage = 1f;
    bool hitsCompleted; 

    public void Setup(Transform target, float speedMultiplier)
    {
        forceSpeed *= speedMultiplier;
        this.target = target;
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce((target.position - transform.position).normalized * forceSpeed, forceMode);

        rb.AddTorque(((Random.Range(0, 2) * 2) - 1) * Random.Range(torqueForceMin, torqueForceMax));
    }

    void Die()
    {
        hitsCompleted = true;

        GameManager.instance.AddScore();

        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
        ps.transform.parent = null;
        ps.Emit(16);
        Destroy(ps.gameObject, 1f);

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hitsCompleted)
        {
            if (other.tag == "Player")
            {
                other.GetComponent<PlayerHealth>().TakeDamage(damage);
                Die();
            }
            else if (other.tag == "Guard1")
            {
                other.GetComponent<Guard>().TakeDamage(damage);
                if (ballType == BallType.Normal)
                {
                    Die();
                }
                else AudioManager.Instance.Missed();
            }
            else if (other.tag == "Guard2")
            {
                other.GetComponent<Guard>().TakeDamage(damage);
                if (ballType == BallType.Normal || ballType == BallType.IceBall)
                {
                    Die();
                }
                else AudioManager.Instance.Missed();
            }
            else if (other.tag == "Guard3")
            {
                other.GetComponent<Guard>().TakeDamage(damage);
                Die();
            }
        }
    }
}
