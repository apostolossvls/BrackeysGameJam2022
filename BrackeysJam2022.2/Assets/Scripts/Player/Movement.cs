using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody rd;
    public float moveSpeed = 5f;
    public float rotationSpeed = 10.0f;
    Vector3 movement;

    void Start(){
        if (rd==null && gameObject.GetComponent<Rigidbody>()){
            rd = gameObject.GetComponent<Rigidbody>();
        }
    }

    void Update(){
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");
        //Debug.DrawLine(rd.position, rd.position +  movement, Color.red, 1f);

    }

    void FixedUpdate(){
        rd.MovePosition(rd.position +  movement * moveSpeed * Time.fixedDeltaTime);
        bool moving = movement != Vector3.zero;
        if (moving)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * rotationSpeed);
        }
    }
}
