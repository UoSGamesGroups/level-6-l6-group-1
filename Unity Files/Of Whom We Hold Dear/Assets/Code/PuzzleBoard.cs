using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class PuzzleBoard : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Texture PuzzleImage;
    public int puzzleSize;
    public bool isCompleted;
    public GameObject destination;
    public float waitForGoodData;

    // Use this for initialization
    void Start()
    {
        waitForGoodData = 1;
    }

    void Update()
    {
        if (isCompleted)
        {

            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().detectCollisions = false;
            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.SetDestination(destination.transform.position);

            navMeshAgent.updateRotation = true;
            navMeshAgent.updatePosition = true;    

            if (waitForGoodData > 0)
            {
                waitForGoodData -= Time.deltaTime;
            }
            else if(navMeshAgent.remainingDistance <= 0.1f)
            {
                Debug.Log("Arrived");
                
            }
        }  
    }
}
