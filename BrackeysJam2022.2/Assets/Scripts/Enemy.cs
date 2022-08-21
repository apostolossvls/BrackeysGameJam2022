using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target = null;
    Rigidbody rig = null;
    public float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        target = Tower.GetClosestTargetPoint(transform);
        rig = GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null){
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed);
            //rig.AddForce((target.position - transform.position) * speed * Time.deltaTime, ForceMode.VelocityChange);
        }
    }
}
