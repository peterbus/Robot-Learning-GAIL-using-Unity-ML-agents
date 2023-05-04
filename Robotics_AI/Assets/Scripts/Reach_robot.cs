using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class Reach_robot : Agent

{

    public GameObject pendulumA;
    public GameObject pendulumB;
    public GameObject pendulumC;
    public GameObject pendulumD;
    public GameObject pendulumE;
    public GameObject pendulumF;

    Rigidbody m_RbA;
    Rigidbody m_RbB;
    Rigidbody m_RbC;
    Rigidbody m_RbD;
    Rigidbody m_RbE;
    Rigidbody m_RbF;

    public GameObject end;
    public GameObject targetgoal;

    public override void Initialize()
    {
        m_RbA = pendulumA.GetComponent<Rigidbody>();
        m_RbB = pendulumB.GetComponent<Rigidbody>();
        m_RbC = pendulumC.GetComponent<Rigidbody>();
        m_RbD = pendulumD.GetComponent<Rigidbody>();
        m_RbE = pendulumE.GetComponent<Rigidbody>();
        m_RbF = pendulumF.GetComponent<Rigidbody>();
        

    }

    public override void OnEpisodeBegin()
    {
        pendulumA.transform.position = new Vector3(0f, 0f, 0f) + transform.position;
        pendulumA.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        m_RbA.velocity = Vector3.zero;
        m_RbA.angularVelocity = Vector3.zero;

        pendulumB.transform.position = new Vector3(0f, 0f, 0f) + transform.position;
        pendulumB.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        m_RbB.velocity = Vector3.zero;
        m_RbB.angularVelocity = Vector3.zero;

        pendulumC.transform.position = new Vector3(0f, 0f, 0f) + transform.position;
        pendulumC.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        m_RbC.velocity = Vector3.zero;
        m_RbC.angularVelocity = Vector3.zero;

        pendulumD.transform.position = new Vector3(0f, 0f, 0f) + transform.position;
        pendulumD.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        m_RbD.velocity = Vector3.zero;
        m_RbD.angularVelocity = Vector3.zero;

        pendulumE.transform.position = new Vector3(0f, 0f, 0f) + transform.position;
        pendulumE.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        m_RbE.velocity = Vector3.zero;
        m_RbE.angularVelocity = Vector3.zero;

        pendulumF.transform.position = new Vector3(0f, 0f, 0f) + transform.position;
        pendulumF.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        m_RbF.velocity = Vector3.zero;
        m_RbF.angularVelocity = Vector3.zero;

    



    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(pendulumA.transform.localPosition);
        sensor.AddObservation(pendulumA.transform.rotation);
        sensor.AddObservation(m_RbA.angularVelocity);
        sensor.AddObservation(m_RbA.velocity);

        sensor.AddObservation(pendulumB.transform.localPosition);
        sensor.AddObservation(pendulumB.transform.rotation);
        sensor.AddObservation(m_RbB.angularVelocity);
        sensor.AddObservation(m_RbB.velocity);

        sensor.AddObservation(pendulumC.transform.localPosition);
        sensor.AddObservation(pendulumC.transform.rotation);
        sensor.AddObservation(m_RbC.angularVelocity);
        sensor.AddObservation(m_RbC.velocity);

        sensor.AddObservation(pendulumD.transform.localPosition);
        sensor.AddObservation(pendulumD.transform.rotation);
        sensor.AddObservation(m_RbD.angularVelocity);
        sensor.AddObservation(m_RbD.velocity);

        sensor.AddObservation(pendulumE.transform.localPosition);
        sensor.AddObservation(pendulumE.transform.rotation);
        sensor.AddObservation(m_RbE.angularVelocity);
        sensor.AddObservation(m_RbE.velocity);

        sensor.AddObservation(pendulumF.transform.localPosition);
        sensor.AddObservation(pendulumF.transform.rotation);
        sensor.AddObservation(m_RbF.angularVelocity);
        sensor.AddObservation(m_RbF.velocity);

        sensor.AddObservation(targetgoal.transform.localPosition);
        sensor.AddObservation(end.transform.localPosition);



    }

    public void OnActionReceived(float[] vectorAction)
    {
        var torque = Mathf.Clamp(vectorAction[0], -1f, 1f) * 150f;
        m_RbA.AddTorque(new Vector3(0f, torque, 0f));

        torque = Mathf.Clamp(vectorAction[1], -1f, 1f) * 150f;
        m_RbB.AddTorque(new Vector3(0f, 0f, torque));

        torque = Mathf.Clamp(vectorAction[2], -1f, 1f) * 150f;
        m_RbC.AddTorque(new Vector3(0f, 0f, torque));

        torque = Mathf.Clamp(vectorAction[3], -1f, 1f) * 150f;
        m_RbD.AddTorque(new Vector3(0f, torque, 0f));

        torque = Mathf.Clamp(vectorAction[4], -1f, 1f) * 150f;
        m_RbE.AddTorque(new Vector3(0f, 0f, torque));

        torque = Mathf.Clamp(vectorAction[5], -1f, 1f) * 150f;
        m_RbF.AddTorque(new Vector3(0f, torque, 0f));
        
    }

}
