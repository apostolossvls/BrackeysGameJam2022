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
    public ForceMode2D forceMode;
    public BallType ballType = BallType.Normal;
    public float damage = 1f;
    bool hitsCompleted; 

    public void Setup(Transform target)
    {
        this.target = target;
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce((target.position - transform.position).normalized * forceSpeed, forceMode);
    }

    void Die()
    {
        hitsCompleted = true;
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
                if (ballType == BallType.Normal) Die();
            }
            else if (other.tag == "Guard2")
            {
                other.GetComponent<Guard>().TakeDamage(damage);
                if (ballType == BallType.Normal || ballType == BallType.IceBall) Die();
            }
            else if (other.tag == "Guard3")
            {
                other.GetComponent<Guard>().TakeDamage(damage);
                Die();
            }
        }
    }
}
