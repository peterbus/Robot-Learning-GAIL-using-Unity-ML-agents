using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToPathEnd_agent : Agent {


    [SerializeField] private Transform targetTransform;//user defined 
    [SerializeField] private Transform start_transform;//user defined 
    [SerializeField] private Transform end_transform;//user defined 
    [SerializeField] private Transform agent_transform;//user defined 
    [SerializeField] private GameObject[] checkpoints;
    [SerializeField] private TrackCheckpoints trackCheckpoints;
    public GameObject Effector;
    private Vector3 actualPosition;
    private Waypoints robotTransform;

    public int numberOfPositions;
    private int current;

    // [SerializeField] private Transform guideTransform; //guide Hand object navigating the Target Agent


    private void Awake()
    {
        robotTransform = GetComponent<Waypoints>();



    }

    private void Start()
    {
        //trackCheckpoints = GetComponent<TrackCheckpoints>();
        trackCheckpoints.OnRobotCorrectCheckpoint += TrackCheckpoints_OnRobotCorrectCheckpoint;
        trackCheckpoints.OnRobotWrongCheckpoint += TrackCheckpoints_OnRobotWrongCheckpoint;
        current = 1;
    }

    private void TrackCheckpoints_OnRobotWrongCheckpoint(object sender, TrackCheckpoints.OnRobotCheckEventArgs e)
    {
        if (e.robotTransform == transform)
        {
            AddReward(-0.5f);
           // EndEpisode();
        }
    }

    private void TrackCheckpoints_OnRobotCorrectCheckpoint(object sender, TrackCheckpoints.OnRobotCheckEventArgs e)
    {
        if (e.robotTransform == transform)
        {
            AddReward(0.5f);
        }
    }




    public override void OnEpisodeBegin()
    {
      //   Vector3 zeroV = new Vector3(0, -0.03f, 0);
       //  targetTransform.localPosition = zeroV;
       //  transform.localPosition = zeroV;
        

        float speed = 1f;
       // targetTransform.localPosition = Vector3.MoveTowards(targetTransform.localPosition, start_transform.position, speed * Time.deltaTime);

      

        
        foreach (GameObject checkpoint in checkpoints)
        {
            checkpoint.transform.position=checkpoint.transform.position + new Vector3(UnityEngine.Random.Range(-0.008f, +0.008f), 0, UnityEngine.Random.Range(-0.008f, +0.008f));
        }

    }


    public override void CollectObservations(VectorSensor sensor)
    {
        //inputs for the AI. What data the AI needs?
        //position
        //postiton of the Path_end

        Vector3 checkpointForward = trackCheckpoints.transform.forward;
        float directionDot = Vector3.Dot(transform.forward, checkpointForward);
       // sensor.AddObservation(directionDot);
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        ActionSegment<int> discreteActions = actions.DiscreteActions;
       
        float forwardAmount = 0f;
        float turnAmount = 0f;
        float updown = 0f;
       

        switch (actions.DiscreteActions[0])
        {
            case 0: forwardAmount = 0f; break;
            case 1: forwardAmount = +1f; break;
            case 2: forwardAmount = -1f; break;
        }
        switch (actions.DiscreteActions[1])   
        {
            case 0: updown = 0f; break;
            case 1: updown = +0.05f; break;
            case 2: updown = -0.05f; break;
        }
        switch (actions.DiscreteActions[2])
        {
            case 0: turnAmount = 0f; break;
            case 1: turnAmount = +1f; break;
            case 2: turnAmount = -1f; break;
        }
 

       

        float moveSpeed = 0.8f;
        
        transform.localPosition += new Vector3(forwardAmount, updown, turnAmount) * Time.deltaTime * moveSpeed;

        float speed = 0.8f;
        targetTransform.localPosition = Vector3.MoveTowards(targetTransform.localPosition, transform.localPosition, speed * Time.deltaTime);
        transform.right = transform.localPosition;
        transform.rotation = Quaternion.LookRotation(targetTransform.localPosition);





        float distance2Checkpoint = Vector3.Distance(targetTransform.localPosition, end_transform.localPosition);
  
        if (targetTransform.localPosition == end_transform.localPosition)
        {
            SetReward(0.1f);
            EndEpisode();
           // Debug.Log("Episode ended after the vectors met");
           // Debug.Log("Reward Set successfully after the vectors met");
        }

        //Debug.Log(distance2Checkpoint);

        if (distance2Checkpoint < 1.762)
        {
            SetReward(0.1f);
            EndEpisode();
           // Debug.Log("Episode ended after the distance measurement");
           // Debug.Log("Reward Set successfully after the distance measurement");
        }

        

    }

    public override void Heuristic(in ActionBuffers actionsOut)//for imitation learning
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;


        agent_transform.transform.localPosition = targetTransform.localPosition;
        Effector.transform.position=actualPosition;
        

        float speed = 1f;

        if (actualPosition == checkpoints[current].transform.position && current != numberOfPositions - 1)
        {
            current++;
        }

        if (actualPosition == checkpoints[current].transform.position && current != numberOfPositions)
        {
            current = 1;
            EndEpisode();

        }

       // actualPosition = Vector3.MoveTowards(actualPosition, start_transform.transform.position, speed * Time.deltaTime);
        actualPosition = Vector3.MoveTowards(actualPosition, checkpoints[current].transform.position, speed * Time.deltaTime);
        transform.right = checkpoints[current].transform.position;
        transform.rotation = Quaternion.LookRotation(checkpoints[current].transform.position);

        float moveSpeed = 0.5f;
        continuousActions[0] = actualPosition.x;
        continuousActions[1] = actualPosition.y;
        continuousActions[2] = actualPosition.z;
        transform.localPosition += new Vector3(actualPosition.x, actualPosition.y, actualPosition.z) * Time.deltaTime * moveSpeed;
        //agent_transform.transform.localPosition = transform.localPosition;
        if (actualPosition == end_transform.localPosition)
        {
           
            EndEpisode();
            // Debug.Log("Episode ended after the vectors met");
            // Debug.Log("Reward Set successfully after the vectors met");
        }

    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.TryGetComponent<Goal>(out Goal goal)) { 
            SetReward(+1f);//reward when hit the collider
            EndEpisode();
            Debug.Log("Episode ended after the collision");
            Debug.Log("Reward Set successfully after the collison");

        }
        //AddReward();

        if (other.gameObject.TryGetComponent<Wall>(out Wall wall)) {
            AddReward(-1f);//reward when hit the collider
            EndEpisode();
            OnEpisodeBegin();
            Debug.Log("Reward Set successfully after the collison with Wall");
        }

        if (other.gameObject.TryGetComponent<CheckpointSingle>(out CheckpointSingle checkpoint))
        {
           SetReward(1f);
           //EndEpisode();
           Debug.Log("Reward Set successfully after the collison with Checkpoint");
        }

          if (other.gameObject.TryGetComponent<Waypoints>(out Waypoints waypoints))
          {
              AddReward(0.1f);

          }


    }
    
   /*  public void Update()
     {
        agent_transform.transform.localPosition = targetTransform.localPosition;



         if (targetTransform.localPosition == checkpoints[current].transform.position && current != numberOfPositions - 1)
         {
             current++;
         }

        if (targetTransform.localPosition == checkpoints[current].transform.position && current != numberOfPositions)
         {
             current = 1;
         }


        float speed = 0.5f;
        targetTransform.transform.position = Vector3.MoveTowards(targetTransform.transform.position, checkpoints[current].transform.position, speed * Time.deltaTime);
        transform.right = checkpoints[current].transform.position;
        transform.rotation = Quaternion.LookRotation(checkpoints[current].transform.position);

     }*/
}
