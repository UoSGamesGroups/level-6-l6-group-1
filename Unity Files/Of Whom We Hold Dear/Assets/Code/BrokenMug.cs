using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenMug : MonoBehaviour {

    public PlayerMovement playerMovement;
    public float timeTillNextMemory;
    public bool callOnce;

	// Use this for initialization
	void Start () {
        callOnce = false;
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        timeTillNextMemory = 5;      
    }
	
	// Update is called once per frame
	void Update () {

        if(timeTillNextMemory <=0 && !callOnce)
        {
            playerMovement.CallAnimations("FallProggession", 6);
            callOnce = true;
        }
        else
        {
            timeTillNextMemory -= Time.deltaTime;
        }
    }
}
