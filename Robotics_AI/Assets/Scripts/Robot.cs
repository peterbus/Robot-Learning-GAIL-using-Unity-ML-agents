using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    private float forwardAmount;
    private float turnAmount;
    private float updown;
    private Vector3 position;
    private Quaternion rotation;

    // Start is called before the first frame update
    void Awake()
    {
        position = transform.localPosition; 
        rotation = transform.localRotation;
    }
}
