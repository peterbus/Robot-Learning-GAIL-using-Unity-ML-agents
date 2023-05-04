using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] private GameObject Hand;
    [SerializeField] private float speed = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Hand.transform.position, speed * Time.deltaTime);
       // transform.up = Hand.transform.position - transform.position;
    }
}
