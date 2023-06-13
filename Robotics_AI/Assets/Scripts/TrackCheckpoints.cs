using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


// some parts of the code and settings  are informed by the tutorial "Simple Checkpoints in Unity" by CodeMonkey https://www.youtube.com/watch?v=IOYNg6v9sfc
// and also "AI learns to drive a Car" https://www.youtube.com/watch?v=2X5m_nDBvS4.
// A reference code has been studied too, which elaborates certain methods in the TrackCheckpoint Class: IR-Project "SelfDrivingCars" by @monidp9
// link: https://github.com/monidp9/IR-Project/tree/master/SelfDrivingCars. 
// All accessed January-June 2023 [ONLINE]


public class TrackCheckpoints : MonoBehaviour
{
    public event EventHandler<OnRobotCheckEventArgs> OnRobotCorrectCheckpoint;
    public event EventHandler<OnRobotCheckEventArgs> OnRobotWrongCheckpoint;
    private int nextCheckpointSingleIndex; //keep track of the order through this

    public class OnRobotCheckEventArgs : EventArgs
    {
        public Transform robotTransform;
    }

    private List<CheckpointSingle> checkpointSingleList;//let's keep the order of the chcekpoints
    private List<int> nextCheckpointSingleIndexList;
    [SerializeField] private List<Transform> robotTransformList;
    

    private void Awake()
    {

        Transform checkpointsTransform = transform.Find("Checkpoints");
        checkpointSingleList = new List<CheckpointSingle>();//initialise the List
        

        foreach (Transform checkpointsSingleTransform in checkpointsTransform)
        {


            CheckpointSingle checkpointSingle = checkpointsSingleTransform.GetComponent<CheckpointSingle>();
            checkpointSingle.SetTrackCheckpoints(this);
            checkpointSingleList.Add(checkpointSingle); //list of our checkpoints
            
            //Debug.Log(checkpointsSingleTransform);


        }
          //nextCheckpointSingleIndex = 0;
          nextCheckpointSingleIndexList = new List<int>();//in case of multiple robots
           foreach (Transform robotTransform in robotTransformList)
           {
              nextCheckpointSingleIndexList.Add(0);
           }

    }

    public bool RobotThroughCheckpoint(CheckpointSingle checkpointSingle, Transform robotTransform)
    {
        // Debug.Log(checkpointSingle.transform.name); //through this we know the robot goes through the checkpoints
        //int nextCheckpointSingleIndex = nextCheckpointSingleIndexList[robotTransformList.IndexOf(robotTransform)];//in case of multiple robots

        //   Debug.Log(checkpointSingleList.IndexOf(checkpointSingle));
        int robotIndex = robotTransformList.IndexOf(robotTransform);
       
        Debug.Log(robotIndex);
        int nextCheckpointSingleIndex = nextCheckpointSingleIndexList[robotIndex];
        int checkpointIndex = checkpointSingleList.IndexOf(checkpointSingle);

        if (/*checkpointSingleList.IndexOf(checkpointSingle)*/ checkpointIndex == nextCheckpointSingleIndex)
        { //if the chcekpoint the robot goes thorugh is the next one
          //correct checkpoint
            Debug.Log("Correct");
            Debug.Log(nextCheckpointSingleIndex);
           // nextCheckpointSingleIndex++;
            //  if (nextCheckpointSingleIndex == 18)
            //  {
            //      nextCheckpointSingleIndex = 0;
            //  }
            nextCheckpointSingleIndexList[robotIndex] = (nextCheckpointSingleIndex + 1) % checkpointSingleList.Count;//basically this start counting after it reaches the amount from 0 again
            OnRobotCorrectCheckpoint?.Invoke(this, new OnRobotCheckEventArgs { robotTransform = robotTransform });
            return true;
            //      CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];//in case multiple robots.
            //      nextCheckpointSingleIndexList[robotTransformList.IndexOf(robotTransform)]
            //      = (nextCheckpointSingleIndex + 1) % checkpointSingleList.Count;
            //      OnRobotCorrectCheckpoint?.Invoke(this, new OnRobotCheckEventArgs { robotTransform = robotTransform});
        }
        else
        {
            //wrong checkpoint
            Debug.Log("Wrong");
            OnRobotWrongCheckpoint?.Invoke(this, new OnRobotCheckEventArgs { robotTransform = robotTransform });
            //CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];
            return false;
        }
    }

    public CheckpointSingle GetNextCheckpoint(Transform robotTransform)
    {
        int robotIndex = robotTransformList.IndexOf(robotTransform);
        int nextCheckpointIndex = nextCheckpointSingleIndexList[robotIndex];
        return checkpointSingleList[nextCheckpointIndex];
    }

    public void ResetCheckpoint(Transform robotTransform)
    {
        int robotIndex = robotTransformList.IndexOf(robotTransform);  
        nextCheckpointSingleIndexList[robotIndex] = 0;
    }

    public bool IsLastCheckpoint(CheckpointSingle checkpointSingle)
    {
        int checkpointIndex = checkpointSingleList.IndexOf(checkpointSingle);
        int lastCheckpointIndex = checkpointSingleList.Count - 1;
        return checkpointIndex == lastCheckpointIndex;
    }


}
