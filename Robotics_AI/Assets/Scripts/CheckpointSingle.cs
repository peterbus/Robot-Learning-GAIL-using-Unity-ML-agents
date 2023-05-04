using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CheckpointSingle : MonoBehaviour
{
    private TrackCheckpoints trackCheckpoints;
    
    public  GameObject checkpoint;

   /* public void Update()
    {
        float speed = 0.02f;
        Vector3 way = checkpoint.transform.position + new Vector3(UnityEngine.Random.Range(-0.005f, +0.005f), 0, UnityEngine.Random.Range(-0.005f, +0.005f));
        checkpoint.transform.position = Vector3.MoveTowards(checkpoint.transform.position, way, speed * Time.deltaTime);

    }*/

    private void OnTriggerEnter(Collider other)
    {

        if(other.TryGetComponent<Move_Along>(out Move_Along robot)){
            trackCheckpoints.RobotThroughCheckpoint(this, other.transform);

        }

    
    }

    public void SetTrackCheckpoints(TrackCheckpoints trackCheckpoints)
    {
        this.trackCheckpoints = trackCheckpoints;

    }





}
