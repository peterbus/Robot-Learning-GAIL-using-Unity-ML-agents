using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// some parts of the code and settings  are informed by the tutorial "Simple Checkpoints in Unity" by CodeMonkey https://www.youtube.com/watch?v=IOYNg6v9sfc
// and also "AI learns to drive a Car" https://www.youtube.com/watch?v=2X5m_nDBvS4.
// A reference code has been studied too, which elaborates certain methods in the TrackCheckpoint and CheckpointSingle Class: IR-Project "SelfDrivingCars" by @monidp9
// link: https://github.com/monidp9/IR-Project/tree/master/SelfDrivingCars. 
// All accessed January-June 2023 [ONLINE]


public class CheckpointSingle : MonoBehaviour
{
    private TrackCheckpoints trackCheckpoints;//making a reference
    
    public  GameObject checkpoint;
    

  /*  private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.TryGetComponent<Robot>(out Robot robot)){//this is the reference to robot components inside the ML Agent class
            trackCheckpoints.RobotThroughCheckpoint(this, other.transform);
            Debug.Log("Checkpoint!");
        }
    
    }*/

    public bool IsCorrectCheckPoint(Robot robot)
    {
        return trackCheckpoints.RobotThroughCheckpoint(this, robot.transform);
    }

    public bool IsLastCheckpoint()
    {
        return trackCheckpoints.IsLastCheckpoint(this);
        
    }


    public void SetTrackCheckpoints(TrackCheckpoints trackCheckpoints)
    {
        this.trackCheckpoints = trackCheckpoints;

    }





}
