using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

 public class TrackCheckpoints : MonoBehaviour
{
    public event EventHandler<OnRobotCheckEventArgs> OnRobotCorrectCheckpoint;
    public event EventHandler<OnRobotCheckEventArgs> OnRobotWrongCheckpoint;

    public class OnRobotCheckEventArgs : EventArgs
    {
        public Transform robotTransform;
    }
    
    private List<CheckpointSingle> checkpointSingleList;
    private List<int> nextCheckpointSingleIndexList;
    [SerializeField] private List<Transform> robotTransformList;


    private void Awake()
    {

        Transform checkpointsTransform = transform.Find("Checkpoints");
        checkpointSingleList = new List<CheckpointSingle>();
        

        foreach (Transform checkpointsSingleTransform in checkpointsTransform)
        {
            CheckpointSingle checkpointSingle = checkpointsSingleTransform.GetComponent<CheckpointSingle>();
            checkpointSingle.SetTrackCheckpoints(this);
            checkpointSingleList.Add(checkpointSingle);

        }

        nextCheckpointSingleIndexList = new List<int>();
        foreach (Transform robotTransform in robotTransformList)
        {
            nextCheckpointSingleIndexList.Add(0);
        }
        
    }

    public void RobotThroughCheckpoint(CheckpointSingle checkpointSingle, Transform robotTransform)
    {
        int nextCheckpointSingleIndex = nextCheckpointSingleIndexList[robotTransformList.IndexOf(robotTransform)];

        if (checkpointSingleList.IndexOf(checkpointSingle) == nextCheckpointSingleIndex){
            //correct checkpoint
            CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];
            nextCheckpointSingleIndexList[robotTransformList.IndexOf(robotTransform)]
            = (nextCheckpointSingleIndex + 1) % checkpointSingleList.Count;
            OnRobotCorrectCheckpoint?.Invoke(this, new OnRobotCheckEventArgs { robotTransform = robotTransform});
        }
        else
        {
            OnRobotWrongCheckpoint?.Invoke(this, new OnRobotCheckEventArgs { robotTransform = robotTransform });
            CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];        
        }
       
    }
}
