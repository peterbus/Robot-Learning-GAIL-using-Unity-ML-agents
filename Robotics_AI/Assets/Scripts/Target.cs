using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject Goal;
   
    [SerializeField] private float speed = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position = Vector3.MoveTowards(transform.position, Goal.transform.position, speed * Time.deltaTime);
        transform.right = Goal.transform.position;
        transform.rotation = Quaternion.LookRotation(Goal.transform.position);
        
    }
}
