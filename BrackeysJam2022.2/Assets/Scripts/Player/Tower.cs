using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public static Tower instance;
    public Transform[] targetPoints;

    void Awake()
    {
        instance = this;
    }

    public static Transform GetClosestTargetPoint(Transform origin){
        int targetIndex = 0;
        float minDistance = float.MaxValue;

        for (int i = 0; i < instance.targetPoints.Length; i++)
        {
            if (minDistance > Vector3.Distance(origin.position, instance.targetPoints[i].position)){
                minDistance = Vector3.Distance(origin.position, instance.targetPoints[i].position);
                targetIndex = i;
            }
        }
    
        return instance.targetPoints[targetIndex];
    }
}
