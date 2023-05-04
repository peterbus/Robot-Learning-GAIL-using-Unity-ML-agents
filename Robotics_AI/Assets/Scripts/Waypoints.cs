using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public GameObject Effector;
    public GameObject[] waypoints;
    public int numberOfPositions;
    private Vector3 actualPosition;
    private int current;
    [SerializeField] private float speed = 1.5f;
    


    // Start is called before the first frame update
    void Start()
    {
        current = 1;
        
    }

    // Update is called once per frame
    void Update()
    {
        actualPosition = Effector.transform.position;
        

        if (actualPosition == waypoints[current].transform.position && current != numberOfPositions - 1)
        {
            current++;
           
        }

            if (actualPosition == waypoints[current].transform.position && current != numberOfPositions)
        {
            current=1;
        }

        Effector.transform.position = Vector3.MoveTowards(actualPosition, waypoints[current].transform.position, speed * Time.deltaTime);
        transform.right = waypoints[current].transform.position;
        transform.rotation = Quaternion.LookRotation(waypoints[current].transform.position);


    }
}
