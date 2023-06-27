using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private float timeToDestroy = 1f;
    private Vector3 offset = new Vector3(0f, 2f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition += offset;
        Destroy(gameObject, timeToDestroy);
    }
}
