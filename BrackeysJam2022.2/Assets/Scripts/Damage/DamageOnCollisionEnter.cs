using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnCollisionEnter : MonoBehaviour
{
    public int damage = 1;
    public string[] tags;
    public string[] layers;

    public virtual void DealDamage(Transform t){
        Health h = t.gameObject.GetComponent<Health>();
        if (h){
            h.AdjustHealth(-damage);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if ((TagMatch(other.transform.tag) || LayerMatch(LayerMask.LayerToName(other.gameObject.layer)))){
            DealDamage(other.transform);
        }
    }

    bool TagMatch (string s){
        foreach (string t in tags)
        {
            if (s==t) return true;
        }
        return false;
    }

    bool LayerMatch (string s){
        foreach (string l in layers)
        {
            if (s==l) return true;
        }
        return false;
    }
}
