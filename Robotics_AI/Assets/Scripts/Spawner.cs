using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject checkpoint;
    [SerializeField] private GameObject checkpositions;
    //[SerializeField] private GameObject navigation_Walls;
    


    void Update()

    {
       

        if (Input.GetKeyDown(KeyCode.Space)) {
            Instantiate(checkpoint, checkpositions.transform.position, Quaternion.AngleAxis(90f, Vector3.up));
           
        }
    }



}
