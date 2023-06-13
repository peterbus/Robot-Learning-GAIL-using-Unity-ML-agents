using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeSpawner : MonoBehaviour



{

    public GameObject spawnee;
    //public GameObject navigation_Wall;
    [SerializeField] private GameObject checkpointPositions;
    public bool stopSpawning = false;
    public float spawnTime;
    public float spawnDelay;
    


    // Start is called before the first frame update
    void Start()
    {
        
        InvokeRepeating("SpawnObject", spawnTime, spawnDelay);
        
    }

    public void SpawnObject()
    {

        Rigidbody checkpointBody = spawnee.GetComponent<Rigidbody>();
        Vector3 movement = new Vector3(checkpointPositions.transform.position.x, checkpointPositions.transform.position.y, checkpointPositions.transform.position.z).normalized;
        Vector3 parallel = new Vector3(checkpointPositions.transform.position.x+0.005f, checkpointPositions.transform.position.y, checkpointPositions.transform.position.z+0.005f).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(movement);
     
        checkpointBody.MoveRotation(targetRotation);
        

        //Instantiate(spawnee, checkpointPositions.transform.position, checkpointPositions.transform.rotation);
        Instantiate(spawnee, checkpointPositions.transform.position, targetRotation);

        //TO DO
        //Let us put some navigation walls around the pathway to be recognised by the agent
        var up = new Vector3(0, 1, 0);
        var cross = Vector3.Cross(movement, up); //cross product vector
      //  Instantiate(navigation_Wall, cross * 0.15f, targetRotation);
 

        if (stopSpawning)
        {
            CancelInvoke("SpawnObject");
        }

    }


}
