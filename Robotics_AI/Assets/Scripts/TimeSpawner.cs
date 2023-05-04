using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSpawner : MonoBehaviour



{

    public GameObject spawnee;
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
        Quaternion targetRotation = Quaternion.LookRotation(movement);
        checkpointBody.MoveRotation(targetRotation);

        //Instantiate(spawnee, checkpointPositions.transform.position, checkpointPositions.transform.rotation);

        Instantiate(spawnee, checkpointPositions.transform.position, targetRotation);
        

        if (stopSpawning)
        {
            CancelInvoke("SpawnObject");
        }



    } 
}
