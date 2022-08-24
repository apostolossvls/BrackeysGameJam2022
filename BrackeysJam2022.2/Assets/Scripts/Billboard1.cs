using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard1 : MonoBehaviour
{
    private static Camera cam;
    public bool isStaticBillboard = true;

    // Update is called once per frame
    void LateUpdate()
    {
        if (!isStaticBillboard){
            transform.LookAt(cam.transform);
        }
        else {
            transform.rotation = cam.transform.rotation;
        }

        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }

    public static void SetupCamera(){
        cam = Camera.main;
    }
}
