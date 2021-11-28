using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryRandomMap : MonoBehaviour
{
    [SerializeField]private GameObject test;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(test, Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
