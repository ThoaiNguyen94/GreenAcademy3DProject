using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetEnemyLayer : MonoBehaviour
{
    public string layerName;

    // Start is called before the first frame update
    void Start()
    {
        LayerName();
    }

    public string LayerName()
    {
        int layerIndex = gameObject.layer;
        layerName = LayerMask.LayerToName(layerIndex);
        return layerName;
    }
}
