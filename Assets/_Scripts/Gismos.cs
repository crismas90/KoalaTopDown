using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gismos : MonoBehaviour
{
    public float radius;
    void OnDrawGizmosSelected()
    {        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
