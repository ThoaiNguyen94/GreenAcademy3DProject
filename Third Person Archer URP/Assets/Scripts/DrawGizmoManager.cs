using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmoManager : MonoBehaviour
{
    //private readonly Vector3 offSphere = new(0f, 0.8f, 0f);
    //// Draw a sphere to debug attack range in scene
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 30f);
    }
}
