using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;


public class Move_Along : Agent
{
    [SerializeField] private TrackCheckpoints trackCheckpoints;
    [SerializeField] private Transform spawnPosition;
    private Waypoints robotTransform;
    
    [SerializeField] private GameObject end;

    // [SerializeField] private Transform targetTransform;//user defined 

    private void Awake()
    {
        robotTransform = GetComponent<Waypoints>();
        
        

    }

    private void Start()
    {
        //trackCheckpoints = GetComponent<TrackCheckpoints>();
        trackCheckpoints.OnRobotCorrectCheckpoint += TrackCheckpoints_OnRobotCorrectCheckpoint;
        trackCheckpoints.OnRobotWrongCheckpoint += TrackCheckpoints_OnRobotWrongCheckpoint;
    }

    private void TrackCheckpoints_OnRobotWrongCheckpoint(object sender, TrackCheckpoints.OnRobotCheckEventArgs e)
    {
        if (e.robotTransform == transform)
        {
            AddReward(-0.1f);
            EndEpisode();
        }
    }

    private void TrackCheckpoints_OnRobotCorrectCheckpoint(object sender, TrackCheckpoints.OnRobotCheckEventArgs e)
    {
        if (e.robotTransform == transform)
        {
            AddReward(0.1f);
        }
    }

    public override void OnEpisodeBegin()
    {
      /*  Vector3 localZero = new Vector3(0, -0.3f, 0);
        transform.localPosition = localZero;
        transform.forward = spawnPosition.forward;*/
        
        float speed = 1.5f;

        transform.localPosition = Vector3.MoveTowards(transform.localPosition, spawnPosition.transform.localPosition, speed * Time.deltaTime);
      //  transform.right = spawnPosition.transform.localPosition;
      //  transform.rotation = Quaternion.LookRotation(spawnPosition.transform.localPosition);


    }

    public override void CollectObservations(VectorSensor sensor)
    {
       // Vector3 checkpointForward = trackCheckpoints.transform.forward;
       // float directionDot = Vector3.Dot(transform.forward, checkpointForward);
       // sensor.AddObservation(directionDot);
        sensor.AddObservation(spawnPosition.transform.localPosition);
        sensor.AddObservation(transform.localPosition);
        
        //sensor.AddObservation(targetTransform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
          float forwardAmount = 0f;
          float turnAmount = 0f;

          switch (actions.DiscreteActions[0])
          {
              case 0: forwardAmount = 0f; break;
              case 1: forwardAmount = +1f; break;
              case 2: forwardAmount = -1f; break;
          }

          switch (actions.DiscreteActions[1])
          {
              case 0: turnAmount = 0f; break;
              case 1: turnAmount = +1f; break;
              case 2: turnAmount = -1f; break;
          }
          float moveSpeed = 1.5f;
          transform.localPosition += new Vector3(forwardAmount, 0, turnAmount) * Time.deltaTime * moveSpeed;

        //ActionSegment<float> continuousActions = actions.ContinuousActions;
      //  float moveX = actions.ContinuousActions[0];
      //  float moveY = actions.ContinuousActions[1];
      //  float moveZ = actions.ContinuousActions[2];

       // continuousActions[0] = spawnPosition.transform.localPosition.x;
       // continuousActions[1] = spawnPosition.transform.localPosition.y;
       // continuousActions[2] = spawnPosition.transform.localPosition.z;

      //  float moveSpeed = 1f;
      //  transform.localPosition += new Vector3(moveX, moveY, moveZ) * Time.deltaTime * moveSpeed;
      // transform.localPosition += new Vector3(spawnPosition.localPosition.x, spawnPosition.transform.localPosition.y, spawnPosition.localPosition.z) * Time.deltaTime * moveSpeed;
        transform.right = spawnPosition.transform.localPosition;
        transform.rotation = Quaternion.LookRotation(spawnPosition.transform.localPosition);



        float distance2Checkpoint = Vector3.Distance(transform.localPosition, spawnPosition.transform.localPosition);
        
       /* if (Vector3.Distance(spawnPosition.transform.localPosition, Vector3.down) < distance2Checkpoint)
        {
            AddReward(1f);
        }*/

        if (distance2Checkpoint < 0.015)
        {
            AddReward(0.5f); 
        }

       // if (robotTransform.transform.localPosition == transform.localPosition)
      //  {
      //      AddReward(0.5f);
      //  }


        float distance2End = Vector3.Distance(transform.localPosition, end.transform.localPosition);

 

        if (distance2End <= 0.015)
        {
            EndEpisode();
        }
        
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        /* int forwardAction = 0;
         if (Input.GetKey(KeyCode.UpArrow)) forwardAction = 1;
         if (Input.GetKey(KeyCode.DownArrow)) forwardAction = 2;

         int turnAction = 0;
         if (Input.GetKey(KeyCode.RightArrow)) forwardAction = 1;
         if (Input.GetKey(KeyCode.LeftArrow)) forwardAction = 2;

         ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
         discreteActions[0] = forwardAction;
         discreteActions[1] = turnAction;*/

        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        float moveSpeed = 1f;
        continuousActions[0] = spawnPosition.transform.localPosition.x;
        continuousActions[1] = spawnPosition.transform.localPosition.y;
        continuousActions[2] = spawnPosition.transform.localPosition.z;
        transform.localPosition += new Vector3(spawnPosition.transform.localPosition.x, spawnPosition.transform.localPosition.y, spawnPosition.transform.localPosition.z) * Time.deltaTime * moveSpeed;
       // transform.right = spawnPosition.transform.localPosition;
       // transform.rotation = Quaternion.LookRotation(spawnPosition.transform.localPosition);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<Wall>(out Wall wall))
        {
            AddReward(-0.5f);
            EndEpisode();
        }

      //  if (collision.gameObject.TryGetComponent<CheckpointSingle>(out CheckpointSingle checkpoint))
      //  {
      //      AddReward(0.5f);
            
      //  }

      //  if (collision.gameObject.TryGetComponent<Waypoints>(out Waypoints waypoints))
      //  {
      //      AddReward(0.5f);
            
      //  }
    }






}
