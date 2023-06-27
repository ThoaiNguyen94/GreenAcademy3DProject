using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsSaveLoad : MonoBehaviour
{
    public Transform exampleTransform;


    [SerializeField] private Transform resultTransform;
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Debug.Log(playerTransform.name);
    }

    // Update is called once per frame
    void Update()
    {
        // Save
        if (Input.GetKeyDown(KeyCode.N))
        {
            PlayerPrefsManager.SetTransform("ExampleTransform", playerTransform);
            //Debug.Log("Save position: " + exampleTransform.position);
        }
        // Load
        if(Input.GetKeyDown(KeyCode.M))
        {
            PlayerPrefsManager.GetTransform("ExampleTransform", playerTransform);
            //Debug.Log("Load position: " + exampleTransform.position);
        }
    }
}
