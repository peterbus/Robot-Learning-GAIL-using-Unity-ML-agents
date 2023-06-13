using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToPathEnd_agent : Agent
{

    // some parts of the code and settings  are informed by the tutorial "Simple Checkpoints in Unity" by CodeMonkey https://www.youtube.com/watch?v=IOYNg6v9sfc
    // and also "AI learns to drive a Car" https://www.youtube.com/watch?v=2X5m_nDBvS4.
    // A reference code has been studied too, which elaborates certain methods in the TrackCheckpoint Class: IR-Project "SelfDrivingCars" by @monidp9
    // link: https://github.com/monidp9/IR-Project/tree/master/SelfDrivingCars. 
    // All accessed January-June 2023 [ONLINE]




    [SerializeField] private Transform targetTransform;//user defined 
    [SerializeField] private Transform start_transform;//user defined 
    [SerializeField] private Transform end_transform;//user defined 
    [SerializeField] private Transform agent_transform;//user defined 
    [SerializeField] private GameObject[] checkpoints;
    [SerializeField] private TrackCheckpoints trackCheckpoints;
    public GameObject Effector;
    private Vector3 actualPosition;
    private TrackCheckpoints robotTransform;
    private CheckpointSingle checkpointSingle;
    private Robot robot;//just an empty tag for the agent going through the checkpoints
   // private Rigidbody agentRb;
    public int numberOfPositions;
    private int current;

    private void Awake()
    {
        //agentRb = GetComponent<Rigidbody>();
        robot = GetComponent<Robot>();//empty GameObject with a tag attached to the ML_Agent, which itself is goigng through the checkpoints 

    }

    private void Start()
    {
       
        trackCheckpoints.OnRobotCorrectCheckpoint += TrackCheckpoints_OnRobotCorrectCheckpoint;
        trackCheckpoints.OnRobotWrongCheckpoint += TrackCheckpoints_OnRobotWrongCheckpoint;
        current = 0;
    }

    private void TrackCheckpoints_OnRobotWrongCheckpoint(object sender, TrackCheckpoints.OnRobotCheckEventArgs e)

    {
       //this is the main reward logic
        if (e.robotTransform == transform)
        {
            AddReward(-1f);
            Debug.Log("Wrong Checkpoint");
            //EndEpisode(); // this should be definitely OFF, otherwise the agent has no chance to explore other options!!!
        }
    }

    private void TrackCheckpoints_OnRobotCorrectCheckpoint(object sender, TrackCheckpoints.OnRobotCheckEventArgs e)
    {
        if (e.robotTransform == transform)
        {
            AddReward(+1f);
            Debug.Log("Correct Checkpoint");
        }
    }

    public override void OnEpisodeBegin()
    {
        //in case zero start
        Vector3 zeroV = new Vector3(0.268f, -0.285f, 0.012f);
        Debug.Log("EPISODE STARTED");

          transform.localPosition = zeroV;
          targetTransform.localPosition = zeroV;
      // float speed = 1.0f;
      // transform.localPosition = Vector3.MoveTowards(transform.localPosition, zeroV, speed * Time.deltaTime);
      // targetTransform.localPosition = Vector3.MoveTowards(targetTransform.localPosition, zeroV, speed * Time.deltaTime);
 
        trackCheckpoints.ResetCheckpoint(transform);
        
       /* foreach (GameObject checkpoint in checkpoints)
        {
            checkpoint.transform.localPosition = checkpoint.transform.localPosition + new Vector3(UnityEngine.Random.Range(-0.0003f, +0.0003f), 0, UnityEngine.Random.Range(-0.0003f, +0.0003f));
        }*/

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //inputs for the AI. What data the AI needs?
        //position
        CheckpointSingle checkpoint = trackCheckpoints.GetNextCheckpoint(transform);
        Vector3 checkpointForward = trackCheckpoints.GetNextCheckpoint(transform).transform.forward;
        Vector3 checkpointForward2 = trackCheckpoints.GetNextCheckpoint(transform).transform.right;
        Transform checkpointTransform = checkpoint.transform;

        float directionDot = Vector3.Dot(transform.forward, checkpointForward);
        sensor.AddObservation(directionDot);

        float directionDot2 = Vector3.Dot(transform.right, checkpointForward2);
        sensor.AddObservation(directionDot2);

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //ActionSegment<int> discreteActions = actions.DiscreteActions;
        ActionSegment<float> act = actions.ContinuousActions;

        var action0 = act[0];
        var action1 = act[1];
        var action2 = act[2];

        //initial values
        /*  float forwardAmount = 0f;
          float turnAmount = 0f;
          float updown = 0f;*/

        float forwardAmount = action0;
        float turnAmount = action1;
        float updown = action2;
        
        float moveSpeed = 1f;


        
        transform.localPosition += new Vector3(forwardAmount, updown, turnAmount) * Time.deltaTime * moveSpeed;
        targetTransform.transform.localPosition = transform.localPosition;
       // transform.forward = transform.localPosition;
        transform.rotation = Quaternion.LookRotation(transform.forward);

    }
    public override void Heuristic(in ActionBuffers actionsOut)//for imitation learning
    {
        //ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        ActionSegment<float> continuous = actionsOut.ContinuousActions;

        //agent_transform.transform.localPosition = targetTransform.localPosition;
        Effector.transform.localPosition = actualPosition;
        targetTransform.localPosition = actualPosition;//just a visualisation of the sphere-representation of the agent
        transform.localPosition = actualPosition;

        float speed = 1f;

        if (actualPosition == checkpoints[current].transform.position && current != numberOfPositions - 1)
        {
            current++;
        }

        if (actualPosition == checkpoints[current].transform.position && current != numberOfPositions)
        {
            current = 0;
            
        }

        actualPosition = Vector3.MoveTowards(actualPosition, checkpoints[current].transform.position, speed * Time.deltaTime);
        transform.forward = checkpoints[current].transform.position;
        transform.rotation = Quaternion.LookRotation(transform.forward);

        float moveSpeed = 1f;
        continuous[0] = actualPosition.x;
        continuous[1] = actualPosition.y;
        continuous[2] = actualPosition.z;
        transform.localPosition += new Vector3(actualPosition.x, actualPosition.y, actualPosition.z) * Time.deltaTime * moveSpeed;//adding to the current vector position new values

          /*continuous[0] = Input.GetAxis("Horizontal");
            continuous[1] = Input.GetKey("");
            continuous[2] = Input.GetAxis("Vertical");*/

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Wall>(out Wall wall))
        {
            AddReward(-0.005f);//reward when hit the collider
           // EndEpisode();
        }

        bool isCorrect, isLastCheckpoint;

        if (other.gameObject.tag == "Checkpoint")
        {
            checkpointSingle = other.GetComponent<CheckpointSingle>();
            isCorrect = checkpointSingle.IsCorrectCheckPoint(robot);
            isLastCheckpoint = checkpointSingle.IsLastCheckpoint();

            if (checkpointSingle != null && robot != null)
            {
                if (isCorrect)
                {
                    AddReward(+1f);
                    if (isLastCheckpoint)
                    {
                        AddReward(+2f);
                        EndEpisode();
                        Debug.Log("Epsiode ended after the collison with the last checkpoint");


                    }
                }
                else
                {
                    AddReward(-0.005f);

                }
            }
        }
    }

    void OnTriggerStay(Collider other)

    {

        if (other.gameObject.TryGetComponent<Wall>(out Wall wall))
        {
            
            AddReward(-0.005f);
           // EndEpisode();
           // Debug.Log("Epsiode ends after the collison with the wall");

        }


    }


    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.TryGetComponent<Wall>(out Wall wall))
        {

            AddReward(-0.005f);
            // EndEpisode();
            // Debug.Log("Epsiode ends after the collison with the wall");
        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("outside_Wall"))
        {
            EndEpisode();
            AddReward(-0.005f);
            Vector3 zeroV = new Vector3(0.268f, -0.285f, 0.012f);
            transform.localPosition = zeroV;
            targetTransform.localPosition = zeroV;
        }


    }
}
     
       


